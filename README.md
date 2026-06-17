 Global Logistics Management System (GLMS)

Overview

The GLMS is a full-scale Enterprise Application Development Project that was developed in ASP.NET Core MVC for the front-end and ASP.NET Core Web API for the back-end of the application.
This provides a fully functional system which enables logistics providers to administer their clients, contracts and service requests utilizing a secure, highly available and scalable environment.
In addition to providing a complete example of how to implement enterprise development concepts such as RESTful APIs, JWT authentication, integration testing, docker containerization, entity framework core and using github for source code management.

 Features

Client Management
* View clients
* Create clients
* Edit client information
* Delete clients

 Contract Management
* Create and manage contracts
* Contract status tracking
* Contract filtering
* PDF document upload and download

 Service Request Management
* Create service requests
* Link requests to contracts
* Track request status
* Automatic USD to ZAR currency conversion

 REST API
* Clients API
* Contracts API
* Service Requests API
* CRUD Operations
* PATCH Endpoints for status updates

Security
* JWT Authentication
* Protected API Endpoints
* Bearer Token Authorization

Testing
* xUnit Integration Tests
* API Endpoint Validation

 Containerization
* Docker Support
* Docker Compose Configuration
* SQL Server Container
* MVC Application Container
* API Container

Technologies Used
* ASP.NET Core MVC
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* xUnit
* Docker
* Docker Compose
* GitHub
* Swagger/OpenAPI

Solution Structure
GLMS
│
├── GLMS
│   ├── MVC Application
│   ├── Controllers
│   ├── Models
│   ├── Views
│   └── Services
│
├── GLMS.API
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   ├── Data
│   └── Authentication
│
├── GLMS.API.Tests
│   └── Integration Tests
│
└── Docker Configuration
 API Endpoints

 Authentication

http
POST /api/Auth/login

Clients
http
GET    /api/Clients
GET    /api/Clients/{id}
POST   /api/Clients
PUT    /api/Clients/{id}
DELETE /api/Clients/{id}

 Contracts
http
GET    /api/Contracts
POST   /api/Contracts
PUT    /api/Contracts/{id}
PATCH  /api/Contracts/{id}/status
DELETE /api/Contracts/{id}

Service Requests
http
GET    /api/ServiceRequests
GET    /api/ServiceRequests/{id}
POST   /api/ServiceRequests
PUT    /api/ServiceRequests/{id}
PATCH  /api/ServiceRequests/{id}/status
DELETE /api/ServiceRequests/{id}


 Running the Application

 Prerequisites
* .NET 8 SDK
* SQL Server
* Docker Desktop
* Visual Studio 2022
 Clone Repository

bash
git clone <repository-url>
cd GLMS


Run MVC Application

bash
dotnet run --project GLMS
 Run API

bash
dotnet run --project GLMS.API
 Docker Deployment

Navigate to the solution root:

bash
cd GLMS


Build containers:

bash
docker compose build

Start containers:

bash
docker compose up -d

Verify running containers:

bash
docker ps


## Authentication

Use the login endpoint to generate a JWT token:

json
{
  "username": "admin",
  "password": "admin123"
}


Use the returned token as a Bearer token when accessing protected endpoints.

Testing

Run integration tests:

bash
dotnet test


The test suite validates API functionality and endpoint availability.

 Project Outcomes

This project demonstrates:
* Enterprise application architecture
* RESTful API development
* Secure authentication using JWT
* Automated testing
* Docker containerization
* Database integration with Entity Framework Core
* Source control using GitHub

Author
Minentle Namhla Jona 
Project: Global Logistics Management System (GLMS)
