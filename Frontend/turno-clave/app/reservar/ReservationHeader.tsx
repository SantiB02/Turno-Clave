import BackButton from "../components/BackButton"

type Props = {
  title: string
  backButtonUrl: string
}

export default function ReservationHeader({
  title,
  backButtonUrl: backUrl,
}: Props) {
  return (
    <div className="flex relative justify-center items-center mb-4">
      <BackButton href={backUrl} className="absolute left-0" />
      <h1 className="text-2xl font-bold text-dark-blue text-center">{title}</h1>
    </div>
  )
}
