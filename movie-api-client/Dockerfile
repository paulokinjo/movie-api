# Use the official Node.js image
FROM node:22 AS build

# Set the working directory
WORKDIR /app

# Accept environment variable during build
ARG REACT_APP_API_URL="http://localhost:5125/api"
ENV REACT_APP_API_URL=$REACT_APP_API_URL

# Install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the application
COPY . .

# Build the application with the environment variable
RUN npm run build

# Serve the app
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
