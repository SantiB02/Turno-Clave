import type { NextRequest } from "next/server"
import { toJsonProxyResponse } from "@/lib/api/proxy-response"

export async function POST(req: NextRequest) {
  const body = await req.json()

  try {
    const res = await fetch(`${process.env.API_URL}/api/appointments/public`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(body),
    })

    return toJsonProxyResponse(res)
  } catch (error) {
    console.error("[POST /api/appointments/public]", error)
    return Response.json(
      { message: "Failed to create appointment" },
      { status: 500 },
    )
  }
}
