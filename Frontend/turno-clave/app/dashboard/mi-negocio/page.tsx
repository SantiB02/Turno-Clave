import Image from "next/image"
import { redirect } from "next/navigation"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"

export default async function MiNegocio() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  const businesses = await getMyBusinesses()
  console.log(businesses)

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mis Negocios</h1>
      <ul className="space-y-4">
        {businesses.map((business) => (
          <li key={business.externalId}>
            <div className="flex flex-col  justify-between">
              <div className="flex items-center gap-2">
                {business.logoUrl ? (
                  <Image
                    src={business.logoUrl}
                    alt={`${business.name} logo`}
                    className="w-12 h-12 rounded-full object-cover"
                  />
                ) : (
                  <div className="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center">
                    <span className="text-gray-600 font-semibold">
                      {business.name.charAt(0).toUpperCase()}
                    </span>
                  </div>
                )}
                <span className="text-lg font-medium">{business.name}</span>
              </div>
              <p>
                <span className="font-bold text-primary-orange">
                  Descripción:
                </span>{" "}
                {business.description}
              </p>
              <p>
                <span className="font-bold text-primary-orange">
                  Correo electrónico:
                </span>{" "}
                {business.email}
              </p>
              <p>
                <span className="font-bold text-primary-orange">Teléfono:</span>{" "}
                {business.phone}
              </p>
              <p>
                <span className="font-bold text-primary-orange">
                  Dirección:
                </span>{" "}
                {business.address}
              </p>
              <p>
                <span className="font-bold text-primary-orange">Ciudad:</span>{" "}
                {business.city}
              </p>
              <p>
                <span className="font-bold text-primary-orange">Estado:</span>{" "}
                {business.state}
              </p>
              <p>
                <span className="font-bold text-primary-orange">País:</span>{" "}
                {business.country}
              </p>
            </div>
          </li>
        ))}
      </ul>
    </div>
  )
}
