type JsonLike = Record<string, unknown> | unknown[] | string | number | boolean | null

async function readResponseBody(response: Response): Promise<JsonLike> {
  const contentType = response.headers.get("content-type") ?? ""

  if (contentType.includes("application/json")) {
    return response.json()
  }

  const text = await response.text()
  return text ? { message: text } : null
}

export async function toJsonProxyResponse(response: Response) {
  const body = await readResponseBody(response)

  return Response.json(body, { status: response.status })
}
