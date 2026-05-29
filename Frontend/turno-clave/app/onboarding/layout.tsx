import { Didact_Gothic, Geist_Mono } from "next/font/google"
import { redirect } from "next/navigation"
import { getSessionBusinesses } from "@/lib/auth/getSessionBusinesses"
import OrangeWavesBottom from "../components/OrangeWavesBottom"

const didactGothic = Didact_Gothic({
  weight: ["400"],
  subsets: ["latin"],
})

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
})

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  const { session, businesses, isVerified } = await getSessionBusinesses()

  if (!session) {
    redirect("/")
  }

  // Only enable onboarding for the testing account, so we can test the flow without having to create a new business every time. For other users, if they have businesses, redirect them to the dashboard.
  if (session.user?.email !== "doetesting02@gmail.com") {
    if (isVerified && businesses.length > 0) {
      redirect("/dashboard")
    }
  }

  return (
    <main
      className={`${didactGothic.className} ${geistMono.variable} antialiased min-h-screen flex flex-col`}
    >
      <div className="flex-1 pl-10 pt-10">{children}</div>
      <OrangeWavesBottom />
    </main>
  )
}
