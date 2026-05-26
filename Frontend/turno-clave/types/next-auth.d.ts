import type { DefaultSession } from "next-auth"
import type { MinimalBusiness } from "@/types/business"

declare module "next-auth" {
  interface Session {
    backendToken?: string
    backendRefreshToken?: string
    businesses?: MinimalBusiness[]
    userId?: string
    authError?: "RefreshAccessTokenError"
    user?: DefaultSession["user"]
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    backendToken?: string
    backendRefreshToken?: string
    backendTokenExpiresAt?: number
    businesses?: MinimalBusiness[]
    userId?: string
    authError?: "RefreshAccessTokenError"
  }
}
