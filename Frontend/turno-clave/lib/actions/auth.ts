"use server"

import { auth, signOut, type ExtendedSession } from "@/auth"

export async function handleSignOut() {
  const session = (await auth()) as ExtendedSession

  if (session?.backendRefreshToken) {
    try {
      await fetch(`${process.env.NEXTAUTH_URL}/api/auth/revoke`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          refreshToken: session.backendRefreshToken,
        }),
      })
    } catch (error) {
      console.error("Failed to revoke backend refresh token during signout", error)
    }
  }

  await signOut({ redirectTo: "/" })
}
