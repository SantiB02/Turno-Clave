import Image from "next/image"
import type { BusinessDetail } from "@/types/business"

interface BusinessCardProps {
  business: BusinessDetail
}

export default function BusinessCard({ business }: BusinessCardProps) {
  return (
    <div className="bg-white rounded-xl shadow-md hover:shadow-lg transition-shadow border border-gray-200 p-6">
      {/* Header con logo y nombre */}
      <div className="flex items-center gap-4 mb-4">
        {business.logoUrl ? (
          <Image
            src={business.logoUrl}
            alt={`${business.name} logo`}
            width={56}
            height={56}
            className="rounded-full object-cover"
          />
        ) : (
          <div className="w-14 h-14 rounded-full bg-gradient-to-br from-primary-orange to-orange-600 flex items-center justify-center">
            <span className="text-white font-bold text-lg">
              {business.name.charAt(0).toUpperCase()}
            </span>
          </div>
        )}
        <div className="flex-1">
          <h3 className="text-xl font-bold text-gray-900">{business.name}</h3>
          <p className="text-sm text-gray-500">{business.slug}</p>
        </div>
      </div>

      {/* Descripción */}
      {business.description && (
        <p className="text-gray-700 text-sm mb-4 line-clamp-2">
          {business.description}
        </p>
      )}

      {/* Divider */}
      <div className="border-t border-gray-200 my-4"></div>

      {/* Información de contacto */}
      <div className="space-y-3 mb-4">
        <div className="flex items-center gap-3">
          <span className="text-primary-orange font-semibold min-w-fit">
            Correo:
          </span>
          <a
            href={`mailto:${business.email}`}
            className="text-blue-600 hover:underline text-sm break-all"
          >
            {business.email}
          </a>
        </div>
        {business.phone && (
          <div className="flex items-center gap-3">
            <span className="text-primary-orange font-semibold min-w-fit">
              Teléfono:
            </span>
            <a
              href={`tel:${business.phone}`}
              className="text-blue-600 hover:underline text-sm"
            >
              {business.phone}
            </a>
          </div>
        )}
      </div>

      {/* Divider */}
      <div className="border-t border-gray-200 my-4"></div>

      {/* Información de ubicación */}
      <div className="space-y-2">
        <div className="flex items-center gap-3">
          <span className="text-primary-orange font-semibold min-w-fit">
            Dirección:
          </span>
          <span className="text-gray-700 text-sm">{business.address}</span>
        </div>
        <div className="flex items-center gap-3">
          <span className="text-primary-orange font-semibold min-w-fit">
            Ubicación:
          </span>
          <span className="text-gray-700 text-sm">
            {business.city}, {business.state}, {business.country}
          </span>
        </div>
      </div>
    </div>
  )
}
