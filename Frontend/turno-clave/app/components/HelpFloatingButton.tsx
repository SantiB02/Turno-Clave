import { QuestionMarkCircleIcon } from "@heroicons/react/24/outline"
import Link from "next/link"

export default function HelpFloatingButton() {
  return (
    <Link
      href="/dashboard/ayuda"
      className="fixed bottom-6 right-6 bg-primary-orange cursor-pointer text-white rounded-full p-2 shadow-lg hover:bg-orange-600 transition-colors"
    >
      <QuestionMarkCircleIcon className="h-8 w-8" />
    </Link>
  )
}
