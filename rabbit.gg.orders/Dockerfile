# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./rabbit.gg.orders.csproj" --disable-parallel
RUN dotnet publish "./rabbit.gg.orders.csproj" -c release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "rabbit.gg.orders.dll"]
