FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RestaurantManagementService/RestaurantManagementService.csproj", "RestaurantManagementService/"]

RUN dotnet restore "RestaurantManagementService/RestaurantManagementService.csproj"

COPY . .

WORKDIR "/src/RestaurantManagementService"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "RestaurantManagementService.dll"]