import React from "react";
import { Navigate } from "react-router";
import { useAuth } from "../auth/AuthContext";

type Props = {
    children: React.ReactElement;
};

export default function ProtectedRoute({ children }: Props) {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/login" replace />;
    }

    return children;
}