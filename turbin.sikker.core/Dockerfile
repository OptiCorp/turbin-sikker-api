FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["turbin.sikker.core/turbin.sikker.core.csproj", "./turbin.sikker.core"]
WORKDIR ./turbin.sikker.core
RUN dotnet restore 
COPY . .
WORKDIR "/src/turbin.sikker.core"
RUN dotnet build "turbin.sikker.core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "turbin.sikker.core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "localserverturbin.dll"]
