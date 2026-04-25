FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy the project file specifically (matching your screenshot name)
COPY ["RestaurantManagementService/RestaurantManagementService.csproj", "RestaurantManagementService/"]

# 2. Restore dependencies
RUN dotnet restore "RestaurantManagementService/RestaurantManagementService.csproj"

# 3. Copy the entire source code
COPY . .

# 4. Build and Publish
WORKDIR "/src/RestaurantManagementService"
RUN dotnet publish "RestaurantManagementService.csproj" -c Release -o /app/publish

# 5. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]