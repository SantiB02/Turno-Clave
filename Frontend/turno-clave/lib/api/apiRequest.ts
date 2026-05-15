import type { ApiResult } from "@/types/apiResult"

function extractErrorMessage(data: any): string {
  return (
    data?.message ||
    data?.error ||
    data?.detail ||
    (typeof data === "string" ? data : null) ||
    "Error inesperado"
  )
}

export async function apiRequest<T>(
  fetcher: () => Promise<Response>,
): Promise<ApiResult<T>> {
  try {
    const res = await fetcher()

    const data = await res.json().catch(() => null)

    if (!res.ok) {
      return {
        ok: false,
        message: extractErrorMessage(data),
        status: res.status,
      }
    }

    return {
      ok: true,
      data: data as T,
    }
  } catch (error) {
    return {
      ok: false,
      message:
        error instanceof Error
          ? error.message
          : "Error de conexión con el servidor",
    }
  }
}
