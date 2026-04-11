import Image from "next/image"
import { auth } from "@/auth"

export default async function UserAvatar() {
  const session = await auth()

  if (!session?.user) return null

  return (
    <div className="rounded-full mr-2 h-10 w-10 bg-gray-300 flex items-center justify-center">
      <Image
        src={session.user.image || "/default-avatar.png"}
        alt="Avatar de usuario"
        width={40}
        height={40}
        className="rounded-full object-cover"
      />
    </div>
  )
}
