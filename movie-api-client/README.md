# Movie API Application

This is a **Movie API** built with **ASP.NET Core** for the backend and a **React 18** frontend. The API allows you to manage movies and actors with CRUD operations, search functionality, and JWT-based authentication.

## Table of Contents

- [How to Run the Application](#how-to-run-the-application)
  - [Option 1: Run Using NPM](#option-1-run-using-npm)
  - [Option 2: Run Using Docker](#option-2-run-using-docker)
    - [Step 1: Build the Docker Image](#step-1-build-the-docker-image)
    - [Step 2: Run the Docker Container](#step-2-run-the-docker-container)
- [API Endpoints](#api-endpoints)
- [Dockerfile](#dockerfile)
- [License](#license)

## How to Run the Application

### Option 1: Run Using NPM

1. **Install Dependencies**  
   First, install all required dependencies by running the following command in your project’s root directory:

   ```bash
   npm install

2. **Start the Frontend (React)**  
   Navigate to the React app folder in your terminal and run the following command to start the React development server:

   ```bash
    npm start
    ```
    **By default, the frontend should be accessible at:**

    ```bash
    http://localhost:3000

### Option 2: Run Using Docker

To run the application using Docker, follow these steps.

1. **Build the Docker Image**  
    ```bash
    docker build --build-arg REACT_APP_API_URL="http://host.docker.internal:5000/api" -t reactweb .

2. **RUN the Docker Image**  
    ```bash
    docker run --rm --name web -p 3000:80 -e REACT_APP_API_URL="http://host.docker.internal:5000/api" reactweb

Once the containers are up, access the frontend at:

```bash
http://localhost:3000