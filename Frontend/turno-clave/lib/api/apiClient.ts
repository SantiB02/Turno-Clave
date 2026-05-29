function buildApiUrl(path: string) {
  const normalizedPath = path.startsWith("/api/")
    ? path
    : `/api${path.startsWith("/") ? path : `/${path}`}`
  const baseUrl =
    (
      typeof window === "undefined"
        ? process.env.API_URL
        : process.env.NEXT_PUBLIC_API_URL
    )?.replace(/\/$/, "") ?? ""

  return `${baseUrl}${normalizedPath}`
}

export async function apiFetch(
  path: string,
  token?: string,
  options: RequestInit = {},
) {
  const url = buildApiUrl(path)

  console.log("[apiFetch]", {
    url,
    method: options.method ?? "GET",
    body: options.body,
  })

  const headers = new Headers(options.headers)

  if (!headers.has("Content-Type")) {
    headers.set("Content-Type", "application/json")
  }

  if (token) {
    headers.set("Authorization", `Bearer ${token}`)
  }

  return fetch(url, {
    ...options,
    headers,
    cache: "no-store",
  })
}
