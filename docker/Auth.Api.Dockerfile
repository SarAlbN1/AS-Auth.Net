FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Auth.Api/Auth.Api.csproj", "src/Auth.Api/"]
COPY ["src/Auth.Business/Auth.Business.csproj", "src/Auth.Business/"]
COPY ["src/Auth.Data/Auth.Data.csproj", "src/Auth.Data/"]

RUN dotnet restore "src/Auth.Api/Auth.Api.csproj"

COPY . .
WORKDIR "/src/src/Auth.Api"
RUN dotnet publish "Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Auth.Api.dll"]
