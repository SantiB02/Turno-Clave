export const PAYMENT_METHODS = ["Cash", "Card", "QR", "BankTransfer"] as const

export type PaymmentMethod = (typeof PAYMENT_METHODS)[number]

export const PAYMENT_METHOD_LABELS: Record<PaymmentMethod, string> = {
  Cash: "Efectivo",
  Card: "Tarjeta",
  QR: "QR",
  BankTransfer: "Transferencia",
}
