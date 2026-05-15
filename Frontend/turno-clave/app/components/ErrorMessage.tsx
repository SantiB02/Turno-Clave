type ErrorMessageProps = {
  title?: string
  message: string
}

export default function ErrorMessage({
  title,
  message: error,
}: ErrorMessageProps) {
  return (
    <div className="rounded border border-red-400 bg-red-200 px-4 py-3 text-red-700">
      {title && <p>{title}</p>}
      <p>{error}</p>
    </div>
  )
}
