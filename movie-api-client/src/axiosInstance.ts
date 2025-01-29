// src/axiosInstance.ts
import axios from 'axios';
import { getAuthToken, setAuthToken } from './utils/auth';

const api = axios.create({
    baseURL: process.env.REACT_APP_API_URL || 'http://localhost:5125/api',
});

// Add request interceptor to attach Authorization token
api.interceptors.request.use(
    (config) => {
        const token = getAuthToken();
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        if (error.response && error.response.status === 401) {
            // e.g., clear token, redirect to login page
            console.log(error.response);
        }
        return Promise.reject(error);
    }
);


export const login = async () => {
    const response = await api.post('/auth/login', {});
    const token = response.data;
    setAuthToken(token);
}

export default api;

