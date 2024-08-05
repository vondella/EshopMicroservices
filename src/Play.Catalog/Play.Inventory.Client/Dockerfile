#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Play.Catalog/Play.Inventory.Client/Play.Inventory.Client.csproj", "Play.Catalog/Play.Inventory.Client/"]
RUN dotnet restore "./Play.Catalog/Play.Inventory.Client/./Play.Inventory.Client.csproj"
COPY . .
WORKDIR "/src/Play.Catalog/Play.Inventory.Client"
RUN dotnet build "./Play.Inventory.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Play.Inventory.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Play.Inventory.Client.dll"]