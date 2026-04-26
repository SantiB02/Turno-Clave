import type { Country } from "react-country-state-city/dist/esm/types"

export type SupportedCountry = Country & {
  timezone: string
}

export const supportedCountries: SupportedCountry[] = [
  {
    id: 11,
    name: "Argentina",
    iso3: "ARG",
    iso2: "AR",
    numeric_code: "032",
    phone_code: "54",
    capital: "Buenos Aires",
    currency: "ARS",
    currency_name: "Argentine peso",
    currency_symbol: "$",
    tld: ".ar",
    native: "Argentina",
    region: "Americas",
    subregion: "South America",
    latitude: "-34.00000000",
    longitude: "-64.00000000",
    emoji: "🇦🇷",
    emojiU: "U+1F1E6 U+1F1F7",
    hasStates: true,
    timezone: "America/Argentina/Buenos_Aires",
  },
]
