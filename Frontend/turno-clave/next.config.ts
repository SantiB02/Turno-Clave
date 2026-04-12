import type { NextConfig } from "next"

const nextConfig: NextConfig = {
  /* config options here */
  images: {
    remotePatterns: [new URL("https://lh3.googleusercontent.com/**")], // For Google profile pictures
  },
}

export default nextConfig
