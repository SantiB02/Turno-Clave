"use client"

import { ExclamationTriangleIcon } from "@heroicons/react/20/solid"
import {
  PencilSquareIcon,
  QuestionMarkCircleIcon,
} from "@heroicons/react/24/outline"
import { useRouter } from "next/navigation"
import { useEffect, useState } from "react"
import {
  CitySelect,
  GetCity,
  GetState,
  StateSelect,
} from "react-country-state-city"
import type { City, State } from "react-country-state-city/dist/esm/types"
import "react-country-state-city/dist/react-country-state-city.css"
import ModalForm from "@/app/components/ModalForm"
import {
  type SupportedCountry,
  supportedCountries,
} from "@/data/supportedCountries"
import {
  PAYMENT_METHOD_LABELS,
  PAYMENT_METHODS,
  type PaymmentMethod,
} from "@/enums/paymentMethods"
import { updateBusiness } from "@/services/businessService"
import type { BusinessDetail, UpdateBusinessDTO } from "@/types/business"

type InformacionTabProps = {
  business: BusinessDetail
}

type SelectDefaultValue<T> =
  | ((string | number | readonly string[]) & T)
  | undefined

export default function InformacionTab({ business }: InformacionTabProps) {
  const router = useRouter()

  const [isEditModalOpen, setIsEditModalOpen] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)
  const [name, setName] = useState<string>(business.name)
  const [description, setDescription] = useState(business.description ?? "")
  const [paymentMethods, setPaymentMethods] = useState<PaymmentMethod[]>(
    business.paymentMethods,
  )
  const [phone, setPhone] = useState<string>(business.phone)
  const [country, setCountry] = useState<SupportedCountry | null>(null)
  const [state, setState] = useState<State | null>(null)
  const [city, setCity] = useState<City | null>(null)
  const [address, setAddress] = useState<string>(business.address)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let isActive = true

    const syncLocation = async () => {
      const selectedCountry =
        supportedCountries.find((item) => item.name === business.country) ??
        null

      if (!isActive) return

      setCountry(selectedCountry)
      setState(null)
      setCity(null)

      if (!selectedCountry) return

      const states = await GetState(selectedCountry.id)
      if (!isActive) return

      const selectedState =
        states.find((item) => item.name === business.state) ?? null

      setState(selectedState)

      if (
        !selectedState ||
        selectedState.name === "Ciudad Autónoma de Buenos Aires"
      ) {
        return
      }

      const cities = await GetCity(selectedCountry.id, selectedState.id)
      if (!isActive) return

      const selectedCity =
        cities.find((item) => item.name === business.city) ?? null

      setCity(selectedCity)
    }

    setName(business.name)
    setDescription(business.description ?? "")
    setPaymentMethods(business.paymentMethods)
    setPhone(business.phone)
    setAddress(business.address)
    void syncLocation()

    return () => {
      isActive = false
    }
  }, [business])

  const handleClose = () => {
    setIsEditModalOpen(false)
    setName(business.name)
    setDescription(business.description ?? "")
    setPaymentMethods(business.paymentMethods)
    setPhone(business.phone)
    setAddress(business.address)
    setError(null)
    void (async () => {
      const selectedCountry =
        supportedCountries.find((item) => item.name === business.country) ??
        null

      setCountry(selectedCountry)
      setState(null)
      setCity(null)

      if (!selectedCountry) return

      const states = await GetState(selectedCountry.id)
      const selectedState =
        states.find((item) => item.name === business.state) ?? null

      setState(selectedState)

      if (
        !selectedState ||
        selectedState.name === "Ciudad Autónoma de Buenos Aires"
      ) {
        return
      }

      const cities = await GetCity(selectedCountry.id, selectedState.id)
      const selectedCity =
        cities.find((item) => item.name === business.city) ?? null

      setCity(selectedCity)
    })()
  }

  const handleOnChangePaymentMethod = (
    e: React.ChangeEvent<HTMLInputElement>,
    method: PaymmentMethod,
  ) => {
    const newPaymentMethods = paymentMethods.filter((item) => item !== method)

    if (e.target.checked) {
      newPaymentMethods.push(method)
    }

    setPaymentMethods(newPaymentMethods)
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    setError(null)
    let shouldCloseModal = false

    try {
      if (!country || !state) {
        setError("Por favor, complete todos los campos requeridos.")
        return
      }

      const resolvedCity =
        state.name === "Ciudad Autónoma de Buenos Aires"
          ? "Ciudad Autónoma de Buenos Aires"
          : city?.name

      if (!resolvedCity) {
        setError("Por favor, seleccione una ciudad.")
        return
      }

      const data: UpdateBusinessDTO = {
        name,
        description,
        phone,
        country: country.name,
        state: state.name,
        city: resolvedCity,
        timeZone: country.timezone,
        address,
        paymentMethods,
      }

      console.log("DATA FOR UPDATE", data)

      await updateBusiness(business.externalId, data)
      shouldCloseModal = true
      router.refresh()
    } catch (submitError) {
      setError(
        submitError instanceof Error ? submitError.message : "Unknown error",
      )
    } finally {
      if (shouldCloseModal) {
        handleClose()
      }

      setLoading(false)
    }
  }

  const phoneRegex = /^\+?[0-9]{6,15}$/
  const isValidPhone = phoneRegex.test(phone)

  const isSubmitDisabled =
    name.trim().length < 3 ||
    !phone ||
    !isValidPhone ||
    !country ||
    !state ||
    (!city && state.name !== "Ciudad Autónoma de Buenos Aires") ||
    address.trim().length < 5 ||
    error !== null

  return (
    <div>
      <div className="flex underline">
        <ExclamationTriangleIcon className="h-6 w-6 text-yellow-400 mr-1" />
        <p>Estos datos seran publicos para tus clientes.</p>
      </div>
      <div className="max-w-120 border rounded-xl px-4 py-3 mt-2 border-gray-400">
        <ul className="flex flex-col gap-1">
          <li>
            <span className="text-primary-orange font-bold">Nombre:</span>{" "}
            {business.name}{" "}
            <span className="text-sm text-gray-500 ml-1">
              ({business.slug})
            </span>
            <div className="relative inline-block group">
              <QuestionMarkCircleIcon className="h-5 w-5 text-gray-500 ml-1 inline-block cursor-pointer" />

              <div className="absolute left-1/2 -translate-x-1/2 mt-2 hidden group-hover:block bg-gray-800 text-white text-sm rounded px-2 py-1 whitespace-nowrap z-10">
                Este es el identificador unico de tu negocio en el link
                compartido a tus clientes.
              </div>
            </div>
          </li>
          {business.description && (
            <li>
              <span className="text-primary-orange font-bold">
                Descripcion:
              </span>{" "}
              {business.description}
            </li>
          )}
          <li>
            <span className="text-primary-orange font-bold">Ubicacion:</span>{" "}
            {business.address}, {business.city}, {business.state},{" "}
            {business.country}
          </li>
          <li>
            <span className="text-primary-orange font-bold">Telefono:</span>{" "}
            {business.phone}
          </li>
          <li>
            <span className="text-primary-orange font-bold">
              Medios de pago:
            </span>{" "}
            {business.paymentMethods.length === 0 ? (
              <span className="text-gray-600">No configurados.</span>
            ) : (
              business.paymentMethods
                .map((method) => PAYMENT_METHOD_LABELS[method])
                .join(", ")
            )}
          </li>
        </ul>
        <button
          type="button"
          onClick={() => setIsEditModalOpen(true)}
          className="flex text-primary-orange cursor-pointer border-1 rounded-2xl border-primary-orange px-3 py-1 mt-3 hover:bg-primary-orange hover:text-white transition-colors"
        >
          <PencilSquareIcon className="h-6 w-6 mr-1" />
          Editar
        </button>
      </div>

      <ModalForm
        open={isEditModalOpen}
        onClose={handleClose}
        title="Editar negocio"
        onSubmit={handleSubmit}
        submitLabel="Guardar cambios"
        loading={loading}
        loadingLabel="Guardando cambios..."
        submitDisabled={isSubmitDisabled}
        width="2xl"
      >
        <div className="space-y-3">
          <section>
            <h3 className="font-semibold text-lg mb-3">Datos basicos</h3>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label htmlFor="name" className="block mb-1">
                  Nombre
                </label>

                <input
                  name="name"
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  placeholder="Nombre del negocio"
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                />
              </div>

              <div>
                <label htmlFor="phone" className="block mb-1">
                  Telefono
                </label>

                <input
                  name="phone"
                  type="text"
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
                  placeholder="Telefono"
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                />
              </div>
            </div>
          </section>

          <section>
            <h3 className="font-semibold text-lg mb-3">Descripcion</h3>

            <textarea
              name="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Descripcion del negocio"
              rows={4}
              className="w-full border border-gray-300 rounded-lg px-3 py-2 resize-none focus:outline-none focus:ring-2 focus:ring-orange-500"
            />
          </section>

          <section>
            <h3 className="font-semibold text-lg mb-3">Metodos de pago</h3>
            <div className="flex flex-wrap gap-2">
              {PAYMENT_METHODS.map((method) => (
                <label
                  key={method}
                  htmlFor={method}
                  className="flex items-center unselectable gap-2 bg-orange-100 text-orange-800 px-3 py-1 rounded-full"
                >
                  <input
                    type="checkbox"
                    id={method}
                    name="paymentMethods"
                    value={method}
                    checked={paymentMethods.includes(method)}
                    onChange={(e) => handleOnChangePaymentMethod(e, method)}
                    className="w-4 h-4"
                  />
                  {PAYMENT_METHOD_LABELS[method]}
                </label>
              ))}
            </div>
          </section>

          <section>
            <h3 className="font-semibold text-lg mb-3">Ubicacion</h3>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label htmlFor="country" className="block mb-1">
                  Pais
                </label>

                <select
                  name="country"
                  value={country ? country.iso2 : ""}
                  onChange={(e) => {
                    const selectedCountry =
                      supportedCountries.find(
                        (item) => item.iso2 === e.target.value,
                      ) ?? null

                    setCountry(selectedCountry)
                    setState(null)
                    setCity(null)
                    setAddress("")
                  }}
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                >
                  <option value="">Seleccionar pais</option>
                  {supportedCountries.map((item) => (
                    <option key={item.iso2} value={item.iso2}>
                      {item.name}
                    </option>
                  ))}
                </select>
              </div>

              {country && (
                <div>
                  <label htmlFor="state" className="block mb-1">
                    Provincia
                  </label>

                  <StateSelect
                    name="state"
                    countryid={country.id}
                    defaultValue={state as SelectDefaultValue<State>}
                    onChange={(_state) => {
                      setState(_state as State)
                      setCity(null)
                      setAddress("")
                    }}
                    placeHolder="Seleccionar provincia"
                    containerClassName="modal-floating-select w-full"
                    inputClassName="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                  />
                </div>
              )}

              {country &&
                state &&
                state.name !== "Ciudad Autónoma de Buenos Aires" && (
                  <div>
                    <label htmlFor="city" className="block mb-1">
                      Ciudad
                    </label>

                    <CitySelect
                      name="city"
                      countryid={country.id}
                      stateid={state.id}
                      defaultValue={city as SelectDefaultValue<City>}
                      onChange={(_city) => {
                        setCity(_city as City)
                        setAddress("")
                      }}
                      placeHolder="Seleccionar ciudad"
                      containerClassName="modal-floating-select modal-floating-select-upward w-full"
                      inputClassName="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                    />
                  </div>
                )}

              {country &&
                state &&
                state.name === "Ciudad Autónoma de Buenos Aires" && (
                  <div>
                    <label htmlFor="city" className="block mb-1">
                      Ciudad
                    </label>

                    <input
                      name="city"
                      type="text"
                      value="Ciudad Autónoma de Buenos Aires"
                      readOnly
                      className="w-full border border-gray-300 rounded-lg px-3 py-2 bg-gray-50 focus:outline-none"
                    />
                  </div>
                )}

              {country &&
                state &&
                (city || state.name === "Ciudad Autónoma de Buenos Aires") && (
                  <div>
                    <label htmlFor="address" className="block mb-1">
                      Direccion
                    </label>
                    <input
                      name="address"
                      type="text"
                      value={address}
                      onChange={(e) => setAddress(e.target.value)}
                      placeholder="Direccion"
                      className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-orange-500"
                    />
                  </div>
                )}
            </div>
          </section>
        </div>
        {error && (
          <div className="w-full mt-4 bg-red-200 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-3">
            <p>Ocurrio un error al editar el negocio:</p>
            <p>{error}</p>
          </div>
        )}
      </ModalForm>
    </div>
  )
}
