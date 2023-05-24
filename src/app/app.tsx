// eslint-disable-next-line @typescript-eslint/no-unused-vars
import './app.module.scss';

import ReactWeather from 'react-open-weather';
import { useApiWeather } from './weather-provider';
import { useState } from 'react';

export function App() {
  const [state, setState] = useState({
    lat: '52.0216522',
    lon: '4.6794943',
    location: 'Gouda - Tielweg 16',
    lang: 'en',
  });

  const { data, isLoading, errorMessage } = useApiWeather(state);

  const setLocation = () => {
    navigator.geolocation.getCurrentPosition((position) => {
      const lat = position.coords.latitude;
      const long = position.coords.longitude;

      setState({
        ...state,
        lat: `${lat}`,
        lon: `${long}`,
        location: 'Current location',
      });

      console.log(`lat: ${lat} long: ${long}`);
    });
  };

  const setGouda = () => {
    setState({
      ...state,
      lat: '52.0216522',
      lon: '4.6794943',
      location: 'Gouda - Tielweg 16',
    });
  };

  const myLocationLink = (
    <div>
      <button onClick={setLocation} style={{ cursor: 'pointer' }}>
        Use my location
      </button>
    </div>
  );

  const goudaLocation = (
    <div>
      <button onClick={setGouda} style={{ cursor: 'pointer' }}>
        Back to Gouda
      </button>
    </div>
  );

  return (
    <div className="app">
      <ReactWeather
        isLoading={isLoading}
        errorMessage={errorMessage}
        data={data}
        lang="en"
        locationLabel={state.location}
        unitsLabels={{ temperature: 'C', windSpeed: 'Km/h' }}
        showForecast
      />
      {myLocationLink}
      {goudaLocation}
      <h1>{errorMessage}</h1>
      <h2>Lat : {state.lat}</h2>
      <h2>Long: {state.lon}</h2>
    </div>
  );
}

export default App;
