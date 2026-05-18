import os from "node:os"
import type { NextConfig } from "next"

function getAllowedLanDevOrigins() {
  const privateLanPatterns = new Set<string>()

  for (const addresses of Object.values(os.networkInterfaces())) {
    for (const address of addresses ?? []) {
      if (address.internal || address.family !== "IPv4") {
        continue
      }

      const octets = address.address.split(".")

      if (octets.length !== 4) {
        continue
      }

      const [first, second, third] = octets
      const isPrivateLan =
        first === "10" ||
        (first === "172" &&
          Number(second) >= 16 &&
          Number(second) <= 31) ||
        (first === "192" && second === "168")

      if (isPrivateLan) {
        privateLanPatterns.add(`${first}.${second}.${third}.*`)
      }
    }
  }

  return [...privateLanPatterns]
}

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [new URL("https://lh3.googleusercontent.com/**")], // For Google profile pictures
  },
  // Allow phones/tablets on the same LAN to access Next.js dev resources.
  allowedDevOrigins: getAllowedLanDevOrigins(),
}

export default nextConfig
