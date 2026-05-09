import { jwtDecode } from "jwt-decode"
import NextAuth, { type Session } from "next-auth"
import type { JWT } from "next-auth/jwt"
import Google from "next-auth/providers/google"

interface ExtendedJWT extends JWT {
  backendToken?: string
  backendRefreshToken?: string
  backendTokenExpiresAt?: number
  userId?: string
  authError?: "RefreshAccessTokenError"
}

type DecodedBackendToken = {
  userId: string
  exp: number
}

const inFlightRefreshes = new Map<string, Promise<ExtendedJWT>>()
const recentRefreshResults = new Map<
  string,
  { token: ExtendedJWT; expiresAt: number }
>()
const REFRESH_WINDOW_MS = 10_000
const RECENT_REFRESH_TTL_MS = 5_000

export interface ExtendedSession extends Session {
  backendToken?: string
  backendRefreshToken?: string
  userId?: string
  authError?: "RefreshAccessTokenError"
}

export const { handlers, signIn, signOut, auth } = NextAuth({
  providers: [
    Google({
      clientId: process.env.GOOGLE_ID,
      clientSecret: process.env.GOOGLE_SECRET,
    }),
  ],
  callbacks: {
    async jwt({ token, account }) {
      const extendedToken = token as ExtendedJWT

      if (account?.provider === "google" && account.id_token) {
        const response = await fetch(
          `${process.env.NEXTAUTH_URL}/api/auth/google`,
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ idToken: account.id_token }),
          },
        )

        const data = await response.json()

        const decodedToken = jwtDecode<DecodedBackendToken>(data.accessToken)

        return {
          ...extendedToken,
          backendToken: data.accessToken,
          backendRefreshToken: data.refreshToken,
          backendTokenExpiresAt: decodedToken.exp * 1000,
          userId: decodedToken.userId,
        }
      }

      if (
        extendedToken.backendToken &&
        extendedToken.backendTokenExpiresAt &&
        Date.now() < extendedToken.backendTokenExpiresAt - REFRESH_WINDOW_MS
      ) {
        return extendedToken
      }

      if (extendedToken.backendRefreshToken) {
        return refreshBackendToken(extendedToken)
      }

      return extendedToken
    },

    async session({ session, token }) {
      const extendedToken = token as ExtendedJWT
      const extendedSession = session as ExtendedSession

      extendedSession.backendToken = extendedToken.backendToken
      extendedSession.backendRefreshToken = extendedToken.backendRefreshToken
      extendedSession.userId = extendedToken.userId
      extendedSession.authError = extendedToken.authError

      return extendedSession
    },
  },
})

async function refreshBackendToken(token: ExtendedJWT): Promise<ExtendedJWT> {
  if (!token.backendRefreshToken) {
    return {
      ...token,
      backendToken: undefined,
      backendRefreshToken: undefined,
      backendTokenExpiresAt: undefined,
      authError: "RefreshAccessTokenError",
    }
  }

  const recentRefresh = recentRefreshResults.get(token.backendRefreshToken)

  if (recentRefresh && recentRefresh.expiresAt > Date.now()) {
    return recentRefresh.token
  }

  const existingRefresh = inFlightRefreshes.get(token.backendRefreshToken)

  if (existingRefresh) {
    return existingRefresh
  }

  const refreshPromise = (async (): Promise<ExtendedJWT> => {
    try {
      const response = await fetch(
        `${process.env.NEXTAUTH_URL}/api/auth/refresh`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            refreshToken: token.backendRefreshToken,
          }),
        },
      )

      if (!response.ok) {
        throw new Error("Failed to refresh backend token")
      }

      const data = await response.json()

      const decodedToken = jwtDecode<DecodedBackendToken>(data.accessToken)

      return {
        ...token,
        backendToken: data.accessToken,
        backendRefreshToken: data.refreshToken,
        backendTokenExpiresAt: decodedToken.exp * 1000,
        authError: undefined,
      }
    } catch {
      return {
        ...token,
        backendToken: undefined,
        backendRefreshToken: undefined,
        backendTokenExpiresAt: undefined,
        authError: "RefreshAccessTokenError",
      }
    }
  })()

  inFlightRefreshes.set(token.backendRefreshToken, refreshPromise)

  try {
    const refreshedToken = await refreshPromise

    if (!refreshedToken.authError) {
      recentRefreshResults.set(token.backendRefreshToken, {
        token: refreshedToken,
        expiresAt: Date.now() + RECENT_REFRESH_TTL_MS,
      })
    }

    return refreshedToken
  } finally {
    if (inFlightRefreshes.get(token.backendRefreshToken) === refreshPromise) {
      inFlightRefreshes.delete(token.backendRefreshToken)
    }
  }
}
