"use client"

import { useState } from "react"
import {
  CitySelect,
  CountrySelect,
  PhonecodeSelect,
  StateSelect,
} from "react-country-state-city"
import type {
  City,
  Country,
  State,
} from "react-country-state-city/dist/esm/types"
import "react-country-state-city/dist/react-country-state-city.css"
import NextStepButton from "../NextStepButton"

/* type CountrySelectDefaultValue =
  | ((string | number | readonly string[]) & Country)
  | undefined */

export default function OnboardingBusinessForm() {
  /* const [defaultCountry, setDefaultCountry] = useState<Country | undefined>(
    undefined,
  ) */

  const [name, setName] = useState<string>("")
  const [email, setEmail] = useState<string>("")
  const [phonecode, setPhonecode] = useState<Country | null>(null)
  const [phone, setPhone] = useState<string>("")
  const [country, setCountry] = useState<Country | null>(null)
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

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError(null)

    try {
      if (!name || name.trim() === "") {
        setError("El nombre del negocio es obligatorio.")
        return
      }
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

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  const phoneRegex = /^[0-9]+$/

  const isValidEmail = emailRegex.test(email)
  const isValidPhone = phoneRegex.test(phone)

  const isSubmitDisabled =
    name.trim().length < 3 ||
    !country ||
    !state ||
    !city ||
    address.trim().length < 5 ||
    phone.trim().length < 5 ||
    email.trim().length < 5 ||
    !isValidEmail ||
    !isValidPhone ||
    error !== null ||
    !phonecode

  return (
    <div className="mb-30">
      <form onSubmit={handleSubmit}>
        <h2 className="text-2xl text-center mb-4">
          ¿Cómo se llama tu negocio?
        </h2>
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
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
            <input
              maxLength={100}
              type="text"
              name="email"
              placeholder="Correo electrónico"
              className="border rounded-2xl border-primary-orange mb-4 focus:outline-primary-orange focus:ring-primary-orange p-2 w-64"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <div className="flex">
              <PhonecodeSelect
                containerClassName=" w-30 mr-4"
                required
                placeholder="+1"
                onChange={(_phonecode) => setPhonecode(_phonecode as Country)}
              />
              <input
                maxLength={15}
                type="text"
                name="phone"
                placeholder="Número de teléfono"
                className="border rounded-2xl border-primary-orange focus:outline-primary-orange focus:ring-primary-orange p-2 w-64"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                required
              />
            </div>
          </div>

          <h2 className="text-2xl mt-6 mb-4">¿Dónde está ubicado?</h2>
          <div className=" gap-4 flex flex-col items-stretch justify-center">
            <div className="flex items-stretch">
              <p className="text-xl bg-dark-blue text-white py-2 px-6 rounded-l-3xl flex items-center justify-center w-40 whitespace-nowrap flex-shrink-0 overflow-hidden">
                País
              </p>
              <CountrySelect
                required
                containerClassName="country-select border-dark-blue border rounded-r-3xl w-64"
                onChange={(_country) => setCountry(_country as Country)}
                placeHolder="Seleccionar país"
              />
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
            {country && state && (
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
            {country && state && city && (
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
            classname="mt-6"
          />
        </div>
      </form>
    </div>
  )
}
