# ���� 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ../CinemaManagementSystem/CinemaManagementSystem.csproj CinemaManagementSystem/
COPY ../Core/CinemaManagementSystem.Core.csproj Data/
COPY ../Infrastructure/CinemaManagementSystem.Infrastructure.csproj CinemaManagementSystem.Infrastructure/
COPY ../WebAPI/CinemaManagementSystem.WebAPI.csproj CinemaManagementSystem.WebAPI/
COPY ../Application/Application.csproj Application/
COPY ../Contracts/Contracts.csproj Contracts/
COPY ../Shared/Shared.csproj Shared/

RUN dotnet restore CinemaManagementSystem.WebAPI/CinemaManagementSystem.WebAPI.csproj

COPY . .
WORKDIR /src/WebAPI
RUN dotnet publish "CinemaManagementSystem.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ���� 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "CinemaManagementSystem.WebAPI.dll"]
