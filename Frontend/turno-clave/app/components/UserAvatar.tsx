import Image from "next/image"
import { auth } from "@/auth"

type UserAvatarProps = {
  height?: number
  width?: number
  rounded?: boolean
  className?: string
}

export default async function UserAvatar({
  height = 40,
  width = 40,
  rounded = true,
}: UserAvatarProps) {
  const session = await auth()

  if (!session?.user) return null

  return (
    <Image
      src={session.user.image || "/default-avatar.png"}
      alt="Avatar de usuario"
      width={width}
      height={height}
      className={rounded ? "rounded-full object-cover" : "object-cover"}
    />
  )
}
