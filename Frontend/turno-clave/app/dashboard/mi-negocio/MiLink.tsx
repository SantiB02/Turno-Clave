"use client"

import { ExclamationTriangleIcon } from "@heroicons/react/20/solid"
import { ShareIcon } from "@heroicons/react/24/solid"
import Link from "next/link"
import { useState } from "react"
import ErrorMessage from "@/app/components/ErrorMessage"
import ModalForm from "@/app/components/ModalForm"
import Switch from "@/app/components/Switch"
import { updateBusinessPublicLinkStatus } from "@/services/businessService"
import type { BusinessDetail } from "@/types/business"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"

type MiLinkProps = {
  business: BusinessDetail
  professionals: Professional[]
  loadingProfessionals: boolean
  professionalsError: string | null
  services: Service[]
}

export default function MiLink({
  business,
  professionals,
  loadingProfessionals,
  professionalsError,
  services,
}: MiLinkProps) {
  const [isBusinessActive, setIsBusinessActive] = useState(
    business.isPublicLinkEnabled,
  )
  const [loading, setLoading] = useState(false)
  const [openLinkModal, setOpenLinkModal] = useState(false)
  const [isLinkCopied, setIsLinkCopied] = useState(false)
  const [isLinkCopiedFromContainer, setIsLinkCopiedFromContainer] =
    useState(false)
  const [error, setError] = useState<string | null>(null)

  const noProfessionals =
    !loadingProfessionals && !professionalsError && professionals.length === 0
  const noServices = services.length === 0
  const noPaymentMethods = business.paymentMethods.length === 0

  const hasRequirementsErrors =
    noProfessionals || noServices || noPaymentMethods

  const isLinkDisabled = !isBusinessActive || hasRequirementsErrors

  const baseUrl = process.env.NEXT_PUBLIC_BASE_URL?.replace(/\/$/, "")

  const publicLink = `${baseUrl}/reservar/${business.slug}`

  const handleContainerCopy = async () => {
    if (isLinkDisabled) return

    try {
      await navigator.clipboard.writeText(publicLink)

      setIsLinkCopiedFromContainer(true)

      setTimeout(() => {
        setIsLinkCopiedFromContainer(false)
      }, 3000)
    } catch {
      console.error("No se pudo copiar el link")
    }
  }

  const handleCopy = async () => {
    if (isLinkDisabled) return

    try {
      await navigator.clipboard.writeText(publicLink)
      setIsLinkCopied(true)
      setTimeout(() => setIsLinkCopied(false), 3000)
    } catch {
      console.error("No se pudo copiar el link")
    }
  }

  const handleShare = async () => {
    if (isLinkDisabled) return

    try {
      await navigator.share({
        title: business.name,
        text: "Reservá tu turno acá",
        url: publicLink,
      })
    } catch {
      console.error("No se pudo compartir")
    }
  }

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    try {
      e.preventDefault()
      setLoading(true)
      setError(null)

      const payload = {
        isPublicLinkEnabled: !isBusinessActive,
      }

      const result = await updateBusinessPublicLinkStatus(
        business.externalId,
        payload,
      )

      if (!result.ok) {
        setError(result.message)
        return
      }

      setIsBusinessActive(!isBusinessActive)
    } finally {
      setOpenLinkModal(false)
      setLoading(false)
    }
  }

  return (
    <div>
      <div>
        <h2 className="mb-3 text-xl text-dark-blue font-bold">Estado</h2>
        {error && (
          <div className="mb-4">
            <ErrorMessage
              title="Error al actualizar el estado del link:"
              message={error}
            />
          </div>
        )}
        <div className="flex gap-2">
          <button
            type="button"
            onClick={() => {
              if (!hasRequirementsErrors) {
                setOpenLinkModal(true)
              }
            }}
            disabled={hasRequirementsErrors}
            className={`flex items-center shadow-sm unselectable mb-3 border max-w-max rounded-2xl pl-2 pr-4 py-1 transition ${isBusinessActive ? "border-primary-orange" : hasRequirementsErrors ? "border-gray-300" : "border-gray-500"} ${!hasRequirementsErrors && "cursor-pointer hover:bg-gray-50"}`}
          >
            <Switch
              checked={isBusinessActive}
              onChange={() => {}}
              disabled={hasRequirementsErrors}
            />
            <p
              className={`ml-2 ${isBusinessActive ? "text-primary-orange font-bold" : hasRequirementsErrors ? "text-gray-300" : "text-black"}`}
            >
              {isBusinessActive ? "Activo" : "Inactivo"}
            </p>
          </button>
          <p className="mt-1.5 text-gray-600">
            {isLinkDisabled
              ? "Tus clientes no pueden acceder a tu link ni reservar turnos."
              : "Tus clientes pueden acceder a tu link y reservar turnos."}
          </p>
        </div>
        {(noProfessionals || noServices || noPaymentMethods) && (
          <div className="border-2 border-yellow-400 max-w-max pr-16 pl-4 py-3 rounded-2xl">
            <p className="font-bold">
              Completa los requisitos para activar tu link:
            </p>

            <ul className="flex flex-col gap-1">
              {noProfessionals && (
                <li className="flex items-center">
                  <ExclamationTriangleIcon className="h-6 w-6 text-yellow-400 mr-1" />
                  Al menos 1 profesional creado
                  <Link
                    className="ml-2 flex items-center text-primary-orange underline"
                    href="/dashboard/mi-negocio?tab=profesionales"
                  >
                    Ir →
                  </Link>
                </li>
              )}

              {noServices && (
                <li className="flex items-center">
                  <ExclamationTriangleIcon className="h-6 w-6 text-yellow-400 mr-1" />
                  Al menos 1 servicio creado
                  <Link
                    className="ml-2 flex items-center text-primary-orange underline"
                    href="/dashboard/mis-servicios"
                  >
                    Ir →
                  </Link>
                </li>
              )}

              {noPaymentMethods && (
                <li className="flex items-center">
                  <ExclamationTriangleIcon className="h-6 w-6 text-yellow-400 mr-1" />
                  Método de pago configurado{" "}
                  <Link
                    className="ml-2 flex items-center text-primary-orange underline"
                    href="/dashboard/mi-negocio?tab=informacion"
                  >
                    Ir →
                  </Link>
                </li>
              )}
            </ul>
          </div>
        )}
      </div>

      {/* Divider */}
      <div className="border-t border-gray-300 my-4"></div>

      <div className="unselectable">
        <h2
          className={`text-xl text-dark-blue font-bold mb-2 ${isLinkDisabled ? "text-gray-400" : ""}`}
        >
          Link
        </h2>

        <p className={`${isLinkDisabled ? "text-gray-400" : ""}`}>
          🔗 Link de reserva de turnos para compartir a tus clientes
        </p>

        <div className="flex items-center gap-3">
          <button
            type="button"
            onClick={handleContainerCopy}
            disabled={isLinkDisabled}
            className={`
              border
              rounded-full
              max-w-md
              px-4
              py-2
              mt-3
              inline-block
              transition

              ${
                isLinkDisabled
                  ? "border-gray-300 bg-gray-100"
                  : "cursor-pointer shadow-sm border-primary-orange bg-orange-100 hover:underline decoration-primary-orange"
              }
            `}
          >
            <p
              className={`truncate ${
                isLinkDisabled ? "text-gray-400" : "text-primary-orange"
              }`}
              title={publicLink}
            >
              {isLinkDisabled ? "No disponible" : publicLink}
            </p>
          </button>
          {isLinkCopiedFromContainer && (
            <span className="text-md text-primary-orange mt-2">¡Copiado!</span>
          )}
        </div>
        <div className="flex gap-3 my-4">
          <button
            type="button"
            onClick={handleCopy}
            disabled={isLinkDisabled}
            className={`
              border
              rounded-lg
              transition
              py-1
              px-3

              ${
                isLinkDisabled
                  ? "border-gray-300 text-gray-400 bg-gray-100"
                  : "cursor-pointer shadow-sm border-primary-orange text-primary-orange hover:bg-primary-orange hover:text-white"
              }
            `}
          >
            {isLinkCopied ? "¡Copiado!" : "Copiar link"}
          </button>
          <button
            type="button"
            onClick={handleShare}
            className={`
              flex
              items-center
              gap-2
              border
              rounded-lg
              transition
              py-1
              px-3

              ${
                isLinkDisabled
                  ? "border-gray-300 text-gray-400 bg-gray-100"
                  : "cursor-pointer shadow-sm border-primary-orange text-primary-orange hover:bg-primary-orange hover:text-white"
              }
            `}
          >
            <ShareIcon className="h-4 w-4" />
            Compartir
          </button>
          {isLinkDisabled ? (
            <span className="border border-gray-300 unselectable text-gray-400 bg-gray-100 rounded-lg py-1 px-3">
              Acceder como cliente
            </span>
          ) : (
            <Link
              href={publicLink}
              target="_blank"
              className="cursor-pointer shadow-sm border border-primary-orange text-primary-orange rounded-lg transition py-1 px-3 hover:bg-primary-orange hover:text-white"
            >
              Acceder como cliente
            </Link>
          )}
        </div>
      </div>

      <ModalForm
        open={openLinkModal}
        onSubmit={(e) => handleSubmit(e)}
        onClose={() => setOpenLinkModal(false)}
        title={isBusinessActive ? "Desactivar link" : "Activar link"}
        submitLabel={isBusinessActive ? "Desactivar link" : "Activar link"}
        loading={loading}
        loadingLabel={
          isBusinessActive ? "Desactivando link..." : "Activando link..."
        }
        submitButtonBgColor={isBusinessActive ? "bg-red-600" : undefined}
        submitButtonBgHoverColor={
          isBusinessActive ? "hover:bg-red-700" : undefined
        }
      >
        {isBusinessActive ? (
          <p>
            Los clientes ya no podrán acceder a tu página ni reservar nuevos
            turnos.
          </p>
        ) : (
          <p>
            Tu link público quedará disponible para clientes y podrán reservar
            turnos.
          </p>
        )}
      </ModalForm>
    </div>
  )
}
