FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . ./turbin.sikker.core/
WORKDIR /source/turbin.sikker.core
RUN dotnet publish -c release -r linux-x64 --self-contained true -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "localserverturbin.dll"]
