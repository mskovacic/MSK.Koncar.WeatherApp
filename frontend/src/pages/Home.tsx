import { useState, useEffect } from "react";
import CurrentWeatherWidget from "../components/CurrentWeatherWidget";
import SmallContainerWithBackground from "../components/SmallContainerWithBackground";
import Navbar from "../components/Navbar";
import { useAuth } from "../auth/AuthContext";

export default function Home() {
    const [position, setPosition] = useState<GeolocationCoordinates | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [value, setValue] = useState("");
    const [loading, setLoading] = useState(false);
    const [response, setResponse] = useState<string | null>(null);
    const [formError, setFormError] = useState<string | null>(null);
    const { token } = useAuth();

    useEffect(() => {
        if (!("geolocation" in navigator)) {
            setError("Geolocation is not supported by your browser.");
            return;
        }

        const watchId = navigator.geolocation.watchPosition(
            (pos) => {
                setPosition(pos.coords);
            },
            (err) => {
                setError(err.message);
            },
            {
                enableHighAccuracy: true,
                maximumAge: 0,
            }
        );

        return () => {
            navigator.geolocation.clearWatch(watchId);
        };
    }, []);

    const handleRequest = async () => {
        if (!value.trim()) return;

        setLoading(true);
        setFormError(null);
        setResponse(null);

        try {
            const res = await fetch(`/api/weather/forecast?cityId=${value}`, {
                headers: {
                    'Authentication': `Bearer ${token}`
                }
            });

            if (!res.ok) {
                throw new Error("Request failed");
            }

            const data = await res.json();

            setResponse(JSON.stringify(data, null, 2));
        } catch (err) {
            setFormError((err as Error).message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <Navbar />
            <SmallContainerWithBackground>
                <h1 className="text-2xl font-bold mb-4">Geolocation Example</h1>
                {position && <p>Latitude: {position.latitude}, Longitude: {position.longitude}</p>}

                <br />
                <br />
                {position != null ? (
                    <CurrentWeatherWidget lattitude={position.latitude} longitude={position.longitude} />
                ) : (
                    <p className="text-red-500">{error}</p>
                )}

                <br />

                <div style={{ border: "1px solid black", borderRadius: "10px" }}>
                    <input
                        value={value}
                        onChange={(e) => setValue(e.target.value)}
                    />

                    <button
                        onClick={handleRequest}
                        disabled={loading}
                    >
                        {loading ? "Sending..." : "Search"}
                    </button>

                    {error && <p>{formError}</p>}
                    {response && <pre>{response}</pre>}
                </div>
            </SmallContainerWithBackground>

        </>)
}