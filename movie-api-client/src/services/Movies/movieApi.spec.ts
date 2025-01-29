import api from '../../axiosInstance';
import { Movie } from '../../types/movie';
import { Actor } from '../../types/actor';
import { MovieRating } from '../../types/movieRatings';
import * as movieApi from './movieApi';

jest.mock('../../axiosInstance');

describe('movieApi', () => {
    const mockActors: Actor[] = [
        { id: 1, name: 'Actor 1' },
        { id: 2, name: 'Actor 2' },
    ];

    const mockRatings: MovieRating[] = [
        { rating: 5, review: 'Excellent movie!' },
        { rating: 3, review: 'It was okay.' },
    ];

    const mockMovies: Movie[] = [
        { id: 1, title: 'Movie 1', year: 2021, actors: mockActors, ratings: mockRatings },
        { id: 2, title: 'Movie 2', year: 2020, actors: [], ratings: [] },
    ];

    const mockMovie: Movie = { id: 1, title: 'Movie 1', year: 2021, actors: mockActors, ratings: mockRatings };

    it('should fetch movies with actors and ratings', async () => {
        (api.get as jest.Mock).mockResolvedValue({ data: mockMovies });

        const result = await movieApi.getMovies();
        expect(result).toEqual(mockMovies);
        expect(api.get).toHaveBeenCalledWith('/movie');
    });

    it('should handle errors when fetching movies with actors and ratings', async () => {
        (api.get as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.getMovies()).rejects.toThrow('Request failed');
    });

    it('should add a movie with actors and ratings', async () => {
        (api.post as jest.Mock).mockResolvedValue({ data: mockMovie });

        const result = await movieApi.addMovie(mockMovie);
        expect(result).toEqual(mockMovie);
        expect(api.post).toHaveBeenCalledWith('/movie', mockMovie);
    });

    it('should handle errors when adding a movie with actors and ratings', async () => {
        (api.post as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.addMovie(mockMovie)).rejects.toThrow('Request failed');
    });

    it('should delete a movie', async () => {
        (api.delete as jest.Mock).mockResolvedValue({});

        await movieApi.deleteMovie(mockMovie.id);
        expect(api.delete).toHaveBeenCalledWith(`/movie/${mockMovie.id}`);
    });

    it('should handle errors when deleting a movie', async () => {
        (api.delete as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.deleteMovie(mockMovie.id)).rejects.toThrow('Request failed');
    });

    it('should update a movie with actors and ratings', async () => {
        const updatedMovie: Movie = { ...mockMovie, title: 'Updated Movie', actors: mockActors, ratings: mockRatings };
        (api.put as jest.Mock).mockResolvedValue({ data: updatedMovie });

        const result = await movieApi.updateMovie(mockMovie.id, updatedMovie);
        expect(result).toEqual(updatedMovie);
        expect(api.put).toHaveBeenCalledWith(`/movie/${mockMovie.id}`, updatedMovie);
    });

    it('should handle errors when updating a movie with actors and ratings', async () => {
        const updatedMovie: Movie = { ...mockMovie, title: 'Updated Movie', actors: mockActors, ratings: mockRatings };
        (api.put as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.updateMovie(mockMovie.id, updatedMovie)).rejects.toThrow('Request failed');
    });

    it('should fetch a single movie with actors and ratings by id', async () => {
        (api.get as jest.Mock).mockResolvedValue({ data: mockMovie });

        const result = await movieApi.getMovie(mockMovie.id);
        expect(result).toEqual(mockMovie);
        expect(api.get).toHaveBeenCalledWith(`/movie/${mockMovie.id}`);
    });

    it('should handle errors when fetching a single movie with actors and ratings by id', async () => {
        (api.get as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.getMovie(mockMovie.id)).rejects.toThrow('Request failed');
    });

    it('should search movies with actors and ratings', async () => {
        const query = 'action';
        (api.get as jest.Mock).mockResolvedValue({ data: mockMovies });

        const result = await movieApi.searchMovies(query);
        expect(result).toEqual(mockMovies);
        expect(api.get).toHaveBeenCalledWith(`/movie/search?query=${query}`);
    });

    it('should handle errors when searching for movies with actors and ratings', async () => {
        const query = 'action';
        (api.get as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(movieApi.searchMovies(query)).rejects.toThrow('Request failed');
    });
});
