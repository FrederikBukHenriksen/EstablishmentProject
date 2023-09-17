# Use the official Node.js image as the base image
FROM node:latest AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the Angular app for production
RUN npm run build --prod

# Use a lightweight Nginx image as the final image
FROM nginx:alpine

# Copy the built Angular app to the Nginx web server directory
COPY --from=build /app/dist/app /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]