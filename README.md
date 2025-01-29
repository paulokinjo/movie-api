# Movie API Application

This project consists of a **Movie API** backend built with **ASP.NET Core 6.0**, a **React 18** frontend using **TypeScript**, and Docker containers for both parts. The API allows you to perform CRUD operations on movies and actors, with JWT-based authentication for secure access.

## Table of Contents

1. [Backend (API)](#backend-api)
2. [Frontend (React - TypeScript)](#frontend-react---typescript)
3. [Database](#database)
4. [Authentication & Security](#authentication--security)
5. [Testing & Debugging](#testing--debugging)
6. [Dockerization](#dockerization)

## Backend (API)

- **Framework**: ASP.NET Core 6.0
- **Authentication**: JWT-based authentication (Bearer tokens).
- **Controllers**: The API exposes endpoints for managing:
  - **Movies**: Create, Read, Update, Delete (CRUD operations).
  - **Actors**: Create, Read, Update, Delete (CRUD operations).
  - **Search functionality** for movies.
- **Services**: The backend includes service classes (`IMovieService`, `IActorService`) that handle business logic separately from the controllers.
- **Database**: The backend uses **Entity Framework Core** with an in-memory database during development. The database stores movies, actors, and ratings.
- **Swagger**: The backend has Swagger integrated for API documentation and testing.

## Frontend (React - TypeScript)

- **React 18** application with **TypeScript**.
- **Environment Variable**: The app uses `REACT_APP_API_BASE_URL` to dynamically point to the backend API, which is configured to be `host.docker.internal:5000` in the Docker setup.
- The frontend communicates with the backend API to display movie and actor information.

## Database

- **In-memory Database**: During development, an in-memory database is used with Entity Framework Core to store movies, actors, and ratings.
- **Persistent Database**: In production, you can switch to a persistent database (e.g., SQL Server, PostgreSQL).

## Authentication & Security

- The API uses **JWT tokens** for authentication. 
- **API Secret** is configured in the `appsettings.Development.json` file and injected into the application using **IOptions<AppSettings>**.
- The backend API validates tokens to secure the routes that require authentication.

## Testing & Debugging

- **Backend**: 
  - Unit tests and integration tests should be implemented for the API (e.g., using **xUnit** or **NUnit**).
  - Docker is used for local testing and debugging, allowing you to run the application in an isolated environment.
- **Frontend**: 
  - Unit and integration tests should be written for the React components using **Jest** and **React Testing Library**.

## Dockerization

Both the frontend and backend are containerized using Docker.
