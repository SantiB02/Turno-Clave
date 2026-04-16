import { redirect } from "next/navigation"
import BusinessCard from "@/app/components/BusinessCard"
import { auth } from "@/auth"
import { getMyBusinesses } from "@/services/businessService"

export default async function MisNegocios() {
  const session = await auth()

  if (!session) {
    redirect("/")
  }

  const businesses = await getMyBusinesses()

  return (
    <div>
      <h1 className="font-bold text-4xl mb-9">Mis Negocios</h1>
      {businesses.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg">
            No tienes negocios registrados aún
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {businesses.map((business) => (
            <BusinessCard key={business.externalId} business={business} />
          ))}
        </div>
      )}
    </div>
  )
}
