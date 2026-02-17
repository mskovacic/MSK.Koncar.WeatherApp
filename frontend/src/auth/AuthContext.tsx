import React, { createContext, useContext, useState, useEffect } from "react";

type AuthContextType = {
    user: string | null
    token: string | null
    login: (username: string, token: string) => void
    logout: () => void
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
    children,
}) => {
    const [user, setUser] = useState<string | null>(null);
    const [token, setToken] = useState<string | null>(null);

    useEffect(() => {
        const storedUser = localStorage.getItem("user");
        const accessToken = localStorage.getItem("token");
        if (storedUser && accessToken) {
            setUser(storedUser)
            setToken(accessToken)
        }
    }, [])

    const login = (username: string, token: string) => {
        localStorage.setItem("user", username);
        localStorage.setItem("token", token);

        setUser(username);
    };

    const logout = () => {
        localStorage.removeItem("user");
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used inside AuthProvider");
    }
    return context;
}