# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for better caching
COPY Domain/Domain.csproj Domain/
COPY Infra/Infra.csproj Infra/
COPY API/API.csproj API/

# Copy solution file
COPY GabaritandoQuimica.sln ./

# Restore dependencies
RUN dotnet restore API/API.csproj

# Copy source code
COPY Domain/ Domain/
COPY Infra/ Infra/
COPY API/ API/

# Build application
RUN dotnet build API/API.csproj -c Release -o /app/build --no-restore

# Publish application (removendo --no-build para garantir)
RUN dotnet publish API/API.csproj -c Release -o /app/publish --no-restore

# EF Tools stage - Install dotnet-ef in SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS ef-tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy EF Tools from ef-tools stage
COPY --from=ef-tools /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy published application
COPY --from=build /app/publish .

# Create non-root user for security
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "API.dll"]