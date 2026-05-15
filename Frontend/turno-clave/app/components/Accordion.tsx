import { ChevronDownIcon } from "@heroicons/react/24/outline"
import { type ReactNode, useState } from "react"

interface AccordionProps {
  title: string
  children: ReactNode
}

export default function Accordion({ title, children }: AccordionProps) {
  const [open, setOpen] = useState(false)

  return (
    <div className="max-w-xl rounded-xl overflow-hidden shadow-sm border border-gray-200">
      <button
        type="button"
        onClick={() => setOpen(!open)}
        className="w-full cursor-pointer flex items-center justify-between bg-primary-orange text-white px-4 py-3"
      >
        <span className="text-xl">{title}</span>
        <ChevronDownIcon
          className={`w-5 h-5 transition-transform ${open ? "rotate-180" : ""}`}
        />
      </button>
      {open && children}
    </div>
  )
}
