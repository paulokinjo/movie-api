// src/types/movie.ts
import { Actor } from "./actor";
import { MovieRating } from "./movieRatings";

export interface Movie {
  id: number;
  title: string;
  year: number;
  actors: Actor[];
  ratings: MovieRating[];
}