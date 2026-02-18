import { useState, useEffect } from "react";
import CurrentWeatherWidget from "../components/CurrentWeatherWidget";
import SmallContainerWithBackground from "../components/SmallContainerWithBackground";
import Navbar from "../components/Navbar";

export default function Home() {
    const [position, setPosition] = useState<GeolocationCoordinates | null>(null);
    const [error, setError] = useState<string | null>(null);

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


            </SmallContainerWithBackground>

        </>)
}