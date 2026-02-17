import React, { useState, type FormEvent } from "react";
import { useNavigate } from "react-router";
import type { ProblemDetailsResponse } from "../lib/ProblemDetailsResponse";

type RegisterForm = {
    email: string;
    password: string;
    confirmPassword: string;
};

export default function Register() {
    const [form, setForm] = useState<RegisterForm>({
        email: "",
        password: "",
        confirmPassword: "",
    });

    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate()

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm(prev => ({
            ...prev,
            [e.target.name]: e.target.value,
        }));
    };

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setError(null);

        if (form.password !== form.confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        setLoading(true);

        try {
            const response = await fetch('/identity/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    email: form.email,
                    password: form.password
                })
            })

            if (!response.ok) {
                const data: ProblemDetailsResponse = await response.json()
                console.log({ data })
                throw new Error(Object.values(data.errors).join("\n"))
            }

            alert("Account created!");
            await navigate('/', {replace: true})
        } catch(e) {
            setError("Registration failed " + e);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <form className="auth-card" onSubmit={handleSubmit}>
                <h2>Create Account</h2>

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

                <input
                    type="password"
                    name="confirmPassword"
                    placeholder="Confirm password"
                    value={form.confirmPassword}
                    onChange={handleChange}
                    required
                />

                <button type="submit" disabled={loading}>
                    {loading ? "Creating..." : "Register"}
                </button>
            </form>
        </div>
    );
}