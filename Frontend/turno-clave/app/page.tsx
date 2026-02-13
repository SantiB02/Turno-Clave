export default function Home() {
	return (
		<div className="min-h-screen bg-white">
			{/* Navigation */}
			<nav className="fixed top-0 w-full bg-white shadow-sm z-50">
				<div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
					<div className="flex justify-between items-center h-16">
						<div className="flex items-center gap-2">
							<div className="w-8 h-8 bg-orange-500 rounded-lg flex items-center justify-center">
								<span className="text-white font-bold text-lg">T</span>
							</div>
							<span className="text-2xl font-bold text-gray-900">
								Turno Clave
							</span>
						</div>
						<div className="hidden md:flex gap-8">
							<a
								href="#features"
								className="text-gray-700 hover:text-orange-500 transition"
							>
								Features
							</a>
							<a
								href="#benefits"
								className="text-gray-700 hover:text-orange-500 transition"
							>
								Benefits
							</a>
						</div>
						<button className="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-lg transition">
							Get Started
						</button>
					</div>
				</div>
			</nav>

			{/* Hero Section */}
			<section className="pt-32 pb-20 px-4 sm:px-6 lg:px-8">
				<div className="max-w-7xl mx-auto">
					<div className="grid md:grid-cols-2 gap-12 items-center">
						<div>
							<h1 className="text-5xl md:text-6xl font-bold text-gray-900 mb-6 leading-tight">
								Manage Appointments with{" "}
								<span className="text-orange-500">Turno Clave</span>
							</h1>
							<p className="text-xl text-gray-600 mb-8 leading-relaxed">
								The powerful appointment management platform built for
								businesses. Schedule, organize, and optimize your bookings
								effortlessly.
							</p>
							<div className="flex flex-col sm:flex-row gap-4">
								<button className="bg-orange-500 hover:bg-orange-600 text-white font-semibold px-8 py-3 rounded-lg transition text-lg">
									Start Free Trial
								</button>
								<button className="border-2 border-orange-500 text-orange-500 hover:bg-orange-50 font-semibold px-8 py-3 rounded-lg transition text-lg">
									Watch Demo
								</button>
							</div>
						</div>
						<div className="bg-blue-50 rounded-2xl h-96 flex items-center justify-center">
							<div className="text-center text-gray-400">
								<svg
									className="w-24 h-24 mx-auto mb-4 opacity-50"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth={2}
										d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
									/>
								</svg>
								<p>Visual appointment calendar</p>
							</div>
						</div>
					</div>
				</div>
			</section>

			{/* Features Section */}
			<section id="features" className="py-20 px-4 sm:px-6 lg:px-8 bg-blue-50">
				<div className="max-w-7xl mx-auto">
					<div className="text-center mb-16">
						<h2 className="text-4xl font-bold text-gray-900 mb-4">
							Powerful Features
						</h2>
						<p className="text-xl text-gray-600 max-w-2xl mx-auto">
							Everything you need to manage your business appointments
						</p>
					</div>
					<div className="grid md:grid-cols-3 gap-8">
						{[
							{
								icon: "📅",
								title: "Smart Scheduling",
								description:
									"Intuitive calendar interface to manage all your appointments in one place",
							},
							{
								icon: "🔔",
								title: "Automated Reminders",
								description:
									"Automatic notifications to reduce no-shows and keep clients informed",
							},
							{
								icon: "👥",
								title: "Multi-User Support",
								description:
									"Collaborate with your team and manage permissions with ease",
							},
							{
								icon: "📱",
								title: "Mobile Friendly",
								description:
									"Manage appointments on the go with our responsive mobile interface",
							},
							{
								icon: "⚙️",
								title: "Customizable Settings",
								description:
									"Tailor the platform to match your business needs perfectly",
							},
							{
								icon: "📊",
								title: "Analytics & Reports",
								description:
									"Track your booking patterns and optimize your business performance",
							},
						].map((feature, idx) => (
							<div
								key={idx}
								className="bg-white p-8 rounded-xl shadow-sm hover:shadow-md transition"
							>
								<div className="text-4xl mb-4">{feature.icon}</div>
								<h3 className="text-xl font-bold text-gray-900 mb-2">
									{feature.title}
								</h3>
								<p className="text-gray-600">{feature.description}</p>
							</div>
						))}
					</div>
				</div>
			</section>

			{/* Benefits Section */}
			<section id="benefits" className="py-20 px-4 sm:px-6 lg:px-8">
				<div className="max-w-7xl mx-auto">
					<div className="grid md:grid-cols-2 gap-12 items-center">
						<div className="bg-orange-500 bg-opacity-10 rounded-2xl h-96 flex items-center justify-center">
							<div className="text-center text-gray-400">
								<svg
									className="w-24 h-24 mx-auto mb-4 opacity-50"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth={2}
										d="M13 10V3L4 14h7v7l9-11h-7z"
									/>
								</svg>
								<p>Boost efficiency</p>
							</div>
						</div>
						<div>
							<h2 className="text-4xl font-bold text-gray-900 mb-8">
								Why Choose Turno Clave?
							</h2>
							<ul className="space-y-4">
								<li className="flex items-start gap-4">
									<span className="text-orange-500 text-2xl mt-1">✓</span>
									<div>
										<h3 className="font-bold text-gray-900 mb-1">
											Save Hours Every Week
										</h3>
										<p className="text-gray-600">
											Automate scheduling and reduce manual work significantly
										</p>
									</div>
								</li>
								<li className="flex items-start gap-4">
									<span className="text-orange-500 text-2xl mt-1">✓</span>
									<div>
										<h3 className="font-bold text-gray-900 mb-1">
											Reduce No-Shows
										</h3>
										<p className="text-gray-600">
											Automated reminders help clients remember their
											appointments
										</p>
									</div>
								</li>
								<li className="flex items-start gap-4">
									<span className="text-orange-500 text-2xl mt-1">✓</span>
									<div>
										<h3 className="font-bold text-gray-900 mb-1">
											Easy Integration
										</h3>
										<p className="text-gray-600">
											Seamlessly connects with your existing business tools
										</p>
									</div>
								</li>
								<li className="flex items-start gap-4">
									<span className="text-orange-500 text-2xl mt-1">✓</span>
									<div>
										<h3 className="font-bold text-gray-900 mb-1">
											24/7 Support
										</h3>
										<p className="text-gray-600">
											Our dedicated team is always here to help you succeed
										</p>
									</div>
								</li>
							</ul>
						</div>
					</div>
				</div>
			</section>

			{/* CTA Section */}
			<section className="py-20 px-4 sm:px-6 lg:px-8 bg-gradient-to-r from-orange-500 to-orange-600">
				<div className="max-w-4xl mx-auto text-center">
					<h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
						Ready to Transform Your Scheduling?
					</h2>
					<p className="text-xl text-orange-50 mb-8 max-w-2xl mx-auto">
						Join hundreds of businesses already using Turno Clave to streamline
						their appointment management
					</p>
					<button className="bg-white text-orange-500 hover:bg-gray-50 font-bold px-10 py-4 rounded-lg transition text-lg">
						Start Your Free 14-Day Trial
					</button>
				</div>
			</section>

			{/* Footer */}
			<footer className="bg-gray-900 text-white py-12 px-4 sm:px-6 lg:px-8">
				<div className="max-w-7xl mx-auto">
					<div className="grid md:grid-cols-4 gap-8 mb-8">
						<div>
							<div className="flex items-center gap-2 mb-4">
								<div className="w-8 h-8 bg-orange-500 rounded-lg flex items-center justify-center">
									<span className="text-white font-bold">T</span>
								</div>
								<span className="text-xl font-bold">Turno Clave</span>
							</div>
							<p className="text-gray-400">
								Professional appointment management for modern businesses
							</p>
						</div>
						<div>
							<h4 className="font-bold mb-4">Product</h4>
							<ul className="space-y-2 text-gray-400">
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Features
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Pricing
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Demo
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h4 className="font-bold mb-4">Company</h4>
							<ul className="space-y-2 text-gray-400">
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										About
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Blog
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Contact
									</a>
								</li>
							</ul>
						</div>
						<div>
							<h4 className="font-bold mb-4">Legal</h4>
							<ul className="space-y-2 text-gray-400">
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Privacy
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Terms
									</a>
								</li>
								<li>
									<a href="#" className="hover:text-orange-500 transition">
										Security
									</a>
								</li>
							</ul>
						</div>
					</div>
					<div className="border-t border-gray-800 pt-8 text-center text-gray-400">
						<p>
							&copy; {new Date().getFullYear()} Turno Clave. Todos los derechos
							reservados.
						</p>
					</div>
				</div>
			</footer>
		</div>
	);
}
