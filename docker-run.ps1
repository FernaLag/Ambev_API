# Script to run the Ominia application with Docker
# Usage: .\docker-run.ps1 [-api] [-postgres] [-mongodb] [-rabbitmq]
# If no parameters are provided, all containers will be started

param(
    [switch]$api,
    [switch]$postgres,
    [switch]$mongodb,
    [switch]$rabbitmq,
    [switch]$help
)

# Load environment variables from .env file
if (Test-Path ".env") {
    Get-Content ".env" | ForEach-Object {
        if ($_ -match "^([^#][^=]*)=(.*)$") {
            [Environment]::SetEnvironmentVariable($matches[1], $matches[2], "Process")
        }
    }
}

if ($help) {
    Write-Host "Usage: .\docker-run.ps1 [-api] [-postgres] [-mongodb] [-rabbitmq]" -ForegroundColor Green
    Write-Host "Options:" -ForegroundColor Yellow
    Write-Host "  -api       Start only the API container" -ForegroundColor Cyan
    Write-Host "  -postgres  Start only the PostgreSQL container" -ForegroundColor Cyan
    Write-Host "  -mongodb   Start only the MongoDB container" -ForegroundColor Cyan
    Write-Host "  -rabbitmq  Start only the RabbitMQ container" -ForegroundColor Cyan
    Write-Host "  -help      Show this help message" -ForegroundColor Cyan
    Write-Host "" -ForegroundColor White
    Write-Host "If no options are provided, all containers will be started." -ForegroundColor Yellow
    exit 0
}

Write-Host "=== Ambev Ominia API - Docker Setup ===" -ForegroundColor Green

# Determine which services to run
$services = @()
if ($api) { $services += "ambev.ominia.api" }
if ($postgres) { $services += "postgres" }
if ($mongodb) { $services += "mongodb" }
if ($rabbitmq) { $services += "rabbitmq" }

# If no specific services selected, run all
if ($services.Count -eq 0) {
    Write-Host "No specific services selected. Starting all containers..." -ForegroundColor Yellow
    $runAll = $true
} else {
    Write-Host "Starting selected services: $($services -join ', ')" -ForegroundColor Yellow
    $runAll = $false
}

# Stop existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
if ($runAll) {
    docker-compose down
} else {
    foreach ($service in $services) {
        docker-compose stop $service
        docker-compose rm -f $service
    }
}

# Remove old API images if API is being started
if ($runAll -or $api) {
    Write-Host "Removing old API images..." -ForegroundColor Yellow
    docker rmi ambev_ominia_api -f 2>$null
}

# Build and start containers
Write-Host "Building and starting containers..." -ForegroundColor Yellow
if ($runAll) {
    docker-compose up --build -d
} else {
    $serviceArgs = $services -join " "
    Invoke-Expression "docker-compose up --build -d $serviceArgs"
}

# Wait for containers to be ready (only when starting all containers)
if ($runAll) {
    Write-Host "Waiting for containers to be ready..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
}

# Check container status
Write-Host "Container status:" -ForegroundColor Green
docker-compose ps

Write-Host ""
Write-Host "=== Application available at: ===" -ForegroundColor Green
if ($runAll -or $api) {
    Write-Host "API: http://localhost:8080" -ForegroundColor Cyan
    Write-Host "Swagger: http://localhost:8080/swagger" -ForegroundColor Cyan
}
if ($runAll -or $rabbitmq) {
    Write-Host "RabbitMQ Management: http://localhost:15672 (user: $env:RABBITMQ_USER, pass: $env:RABBITMQ_PASSWORD)" -ForegroundColor Cyan
}
Write-Host ""
Write-Host "To stop containers: docker-compose down" -ForegroundColor Yellow
Write-Host "To view logs: docker-compose logs -f" -ForegroundColor Yellow