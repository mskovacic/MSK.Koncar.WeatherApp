import { useEffect, useState } from "react";
import { Spinner } from "./ui/spinner";
import { useAuth } from "../auth/AuthContext";
import { Button } from "./ui/button";

interface WeatherResponse {
    city: string
    weather: {
        temperatureCelsius: number
        iconURL: string
        description: string
    }
}

export default function CurrentWeatherWidget({ lattitude, longitude }: { lattitude : number, longitude : number }) {
    const [weatherData, setWeatherData] = useState<WeatherResponse | null>(null)
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const { token } = useAuth()


    const fetchWeatherForecast = async () => {
        setLoading(true)
        setError(null)

        try {
            const response = await fetch(`/api/weather/current?lattitude=${lattitude}&longitude=${longitude}`, { headers: { 'Authentication': `Bearer ${token}` } })

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`)
            }

            const data: WeatherResponse = await response.json()
            setWeatherData(data)
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to fetch weather data')
            console.error('Error fetching weather forecast:', err)
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        fetchWeatherForecast()
    }, [lattitude, longitude])

    if (loading) {
        return (<Spinner />)
    }

    if (error) {
        return (<>Error <Button onClick={async () => await fetchWeatherForecast() }>Refresh</Button></>)
    }

    return (
        <>
            <div style={{ border: "1px solid black", borderRadius: "10px"} }>
                <div>
                    {weatherData?.city}
                </div>
                <div>
                    {weatherData?.weather.temperatureCelsius} °C
                </div>
                <img src={ weatherData?.weather.iconURL}/>
                <div>
                    {weatherData?.weather.description}
                </div>
            </div> 
        </>)
}