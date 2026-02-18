import Navbar from "../components/Navbar";
import { useAuth } from "../auth/AuthContext";
import { useState, useEffect } from "react";

export default function Stats() {
    const { token } = useAuth()
    const [topCitiesData, setTopCitiesData] = useState<string | null>(null)
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchStats = async () => {
        setLoading(true)
        setError(null)

        try {
            const response = await fetch(`/api/statistics/top-cities`, { headers: { 'Authentication': `Bearer ${token}` } })

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`)
            }

            const data = await response.json()
            setTopCitiesData(JSON.stringify(data, null, 2));
        } catch (err) {
            setError((err as Error).message)
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        fetchStats()
    }, [])

    return (
        <>
            <Navbar />
            <div className="stats-container">
                <h1>Stats Page</h1>
                <p>This page will display various statistics and data visualizations.</p>

                <br />
                <p>Top Cities with Most Weather Requests:</p>
                {topCitiesData && <pre>{topCitiesData}</pre>}
            </div>
        </>
        
    )
}