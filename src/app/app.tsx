// eslint-disable-next-line @typescript-eslint/no-unused-vars
import './app.module.scss';

import ReactWeather from 'react-open-weather';
import { useApiWeather } from './weather-provider';

export function App() {
  const weatherParams = {
    key: '',
    lang: 'en',
    unit: 'metric', // values are (metric, standard, imperial),
    lat: '0',
    lon: '0',
  };
  const useMyLocation = window.location.href.endsWith('mylocation');

  if (!useMyLocation || window.location.hostname === 'localhost') {
    weatherParams.lat = '52.0216522';
    weatherParams.lon = '4.6794943';
  }

  const { data, isLoading, errorMessage } = useApiWeather(weatherParams);

  const myLocationLink = useMyLocation ? (
    <div></div>
  ) : (
    <div className="my-location-link">
      <button onClick={handleClick} style={{ cursor: 'pointer' }}>
        Use my location
      </button>
    </div>
  );

  const location = useMyLocation ? 'Current location' : 'Gouda - Tielweg 16';

  function handleClick() {
    window.location.href = '/mylocation';
  }

  return (
    <div className="app">
      <ReactWeather
        isLoading={isLoading}
        errorMessage={errorMessage}
        data={data}
        lang="en"
        locationLabel={location}
        unitsLabels={{ temperature: 'C', windSpeed: 'Km/h' }}
        showForecast
      />
      {isLoading ? <div></div> : myLocationLink}
      <h1>{errorMessage}</h1>
    </div>
  );
}

export default App;
