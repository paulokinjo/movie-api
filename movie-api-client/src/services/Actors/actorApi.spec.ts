import api from '../../axiosInstance';
import { Actor } from '../../types/actor';
import * as actorApi from './actorApi';

jest.mock('../../axiosInstance');

describe('actorApi', () => {
    const mockActors: Actor[] = [
        { id: 1, name: 'Actor 1' },
        { id: 2, name: 'Actor 2' },
    ];

    const mockActor: Actor = { id: 1, name: 'Actor 1' };

    it('should fetch actors', async () => {
        (api.get as jest.Mock).mockResolvedValue({ data: mockActors });

        const result = await actorApi.getActors();
        expect(result).toEqual(mockActors);
        expect(api.get).toHaveBeenCalledWith('/actor');
    });

    it('should handle errors when fetching actors', async () => {
        (api.get as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(actorApi.getActors()).rejects.toThrow('Request failed');
    });

    it('should add an actor', async () => {
        (api.post as jest.Mock).mockResolvedValue({ data: mockActor });

        const result = await actorApi.addActor(mockActor);
        expect(result).toEqual(mockActor);
        expect(api.post).toHaveBeenCalledWith('/actor', mockActor);
    });

    it('should handle errors when adding an actor', async () => {
        (api.post as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(actorApi.addActor(mockActor)).rejects.toThrow('Request failed');
    });

    it('should update an actor', async () => {
        const updatedActor: Actor = { id: 1, name: 'Updated Actor' };
        (api.put as jest.Mock).mockResolvedValue({ data: updatedActor });

        const result = await actorApi.updateActor(mockActor.id, updatedActor);
        expect(result).toEqual(updatedActor);
        expect(api.put).toHaveBeenCalledWith(`/actor/${mockActor.id}`, updatedActor);
    });

    it('should handle errors when updating an actor', async () => {
        const updatedActor: Actor = { id: 1, name: 'Updated Actor' };
        (api.put as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(actorApi.updateActor(mockActor.id, updatedActor)).rejects.toThrow('Request failed');
    });

    it('should delete an actor', async () => {
        (api.delete as jest.Mock).mockResolvedValue({});

        await actorApi.deleteActor(mockActor.id);
        expect(api.delete).toHaveBeenCalledWith(`/actor/${mockActor.id}`);
    });

    it('should handle errors when deleting an actor', async () => {
        (api.delete as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(actorApi.deleteActor(mockActor.id)).rejects.toThrow('Request failed');
    });

    it('should search actors', async () => {
        const query = 'action';
        (api.get as jest.Mock).mockResolvedValue({ data: mockActors });

        const result = await actorApi.searchActors(query);
        expect(result).toEqual(mockActors);
        expect(api.get).toHaveBeenCalledWith(`/actor/search?query=${query}`);
    });

    it('should handle errors when searching for actors', async () => {
        const query = 'action';
        (api.get as jest.Mock).mockRejectedValue(new Error('Request failed'));

        await expect(actorApi.searchActors(query)).rejects.toThrow('Request failed');
    });
});
