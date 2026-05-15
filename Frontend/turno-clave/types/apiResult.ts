export type ApiResult<T> =
  | { ok: true; data: T }
  | { ok: false; message: string; status?: number }
