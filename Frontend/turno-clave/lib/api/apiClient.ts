export async function apiFetch(
  path: string,
  token: string | undefined,
  options: RequestInit = {},
) {
  console.log("[apiFetch]", {
    url: `${process.env.API_URL}/api${path}`,
    method: options.method ?? "GET",
    body: options.body,
  })
  return fetch(`${process.env.API_URL}/api${path}`, {
    ...options,
    headers: {
      ...(options.headers || {}),
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
    cache: "no-store",
  })
}
