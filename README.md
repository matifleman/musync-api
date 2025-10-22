# 🎵 Musync API

Musync is a **.NET 9 RESTful API** that implements user authentication, identity management, and backend endpoints for the Musync platform.

---


## ⚙️ Environment Setup

The project uses **SQLite** by default, with the file `Musync.db` located in `Musync.Api/`.

To persist data between builds, the `docker-compose.yml` mounts the local database file into the container:

```yaml
volumes:
  - ./Musync.Api/Musync.db:/app/Musync.db
```

## 🐳 Run with Docker
### 1️⃣ Build and start containers

From the repository root (where the docker-compose.yml file is located):

```
docker-compose up -d
```

This will:

- Build the musyncapi image using the Dockerfile inside Musync.Api

- Expose the default port 5000 used from the expo app and 8080 for HTTP and 8081 for HTTPS

- Mount the local Musync.db file into /app/Musync.db inside the container

### 2️⃣ Access the API

Once running, you can access the Swagger UI at:

👉 http://localhost:5000/swagger

### 3️⃣ Stop and remove containers

To stop all running containers:
```
docker-compose down
```

If you also want to remove built images:
```
docker-compose down --rmi all
```
## ✨ Authors
### 👨‍💻 Pablo Lamela
### 👨‍💻 Matias Fleman