# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and project files
COPY FooBooLoo.sln .
COPY FooBooLooGameAPI/FooBooLooGameAPI.csproj FooBooLooGameAPI/
COPY FooBooLooGameAPI.Tests/FooBooLooGameAPI.Tests.csproj FooBooLooGameAPI.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application code
COPY . . 

# Build and publish the application
RUN dotnet publish FooBooLooGameAPI/FooBooLooGameAPI.csproj -c Release -o out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app runs on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "FooBooLooGameAPI.dll"]
