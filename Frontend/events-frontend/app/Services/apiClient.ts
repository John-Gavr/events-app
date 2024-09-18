const API_URL = process.env.NEXT_PUBLIC_API_URL;

export async function apiFetch(
  endpoint: string,
  options?: RequestInit
): Promise<Response> {
  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      ...options?.headers,
    },
  });

  console.log("fetch works", response);

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  return response;
}
