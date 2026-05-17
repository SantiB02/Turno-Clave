export async function apiFetch(
  path: string,
  token?: string,
  options: RequestInit = {},
) {
  console.log("[apiFetch]", {
    url: `${process.env.API_URL}/api${path}`,
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

  return fetch(`${process.env.API_URL}/api${path}`, {
    ...options,
    headers,
    cache: "no-store",
  })
}
