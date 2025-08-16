# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Ambev.Ominia.Api/Ambev.Ominia.Api.csproj", "src/Ambev.Ominia.Api/"]
COPY ["src/Ambev.Ominia.Application/Ambev.Ominia.Application.csproj", "src/Ambev.Ominia.Application/"]
COPY ["src/Ambev.Ominia.Domain/Ambev.Ominia.Domain.csproj", "src/Ambev.Ominia.Domain/"]
COPY ["src/Ambev.Ominia.Crosscutting/Ambev.Ominia.Crosscutting.csproj", "src/Ambev.Ominia.Crosscutting/"]
COPY ["src/Ambev.Ominia.Persistence/Ambev.Ominia.Persistence.csproj", "src/Ambev.Ominia.Persistence/"]
COPY ["src/Ambev.Ominia.EventStore/Ambev.Ominia.EventStore.csproj", "src/Ambev.Ominia.EventStore/"]
COPY ["src/Ambev.Ominia.Messaging/Ambev.Ominia.Messaging.csproj", "src/Ambev.Ominia.Messaging/"]

# Restore dependencies
RUN dotnet restore "src/Ambev.Ominia.Api/Ambev.Ominia.Api.csproj"

# Copy all source files
COPY . .

# Set working directory
WORKDIR "/src/src/Ambev.Ominia.Api"

# Build the project
RUN dotnet build "./Ambev.Ominia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ambev.Ominia.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image to run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.Ominia.Api.dll"]