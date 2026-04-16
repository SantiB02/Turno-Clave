import { auth, type ExtendedSession } from "@/auth"
import { createBusiness } from "@/services/businessService"

export async function POST(req: Request) {
  const session = (await auth()) as ExtendedSession
  const body = await req.json()

  if (!session?.backendToken) {
    return new Response("Unauthorized", { status: 401 })
  }

  try {
    const data = await createBusiness(body, session.backendToken)
    return Response.json(data)
  } catch (err: any) {
    if (err.message.includes("401")) {
      return new Response("Unauthorized", { status: 401 })
    }

    return new Response(err.message || "Error", { status: 500 })
  }
}
