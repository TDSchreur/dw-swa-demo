// eslint-disable-next-line @typescript-eslint/no-explicit-any

import { useReducer, useState, useEffect } from 'react';
import { getIcon } from './iconsMap';
import axios from 'axios';

export const SUCCESS = 'SUCCESS';
export const FAILURE = 'FAILURE';

const initialState = {
  data: null,
  errorMessage: null,
};

export const fetchReducer = (state, { type, payload }) => {
  switch (type) {
    case SUCCESS:
      return {
        data: payload,
        errorMessage: null,
      };
    case FAILURE:
      return { data: null, errorMessage: 'Failed to load weather' };
    default:
      return state;
  }
};

export const mapCurrent = (day) => {
  return {
    provider: 'api',
    date: day.dateTime,
    description: day.phrase,
    icon: getIcon(day.iconCode),
    temperature: {
      current: day.temperature.value.toFixed(0),
      min: undefined, // openweather doesnt provide min/max on current weather
      max: undefined,
    },
    wind: day.wind.speed.value.toFixed(0),
    humidity: day.relativeHumidity,
  };
};

export const mapData = (todayData, lang) => {
  const mapped = {};
  if (todayData) {
    mapped.current = mapCurrent(todayData, lang);
    mapped.forecast = [];
  }
  return mapped;
};

const useApiWeather = (options) => {
  const [state, dispatch] = useReducer(fetchReducer, initialState);
  const { data, errorMessage } = state;
  const [isLoading, setIsLoading] = useState(false);
  const { lang, lon, lat } = options;

  const fetchData = async () => {
    const params = {};
    params.coordinates = `${lat},${lon}`;

    setIsLoading(true);
    try {
      const forecastResponse = await axios.get('/api/GetWeather', { params });
      const forecastResponseData = forecastResponse.data;

      const payload = mapData(forecastResponseData.results[0], lang);

      dispatch({
        type: SUCCESS,
        payload,
      });
    } catch (error) {
      console.log(error);
      dispatch({ type: FAILURE, payload: error.message || error });
    }
    setIsLoading(false);
  };
  useEffect(() => {
    fetchData();
  }, [lon, lat]);
  return { data, isLoading, errorMessage, fetchData };
};

export default useApiWeather;
