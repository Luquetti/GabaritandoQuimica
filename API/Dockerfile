# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy project files in dependency order
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["API/API.csproj", "API/"]

# Copy solution file if exists
COPY ["GabaritandoQuimica.sln", "./"]

# Restore NuGet packages
RUN dotnet restore "API/API.csproj"

# Copy all source code
COPY ["Domain/", "Domain/"]
COPY ["Infra/", "Infra/"]
COPY ["API/", "API/"]

# Build the application
RUN dotnet build "API/API.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish --no-restore

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Install curl for health checks
RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Create non-root user for security
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
RUN chown -R appuser:appgroup /app
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Add health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Set the entry point
ENTRYPOINT ["dotnet", "API.dll"]