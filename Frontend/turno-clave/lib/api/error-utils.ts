export function isUnauthorizedError(error: unknown) {
  return error instanceof Error && error.message === "UNAUTHORIZED"
}

export async function throwResponseError(
  response: Response,
  message: string,
): Promise<never> {
  const errorText = (await response.text()) || "Unknown error"

  throw new Error(`${message}: ${response.status} ${errorText}`)
}

export function rethrowWithFallback(error: unknown, fallbackMessage: string): never {
  if (isUnauthorizedError(error)) {
    throw error
  }

  if (error instanceof Error) {
    throw error
  }

  throw new Error(fallbackMessage)
}
