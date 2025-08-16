#!/bin/bash

# MongoDB Connection Script for Linux/Mac
# This script connects to the MongoDB container with proper authentication

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# MongoDB connection parameters from .env
MONGO_USER="${MONGO_USER:-admin}"
MONGO_PASSWORD="${MONGO_PASSWORD:-admin123}"
MONGO_HOST="localhost"
MONGO_PORT="${MONGO_PORT:-27017}"
MONGO_DATABASE="${MONGO_DATABASE:-ominia_eventstore}"
AUTH_DATABASE="admin"

echo -e "${BLUE}=== MongoDB Connection Script ===${NC}"
echo -e "${YELLOW}Connecting to MongoDB...${NC}"
echo -e "Host: ${MONGO_HOST}:${MONGO_PORT}"
echo -e "Database: ${MONGO_DATABASE}"
echo -e "User: ${MONGO_USER}"
echo ""

# Check if mongosh is available
if ! command -v mongosh &> /dev/null; then
    echo -e "${RED}Error: mongosh is not installed or not in PATH${NC}"
    echo -e "${YELLOW}Please install MongoDB Shell (mongosh) first${NC}"
    echo "Visit: https://www.mongodb.com/try/download/shell"
    exit 1
fi

# Check if MongoDB container is running
if ! docker ps | grep -q "ambev_ominia_mongodb"; then
    echo -e "${RED}Error: MongoDB container 'ambev_ominia_mongodb' is not running${NC}"
    echo -e "${YELLOW}Please start the Docker containers first:${NC}"
    echo "  ./docker-run.sh"
    exit 1
fi

echo -e "${GREEN}Connecting to MongoDB...${NC}"
echo -e "${YELLOW}Useful commands after connection:${NC}"
echo "  show dbs                    - List all databases"
echo "  use EventStore              - Switch to EventStore database"
echo "  show collections            - List collections in current database"
echo "  db.events.find().pretty()   - View events in pretty format"
echo "  db.events.countDocuments()  - Count total events"
echo "  exit                        - Exit MongoDB shell"
echo ""

# Connect to MongoDB with authentication
mongosh "mongodb://${MONGO_USER}:${MONGO_PASSWORD}@${MONGO_HOST}:${MONGO_PORT}/${MONGO_DATABASE}?authSource=${AUTH_DATABASE}"

# Check exit status
if [ $? -eq 0 ]; then
    echo -e "${GREEN}MongoDB connection closed successfully${NC}"
else
    echo -e "${RED}MongoDB connection failed${NC}"
    echo -e "${YELLOW}Troubleshooting:${NC}"
    echo "1. Verify MongoDB container is running: docker ps"
    echo "2. Check container logs: docker logs ambev_ominia_mongodb"
    echo "3. Verify credentials in .env file"
fi