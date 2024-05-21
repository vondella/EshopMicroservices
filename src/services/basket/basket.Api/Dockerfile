#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/basket/basket.Api/basketApi.csproj", "services/basket/basket.Api/"]
COPY ["buildingBlock/buildingBlock/buildingBlock.csproj", "buildingBlock/buildingBlock/"]
RUN dotnet restore "./services/basket/basket.Api/./basketApi.csproj"
COPY . .
WORKDIR "/src/services/basket/basket.Api"
RUN dotnet build "./basketApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./basketApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "basketApi.dll"]