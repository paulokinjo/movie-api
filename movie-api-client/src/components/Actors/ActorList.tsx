import React, { useEffect, useState } from 'react';
import { CircularProgress, Container, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Divider } from '@mui/material';
import { Actor } from '../../types/actor';
import { Movie } from '../../types/movie';
import { service } from '../../services/api';
import ListPaginator from '../../utils/ListPaginator';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import PreviewIcon from '@mui/icons-material/Preview';
import ActorFormDialog from './ActorFormDialog';
import SearchComponent from '../../utils/Search';

const ActorList: React.FC = () => {
    const [actors, setActors] = useState<Actor[]>([]);
    const [movies, setMovies] = useState<Movie[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [page, setPage] = useState<number>(0);
    const [rowsPerPage, setRowsPerPage] = useState<number>(5);
    const [openForm, setOpenForm] = useState<boolean>(false);
    const [newActor, setNewActor] = useState<Actor>({ id: 0, name: '' });
    const [editingActorId, setEditingActorId] = useState<number | null>(null);
    const [openDetailsDialog, setOpenDetailsDialog] = useState<boolean>(false);
    const [selectedActor, setSelectedActor] = useState<Actor | null>(null);
    const [query, setQuery] = useState<string>('');

    useEffect(() => {
        fetchActors();
        fetchMovies();
        if (openForm && !editingActorId) {
            setNewActor({ id: 0, name: '' });
        }
    }, [openForm, editingActorId, query]);

    const fetchActors = async () => {
        try {
            let fetchedActors: Actor[] = [];
            if (query) {
                fetchedActors = await service.actor.searchActors(query);
            } else {
                fetchedActors = await service.actor.getActors();
            }

            setActors(fetchedActors);
            setLoading(false);
        } catch (err) {
            setError('Failed to fetch actors');
            setLoading(false);
        }
    };

    const fetchMovies = async () => {
        try {
            const fetchedMovies = await service.movie.getMovies();
            setMovies(fetchedMovies);
        } catch (err) {
            setError('Failed to fetch movies');
        }
    };

    const handleDeleteActor = async (actorId: number) => {
        try {
            await service.actor.deleteActor(actorId);
            setActors(actors.filter(actor => actor.id !== actorId));
        } catch (err) {
            setError('Failed to delete actor');
        }
    };

    const handleChangePage = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleAddActor = async () => {
        try {
            const { name } = newActor;
            const newActorData: Actor = { id: 0, name };
            await service.actor.addActor(newActorData);
            setNewActor({ id: 0, name: '' });
            setOpenForm(false);
            fetchActors();
        } catch (err) {
            setError('Failed to add actor');
        }
    };

    const handleEditActor = async () => {
        try {
            const { id, name } = newActor;
            const updatedActor = { id, name };
            await service.actor.updateActor(editingActorId!, updatedActor);
            setOpenForm(false);
            fetchActors();
        } catch (err) {
            setError('Failed to update actor');
        }
    };

    const handleEditClick = (actor: Actor) => {
        setNewActor({
            id: actor.id,
            name: actor.name,
        });
        setEditingActorId(actor.id);
        setOpenForm(true);
    };

    const handleOpenDetailsDialog = (actor: Actor) => {
        setSelectedActor(actor);
        setOpenDetailsDialog(true);
    };

    const handleCloseDetailsDialog = () => {
        setOpenDetailsDialog(false);
        setSelectedActor(null);
    };

    const handleFormCancel = () => {
        setOpenForm(false);
        setNewActor({ id: 0, name: '' })
        setEditingActorId(undefined);
    };

    const getMoviesForActor = (actorId: number) => {
        return movies.filter(movie => movie.actors.some(actor => actor.id === actorId));
    };

    const handleSearch = (searchQuery: string) => {
        setQuery(searchQuery);
    };

    const emptyRows = page > 0 ? Math.max(0, (1 + page) * rowsPerPage - actors.length) : 0;

    return (
        <Container>
            <Divider sx={{ margin: '16px 0' }} />

            <SearchComponent onSearch={handleSearch} placeholder="Search by Actor Name" />

            <ActorFormDialog
                openForm={openForm}
                onCancel={handleFormCancel}
                handleAddActor={handleAddActor}
                newActor={newActor}
                setNewActor={setNewActor}
                editingActorId={editingActorId}
                handleUpdateActor={handleEditActor}
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
                        Actors
                    </Typography>
                    <Button variant="contained" color="primary" onClick={() => setOpenForm(true)}>
                        Add New Actor
                    </Button>
                    <Divider sx={{ margin: '16px 0' }} />

                    <TableContainer component={Paper}>
                        <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>Name</TableCell>
                                    <TableCell align="right">Movies</TableCell>
                                    <TableCell align="right">Actions</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {(rowsPerPage > 0
                                    ? actors.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                    : actors
                                ).map((actor) => (
                                    <TableRow key={actor.id}>
                                        <TableCell>{actor.name}</TableCell>
                                        <TableCell align="right">
                                            {/* Get the movies for the actor */}
                                            {getMoviesForActor(actor.id).map((movie) => movie.title).join(', ')}
                                        </TableCell>
                                        <TableCell align="right">
                                            <IconButton color="primary" onClick={() => handleOpenDetailsDialog(actor)}>
                                                <PreviewIcon />
                                            </IconButton>
                                            <IconButton color="primary" onClick={() => handleEditClick(actor)}>
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton color="secondary" onClick={() => handleDeleteActor(actor.id)}>
                                                <DeleteIcon />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                ))}
                                {emptyRows > 0 && (
                                    <TableRow style={{ height: 53 * emptyRows }}>
                                        <TableCell colSpan={3} />
                                    </TableRow>
                                )}
                            </TableBody>
                            <ListPaginator
                                count={actors.length}
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
                <DialogTitle>Actor Details</DialogTitle>
                <DialogContent>
                    {selectedActor && (
                        <div>
                            <Typography variant="h6">Name: {selectedActor.name}</Typography>
                            <Typography variant="body1">
                                Movies: {getMoviesForActor(selectedActor.id).map(movie => movie.title).join(', ')}
                            </Typography>
                        </div>
                    )}
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

export default ActorList;
