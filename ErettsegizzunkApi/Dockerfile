# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7066
EXPOSE 5000


# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["ErettsegizzunkApi/ErettsegizzunkApi.csproj", "ErettsegizzunkApi/"]
COPY Certs /Certs
WORKDIR /src/ErettsegizzunkApi
RUN dotnet restore

# Copy the entire project and build
COPY . .
RUN dotnet build "ErettsegizzunkApi/ErettsegizzunkApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ErettsegizzunkApi/ErettsegizzunkApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ErettsegizzunkApi.dll"]

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_URLS=https://+:7066

