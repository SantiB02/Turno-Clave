type SwitchProps = {
  checked: boolean
  onChange: (checked: boolean) => void
  disabled?: boolean
}

const switchClasses = `
  relative w-11 h-6
  bg-gray-400
  rounded-full
  transition-colors
  peer-checked:bg-primary-orange

  after:content-['']
  after:absolute
  after:top-[2px]
  after:left-[2px]
  after:bg-white
  after:rounded-full
  after:h-5
  after:w-5
  after:transition-all

  peer-checked:after:translate-x-full
`

export default function Switch({ checked, onChange, disabled }: SwitchProps) {
  return (
    <label
      className={`inline-flex items-center ${
        disabled
          ? "opacity-50 cursor-select disabled:peer-checked:bg-gray-400"
          : "cursor-pointer"
      }`}
    >
      <input
        type="checkbox"
        disabled={disabled}
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        className="sr-only peer"
      />

      <div className={switchClasses} />
    </label>
  )
}
