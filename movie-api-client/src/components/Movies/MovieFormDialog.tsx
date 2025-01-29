import React, { useEffect, useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, MenuItem, Select, InputLabel, FormControl, FormHelperText, Checkbox, SelectChangeEvent, List, ListItem, ListItemText, Divider } from '@mui/material';
import { Actor } from '../../types/actor'
import { Movie } from '../../types/movie'
import { MovieRating } from '../../types/movieRatings'
import { service } from '../../services/api';

interface MovieFormDialogProps {
  openForm: boolean;
  onCancel: () => void;
  handleAddMovie: () => void;
  handleUpdateMovie: () => void;
  newMovie: Movie;
  setNewMovie: React.Dispatch<React.SetStateAction<Movie>>;
  editingMovieId: number | null;
}

const MovieFormDialog: React.FC<MovieFormDialogProps> = ({ openForm, onCancel, handleAddMovie, handleUpdateMovie, newMovie, setNewMovie, editingMovieId }) => {
  const [actors, setActors] = useState<Actor[]>([]);
  const [error, setError] = useState('');
  const [rating, setRating] = useState<number | ''>('');
  const [review, setReview] = useState<string>('');

  useEffect(() => {
    const fetchActors = async () => {
      try {
        const actorList = await service.actor.getActors();
        setActors(actorList);
      } catch (err) {
        setError('Failed to fetch actors');
      }
    };
    fetchActors();
  }, []);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewMovie((prevMovie) => ({
      ...prevMovie,
      [name]: value,
    }));
  };

  const handleActorChange = (e: SelectChangeEvent<any>) => {
    const selectedActorIds = e.target.value as number[];
    const selectedActors = actors.filter(actor => selectedActorIds.includes(actor.id));

    setNewMovie((prevMovie) => ({
      ...prevMovie,
      actors: selectedActors,
    }));
  };

  const handleAddRating = () => {
    if (rating !== '' && review) {
      const newRating: MovieRating = {
        rating,
        review,
      };

      setNewMovie((prevMovie) => ({
        ...prevMovie,
        ratings: [...prevMovie.ratings, newRating],
      }));

      setRating('');
      setReview('');
    } else {
      setError('Please provide both a rating and a review.');
    }
  };

  const handleSubmit = () => {
    if (!newMovie.title || !newMovie.year || newMovie.year <= 0 || newMovie.actors.length === 0) {
      setError('Please fill in all fields with valid values.');
      return;
    }

    setError('');

    if (editingMovieId) {
      handleUpdateMovie();
    } else {
      handleAddMovie();
    }
  };

  return (
    <Dialog open={openForm} onClose={() => { onCancel() }}>
      <DialogTitle>{editingMovieId ? 'Edit Movie' : 'Add New Movie'}</DialogTitle>
      <DialogContent>
        {/* Title Field */}
        <TextField
          label="Title"
          name="title"
          value={newMovie.title}
          onChange={handleInputChange}
          fullWidth
          margin="normal"
          required
          error={!newMovie.title}
          helperText={!newMovie.title ? 'Title is required' : ''}
        />

        {/* Year Field */}
        <TextField
          label="Year"
          name="year"
          value={newMovie.year}
          onChange={handleInputChange}
          fullWidth
          margin="normal"
          type="number"
          required
          error={newMovie.year <= 0}
          helperText={newMovie.year <= 0 ? 'Year must be greater than 0' : ''}
        />

        {/* Actors Dropdown (Multiple Selection) */}
        <FormControl fullWidth margin="normal" required error={newMovie.actors.length === 0}>
          <InputLabel>Actors</InputLabel>
          <Select
            multiple
            value={newMovie.actors.map(actor => actor.id)}
            onChange={handleActorChange}
            label="Actors"
            name="actors"
            renderValue={(selected) => selected.map((id) => actors.find((a) => a.id === id)?.name).join(', ')}
          >
            {actors.map((actor) => (
              <MenuItem key={actor.id} value={actor.id}>
                <Checkbox checked={newMovie.actors.some((a) => a.id === actor.id)} />
                <ListItemText primary={actor.name} />
              </MenuItem>
            ))}
          </Select>
          {newMovie.actors.length === 0 && <FormHelperText>At least one actor is required</FormHelperText>}
        </FormControl>

        {/* Rating Field */}
        <TextField
          label="Rating (out of 10)"
          value={rating}
          onChange={(e) => setRating(Number(e.target.value))}
          fullWidth
          margin="normal"
          type="number"
          inputProps={{ min: 0, max: 10, step: 0.1 }}
        />

        {/* Review Field */}
        <TextField
          label="Review"
          value={review}
          onChange={(e) => setReview(e.target.value)}
          fullWidth
          margin="normal"
          multiline
          rows={4}
        />

        {/* Add Rating Button */}
        <Button onClick={handleAddRating} color="primary" variant="contained" style={{ marginTop: 10 }}>
          Add Rating
        </Button>

        {/* Error Message */}
        {error && <div style={{ color: 'red', marginTop: 10 }}>{error}</div>}

        {/* Display Existing Ratings */}
        {newMovie.ratings.length > 0 && (
          <>
            <Divider sx={{ my: 2 }} />
            <List>
              {newMovie.ratings.map((rating, index) => (
                <ListItem key={index}>
                  <ListItemText
                    primary={`Rating: ${rating.rating}/10`}
                    secondary={rating.review}
                  />
                </ListItem>
              ))}
            </List>
          </>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={() => onCancel()} color="secondary">
          Cancel
        </Button>
        <Button onClick={handleSubmit} color="primary">
          {editingMovieId ? 'Update Movie' : 'Add Movie'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default MovieFormDialog;
