import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from '@mui/material';
import { Actor } from '../../types/actor';

interface ActorFormDialogProps {
    openForm: boolean;
    onCancel: () => void;
    handleAddActor: () => void;
    handleUpdateActor: () => void;
    newActor: Actor;
    setNewActor: React.Dispatch<React.SetStateAction<Actor>>;
    editingActorId: number | null;
}

const ActorFormDialog: React.FC<ActorFormDialogProps> = ({
    openForm,
    onCancel,
    handleAddActor,
    handleUpdateActor,
    newActor,
    setNewActor,
    editingActorId,
}) => {
    const [error, setError] = useState('');

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setNewActor((prevActor) => ({
            ...prevActor,
            [name]: value,
        }));
    };

    const handleSubmit = () => {
        if (!newActor.name) {
            setError('Actor name is required.');
            return;
        }

        setError('');

        if (editingActorId) {
            handleUpdateActor();
        } else {
            handleAddActor();
        }
    };

    return (
        <Dialog open={openForm} onClose={() => { onCancel() }}>
            <DialogTitle>{editingActorId ? 'Edit Actor' : 'Add New Actor'}</DialogTitle>
            <DialogContent>
                {/* Actor Name Field */}
                <TextField
                    label="Actor Name"
                    name="name"
                    value={newActor.name}
                    onChange={handleInputChange}
                    fullWidth
                    margin="normal"
                    placeholder='Actor Name'
                    required
                    error={!newActor.name}
                    helperText={!newActor.name ? 'Actor name is required' : ''}
                />

                {error && <div style={{ color: 'red', marginTop: 10 }}>{error}</div>}
            </DialogContent>
            <DialogActions>
                <Button onClick={() => onCancel()} color="secondary">
                    Cancel
                </Button>
                <Button onClick={handleSubmit} color="primary">
                    {editingActorId ? 'Update Actor' : 'Add Actor'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ActorFormDialog;
