#=== Build stage ===
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# COPY NotificationService.csproj ./
# COPY appsettings.json ./

# RUN dotnet restore NotificationService.csproj

# COPY . .
# RUN dotnet publish NotificationService.csproj -c Release -o /app/publish

# # === Runtime stage ===
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# WORKDIR /app
# COPY --from=build /app/publish .

# ENTRYPOINT ["dotnet", "NotificationService.dll"]
# Use the official .NET SDK image for build

# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
# WORKDIR /src

# # Копіюємо csproj файл (шлях відносно контексту, тобто без NotificationService/)
# COPY NotificationService.csproj ./

# # Відновлюємо залежності
# RUN dotnet restore NotificationService.csproj

# # Копіюємо увесь код проекту
# COPY . ./

# # Збираємо проєкт у Release
# RUN dotnet publish NotificationService.csproj -c Release -o /app/publish

# # Use runtime image for final container
# FROM mcr.microsoft.com/dotnet/aspnet:7.0
# WORKDIR /app

# # Копіюємо зібраний додаток з build stage
# COPY --from=build-env /app/publish .

# # Копіюємо конфіг (якщо треба, і він у цій папці)
# COPY appsettings.json ./

# # Відкриваємо порт, якщо потрібно
# EXPOSE 80

# # Запускаємо додаток
# ENTRYPOINT ["dotnet", "NotificationService.dll"]



# === Build stage ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо всі необхідні проекти збереженням структури
COPY Common/Common.csproj Common/
COPY Contracts/Contracts.csproj Contracts/
COPY NotificationService/NotificationService.csproj NotificationService/

RUN dotnet restore NotificationService/NotificationService.csproj

# Копіюємо всі файли (уже після restore)
COPY . .

WORKDIR /src/NotificationService
RUN dotnet publish NotificationService.csproj -c Release -o /app/publish

# === Runtime stage ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NotificationService.dll"]
