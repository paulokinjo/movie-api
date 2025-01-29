import React, { useState, useEffect } from 'react';
import { Button, Container, Typography, CircularProgress } from '@mui/material';
import MovieList from '../Movies/MovieList';
import ActorList from '../Actors/ActorList';
import api, { login } from '../../axiosInstance'; // Assuming you have an axios instance setup
import { getAuthToken, removeAuthToken } from '../../utils/auth';

const Home: React.FC = () => {
    const [currentView, setCurrentView] = useState<'movies' | 'actors'>('movies');
    const [isLoading, setIsLoading] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(!!getAuthToken());

    useEffect(() => {
        console.log(process.env.REACT_APP_API_URL);
        setIsLoggedIn(!!getAuthToken());
    }, []);

    const handleShowMovies = () => {
        setCurrentView('movies');
    };

    const handleShowActors = () => {
        setCurrentView('actors');
    };

    const handleLogin = async () => {
        try {
            setIsLoading(true);

            await login();

            setIsLoggedIn(true);
            setIsLoading(false);

        } catch (error) {
            console.error('Login failed', error);
            setIsLoading(false);
        }
    };

    const handleLogout = () => {
        removeAuthToken();
        setIsLoggedIn(false);
    };

    const ViewButton: React.FC<{ label: string, view: 'movies' | 'actors', onClick: () => void }> = ({ label, view, onClick }) => (
        <Button
            variant="contained"
            color={view === 'movies' ? 'primary' : 'secondary'}
            onClick={onClick}
            sx={{ marginRight: 2 }}
            aria-label={`Show ${label}`}
        >
            {label}
        </Button>
    );

    return (
        <Container>
            <Typography variant="h3" gutterBottom>
                Welcome to the Movie App!
            </Typography>

            {/* Buttons to toggle between views */}
            <div>
                <ViewButton label="Movies" view="movies" onClick={handleShowMovies} />
                <ViewButton label="Actors" view="actors" onClick={handleShowActors} />
            </div>

            {/* Login/Logout button */}
            <div style={{ display: 'flex', justifyContent: 'flex-end', marginTop: '20px' }}>
                {isLoggedIn ? (
                    <Button variant="outlined" color="secondary" onClick={handleLogout}>
                        Logout
                    </Button>
                ) : (
                    <Button variant="contained" color="primary" onClick={handleLogin} disabled={isLoading}>
                        {isLoading ? <CircularProgress size={24} /> : 'Login'}
                    </Button>
                )}
            </div>

            {/* Conditional rendering of MovieList or ActorsList */}
            {isLoading ? (
                <CircularProgress />
            ) : currentView === 'movies' ? (
                <MovieList />
            ) : (
                <ActorList />
            )}
        </Container>
    );
};

export default Home;
