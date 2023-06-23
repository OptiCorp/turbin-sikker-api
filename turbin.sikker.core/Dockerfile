FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

COPY *.sln .
COPY *.csproj .
RUN dotnet restore

FROM build AS publish
RUN dotnet publish "turbin.sikker.core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "localserverturbin.dll"]
