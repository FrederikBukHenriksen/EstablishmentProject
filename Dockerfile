# Use the official .NET SDK as a base image for building
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY MyCSharpApp/*.csproj .
RUN dotnet restore

# Copy the remaining source code
COPY MyCSharpApp/ .

# Build the application
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out .

# Define the command to run your app
ENTRYPOINT ["dotnet", "MyCSharpApp.dll"]