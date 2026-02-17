import { useState, type FormEvent } from "react"
import { useNavigate, Link } from "react-router"
import { useAuth } from "../auth/AuthContext"

type LoginForm = {
    email: string
    password: string
}

type LoginResponse = {
    tokenType: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}

export default function Login() {
    const [form, setForm] = useState<LoginForm>({
        email: "",
        password: "",
    })
    const { login } = useAuth()
    const navigate = useNavigate()
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm(prev => ({
            ...prev,
            [e.target.name]: e.target.value,
        }))
    }

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault()
        setLoading(true)
        setError(null)

        try {
            const response = await fetch('/identity/login?useCookies=false', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: form.email,
                    password: form.password
                }),
            })

            if (!response.ok) { 
                throw new Error("Invalid credentials")
            }

            const data: LoginResponse = await response.json()
            console.log(data)
            alert("Logged in successfully!")
            login(form.email, data.accessToken)
            await navigate("/", { replace: true })
        } catch (err) {
            console.log("Login error:", err)
            setError("Login failed")
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-container">
            <form className="auth-card" onSubmit={handleSubmit}>
                <h2>Login</h2>

                {error && <p className="error">{error}</p>}

                <input
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={form.email}
                    onChange={handleChange}
                    required
                />

                <input
                    type="password"
                    name="password"
                    placeholder="Password"
                    value={form.password}
                    onChange={handleChange}
                    required
                />

                <button type="submit" disabled={loading}>
                    {loading ? "Signing in..." : "Login"}
                </button>
            </form>
            <Link to="/register">Register</Link>
        </div>

    )
}