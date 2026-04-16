import { auth, type ExtendedSession } from "@/auth"
import { apiFetch } from "./apiClient"

export async function authenticatedFetch(
  path: string,
  options: RequestInit = {},
) {
  const session = (await auth()) as ExtendedSession

  if (!session?.backendToken) {
    throw new Error("UNAUTHORIZED")
  }

  const res = await apiFetch(path, session.backendToken, options)

  // Manejar 401 - Token expirado
  if (res.status === 401) {
    throw new Error("UNAUTHORIZED")
  }

  return res
}
