# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RestaurantManagementService.csproj", "."]
RUN dotnet restore "./RestaurantManagementService.csproj"
COPY . .
RUN dotnet build "./RestaurantManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestaurantManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080;https://0.0.0.0:8443

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]
