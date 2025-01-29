import React, { useEffect, useState } from 'react';
import { CircularProgress, Container, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Divider } from '@mui/material';
import { Movie } from '../../types/movie';
import MovieDetails from './MovieDetails';
import { service } from '../../services/api';
import MovieFormDialog from './MovieFormDialog';
import ListPaginator from '../../utils/ListPaginator';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import PreviewIcon from '@mui/icons-material/Preview';
import SearchComponent from '../../utils/Search';

const MovieList: React.FC = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState<number>(0);
  const [rowsPerPage, setRowsPerPage] = useState<number>(5);
  const [openForm, setOpenForm] = useState<boolean>(false);
  const [newMovie, setNewMovie] = useState<Movie>({
    id: 0,
    title: '',
    year: 0,
    actors: [],
    ratings: [],
  });
  const [editingMovieId, setEditingMovieId] = useState<number | null>(null);
  const [openDetailsDialog, setOpenDetailsDialog] = useState<boolean>(false);
  const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null);
  const [query, setQuery] = useState<string>('');

  useEffect(() => {
    fetchMovies();

    if (openForm && !editingMovieId) {
      setNewMovie({ id: 0, title: '', year: 0, actors: [], ratings: [] });
    }
  }, [openForm, editingMovieId, query]);

  const handleDeleteMovie = async (movieId: number) => {
    try {
      await service.movie.deleteMovie(movieId);
      setMovies(movies.filter(movie => movie.id !== movieId));
    } catch (err) {
      setError('Failed to delete movie');
    }
  };

  const fetchMovies = async () => {
    try {
      let fetchedMovies: Movie[] = [];
      if (query) {
        fetchedMovies = await service.movie.searchMovies(query);
      }
      else {
        fetchedMovies = await service.movie.getMovies();
      }

      setMovies(fetchedMovies);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch movies');
      setLoading(false);
    }
  };
  const handleChangePage = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const handleAddMovie = async () => {
    try {
      const { title, year, actors } = newMovie;
      const newMovieData: Movie = { id: 0, title, year, actors, ratings: [] };
      await service.movie.addMovie(newMovieData);
      setNewMovie({ id: 0, title: '', year: 0, actors: [], ratings: [] });
      setOpenForm(false);
      fetchMovies();
    } catch (err) {
      setError('Failed to add movie');
    }
  };

  const handleEditMovie = async () => {
    try {
      const { id, title, year, actors, ratings } = newMovie;
      const updatedMovie = { id, title, year, actors, ratings };
      await service.movie.updateMovie(editingMovieId!, updatedMovie);
      setOpenForm(false);
      fetchMovies();
    } catch (err) {
      setError('Failed to update movie');
    }
  };

  const handleEditClick = (movie: Movie) => {
    setNewMovie({
      id: movie.id,
      title: movie.title,
      year: movie.year,
      actors: movie.actors,
      ratings: movie.ratings,
    });
    setEditingMovieId(movie.id);
    setOpenForm(true);
  };

  const handleOpenDetailsDialog = (movie: Movie) => {
    setSelectedMovie(movie);
    setOpenDetailsDialog(true);
  };

  const handleCloseDetailsDialog = () => {
    setOpenDetailsDialog(false);
    setSelectedMovie(null);
  };

  const handleFormCancel = () => {
    setOpenForm(false);
    setNewMovie({ id: 0, title: '', year: 0, actors: [], ratings: [] });
    setEditingMovieId(undefined);
  };

  const handleSearch = (searchQuery: string) => {
    setQuery(searchQuery);
  };

  const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - movies.length) : 0;

  return (
    <Container>
      <Divider sx={{ margin: '16px 0' }} />

      <SearchComponent onSearch={handleSearch} placeholder="Search by Movie Title" />

      <MovieFormDialog
        openForm={openForm}
        onCancel={handleFormCancel}
        handleAddMovie={handleAddMovie}
        newMovie={newMovie}
        setNewMovie={setNewMovie}
        editingMovieId={editingMovieId}
        handleUpdateMovie={handleEditMovie}
      />

      {loading ? (
        <CircularProgress />
      ) : error ? (
        <Typography variant="h6" color="error">
          {error}
        </Typography>
      ) : (
        <>
          <Typography variant="h4" gutterBottom>
            Movies
          </Typography>
          <Button variant="contained" color="primary" onClick={() => setOpenForm(true)}>
            Add New Movie
          </Button>
          <Divider sx={{ margin: '16px 0' }} />

          <TableContainer component={Paper}>
            <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
              <TableHead>
                <TableRow>
                  <TableCell>Title</TableCell>
                  <TableCell align="right">Year</TableCell>
                  <TableCell align="right">Actors</TableCell>
                  <TableCell align="right">Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {(rowsPerPage > 0
                  ? movies.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  : movies
                ).map((movie) => (
                  <TableRow key={movie.id}>
                    <TableCell>{movie.title}</TableCell>
                    <TableCell align="right">{movie.year}</TableCell>
                    <TableCell align="right">{movie.actors.map(actor => actor.name).join(', ')}</TableCell>
                    <TableCell align="right">
                      <IconButton color="primary" onClick={() => handleOpenDetailsDialog(movie)}>
                        <PreviewIcon />
                      </IconButton>
                      <IconButton color="primary" onClick={() => handleEditClick(movie)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton color="secondary" onClick={() => handleDeleteMovie(movie.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
                {emptyRows > 0 && (
                  <TableRow style={{ height: 53 * emptyRows }}>
                    <TableCell colSpan={4} />
                  </TableRow>
                )}
              </TableBody>
              <ListPaginator
                count={movies.length}
                page={page}
                rowsPerPage={rowsPerPage}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
              />
            </Table>
          </TableContainer>
        </>
      )}

      <Dialog open={openDetailsDialog} onClose={handleCloseDetailsDialog}>
        <DialogTitle>Movie Details</DialogTitle>
        <DialogContent>
          {selectedMovie && <MovieDetails movie={selectedMovie} />}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDetailsDialog} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default MovieList;
