import Image from "next/image"
import type { User } from "next-auth"

type UserAvatarProps = {
  height?: number
  width?: number
  rounded?: boolean
  user?: User
}

export default function UserAvatar({
  height = 40,
  width = 40,
  rounded = true,
  user,
}: UserAvatarProps) {
  return (
    <Image
      src={user?.image || "/default-avatar.png"}
      alt="Avatar de usuario"
      width={width}
      height={height}
      className={rounded ? "rounded-full object-cover" : "object-cover"}
    />
  )
}
