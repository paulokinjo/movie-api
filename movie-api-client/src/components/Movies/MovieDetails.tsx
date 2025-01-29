// src/components/Movies/MovieDetails.tsx
import React from 'react';
import { Typography, Box, Divider } from '@mui/material';
import { Movie } from '../../types/movie';
import { MovieRating } from '../../types/movieRatings';

interface MovieDetailsProps {
  movie: Movie;
}

const MovieDetails: React.FC<MovieDetailsProps> = ({ movie }) => {
  return (
    <>
      <Typography variant="h4">{movie.title}</Typography>
      <Typography variant="body1" gutterBottom>
        Year: {movie.year}
      </Typography>

      <Typography variant="h6">Actors</Typography>
      <Typography variant="body1">{movie.actors.map((actor) => actor.name).join(', ')}</Typography>

      <Divider sx={{ my: 2 }} />

      <Typography variant="h6">Ratings</Typography>
      {movie.ratings.length > 0 ? (
        movie.ratings.map((rating: MovieRating, index: number) => (
          <Box key={index} sx={{ mb: 2 }}>
            <Typography variant="body1">
              Rating: {rating.rating.toFixed(2)}/10
            </Typography>
            <Typography variant="body2">Review: {rating.review}</Typography>
            <Divider sx={{ my: 1 }} />
          </Box>
        ))
      ) : (
        <Typography variant="body1">No ratings yet.</Typography>
      )}
    </>
  );
};

export default MovieDetails;
