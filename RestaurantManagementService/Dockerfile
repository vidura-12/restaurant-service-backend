# Use the official .NET 8.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and project files from the root context
COPY *.sln .
COPY RestaurantManagementService/*.csproj ./RestaurantManagementService/
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Move into the project directory to publish
WORKDIR /app/RestaurantManagementService
RUN dotnet publish -c Release -o /out

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Set environment variables for the container
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]