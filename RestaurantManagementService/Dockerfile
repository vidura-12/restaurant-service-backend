FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copy the solution and project file
COPY *.sln .
COPY RestaurantManagementService/*.csproj ./RestaurantManagementService/

# 2. Restore (This will now find the files)
RUN dotnet restore

# 3. Copy everything else
COPY . .

# 4. Build
WORKDIR /app/RestaurantManagementService
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]