// src/services/actorApi.ts
import api from '../../axiosInstance';
import { AxiosResponse } from 'axios';
import { Actor } from '../../types/actor';

export const getActors = async (): Promise<Actor[]> => {
    const response: AxiosResponse<Actor[]> = await api.get('/actor');
    return response.data;
};

export const addActor = async (actorData: Actor): Promise<Actor> => {
    const response: AxiosResponse<Actor> = await api.post('/actor', actorData);
    return response.data;
};

export const updateActor = async (actorId: number, updatedActor: Actor): Promise<Actor> => {
    const response: AxiosResponse<Actor> = await api.put(`/actor/${actorId}`, updatedActor);
    return response.data;
};

export const deleteActor = async (actorId: number): Promise<void> => {
    await api.delete(`/actor/${actorId}`);
};

export const searchActors = async (query: string): Promise<Actor[]> => {
    const response: AxiosResponse<Actor[]> = await api.get(`/actor/search?query=${query}`);
    return response.data;
};
