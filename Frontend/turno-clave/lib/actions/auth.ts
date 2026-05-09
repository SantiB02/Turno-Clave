"use server"

import { auth, type ExtendedSession, signOut } from "@/auth"
import { throwResponseError } from "@/lib/api/error-utils"

async function revokeBackendRefreshToken(refreshToken: string) {
  const response = await fetch(`${process.env.NEXTAUTH_URL}/api/auth/revoke`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      refreshToken,
    }),
  })

  if (!response.ok) {
    await throwResponseError(
      response,
      "Failed to revoke backend refresh token during signout",
    )
  }
}

export async function handleSignOut() {
  const session = (await auth()) as ExtendedSession

  if (session?.backendRefreshToken) {
    try {
      await revokeBackendRefreshToken(session.backendRefreshToken)
    } catch (error) {
      console.error(
        "Failed to revoke backend refresh token during signout",
        error,
      )
    }
  }

  await signOut({ redirectTo: "/" })
}
