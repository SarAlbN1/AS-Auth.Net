# TODO: revisar versión exacta de .NET que usen (6.0, 8.0, etc.)

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el código de la solución al contenedor
COPY . .

# Publicar solo el proyecto WebApp.UI
RUN dotnet publish src/WebApp.UI/WebApp.UI.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebApp.UI.dll"]
