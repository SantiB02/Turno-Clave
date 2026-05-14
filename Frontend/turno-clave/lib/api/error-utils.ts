export function isUnauthorizedError(error: unknown) {
  return error instanceof Error && error.message === "UNAUTHORIZED"
}

export async function throwResponseError(
  response: Response,
  fallbackMessage: string,
): Promise<never> {
  let data: unknown = null

  try {
    data = await response.json()
  } catch {
    throw new Error(fallbackMessage)
  }

  if (
    data &&
    typeof data === "object" &&
    "message" in data &&
    typeof data.message === "string"
  ) {
    throw new Error(data.message)
  }

  throw new Error(fallbackMessage)
}

export function rethrowWithFallback(
  error: unknown,
  fallbackMessage: string,
): never {
  if (isUnauthorizedError(error)) {
    throw error
  }

  if (error instanceof Error) {
    throw error
  }

  throw new Error(fallbackMessage)
}
