# Movie API Application

This is a **Movie API** built with **ASP.NET Core** for the backend and a **React 18** frontend. The API allows you to manage movies and actors with CRUD operations, search functionality, and JWT-based authentication.

## How to Run the Application

### Option 1: Run Using Visual Studio (for Development)

1. Open the solution in **Visual Studio**.
2. Ensure your project is set to **Debug** mode.
3. Click the **Run** button (or press **F5**).
4. The backend API will start, and Swagger UI will be available for you to test the endpoints at:
> http://localhost:5125/swagger


### Option 2: Run Using Docker

If you prefer to run the application using Docker, follow the steps below.

#### Step 1: Build the Docker Image

Navigate to the root of the project in your terminal, and run the following command to build the Docker image:

```bash
docker build --build-arg ASPNETCORE_ENVIRONMENT=Development -t movieapi-server .
```

This command does the following:

> --build-arg ASPNETCORE_ENVIRONMENT=Development: Passes the environment variable ASPNETCORE_ENVIRONMENT to set the environment to Development.

> -t movieapi-server: Tags the image with the name movieapi-server.
#### Step 2: Run the Docker Container
Once the image is built, run the following command to start the container:

```bash
docker run --name movie-server --rm -p 5000:80 movieapi-server
This command does the following:
```
> --name movie-server: Names the running container movie-server.
> --rm: Automatically removes the container when it stops.
> -p 5000:80: Maps port 80 in the container to port 5000 on your local machine.

#### Step 3: Access Swagger UI
After the container starts, you can access the Swagger UI to interact with the API at:

```bash
http://localhost:5125/swagger
```
#### Important Notes
Make sure Docker is installed and running on your machine.
If you're running in Docker, ensure that you have exposed the right ports (5000 for the backend API).
You can modify the environment variable (ASPNETCORE_ENVIRONMENT) for other environments, such as Production, by changing the build argument in the docker build command.
API Endpoints
Once the application is running, the Movie API offers the following endpoints:

> GET /movies: Retrieve a list of all movies.

> GET /movies/{id}: Retrieve details of a specific movie.

> POST /movies: Create a new movie.

> PUT /movies/{id}: Update an existing movie.

> DELETE /movies/{id}: Delete a specific movie.

> GET /actors: Retrieve a list of all actors.

> GET /actors/{id}: Retrieve details of a specific actor.

> POST /actors: Create a new actor.

> PUT /actors/{id}: Update an existing actor.

> DELETE /actors/{id}: Delete a specific actor.

Refer to the Swagger UI for a more detailed view of the available endpoints and to test the API interactively.
