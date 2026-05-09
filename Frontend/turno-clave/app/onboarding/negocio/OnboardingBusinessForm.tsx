"use client"

import { useEffect, useState } from "react"
import { CitySelect, StateSelect } from "react-country-state-city"
import type { City, State } from "react-country-state-city/dist/esm/types"
import "react-country-state-city/dist/react-country-state-city.css"
import { useRouter } from "next/navigation"
import {
  type SupportedCountry,
  supportedCountries,
} from "@/data/supportedCountries"
import type { CreateBusinessDTO } from "@/types/business"
import NextStepButton from "../NextStepButton"

/* type CountrySelectDefaultValue =
  | ((string | number | readonly string[]) & Country)
  | undefined */

export default function OnboardingBusinessForm() {
  /* const [defaultCountry, setDefaultCountry] = useState<Country | undefined>(
    undefined,
  ) */
  const router = useRouter()

  const [name, setName] = useState<string>("")
  const [email, setEmail] = useState<string>("")
  const [phonecode, setPhonecode] = useState<SupportedCountry | null>(null)
  const [phone, setPhone] = useState<string>("")
  const [country, setCountry] = useState<SupportedCountry | null>(null)
  const [state, setState] = useState<State | null>(null)
  const [city, setCity] = useState<City | null>(null)
  const [address, setAddress] = useState<string>("")
  const [error, setError] = useState<string | null>(null)

  /* useEffect(() => {
    GetCountries().then((countries) => {
      const argentina = countries.find((c) => c.name === "Argentina")
      setDefaultCountry(argentina)
      })
      }, []) */

  useEffect(() => {
    const storedData = localStorage.getItem("onboardingData")
    if (storedData) {
      // remove the code from the phone (first 3 characters) from the stored phone
      const parsed = JSON.parse(storedData)

      const fullPhone = parsed.phone || ""

      // buscar el país cuyo código matchee el prefijo
      const selectedCountry = supportedCountries.find((c) =>
        fullPhone.startsWith(`+${c.phone_code}`),
      )

      setPhonecode(selectedCountry || null)

      // sacar el código correctamente (no con slice(3))
      if (selectedCountry) {
        const phoneWithoutCode = fullPhone.replace(
          `+${selectedCountry.phone_code}`,
          "",
        )
        setPhone(phoneWithoutCode)
      }

      setName(parsed.name || "")
      setEmail(parsed.email || "")
    }
  }, [])

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  const phoneRegex = /^[0-9]+$/

  const isValidEmail = emailRegex.test(email)
  const isValidPhone = phoneRegex.test(phone)

  // TODO: Considerar válido que no haya ciudad si el estado es "Ciudad Autónoma de Buenos Aires", ya que es una ciudad-estado y no tiene ciudades dentro de ella.
  const isSubmitDisabled =
    name.trim().length < 3 ||
    !country ||
    !state ||
    (!city && state.name !== "Ciudad Autónoma de Buenos Aires") ||
    address.trim().length < 5 ||
    phone.trim().length < 5 ||
    email.trim().length < 5 ||
    !isValidEmail ||
    !isValidPhone ||
    error !== null ||
    !phonecode

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError(null)

    try {
      if (isSubmitDisabled) {
        setError("Por favor, complete todos los campos.")
        return
      }

      const phoneWithCode = `+${phonecode.phone_code}${phone}`

      localStorage.setItem("onboardingData", JSON.stringify({})) // Clear previous data to avoid confusion in case of errors

      if (!city && state.name !== "Ciudad Autónoma de Buenos Aires") {
        setError("Ciudad requerida")
        return
      }

      // To avoid possible null value in city for onboardingData
      if (!city) {
        setError("Ciudad requerida")
        return
      }

      const onboardingData: CreateBusinessDTO = {
        name,
        email,
        phone: phoneWithCode,
        country: country.name,
        state: state.name,
        city:
          state.name === "Ciudad Autónoma de Buenos Aires"
            ? "Ciudad Autónoma de Buenos Aires"
            : city.name,
        address,
        timeZone: country.timezone,
        availabilities: [], // This will be filled in the next step of the onboarding process
      }

      localStorage.setItem("onboardingData", JSON.stringify(onboardingData))
      router.push("/onboarding/horarios")
    } catch (error) {
      setError("Ocurrió un error al enviar el formulario.")
      console.error(
        "[OnboardingBusinessForm] Error:",
        error instanceof Error ? error.message : error,
      )
    }
  }

  const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value)
  }

  const handlePhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    // numbers only
    const numeric = e.target.value.replace(/\D/g, "")
    const cleaned = numeric.replace(/^0+/, "")

    if (cleaned.length > 15) return

    setPhone(cleaned)
  }

  return (
    <div className="mb-30">
      <form onSubmit={handleSubmit}>
        <h2 className="text-2xl text-center mb-4">
          ¿Cómo se llama tu negocio?
        </h2>
        {error && (
          <div className="bg-red-100 mb-4 border border-red-400 text-red-700 px-4 py-3 rounded">
            <p className="font-bold">Error:</p>
            <p>{error}</p>
          </div>
        )}
        <div className="text-center">
          <input
            maxLength={100}
            type="text"
            name="name"
            placeholder="Nombre"
            className="border rounded-2xl border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-2 w-64"
            value={name}
            onChange={handleNameChange}
            required
          />

          <div className="flex flex-col items-center justify-center mt-6">
            <h2 className="text-2xl text-center mb-4">
              ¿Cuál es su información de contacto?
            </h2>
            <div className="w-64">
              <input
                maxLength={100}
                type="text"
                name="email"
                placeholder="Correo electrónico"
                className="border rounded-2xl border-primary-orange mb-4 focus:outline-primary-orange focus:ring-primary-orange p-2 w-full"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
              <div className="flex">
                {/* <PhonecodeSelect
                src=""
                  containerClassName=" w-30 mr-4"
                  required
                  placeholder="+1"
                  onChange={(_phonecode) =>
                    setPhonecode(_phonecode as SupportedCountry)
                  }
                /> */}
                <select
                  className="border first rounded-2xl mr-2  border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-2 w-24"
                  value={phonecode ? phonecode.iso2 : ""}
                  onChange={(e) => {
                    const selectedPhonecode = supportedCountries.find(
                      (c) => c.iso2 === e.target.value,
                    )
                    setPhonecode(selectedPhonecode || null)
                  }}
                >
                  <option value="">Código</option>
                  {supportedCountries.map((c) => (
                    <option key={c.iso2} value={c.iso2}>
                      +{c.phone_code}
                    </option>
                  ))}
                </select>
                <input
                  maxLength={15}
                  type="text"
                  name="phone"
                  placeholder="Número de teléfono"
                  className="border rounded-2xl border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-2 w-full"
                  value={phone}
                  onChange={handlePhoneChange}
                  required
                />
              </div>
            </div>
          </div>

          <h2 className="text-2xl mt-6 mb-4">¿Dónde está ubicado?</h2>
          <div className=" gap-4 flex flex-col items-stretch justify-center">
            <div className="flex items-stretch">
              <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                País
              </p>
              {/* <CountrySelect
                src="/"
                required
                containerClassName="country-select border-dark-blue border rounded-r-3xl w-64"
                onChange={(_country) => setCountry(_country as Country)}
                placeHolder="Seleccionar país"
              /> */}
              <select
                className="border rounded-r-3xl border-dark-blue focus:outline-dark-blue focus:ring-dark-blue p-2 w-64"
                value={country ? country.iso2 : ""}
                onChange={(e) => {
                  const selectedCountry =
                    supportedCountries.find((c) => c.iso2 === e.target.value) ||
                    null
                  setCountry(selectedCountry)
                  setState(null)
                  setCity(null)
                }}
              >
                <option value="">Seleccionar país</option>
                {supportedCountries.map((c) => (
                  <option key={c.iso2} value={c.iso2}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>
            {country && (
              <div className="flex items-stretch">
                <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                  Provincia
                </p>
                <StateSelect
                  required
                  containerClassName="state-select border-dark-blue border rounded-r-3xl w-64"
                  countryid={country.id}
                  onChange={(_state) => {
                    setState(_state as State)
                    setCity(null)
                  }}
                  placeHolder="Seleccionar provincia"
                />
              </div>
            )}
            {country &&
              state &&
              state.name !== "Ciudad Autónoma de Buenos Aires" && (
                <div className="flex items-stretch">
                  <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                    Ciudad
                  </p>
                  <CitySelect
                    required
                    containerClassName="city-select border-dark-blue border rounded-r-3xl w-64"
                    countryid={country.id}
                    stateid={state.id}
                    onChange={(_city) => setCity(_city as City)}
                    placeHolder="Seleccionar ciudad"
                  />
                </div>
              )}
            {country &&
              state &&
              state.name === "Ciudad Autónoma de Buenos Aires" && (
                <div className="flex items-stretch">
                  <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                    Ciudad
                  </p>
                  <input
                    type="text"
                    name="city"
                    placeholder="Ciudad"
                    className="border rounded-r-3xl border-dark-blue focus:outline-dark-blue focus:ring-dark-blue p-2 w-64"
                    value="Ciudad Autónoma de Buenos Aires"
                    readOnly
                  />
                </div>
              )}

            {country &&
              state &&
              (city || state.name === "Ciudad Autónoma de Buenos Aires") && (
                <div className="flex items-stretch">
                  <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                    Dirección
                  </p>
                  <input
                    maxLength={100}
                    type="text"
                    name="address"
                    placeholder="Dirección"
                    className="border rounded-r-3xl border-dark-blue focus:outline-dark-blue focus:ring-dark-blue p-2 w-64"
                    value={address}
                    onChange={(e) => setAddress(e.target.value)}
                    required
                  />
                </div>
              )}
          </div>
          <NextStepButton
            disabled={isSubmitDisabled}
            type="submit"
            className="mt-6"
          />
        </div>
      </form>
    </div>
  )
}
