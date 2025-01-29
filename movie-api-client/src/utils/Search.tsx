// src/components/SearchComponent.tsx
import React, { useState } from 'react';
import { Button, TextField, Box } from '@mui/material';

interface SearchComponentProps {
    onSearch: (query: string) => void;
    placeholder: string;
}

const SearchComponent: React.FC<SearchComponentProps> = ({ onSearch, placeholder }) => {
    const [query, setQuery] = useState('');

    const handleSearch = () => {
        onSearch(query);
    };

    return (
        <Box display="flex" alignItems="center" marginBottom={2}>
            <TextField
                label={placeholder}
                variant="outlined"
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                fullWidth
            />
            <Button variant="contained" color="primary" onClick={handleSearch} style={{ marginLeft: 8 }}>
                Search
            </Button>
        </Box>
    );
};

export default SearchComponent;
