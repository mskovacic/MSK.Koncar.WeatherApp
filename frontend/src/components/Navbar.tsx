import { useAuth } from "../auth/AuthContext"

export default function Navbar() {
    const { user, logout } = useAuth();

    return (
        <nav className="navbar">
            <h1 className="text-2xl font-bold">Weather App</h1>
            <div className="nav-links">
                <a href="/">Home</a>
                <a href="/stats">Stats</a>
                <a href="/login">Login</a>
            </div>

            <p>Welcome {user}</p>
            <button onClick={logout}>Logout</button>
        </nav>
    )
}