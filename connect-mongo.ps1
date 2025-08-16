# MongoDB Connection Script for Windows PowerShell
# This script connects to the MongoDB container with proper authentication

# MongoDB connection parameters from .env
$MongoUser = $env:MONGO_USER
$MongoPassword = $env:MONGO_PASSWORD
$MongoHost = "localhost"
$MongoPort = $env:MONGO_PORT
$MongoDatabase = $env:MONGO_DATABASE
$AuthDatabase = "admin"

# Colors for output
$Red = "Red"
$Green = "Green"
$Yellow = "Yellow"
$Blue = "Blue"
$White = "White"

Write-Host "=== MongoDB Connection Script ===" -ForegroundColor $Blue
Write-Host "Connecting to MongoDB..." -ForegroundColor $Yellow
Write-Host "Host: $MongoHost`:$MongoPort" -ForegroundColor $White
Write-Host "Database: $MongoDatabase" -ForegroundColor $White
Write-Host "User: $MongoUser" -ForegroundColor $White
Write-Host ""

# Check if mongosh is available
try {
    $null = Get-Command mongosh -ErrorAction Stop
} catch {
    Write-Host "Error: mongosh is not installed or not in PATH" -ForegroundColor $Red
    Write-Host "Please install MongoDB Shell (mongosh) first" -ForegroundColor $Yellow
    Write-Host "Visit: https://www.mongodb.com/try/download/shell"
    exit 1
}

# Check if MongoDB container is running
$containerRunning = docker ps --format "table {{.Names}}" | Select-String "ambev_ominia_mongodb"
if (-not $containerRunning) {
    Write-Host "Error: MongoDB container 'ambev_ominia_mongodb' is not running" -ForegroundColor $Red
    Write-Host "Please start the Docker containers first:" -ForegroundColor $Yellow
    Write-Host "  .\docker-run.ps1"
    exit 1
}

Write-Host "Connecting to MongoDB..." -ForegroundColor $Green
Write-Host "Useful commands after connection:" -ForegroundColor $Yellow
Write-Host "  show dbs                    - List all databases"
Write-Host "  use EventStore              - Switch to EventStore database"
Write-Host "  show collections            - List collections in current database"
Write-Host "  db.events.find().pretty()   - View events in pretty format"
Write-Host "  db.events.countDocuments()  - Count total events"
Write-Host "  exit                        - Exit MongoDB shell"
Write-Host ""

# Connect to MongoDB with authentication
$connectionString = "mongodb://$MongoUser`:$MongoPassword@$MongoHost`:$MongoPort/$MongoDatabase`?authSource=$AuthDatabase"
mongosh $connectionString

# Check exit status
if ($LASTEXITCODE -eq 0) {
    Write-Host "MongoDB connection closed successfully" -ForegroundColor $Green
} else {
    Write-Host "MongoDB connection failed" -ForegroundColor $Red
    Write-Host "Troubleshooting:" -ForegroundColor $Yellow
    Write-Host "1. Verify MongoDB container is running: docker ps"
    Write-Host "2. Check container logs: docker logs ambev_ominia_mongodb"
    Write-Host "3. Verify credentials in .env file"
}