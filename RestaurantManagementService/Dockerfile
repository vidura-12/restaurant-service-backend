FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy the project file from the CURRENT directory
# Since Dockerfile is in the same folder as the .csproj, just use "."
COPY ["RestaurantManagementService.csproj", "./"]
COPY ["RestaurantManagementService/RestaurantManagementService.csproj", "./"]
# 2. Restore dependencies
RUN dotnet restore "RestaurantManagementService.csproj"

# 3. Copy everything else from the current folder
COPY . .

# 4. Build and Publish
# No need to change WORKDIR again since we are already in /src
RUN dotnet publish "RestaurantManagementService.csproj" -c Release -o /app/publish

# 5. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]