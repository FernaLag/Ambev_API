#!/bin/bash

# Script to run the Ominia application with Docker
# Usage: ./docker-run.sh [-api] [-postgres] [-mongodb] [-rabbitmq] [-help]
# If no parameters are provided, all containers will be started

# Load environment variables from .env file
if [ -f ".env" ]; then
    export $(grep -v '^#' .env | xargs)
fi

# Parse command line arguments
API=false
POSTGRES=false
MONGODB=false
RABBITMQ=false
HELP=false
RUN_ALL=true

while [[ $# -gt 0 ]]; do
    case $1 in
        -api)
            API=true
            RUN_ALL=false
            shift
            ;;
        -postgres)
            POSTGRES=true
            RUN_ALL=false
            shift
            ;;
        -mongodb)
            MONGODB=true
            RUN_ALL=false
            shift
            ;;
        -rabbitmq)
            RABBITMQ=true
            RUN_ALL=false
            shift
            ;;
        -help|--help|-h)
            HELP=true
            shift
            ;;
        *)
            echo "Unknown option $1"
            HELP=true
            shift
            ;;
    esac
done

if [ "$HELP" = true ]; then
    echo -e "\033[0;32mUsage: ./docker-run.sh [-api] [-postgres] [-mongodb] [-rabbitmq]\033[0m"
    echo -e "\033[1;33mOptions:\033[0m"
    echo -e "\033[0;36m  -api       Start only the API container\033[0m"
    echo -e "\033[0;36m  -postgres  Start only the PostgreSQL container\033[0m"
    echo -e "\033[0;36m  -mongodb   Start only the MongoDB container\033[0m"
    echo -e "\033[0;36m  -rabbitmq  Start only the RabbitMQ container\033[0m"
    echo -e "\033[0;36m  -help      Show this help message\033[0m"
    echo
    echo -e "\033[1;33mIf no options are provided, all containers will be started.\033[0m"
    exit 0
fi

echo -e "\033[0;32m=== Ambev Ominia API - Docker Setup ===\033[0m"

# Determine which services to run
SERVICES=()
if [ "$API" = true ]; then
    SERVICES+=("ambev.ominia.api")
fi
if [ "$POSTGRES" = true ]; then
    SERVICES+=("postgres")
fi
if [ "$MONGODB" = true ]; then
    SERVICES+=("mongodb")
fi
if [ "$RABBITMQ" = true ]; then
    SERVICES+=("rabbitmq")
fi

# Display selected services
if [ "$RUN_ALL" = true ]; then
    echo -e "\033[1;33mNo specific services selected. Starting all containers...\033[0m"
else
    echo -e "\033[1;33mStarting selected services: ${SERVICES[*]}\033[0m"
fi

# Stop existing containers
echo -e "\033[1;33mStopping existing containers...\033[0m"
if [ "$RUN_ALL" = true ]; then
    docker-compose down
else
    for service in "${SERVICES[@]}"; do
        docker-compose stop "$service"
        docker-compose rm -f "$service"
    done
fi

# Remove old API images if API is being started
if [ "$RUN_ALL" = true ] || [ "$API" = true ]; then
    echo -e "\033[1;33mRemoving old API images...\033[0m"
    docker rmi ambev_ominia_api -f 2>/dev/null
fi

# Build and start containers
echo -e "\033[1;33mBuilding and starting containers...\033[0m"
if [ "$RUN_ALL" = true ]; then
    docker-compose up --build -d
else
    docker-compose up --build -d "${SERVICES[@]}"
fi

# Wait for containers to be ready (only when starting all containers)
if [ "$RUN_ALL" = true ]; then
    echo -e "\033[1;33mWaiting for containers to be ready...\033[0m"
    sleep 30
fi

# Check container status
echo -e "\033[0;32mContainer status:\033[0m"
docker-compose ps

echo
echo -e "\033[0;32m=== Application available at: ===\033[0m"
if [ "$RUN_ALL" = true ] || [ "$API" = true ]; then
    echo -e "\033[0;36mAPI: http://localhost:8080\033[0m"
    echo -e "\033[0;36mSwagger: http://localhost:8080/swagger\033[0m"
fi
if [ "$RUN_ALL" = true ] || [ "$RABBITMQ" = true ]; then
    echo -e "\033[0;36mRabbitMQ Management: http://localhost:15672 (user: ${RABBITMQ_USER:-ominia_user}, pass: ${RABBITMQ_PASSWORD:-ominia_pass})\033[0m"
fi
echo
echo -e "\033[1;33mTo stop containers: docker-compose down\033[0m"
echo -e "\033[1;33mTo view logs: docker-compose logs -f\033[0m"