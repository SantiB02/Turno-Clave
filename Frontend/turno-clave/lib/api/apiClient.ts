export async function apiFetch(
  path: string,
  token: string | undefined,
  options: RequestInit = {},
) {
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
