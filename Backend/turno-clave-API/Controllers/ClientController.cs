using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using turno_clave_API.Application.DTOs.Client;
using turno_clave_API.Application.Interfaces;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Controllers
{
    [Route("api/clients")]
    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDTO createClientDTO)
        {
            try
            {
                Client client = await _clientService.CreateAsync(createClientDTO);
                ClientDTO dto = Client.ToDto(client);
                return CreatedAtAction(nameof(GetByExternalId), new { externalId = dto.ExternalId }, dto);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Business not found",
                    detail: ex.Message,
                    type: "/errors/BusinessNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }

        [HttpGet("{externalId:guid}")]
        public async Task<IActionResult> GetByExternalId(Guid externalId)
        {
            Client? client = await _clientService.GetByExternalIdAsync(externalId);
            if (client == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Client Not Found",
                    detail: $"Client with ExternalId {externalId} not found.",
                    type: $"/errors/ClientNotFound",
                    instance: HttpContext.Request.Path
                );
            }
            return Ok(Client.ToDto(client));
        }


        // THIS SHOULD NOT BE INCLUDED IN MVP. A CLIENT'S PERSONAL DATA IS ONLY ASKED ONCE WHEN THEY BOOK AN APPOINTMENT

        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] UpdateClientDTO updateClientDTO)
        //{
        //    try
        //    {
        //        Client? client = await _clientService.UpdateAsync(updateClientDTO);
        //        if (client == null)
        //        {
        //            return Problem(
        //                statusCode: StatusCodes.Status404NotFound,
        //                title: "Client Not Found",
        //                detail: $"Client with ExternalId {updateClientDTO.ExternalId} not found.",
        //                type: $"/errors/ClientNotFound",
        //                instance: HttpContext.Request.Path
        //            );
        //        }
        //        return Ok(Client.ToDto(client));
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return Problem(
        //            statusCode: StatusCodes.Status404NotFound,
        //            title: "Client Not Found",
        //            detail: ex.Message,
        //            type: $"/errors/ClientNotFound",
        //            instance: HttpContext.Request.Path
        //        );
        //    }
        //}

        [HttpDelete("{externalId:guid}")]
        public async Task<IActionResult> Delete(Guid externalId)
        {
            try
            {
                Client? client = await _clientService.DeleteAsync(externalId);
                if (client == null)
                {
                    return Problem(
                        statusCode: StatusCodes.Status404NotFound,
                        title: "Client Not Found",
                        detail: $"Client with ExternalId {externalId} not found.",
                        type: $"/errors/ClientNotFound",
                        instance: HttpContext.Request.Path
                    );
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Client Not Found",
                    detail: ex.Message,
                    type: $"/errors/ClientNotFound",
                    instance: HttpContext.Request.Path
                );
            }
        }
    }
}
