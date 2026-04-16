import { jwtDecode } from "jwt-decode"
import NextAuth, { type Session } from "next-auth"
import type { JWT } from "next-auth/jwt"
import Google from "next-auth/providers/google"

interface ExtendedJWT extends JWT {
  backendToken?: string
  backendTokenExpires?: number
  userId?: string
}

export interface ExtendedSession extends Session {
  backendToken?: string
  userId?: string
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
      // Primera vez: usuario se autentica con Google
      if (account?.provider === "google" && account.id_token) {
        try {
          const response = await fetch(
            `${process.env.NEXTAUTH_URL}/api/auth/google`,
            {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify({
                idToken: account.id_token,
              }),
            },
          )

          if (!response.ok) {
            const error = await response.json()
            console.error("Backend auth failed:", error)
            throw new Error(error.message || "Backend authentication failed")
          }

          const data = await response.json()

          token.backendToken = data.token
          const decodedToken: any = jwtDecode(data.token)
          token.userId = decodedToken.userId
        } catch (error) {
          console.error("Error validating Google token:", error)
          throw error
        }
      }

      return token
    },

    async session({ session, token }) {
      const extendedToken = token as ExtendedJWT
      const extendedSession = session as ExtendedSession

      if (extendedToken.backendToken) {
        extendedSession.backendToken = extendedToken.backendToken
        extendedSession.userId = extendedToken.userId
      }

      return extendedSession
    },
  },
})
