import { auth, type ExtendedSession, signOut } from "@/auth"
import { apiFetch } from "./apiClient"

export async function authenticatedFetch(
  path: string,
  options: RequestInit = {},
) {
  const session = (await auth()) as ExtendedSession

  if (session?.authError || !session?.backendToken) {
    signOut()
  }

  const res = await apiFetch(path, session.backendToken, options)

  // Expired or invalid token
  if (res.status === 401) {
    throw new Error("UNAUTHORIZED")
  }

  return res
}
