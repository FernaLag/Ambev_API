# Ambev Ominia API – Docker Deployment Guide

This guide explains how to run the **Ominia API** using Docker for local development or testing.

---

## 1. Prerequisites

Before you start, ensure the following are installed:

* **Docker Desktop** (running)
* **Docker Compose**
* **PowerShell** (for Windows users)

---

## 2. System Architecture

The application runs inside Docker using four containers:

| Container Name         | Service Name       | Purpose                                    |
| ---------------------- | ------------------ |--------------------------------------------|
| **ambev_ominia_api**   | ambev.ominia.api   | Main API service                           |
| **ambev_ominia_postgres** | postgres        | PostgreSQL database for transactional data |
| **ambev_ominia_mongodb**  | mongodb         | MongoDB database for Event Store           |
| **ambev_ominia_rabbitmq** | rabbitmq        | Message broker for event messaging         |

---

## 3. Running the Application

### Option A – Automated Script (Recommended)

#### Run All Containers

For Windows:
```powershell
.\docker-run.ps1
```

For Mac:
```bash
./docker-run.sh
```

For Linux:
```bash
chmod +x docker-run.sh
```

#### Run Specific Containers

You can now run individual containers or combinations:

**Windows (PowerShell):**
```powershell
# Run only the API
.\docker-run.ps1 -api

# Run only PostgreSQL
.\docker-run.ps1 -postgres

# Run API + PostgreSQL
.\docker-run.ps1 -api -postgres

# Run all databases
.\docker-run.ps1 -postgres -mongodb -rabbitmq

# Show help
.\docker-run.ps1 -help
```

**Linux/Mac (Bash):**
```bash
# Run only the API
./docker-run.sh -api

# Run only PostgreSQL
./docker-run.sh -postgres

# Run API + PostgreSQL
./docker-run.sh -api -postgres

# Run all databases
./docker-run.sh -postgres -mongodb -rabbitmq

# Show help
./docker-run.sh -help
```

**Available Parameters:**
- `-api` - Start only the API container
- `-postgres` - Start only the PostgreSQL container
- `-mongodb` - Start only the MongoDB container
- `-rabbitmq` - Start only the RabbitMQ container
- `-help` - Show help message

> **Note:** If no parameters are provided, all containers will be started.

### Option B – Manual Docker Compose

If you prefer to use Docker Compose directly:

```bash
# Start all containers
docker-compose up --build -d

# Start specific services
docker-compose up --build -d ambev.ominia.api postgres

# Stop all containers
docker-compose down

# View logs
docker-compose logs -f

# Check container status
docker-compose ps
```

---

## 4. Application URLs

Once the containers are running, you can access:

| Service | URL | Description |
|---------|-----|-------------|
| **API** | http://localhost:8080 | Main API endpoint |
| **Swagger UI** | http://localhost:8080/swagger | API documentation |
| **RabbitMQ Management** | http://localhost:15672 | Message broker admin panel |

**RabbitMQ Credentials:**
- Username: `ominia_user`
- Password: `ominia_pass`

---

## 5. Useful Commands

```bash
# View container logs
docker-compose logs -f [service-name]

# Restart a specific service
docker-compose restart [service-name]

# Execute commands inside a container
docker-compose exec [service-name] bash

# Remove all containers and volumes
docker-compose down -v

# Rebuild containers without cache
docker-compose build --no-cache
```

---
