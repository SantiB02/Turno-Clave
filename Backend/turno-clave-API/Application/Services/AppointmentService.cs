using turno_clave_API.Application.DTOs.Appointment;
using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Common;
using turno_clave_API.Infrastructure.Repositories.Interfaces;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IProfessionalService _professionalService;
        private readonly IClientService _clientService;
        private readonly IServiceService _serviceService;
        private readonly IProfessionalAvailabilityService _professionalAvailabilityService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IBusinessRepository businessRepository,
            IProfessionalService professionalService,
            IClientService clientService,
            IServiceService serviceService,
            IProfessionalAvailabilityService professionalAvailabilityService)
        {
            _appointmentRepository = appointmentRepository;
            _businessRepository = businessRepository;
            _professionalService = professionalService;
            _clientService = clientService;
            _serviceService = serviceService;
            _professionalAvailabilityService = professionalAvailabilityService;
        }

        public async Task<Result<Appointment>> CreateAsync(CreateAppointmentDTO dto)
        {
            if (dto.StartDateTime >= dto.EndDateTime)
                return Result<Appointment>.Failure("StartDateTime must be earlier than EndDateTime.");

            if (dto.Items == null || dto.Items.Count == 0)
                return Result<Appointment>.Failure("Appointment must have at least one service item.");

            // Validate business
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(dto.BusinessExternalId);
            if (business == null)
                return Result<Appointment>.Failure($"Business with ExternalId {dto.BusinessExternalId} not found.");

            // Get or create client (deduplicación por email)
            Client? client = await _clientService.GetByEmailAsync(business.Id, dto.Client.Email);
            if (client == null)
            {
                // Cliente no existe, crear uno nuevo
                client = await _clientService.CreateAsync(new CreateClientDTO
                {
                    BusinessExternalId = dto.BusinessExternalId,
                    Name = dto.Client.Name,
                    Email = dto.Client.Email,
                    Phone = dto.Client.Phone,
                    Notes = dto.Client.Notes
                });
            }

            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(business.TimeZone);
            DateTimeOffset startLocal = TimeZoneInfo.ConvertTime(dto.StartDateTime, tz);
            DateTimeOffset endLocal = TimeZoneInfo.ConvertTime(dto.EndDateTime, tz);
            DayOfWeek day = startLocal.DayOfWeek;

            // Validate and create items
            List<AppointmentItem> appointmentItems = new();

            foreach (var itemDto in dto.Items)
            {
                // Validate professional
                Professional? professional = await _professionalService.GetByExternalIdAsync(itemDto.ProfessionalExternalId);
                if (professional == null)
                    return Result<Appointment>.Failure($"Professional with ExternalId {itemDto.ProfessionalExternalId} not found.");

                // Validate service
                Result<Service?> serviceResult = await _serviceService.GetByExternalIdAsync(itemDto.ServiceExternalId);
                if (!serviceResult.IsSuccess || serviceResult.Value == null)
                    return Result<Appointment>.Failure($"Service with ExternalId {itemDto.ServiceExternalId} not found.");

                Service service = serviceResult.Value;

                // Check professional availability for this time block
                bool isAppointmentTaken = await _appointmentRepository.IsAppointmentTakenAsync(professional.Id, dto.StartDateTime, dto.EndDateTime);
                if (isAppointmentTaken)
                    return Result<Appointment>.Failure($"Professional {professional.Name} is not available at the requested time slot.");

                bool isDayWorkDay = await _professionalAvailabilityService.IsDayWorkDayAsync(professional, day);
                if (!isDayWorkDay)
                    return Result<Appointment>.Failure($"Professional {professional.Name} does not work on {day}.");

                // Create appointment item
                appointmentItems.Add(new AppointmentItem
                {
                    ServiceId = service.Id,
                    Service = service,
                    ProfessionalId = professional.Id,
                    Professional = professional,
                    StartDateTime = new DateTime(DateOnly.FromDateTime(dto.StartDateTime.DateTime), itemDto.StartTime).ToUniversalTime(),
                    EndDateTime = new DateTime(DateOnly.FromDateTime(dto.StartDateTime.DateTime), itemDto.EndTime).ToUniversalTime()
                });
            }

            // Create appointment with all items
            Appointment appointment = new()
            {
                ExternalId = Guid.NewGuid(),
                BusinessId = business.Id,
                Business = business,
                ClientId = client.Id,
                Client = client,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Notes = dto.Notes,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                Items = appointmentItems
            };

            _appointmentRepository.AddAppointment(appointment);
            await _appointmentRepository.SaveAsync();

            return Result<Appointment>.Success(appointment);
        }

        public async Task<Appointment?> GetByExternalIdAsync(Guid externalId)
        {
            Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(externalId);
            return appointment;
        }

        public async Task<AvailabilitySlotsResponseDTO> GetAvailableSlotsAsync(SelectionRequestDTO request)
        {
            // Validate business
            Business? business = await _businessRepository.GetBusinessByExternalIdAsync(request.BusinessExternalId);
            if (business == null)
                return new AvailabilitySlotsResponseDTO
                {
                    AvailableSlots = new List<AvailabilitySlotDTO>(),
                    SearchFromDate = DateOnly.FromDateTime(request.SearchFromDate),
                    SearchToDate = DateOnly.FromDateTime(request.SearchToDate)
                };

            // Get all professionals and services for the business via external-id based APIs (return Result<T>)
            var profsResult = await _professionalService.GetByBusinessExternalIdAsync(business.ExternalId);
            List<Professional> professionals = new();
            if (profsResult.IsSuccess && profsResult.Value != null)
            {
                // profsResult.Value is List<ProfessionalDTO>; convert to domain Professionals by fetching each
                foreach (var pdto in profsResult.Value)
                {
                    var p = await _professionalService.GetByExternalIdAsync(pdto.ExternalId);
                    if (p != null) professionals.Add(p);
                }
            }
            else
            {
                professionals = new List<Professional>();
            }

            var servicesResult = await _serviceService.GetByBusinessExternalIdAsync(business.ExternalId);
            List<Service> services = new();
            if (servicesResult.IsSuccess && servicesResult.Value != null)
            {
                // unwrap result value
                services = servicesResult.Value.ToList();
            }
            else
            {
                // If failed to retrieve services, treat as none (will lead to empty result set later)
                services = new List<Service>();
            }

            // Build service info (duration, price) for only requested services
            var requestedServices = new List<(Domain.Entities.Service service, Guid? preferredProfessionalExternalId)>();
            foreach (var rs in request.Services)
            {
                var svcResult = await _serviceService.GetByExternalIdAsync(rs.ServiceExternalId);
                if (!svcResult.IsSuccess || svcResult.Value == null)
                {
                    // If a requested service doesn't exist, return empty result
                    return new AvailabilitySlotsResponseDTO
                    {
                        AvailableSlots = new List<AvailabilitySlotDTO>(),
                        SearchFromDate = DateOnly.FromDateTime(request.SearchFromDate),
                        SearchToDate = DateOnly.FromDateTime(request.SearchToDate)
                    };
                }

                requestedServices.Add((svcResult.Value, rs.ProfessionalExternalId));
            }

            // Gather candidate professional external ids from requested services
            var candidateProfessionalExternalIds = new HashSet<Guid>();
            foreach (var (service, preferred) in requestedServices)
            {
                if (preferred != null && preferred != Guid.Empty)
                {
                    candidateProfessionalExternalIds.Add(preferred.Value);
                }
                else
                {
                    // service.ProfessionalServices includes Professional with ExternalId via repository includes
                    foreach (var ps in service.ProfessionalServices)
                    {
                        if (ps.Professional != null)
                            candidateProfessionalExternalIds.Add(ps.Professional.ExternalId);
                    }
                }
            }

            // Fetch domain Professional entities only for the candidate external ids (bulk)
            var externalIdToProfessional = new Dictionary<Guid, Professional>();
            var candidateExternalIdList = candidateProfessionalExternalIds.ToList();
            List<Professional> fetchedProfessionals = await _professionalService.GetByExternalIdsAsync(candidateExternalIdList);
            foreach (var p in fetchedProfessionals)
            {
                if (p != null) externalIdToProfessional[p.ExternalId] = p;
            }

            // For each service determine candidate professionals (respecting preferred if provided)
            List<List<Professional>> candidatesPerService = new();
            foreach (var (service, preferred) in requestedServices)
            {
                if (preferred != null && preferred != Guid.Empty)
                {
                    if (externalIdToProfessional.TryGetValue(preferred.Value, out var prof))
                    {
                        // ensure this professional can perform the service
                        bool canDo = prof.ProfessionalServices.Any(ps => ps.Service.ExternalId == service.ExternalId);
                        if (canDo)
                            candidatesPerService.Add(new List<Professional> { prof });
                        else
                            candidatesPerService.Add(new List<Professional>());
                    }
                    else
                    {
                        candidatesPerService.Add(new List<Professional>());
                    }
                }
                else
                {
                    var list = new List<Professional>();
                    foreach (var ps in service.ProfessionalServices)
                    {
                        if (ps.Professional != null && externalIdToProfessional.TryGetValue(ps.Professional.ExternalId, out var p))
                        {
                            list.Add(p);
                        }
                    }

                    // Simple heuristic: sort candidates by number of availabilities ascending (fewer availability first)
                    // and cap candidates to avoid combinatorial explosion
                    const int CandidateCap = 5;
                    list = list.OrderBy(p => p.Availabilities?.Count ?? 0).Take(CandidateCap).ToList();

                    candidatesPerService.Add(list);
                }
            }

            // If any service has no candidates, no possible slots
            if (candidatesPerService.Any(l => l.Count == 0))
            {
                return new AvailabilitySlotsResponseDTO
                {
                    AvailableSlots = new List<AvailabilitySlotDTO>(),
                    SearchFromDate = DateOnly.FromDateTime(request.SearchFromDate),
                    SearchToDate = DateOnly.FromDateTime(request.SearchToDate)
                };
            }

            List<AvailabilitySlotDTO> resultSlots = new();

            // Pre-calc total duration
            int totalDurationMinutes = requestedServices.Sum(x => x.service.DurationMinutes);

            // Timezone for business
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(business.TimeZone);

            // Helper: generate all combinations of professionals (cartesian product)
            IEnumerable<List<Professional>> GenerateCombinations()
            {
                IEnumerable<List<Professional>> seed = new[] { new List<Professional>() };
                foreach (var list in candidatesPerService)
                {
                    seed = seed.SelectMany(acc => list.Select(item => { var copy = new List<Professional>(acc); copy.Add(item); return copy; }));
                }
                return seed;
            }

            var combos = GenerateCombinations().ToList();

            // Iterate dates
            DateTime fromDate = request.SearchFromDate.Date;
            DateTime toDate = request.SearchToDate.Date;
            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                DayOfWeek day = date.DayOfWeek;

                // For each combination of professionals, scan day in 15-minute steps
                foreach (var combo in combos)
                {
                    // For performance skip combos where any professional does not work that day
                    bool allWorkDay = true;
                    for (int i = 0; i < combo.Count; i++)
                    {
                        var prof = combo[i];
                        if (!await _professionalAvailabilityService.IsDayWorkDayAsync(prof, day))
                        {
                            allWorkDay = false; break;
                        }
                    }
                    if (!allWorkDay) continue;

                    // Step through the day in 15-minute increments
                    const int step = 15;
                    for (int minute = 0; minute < 24 * 60; minute += step)
                    {
                        TimeOnly candidateStart = TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(minute));
                        TimeOnly candidateEnd = candidateStart.AddMinutes(totalDurationMinutes);
                        if (candidateEnd < candidateStart) // overflowed midnight
                            continue;

                        bool fits = true;
                        var serviceDetails = new List<AvailableServiceDetailDTO>();
                        int offset = 0;

                        for (int i = 0; i < requestedServices.Count; i++)
                        {
                            var svc = requestedServices[i].service;
                            var prof = combo[i];
                            int dur = svc.DurationMinutes;
                            TimeOnly svcStart = candidateStart.AddMinutes(offset);
                            TimeOnly svcEnd = svcStart.AddMinutes(dur);

                            // Check professional availability ranges for this day
                            bool availableRange = prof.Availabilities.Any(av =>
                                av.DayOfWeek == day && av.StartTime <= svcStart && av.EndTime >= svcEnd
                            );
                            if (!availableRange)
                            {
                                fits = false; break;
                            }

                            // Convert svcStart/svcEnd to UTC DateTimeOffset for appointment conflict check
                            DateTime localStart = new DateTime(date.Year, date.Month, date.Day, svcStart.Hour, svcStart.Minute, 0);
                            DateTime localEnd = new DateTime(date.Year, date.Month, date.Day, svcEnd.Hour, svcEnd.Minute, 0);
                            DateTimeOffset startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, tz);
                            DateTimeOffset endUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd, tz);

                            bool isTaken = await _appointmentRepository.IsAppointmentTakenAsync(prof.Id, startUtc, endUtc);
                            if (isTaken)
                            {
                                fits = false; break;
                            }

                            serviceDetails.Add(new AvailableServiceDetailDTO
                            {
                                ServiceExternalId = svc.ExternalId,
                                ServiceName = svc.Name,
                                DurationMinutes = dur,
                                Price = svc.Price,
                                AssignedProfessionalExternalId = prof.ExternalId,
                                AssignedProfessionalName = prof.Name,
                                ServiceStartTime = svcStart,
                                ServiceEndTime = svcEnd
                            });

                            offset += dur;
                        }

                        if (fits)
                        {
                            // Build AvailabilitySlotDTO
                            var slot = new AvailabilitySlotDTO
                            {
                                Date = DateOnly.FromDateTime(date),
                                StartTime = candidateStart,
                                EndTime = candidateStart.AddMinutes(totalDurationMinutes),
                                TotalDurationMinutes = totalDurationMinutes,
                                ServiceDetails = serviceDetails,
                                AvailableMinutesAfter = 0
                            };
                            resultSlots.Add(slot);
                        }
                    }
                }
            }

            return new AvailabilitySlotsResponseDTO
            {
                AvailableSlots = resultSlots,
                SearchFromDate = DateOnly.FromDateTime(request.SearchFromDate),
                SearchToDate = DateOnly.FromDateTime(request.SearchToDate)
            };
        }

        // TODO: Implement UpdateAsync method and add validations (e.g. check if the new time slot is valid and not overlapping with other appointments for the same professional)
        //public async Task<Result<Appointment>> UpdateAsync(UpdateAppointmentDTO dto)
        //{
        //    Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(dto.ExternalId);
        //    if (appointment == null)
        //        return Result<Appointment>.Failure($"Appointment with ExternalId {dto.ExternalId} not found.");
        //    appointment.StartDateTime = dto.StartDateTime;
        //    appointment.EndDateTime = dto.EndDateTime;
        //    appointment.Notes = dto.Notes;
        //    appointment.Status = dto.Status;
        //    appointment.UpdatedAt = DateTimeOffset.UtcNow;
        //    _appointmentRepository.UpdateAppointment(appointment);
        //    await _appointmentRepository.SaveAsync();
        //    return Result<Appointment>.Success(appointment);
        //}

        public async Task<Result<Appointment>> CancelAsync(Guid externalId)
        {
            Appointment? appointment = await _appointmentRepository.GetAppointmentByExternalIdAsync(externalId);
            if (appointment == null)
            {
                return Result<Appointment>.Failure($"Appointment with ExternalId {externalId} not found.");
            }
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.SaveAsync();
            return Result<Appointment>.Success(appointment);
        }

        public async Task<Result<Appointment>> CancelAsync(Appointment appointment)
        {
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.SaveAsync();
            return Result<Appointment>.Success(appointment);
        }
    }
}
