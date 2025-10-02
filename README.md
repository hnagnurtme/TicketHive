# 🎫 TicketHive

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![API Documentation](https://img.shields.io/badge/API-Documentation-green.svg)](https://hnagnurtme.github.io/TicketHive/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**TicketHive** is a robust platform for event ticket management, providing secure authentication, user management, and seamless event operations. This API enables integration with TicketHive's core features, supporting both internal and third-party applications.

## 📖 API Documentation

🔗 **[View Live API Documentation](https://hnagnurtme.github.io/TicketHive/)**

The complete API documentation is available through our interactive Swagger UI, hosted on GitHub Pages. Here you can:

- 📋 Browse all available endpoints
- 🧪 Test API calls directly in the browser  
- 📝 View detailed request/response schemas
- 🔐 Understand authentication requirements
- 💡 See example requests and responses
- 📱 Access from any device with responsive design

### Documentation Links

| Resource | URL | Description |
|----------|-----|-------------|
| 🏠 **Main Documentation** | [GitHub Pages](https://hnagnurtme.github.io/TicketHive/) | Complete API overview and getting started guide |
| 📖 **Interactive API Docs** | [Swagger UI](https://hnagnurtme.github.io/TicketHive/swagger-ui/) | Interactive API testing interface |
| 📄 **OpenAPI Specification** | [swagger.json](https://hnagnurtme.github.io/TicketHive/swagger.json) | Raw OpenAPI 3.0 specification |
| 🧪 **Demo Page** | [Demo](https://hnagnurtme.github.io/TicketHive/demo.html) | Quick access page with links |

### Quick Links

- **Authentication**: `POST /api/auth/login` - User authentication
- **Events**: `GET /api/events` - Retrieve all events
- **Tickets**: `GET /api/tickets` - Retrieve tickets with pagination
- **Users**: `GET /api/users/profile/{userId}` - Get user profile

## 🚀 Features

### 🔐 Authentication & Security
- JWT-based authentication
- Email verification system
- Refresh token mechanism
- Secure password handling

### 🎪 Event Management
- Create and manage events
- Event publishing system
- Pagination and filtering
- Event status tracking

### 🎟️ Ticket Management
- Comprehensive ticket operations
- Ticket activation/deactivation
- Event-specific ticket retrieval
- Advanced filtering and sorting

### 👤 User Management
- User profile management
- Secure registration process
- Profile updates and maintenance

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **Architecture**: Clean Architecture
- **Database**: Entity Framework Core
- **Authentication**: JWT
- **Documentation**: OpenAPI/Swagger
- **Testing**: xUnit
- **Containerization**: Docker

## 📁 Project Structure

```
TicketHive/
├── src/
│   ├── TicketHive.Api/          # Web API layer
│   ├── TicketHive.Application/  # Application business logic
│   ├── TicketHive.Domain/       # Domain entities and rules
│   └── TicketHive.Infrastructure/ # Data access and external services
├── tests/
│   └── TicketHive.Tests/        # Unit and integration tests
├── docs/
│   ├── swagger.json             # OpenAPI specification
│   └── swagger-ui/              # Swagger UI documentation
└── docker-compose.yml           # Docker configuration
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
