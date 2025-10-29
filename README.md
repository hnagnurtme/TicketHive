# TicketHive - Event Ticket Management API

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-4169E1.svg?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Architecture](https://img.shields.io/badge/Clean%20Architecture-Pattern-orange.svg)](https://github.com/jasontaylordev/CleanArchitecture)
[![VNPay](https://img.shields.io/badge/Payment-VNPay-0A8A3A.svg?logo=cashapp&logoColor=white)](https://vnpay.vn/)
[![API Documentation](https://img.shields.io/badge/API-Documentation-green.svg)](https://hnagnurtme.github.io/TicketHive/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> A robust platform for event ticket management with secure authentication, user management, and seamless event operations.

## 📑 Table of Contents

- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Using Docker](#using-docker)
- [API Usage](#-api-usage)
- [Testing](#-testing)
- [Contributing](#-contributing)
- [License](#-license)

##  Features

- **🔐 Authentication & Security** - JWT-based authentication with email verification and refresh tokens
- **🎪 Event Management** - Complete CRUD operations with publishing system and status tracking
- **🎟️ Ticket Management** - Advanced ticket operations with filtering, sorting, and pagination
- **👤 User Management** - Secure registration and profile management
- **💳 Payment Integration** - VNPay payment gateway integration
- **📖 Interactive API Docs** - Comprehensive Swagger UI documentation

## API Documentation
***Swagger UI:*** [http://localhost:5043/swagger/index.html](http://localhost:5043/swagger/index.html)

![Login](docs/image/login.png)

**1. Access Swagger UI**  

Link: [http://localhost:5043/swagger/index.html](http://localhost:5043/swagger/index.html)

**2. Log in to obtain a token**  
> Login interface. Enter **username** and **password** → receive **JWT token**.

![Login](docs/image/login.png)

**3. Enter the token in Swagger**  
> Click **"Authorize"** → enter `Bearer <token>` → confirm.

![Bearer Token Input](docs/image/auth.png)

**4. Use the APIs**  
> You can now call protected endpoints.
---
## Technology Stack

- **.NET 8.0** - Modern, high-performance framework
- **Clean Architecture** - Maintainable and testable design
- **Entity Framework Core** - Powerful ORM for database operations
- **PostgreSQL** - Robust relational database
- **JWT Authentication** - Secure token-based authentication
- **Swagger/OpenAPI** - Interactive API documentation
- **xUnit** - Comprehensive testing framework
- **Docker** - Containerization support

## 📁 Project Structure

```
TicketHive/
├── src/
│   ├── TicketHive.Api/              # Web API layer
│   ├── TicketHive.Application/      # Business logic
│   ├── TicketHive.Domain/           # Domain entities
│   └── TicketHive.Infrastructure/   # Data access
├── tests/
│   └── TicketHive.Tests/            # Unit & integration tests
├── docs/                            # API documentation
└── docker-compose.yml               # Docker configuration
```

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) or SQL Server
- [Docker](https://www.docker.com/) (optional)

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

3. **Configure database**
   
   Update `src/TicketHive.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "your-connection-string"
     }
   }
   ```

4. **Apply migrations**
   ```bash
   dotnet ef database update --project src/TicketHive.Infrastructure
   ```

5. **Run the application**
   ```bash
   dotnet run --project src/TicketHive.Api
   ```

   The API will be available at:
   - HTTPS: `https://localhost:7043`
   - HTTP: `http://localhost:5043`

### Using Docker

```bash
docker-compose up -d
```

##  API Usage

All protected endpoints require a Bearer token:

```http
Authorization: Bearer <your-jwt-token>
```

**Example Request:**
```bash
curl -X GET "https://localhost:7043/api/events" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

## Testing

Run all tests:
```bash
dotnet test
```

Generate coverage report:
```bash
./Test.sh
```

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<div align="center">

**[Documentation](https://hnagnurtme.github.io/TicketHive/)** • **[Report Bug](https://github.com/hnagnurtme/TicketHive/issues)** • **[Request Feature](https://github.com/hnagnurtme/TicketHive/issues)**

Made with ❤️ by [hnagnurtme](https://github.com/hnagnurtme)

</div>