import React, { useState } from 'react';
import { render, fireEvent, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import ActorFormDialog from './ActorFormDialog';
import { Actor } from '../../types/actor';

jest.mock('../../services/Actors/actorApi', () => ({
    handleAddActor: jest.fn(),
    handleUpdateActor: jest.fn(),
}));

describe('ActorFormDialog', () => {
    const mockSetNewActor = jest.fn();

    const mockOnCancel = jest.fn();
    const mockHandleAddActor = jest.fn();
    const mockHandleUpdateActor = jest.fn();

    const defaultActor: Actor = { id: 0, name: '' };

    it('should render the dialog and handle input change for adding a new actor', async () => {
        render(
            <ActorFormDialog
                openForm={true}
                onCancel={mockOnCancel}
                handleAddActor={mockHandleAddActor}
                handleUpdateActor={mockHandleUpdateActor}
                newActor={defaultActor}
                setNewActor={mockSetNewActor}
                editingActorId={null}
            />
        );

        expect(screen.getByText('Add New Actor')).toBeInTheDocument();
        expect(screen.getByPlaceholderText('Actor Name')).toBeInTheDocument();
        expect(screen.getByText('Add Actor')).toBeInTheDocument();

        const input = screen.getByPlaceholderText('Actor Name');
        fireEvent.change(input, { target: { value: 'New Actor' } });

        fireEvent.click(screen.getByText('Add Actor'));

        await waitFor(() => {
            expect(mockSetNewActor).toHaveBeenCalled();
        });
    });

    it('should show an error if the actor name is empty and prevent submit', () => {
        render(
            <ActorFormDialog
                openForm={true}
                onCancel={mockOnCancel}
                handleAddActor={mockHandleAddActor}
                handleUpdateActor={mockHandleUpdateActor}
                newActor={defaultActor}
                setNewActor={mockSetNewActor}
                editingActorId={null}
            />
        );

        fireEvent.click(screen.getByText('Add Actor'));

        expect(screen.getByText('Actor name is required.')).toBeInTheDocument();
        expect(mockHandleAddActor).not.toHaveBeenCalled();
    });

    it('should render the dialog and handle input change for updating an actor', async () => {
        const actorToEdit: Actor = { id: 1, name: 'Actor 1' };

        render(
            <ActorFormDialog
                openForm={true}
                onCancel={mockOnCancel}
                handleAddActor={mockHandleAddActor}
                handleUpdateActor={mockHandleUpdateActor}
                newActor={actorToEdit}
                setNewActor={mockSetNewActor}
                editingActorId={actorToEdit.id}
            />
        );

        expect(screen.getByText('Edit Actor')).toBeInTheDocument();

        const input = screen.getByPlaceholderText('Actor Name');
        fireEvent.change(input, { target: { value: 'Updated Actor' } });

        fireEvent.click(screen.getByText('Update Actor'));

        await waitFor(() => {
            expect(mockHandleUpdateActor).toHaveBeenCalled(); // Check if the update actor function was called
        });
    });

    it('should call onCancel when cancel button is clicked', () => {
        render(
            <ActorFormDialog
                openForm={true}
                onCancel={mockOnCancel}
                handleAddActor={mockHandleAddActor}
                handleUpdateActor={mockHandleUpdateActor}
                newActor={defaultActor}
                setNewActor={mockSetNewActor}
                editingActorId={null}
            />
        );

        fireEvent.click(screen.getByText('Cancel'));

        expect(mockOnCancel).toHaveBeenCalled();
    });
});
