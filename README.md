# IlmPath Backend API

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-9.0-blue.svg)](https://docs.microsoft.com/en-us/ef/core/)

A comprehensive e-learning platform backend built with **Clean Architecture** principles using **ASP.NET Core 9.0**. The IlmPath API provides a robust foundation for managing online courses, user authentication, payments, video streaming, and AI-powered features.

[Video Preview](https://drive.google.com/file/d/1An5ozWmdxH1Gn0MPDvNcTrDt-TlVGGyn/view?usp=sharing)

## 🔗 Related Repositories

- **Frontend Application**: [IlmPath Frontend](https://github.com/ahmedtalaat97/ilmPathFrontend) - Angular 19 with Material Design

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles with clear separation of concerns across four distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                      │
│                   (IlmPath.Api)                            │
│  • Controllers  • Middleware  • DTOs  • Swagger          │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                        │
│                 (IlmPath.Application)                      │
│  • Use Cases  • Commands/Queries  • Interfaces  • DTOs    │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                      │
│                (IlmPath.Infrastructure)                    │
│  • Data Access  • External Services  • Repositories       │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                           │
│                   (IlmPath.Domain)                         │
│  • Entities  • Business Rules  • Domain Logic             │
└─────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

- **🎯 Domain Layer**: Core entities, business rules, and domain logic
- **🔧 Application Layer**: Use cases, CQRS commands/queries, and business orchestration
- **⚡ Infrastructure Layer**: Data persistence, external API integrations, and third-party services
- **🌐 API Layer**: REST endpoints, authentication, authorization, and API documentation

## ✨ Key Features

### 🎓 Course Management
- **Course Creation & Management**: Full CRUD operations for courses
- **Section & Lecture Organization**: Hierarchical content structure
- **Video Upload & Streaming**: Integration with Google Cloud Storage
- **Course Categories**: Organized learning paths
- **Course Ratings & Reviews**: Student feedback system

### 👥 User Management & Authentication
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Admin, Instructor, and Student roles
- **User Profiles**: Customizable user profiles with image upload
- **User Registration & Login**: Secure account management

### 💰 Payment & E-commerce
- **Stripe Integration**: Secure payment processing
- **Shopping Cart**: Redis-powered cart management
- **Coupon System**: Flexible discount management
- **Invoice Generation**: Automated billing system
- **Instructor Payouts**: Revenue sharing with instructors

### 📊 Analytics & Reporting
- **Admin Dashboard**: Comprehensive analytics
- **Revenue Tracking**: Financial reporting
- **User Analytics**: Engagement metrics
- **Course Performance**: Learning analytics

### 🤖 AI-Powered Features
- **AI Chat Assistant**: Hugging Face integration for student support
- **Smart Recommendations**: Personalized learning suggestions

### 📚 Learning Management
- **Enrollment System**: Course registration and tracking
- **Bookmarks**: Save favorite courses and content
- **Progress Tracking**: Monitor learning progress
- **Certificates**: Course completion certificates

## 🛠️ Technology Stack

### Core Technologies
- **Runtime**: .NET 9.0
- **Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core 9.0
- **Database**: SQL Server
- **Authentication**: JWT Bearer Tokens
- **Documentation**: Swagger/OpenAPI

### External Services & Integrations
- **Payment Processing**: Stripe API
- **Video Storage**: Google Cloud Storage
- **Video Processing**: FFMpeg
- **Caching**: Redis (StackExchange.Redis)
- **AI Services**: Hugging Face API
- **Email Services**: SMTP Configuration

### Development Tools
- **Architecture Pattern**: CQRS with MediatR
- **Object Mapping**: AutoMapper
- **Validation**: FluentValidation
- **Result Handling**: FluentResults
- **API Testing**: Swagger UI

## 📋 Prerequisites

Before running the application, ensure you have:

- **.NET 9.0 SDK** or later
- **SQL Server** (LocalDB, Express, or Full)
- **Redis Server** (for caching and cart management)
- **Visual Studio 2022** or **VS Code** (recommended)
- **Google Cloud Storage Account** (for video uploads)
- **Stripe Account** (for payment processing)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd ilmPathServer
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Configure Application Settings

Update `IlmPath.Api/appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IlmPathDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "Redis": "localhost:6379"
  },
  "JWT": {
    "ValidAudience": "Your_Audience",
    "ValidIssuer": "Your_Issuer",
    "Secret": "YourSuperSecretKeyThatIsLongAndComplex"
  },
  "Stripe": {
    "SecretKey": "sk_test_...",
    "PublishableKey": "pk_test_...",
    "WebhookSecret": "whsec_..."
  },
  "GoogleCloud": {
    "StorageBucketName": "your-bucket-name",
    "ProjectId": "your-project-id",
    "CredentialsPath": "path/to/credentials.json"
  },
  "HuggingFace:ApiKey": "your-huggingface-api-key"
}
```

### 4. Database Setup

```bash
# Add migration (if not exists)
dotnet ef migrations add InitialCreate --project IlmPath.Infrastructure --startup-project IlmPath.Api

# Update database
dotnet ef database update --project IlmPath.Infrastructure --startup-project IlmPath.Api
```

### 5. Start Redis Server
```bash
# Using Docker
docker run -d -p 6379:6379 redis:alpine

# Or start local Redis service
redis-server
```

### 6. Run the Application
```bash
dotnet run --project IlmPath.Api
```

The API will be available at:
- **Swagger UI**: `https://localhost:5001` (Development)
- **API Base URL**: `https://localhost:5001/api`

## 📚 API Documentation

### Authentication Endpoints
```
POST /api/users/login          # User authentication
POST /api/users/register       # User registration
GET  /api/users/profile        # Get user profile
POST /api/users/{id}/profile-image # Update profile image
```

### Course Management
```
GET    /api/courses                    # Get all courses
GET    /api/courses/{id}               # Get course by ID
POST   /api/courses                    # Create new course
PUT    /api/courses/{id}               # Update course
DELETE /api/courses/{id}               # Delete course
GET    /api/courses/{id}/sections      # Get course sections
```

### Payment & Commerce
```
POST /api/payments/checkout-session    # Create payment session
POST /api/payments/verify-payment/{sessionId} # Verify payment
GET  /api/carts                        # Get user cart
POST /api/carts/items                  # Add item to cart
```

### Admin Operations
```
GET  /api/admin/reports/revenue        # Revenue analytics
GET  /api/admin/users                  # User management
POST /api/admin/payouts                # Generate instructor payouts
```

### AI Features
```
POST /api/aichat                       # AI-powered chat assistance
```

For complete API documentation, visit the Swagger UI when running in development mode.

## 🏗️ Project Structure

```
IlmPath.sln
├── IlmPath.Api/                    # Presentation Layer
│   ├── Controllers/                # API Controllers
│   ├── Middleware/                 # Custom middleware
│   ├── Program.cs                  # Application entry point
│   └── appsettings.json           # Configuration
├── IlmPath.Application/           # Application Layer
│   ├── Commands/                  # CQRS Commands
│   ├── Queries/                   # CQRS Queries
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Interfaces/                # Application interfaces
│   └── Mappings/                  # AutoMapper profiles
├── IlmPath.Infrastructure/        # Infrastructure Layer
│   ├── Data/                      # Database context
│   ├── Repositories/              # Data repositories
│   ├── Services/                  # External services
│   └── DependencyInjection.cs    # Service registration
└── IlmPath.Domain/               # Domain Layer
    ├── Entities/                 # Domain entities
    └── Common/                   # Shared domain logic
```

## 🔧 Configuration Guide

### Environment Variables
Set the following environment variables for production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection="your-production-db-connection"
JWT__Secret="your-production-jwt-secret"
Stripe__SecretKey="your-production-stripe-key"
```

### Google Cloud Setup
1. Create a Google Cloud Storage bucket
2. Generate service account credentials
3. Update the credentials path in configuration

### Stripe Configuration
1. Create a Stripe account
2. Get your API keys from the Stripe dashboard
3. Configure webhook endpoints for payment processing

## 🐳 Docker Support

### Dockerfile (recommended)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["IlmPath.Api/IlmPath.Api.csproj", "IlmPath.Api/"]
# ... copy other projects
RUN dotnet restore "IlmPath.Api/IlmPath.Api.csproj"
COPY . .
RUN dotnet build "IlmPath.Api/IlmPath.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IlmPath.Api/IlmPath.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IlmPath.Api.dll"]
```

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### API Testing
Use the built-in Swagger UI for interactive API testing, or use tools like:
- **Postman**: Import the OpenAPI specification
- **Insomnia**: REST client testing
- **curl**: Command-line testing

## 🔒 Security Considerations

### Authentication & Authorization
- JWT tokens with configurable expiration
- Role-based access control (Admin, Instructor, Student)
- Secure password requirements (configurable)
- Protected endpoints with `[Authorize]` attributes

### Data Protection
- SQL injection prevention via Entity Framework
- Input validation with FluentValidation
- CORS configuration for cross-origin requests
- HTTPS enforcement in production

### API Security
- Rate limiting (implement as needed)
- Request validation middleware
- Exception handling middleware
- Secure headers configuration

## 📈 Performance Optimization

### Caching Strategy
- **Redis Cache**: Shopping cart and session data
- **Memory Cache**: Frequently accessed data
- **Database Optimization**: Indexed queries and efficient relationships

### Database Performance
- Entity Framework optimizations
- Lazy loading configuration
- Query optimization with projection
- Connection pooling

## 🐛 Troubleshooting

### Common Issues

**Database Connection Issues**
```bash
# Verify connection string
dotnet ef database update --project IlmPath.Infrastructure --startup-project IlmPath.Api --verbose
```

**Redis Connection Problems**
```bash
# Test Redis connectivity
redis-cli ping
```

**JWT Token Issues**
- Verify JWT configuration in appsettings.json
- Check token expiration settings
- Validate issuer and audience claims

**Google Cloud Storage Issues**
- Verify credentials file path
- Check bucket permissions
- Validate service account roles

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines
- Follow Clean Architecture principles
- Use CQRS pattern for commands and queries
- Write unit tests for business logic
- Update documentation for new features
- Follow C# coding standards

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Check existing documentation
- Review the Swagger API documentation

## 🔄 Changelog

### Version 1.0.0
- Initial release with core e-learning features
- User authentication and authorization
- Course management system
- Payment integration with Stripe
- Video upload and streaming
- AI chat assistance
- Admin analytics dashboard

---

**Built with ❤️ using Clean Architecture and .NET 9.0** 