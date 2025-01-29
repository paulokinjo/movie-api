// src/services/service.ts
import * as movieApi from './Movies/movieApi';
import * as actorApi from './Actors/actorApi';

export const service = {
  movie: {
    getMovies: movieApi.getMovies,
    addMovie: movieApi.addMovie,
    updateMovie: movieApi.updateMovie,
    deleteMovie: movieApi.deleteMovie,
    getMovie: movieApi.getMovie,
    searchMovies: movieApi.searchMovies,
  },
  actor: {
    getActors: actorApi.getActors,
    addActor: actorApi.addActor,
    updateActor: actorApi.updateActor,
    deleteActor: actorApi.deleteActor,
    searchActors: actorApi.searchActors,
  }
};
