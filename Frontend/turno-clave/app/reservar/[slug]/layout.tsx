import Image from "next/image"
import type { ReactNode } from "react"
import OrangeWavesBottom from "@/app/components/OrangeWavesBottom"
import { getReservationBusiness } from "./business"
import { ReservationFlowProvider } from "./ReservationFlowProvider"

type Props = {
  children: ReactNode
  params: Promise<{
    slug: string
  }>
}

export default async function RootLayout({ children, params }: Props) {
  const { slug } = await params
  const business = await getReservationBusiness(slug)

  return (
    <main className="antialiased min-h-screen flex flex-col">
      <div className="flex justify-center items-center h-16">
        <Image
          src="/header-logo-300x100.png"
          alt="Turno Clave Logo"
          width={150}
          height={150}
          className="mr-2"
        />
      </div>
      <div className="flex-1 max-w-xl mx-auto w-full">
        <ReservationFlowProvider business={business} slug={slug}>
          {children}
        </ReservationFlowProvider>
      </div>
      <OrangeWavesBottom />
    </main>
  )
}
