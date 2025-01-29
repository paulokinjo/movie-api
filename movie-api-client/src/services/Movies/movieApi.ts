// src/services/movieApi.ts
import api from '../../axiosInstance';
import { AxiosResponse } from 'axios';
import { Movie } from '../../types/movie';

export const getMovies = async (): Promise<Movie[]> => {
    const response: AxiosResponse<Movie[]> = await api.get('/movie');
    return response.data;
};

export const addMovie = async (movieData: Movie): Promise<Movie> => {
    const response: AxiosResponse<Movie> = await api.post('/movie', movieData);
    return response.data;
};

export const deleteMovie = async (movieId: number): Promise<void> => {
    await api.delete(`/movie/${movieId}`);
};

export const updateMovie = async (movieId: number, updatedMovie: Movie): Promise<Movie> => {
    const response: AxiosResponse<Movie> = await api.put(`/movie/${movieId}`, updatedMovie);
    return response.data;
};

export const getMovie = async (id: number): Promise<Movie> => {
    const response: AxiosResponse<Movie> = await api.get(`/movie/${id}`);
    return response.data;
};

export const searchMovies = async (query: string): Promise<Movie[]> => {
    const response: AxiosResponse<Movie[]> = await api.get(`/movie/search?query=${query}`);
    return response.data;
};
