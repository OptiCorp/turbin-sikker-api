# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# Copy and restore NuGet dependencies
COPY turbin.sikker.core/*.sln ./turbin.sikker.core/
COPY turbin.sikker.core/*.csproj ./turbin.sikker.core/
WORKDIR /turbin.sikker.core
RUN dotnet restore

# Copy the rest of the source code and build the application
COPY turbin.sikker.core ./turbin.sikker.core/
WORKDIR /source/turbin.sikker.core
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o /app --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "turbin.sikker.core.dll"]
