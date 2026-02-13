// src/shared/api/createApiClient.ts

/**
 * Создаёт типобезопасный API-клиент для заданного baseUrl.
 * Подходит для BFF, микросервисов и т.д.
 *
 * @example
 * const authApi = createApiClient('http://localhost:3001');
 * const userApi = createApiClient('http://localhost:3002');
 */
export function createApiClient(baseUrl: string) {
  /**
   * Универсальный API-клиент для взаимодействия с сервером.
   * Использует credentials: 'include' для передачи HTTP-only cookies.
   */
  return {
    /**
     * GET-запрос с ожиданием JSON-ответа.
     */
    async get<T>(endpoint: string, params?: Record<string, any>): Promise<T> {
      const url = buildUrl(baseUrl, endpoint, params);

      const response = await fetch(url, {
        method: 'GET',
        headers: { Accept: 'application/json' },
        credentials: 'include',
      });

      return handleResponse<T>(response);
    },

    /**
     * POST-запрос с JSON-телом и ожиданием JSON-ответа.
     */
    async post<T>(endpoint: string, data: any): Promise<T> {
      const url = buildUrl(baseUrl, endpoint);

      const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
        credentials: 'include',
      });

      return handleResponse<T>(response);
    },

    /**
     * POST-запрос с ожиданием пустого ответа (204 No Content).
     */
    async postVoid(endpoint: string, data: any): Promise<void> {
      const url = buildUrl(baseUrl, endpoint);

      const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
        credentials: 'include',
      });

      return handleResponseVoid(response);
    },

    /**
     * DELETE-запрос с ожиданием пустого ответа.
     */
    async deleteVoid(endpoint: string): Promise<void> {
      const url = buildUrl(baseUrl, endpoint);

      const response = await fetch(url, {
        method: 'DELETE',
        credentials: 'include',
      });

      return handleResponseVoid(response);
    },
  };
}

// ─────────────────────────────────────────────
// Внутренние вспомогательные функции
// ─────────────────────────────────────────────

function buildUrl(baseUrl: string, endpoint: string, params?: Record<string, any>): string {
  const cleanBase = baseUrl.replace(/\/$/, '');
  const cleanEndpoint = endpoint.replace(/^\//, '');
  let url = `${cleanBase}/${cleanEndpoint}`;

  if (params && Object.keys(params).length > 0) {
    const searchParams = new URLSearchParams();
    for (const [key, value] of Object.entries(params)) {
      if (value !== undefined && value !== null) {
        searchParams.append(key, String(value));
      }
    }
    const queryString = searchParams.toString();
    if (queryString) {
      url += `?${queryString}`;
    }
  }

  return url;
}

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    throw await parseError(response);
  }

  const text = await response.text();
  if (!text.trim()) {
    throw new Error(`Expected JSON response, but received empty body. Status: ${response.status}`);
  }

  try {
    return JSON.parse(text) as T;
  } catch (e) {
    console.error('Failed to parse JSON response:', text);
    throw new Error(`Invalid JSON in response: ${(e as Error).message}`);
  }
}

async function handleResponseVoid(response: Response): Promise<void> {
  if (!response.ok) {
    throw await parseError(response);
  }

  if (response.status === 204) return;

  const text = await response.text();
  if (text.trim()) {
    throw new Error(`Expected no content (204), but received body: ${text}`);
  }
}

async function parseError(response: Response): Promise<Error> {
  const text = await response.text();
  let message = `HTTP ${response.status} ${response.statusText}`;

  if (text) {
    try {
      const errorData = JSON.parse(text);
      message =
        errorData.detail ||
        errorData.title ||
        errorData.message ||
        errorData.Message ||
        message;

      if (Array.isArray(errorData.errors)) {
        const fieldErrors = errorData.errors
          .map((err: any) => err.msg || err.message || JSON.stringify(err))
          .join(', ');
        message += `: ${fieldErrors}`;
      }
    } catch {
      message = text;
    }
  }

  return new Error(message);
}