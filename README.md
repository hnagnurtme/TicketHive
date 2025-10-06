<h1 align="center">🎫 TicketHive</h1>

<p align="center">
  <i>Booking & Event Management API built with Clean Architecture in ASP.NET Core</i>
</p>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/></a>
  <a href="https://hnagnurtme.github.io/TicketHive/"><img src="https://img.shields.io/badge/API-Documentation-00C853?style=for-the-badge&logo=swagger&logoColor=white"/></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge"/></a>
</p>

---

## 🧩 Overview

**TicketHive** is a robust event ticketing API built with **ASP.NET Core (Clean Architecture)**.  
It provides **secure authentication**, **ticket management**, **VNPay integration**, and **modular microservices-ready architecture**.

> Designed for scalability, reliability, and developer-friendly integrations.

---

## 🧠 Tech Stack

![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat-square&logo=postgresql&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=flat-square&logo=rabbitmq&logoColor=white)
![VNPay](https://img.shields.io/badge/VNPay-0D47A1?style=flat-square)
![SMTP](https://img.shields.io/badge/SMTP-0078D4?style=flat-square&logo=gmail&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white)
![Unit Test](https://img.shields.io/badge/Unit%20Test-6DB33F?style=flat-square&logo=pytest&logoColor=white)

---

## 🚀 Features

### 🔐 Authentication & Security
- JWT-based authentication with refresh tokens  
- Email verification and password reset  
- Secure role-based authorization  

### 🎟️ Ticket & Event Management
- Create, update, publish, and filter events  
- Dynamic ticket lifecycle (activate, deactivate)  
- Real-time event status & analytics  

### 💳 Payment Integration
- Seamless checkout via **VNPay API**  
- Transaction logging and verification  

### ⚙️ System Design
- Clean Architecture (Domain, Application, Infrastructure, API)  
- Caching with Redis  
- Async messaging via RabbitMQ  
- Unit testing and CI-ready structure  

---

## 📖 API Documentation

[![Swagger Screenshot](https://raw.githubusercontent.com/hnagnurtme/TicketHive/master/docs/swagger-screenshot.png)](https://hnagnurtme.github.io/TicketHive/)

🔗 **[View Live API Documentation](https://hnagnurtme.github.io/TicketHive/)**

| Resource | URL | Description |
|----------|-----|-------------|
| 🏠 **Main Docs** | [GitHub Pages](https://hnagnurtme.github.io/TicketHive/) | Complete API overview |
| 🧪 **Swagger UI** | [Swagger UI](https://hnagnurtme.github.io/TicketHive/swagger-ui/) | Interactive testing interface |
| 📄 **OpenAPI Spec** | [swagger.json](https://hnagnurtme.github.io/TicketHive/swagger.json) | Raw OpenAPI file |
| 💡 **Demo Page** | [Demo](https://hnagnurtme.github.io/TicketHive/demo.html) | Example endpoints |

---

## 🏗️ Project Structure

```bash
TicketHive/
├── src/
│   ├── TicketHive.Api/              # API Layer (Controllers, Middleware)
│   ├── TicketHive.Application/      # Core Business Logic
│   ├── TicketHive.Domain/           # Entities, Enums, and Rules
│   └── TicketHive.Infrastructure/   # Database, Repositories, Integrations
│
├── tests/
│   └── TicketHive.Tests/            # Unit and Integration Tests
│
├── docs/
│   ├── swagger.json                 # OpenAPI Specification
│   └── swagger-ui/                  # Swagger UI Static Files
│
└── docker-compose.yml               # Docker Setup

```

## 🏗️ Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional)
- SQL Server or compatible database

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/hnagnurtme/TicketHive.git
   cd TicketHive
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   
   Edit `src/TicketHive.Api/appsettings.json` with your database connection string.

4. **Run database migrations**
   ```bash
   dotnet ef database update --project src/TicketHive.Infrastructure
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/TicketHive.Api
   ```

### Using Docker

```bash
docker-compose up -d
```

The API will be available at `https://localhost:7043` or `http://localhost:5043`.

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

Generate test coverage report:

```bash
./Test.sh
```


### Authentication

All protected endpoints require a Bearer token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes and commit: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
