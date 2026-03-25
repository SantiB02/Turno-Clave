import type { Metadata } from "next"
import { Didact_Gothic, Geist, Geist_Mono } from "next/font/google"
import "./globals.css"
import Footer from "./components/Footer"

const didactGothic = Didact_Gothic({
  weight: ["400"],
  subsets: ["latin"],
})

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
})

export const metadata: Metadata = {
  title: "Turno Clave - Agenda de Turnos para Negocios",
  description: "Ordena tu tiempo, potencia tu negocio.",
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html lang="en">
      <body
        className={`${didactGothic.className} ${geistMono.variable} antialiased`}
      >
        <main className="min-h-screen bg-white">{children}</main>
        <Footer />
      </body>
    </html>
  )
}
