"use client"

import { useRouter, useSearchParams } from "next/navigation"
import { useEffect, useState } from "react"
import ErrorMessage from "@/app/components/ErrorMessage"
import { getProfessionalsByActiveBusiness } from "@/services/professionalService"
import type { BusinessDetail } from "@/types/business"
import type { Professional } from "@/types/professional"
import type { Service } from "@/types/service"
import HorariosTab from "./HorariosTab"
import InformacionTab from "./InformacionTab"
import MiLink from "./MiLink"
import ProfesionalesTab from "./ProfesionalesTab"

const tabs = [
  { id: "informacion", label: "Información" },
  { id: "horarios", label: "Horarios" },
  { id: "profesionales", label: "Profesionales" },
  { id: "mi-link", label: "Mi Link" },
] as const

type MiNegocioTabsProps = {
  business?: BusinessDetail | null
  services: Service[]
  businessError: string | null
  servicesError: string | null
}

export default function MiNegocioTabs({
  business,
  services,
  businessError,
  servicesError,
}: MiNegocioTabsProps) {
  const router = useRouter()
  const searchParams = useSearchParams()

  const [professionals, setProfessionals] = useState<Professional[]>([])
  const [loadingProfessionals, setLoadingProfessionals] =
    useState<boolean>(true)
  const [professionalsError, setProfessionalsError] = useState<string | null>(
    null,
  )

  const activeTab = searchParams.get("tab") || "informacion"

  useEffect(() => {
    async function loadProfessionals() {
      const result = await getProfessionalsByActiveBusiness()

      if (!result.ok) {
        setProfessionalsError(result.message)
        setLoadingProfessionals(false)
        return
      }

      const data = result.data

      data.sort((a, b) => {
        if (a.name < b.name) {
          return -1
        }

        if (a.name > b.name) {
          return 1
        }

        return 0
      })

      setProfessionals(data)
      setLoadingProfessionals(false)
    }

    loadProfessionals()
  }, [])

  function changeTab(tab: string) {
    const params = new URLSearchParams(searchParams.toString())

    params.set("tab", tab)

    if (tab !== "horarios") {
      params.delete("professional")
    }

    router.replace(`?${params.toString()}`)
  }

  const isBusinessLoading = !business && !businessError

  return (
    <div>
      <div className="border-t border-gray-300" />

      <div className="flex gap-8 border-b border-gray-300 px-2">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            type="button"
            onClick={() => changeTab(tab.id)}
            className={`
              py-4 cursor-pointer text-sm md:text-lg  border-b-2 transition-colors
              ${
                activeTab === tab.id
                  ? "border-primary-orange text-primary-orange font-bold"
                  : "border-transparent text-gray-500 hover:text-primary-orange"
              }
            `}
          >
            {tab.label}
          </button>
        ))}
      </div>

      <div className="py-6">
        {activeTab === "informacion" && (
          <>
            {businessError && (
              <ErrorMessage title="Error:" message={businessError} />
            )}

            {isBusinessLoading && <div>Cargando información de negocio...</div>}

            {business && !businessError && (
              <InformacionTab business={business} />
            )}
          </>
        )}

        {activeTab === "horarios" && (
          <>
            {businessError && (
              <div className="text-red-600">{businessError}</div>
            )}

            {isBusinessLoading && <div>Cargando información de negocio...</div>}

            {business && !businessError && (
              <HorariosTab
                business={business}
                professionals={professionals}
                setProfessionals={setProfessionals}
                professionalsError={professionalsError}
              />
            )}
          </>
        )}

        {activeTab === "profesionales" && (
          <ProfesionalesTab
            professionals={professionals}
            setProfessionals={setProfessionals}
            businessServices={services}
            loadingProfessionals={loadingProfessionals}
            professionalsError={professionalsError}
            servicesError={servicesError}
          />
        )}

        {activeTab === "mi-link" && business && !businessError && (
          <MiLink
            business={business}
            professionals={professionals}
            loadingProfessionals={loadingProfessionals}
            professionalsError={professionalsError}
            services={services}
          />
        )}
      </div>
    </div>
  )
}
