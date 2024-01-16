# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy everything and restore dependencies
COPY . .

RUN dir /app
# Restore the dependencies
RUN dotnet restore "./PetService.API/PetService.API.csproj"

# Build and publish the application
RUN dotnet publish "./PetService.API/PetService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "PetService.API.dll"]
