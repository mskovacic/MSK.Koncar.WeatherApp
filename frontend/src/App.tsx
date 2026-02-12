import { useState, useEffect } from 'react';
import './App.css'
import SmallContainerWithBackground from './components/SmallContainerWithBackground';

function App() {
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
            <SmallContainerWithBackground>
                <h1 className="text-2xl font-bold mb-4">Geolocation Example</h1>
                {position && <p>Latitude: {position.latitude}, Longitude: {position.longitude}</p>}
                {error && <p className="text-red-500">{error}</p>}
                <br>
</br>
            </SmallContainerWithBackground>

        </>)
}

export default App
