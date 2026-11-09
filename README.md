# User Management API

A RESTful User Management API built using
ASP.NET Core Minimal APIs and .NET 10.

This project demonstrates CRUD operations,
input validation, custom middleware,
bearer token authentication, and centralized
exception handling.

## Features

- Complete CRUD operations for user management.
- ASP.NET Core Minimal API architecture.
- User input validation using Data Annotations.
- Duplicate email prevention.
- Bearer token authentication.
- Custom request and response logging middleware.
- Centralized exception handling.
- Standardized HTTP error responses using Problem Details.
- Swagger/OpenAPI documentation.
- Microsoft Copilot-assisted development and debugging.

## Technologies Used

- C#
- ASP.NET Core 10
- .NET 10
- Minimal APIs
- Swagger / OpenAPI
- Data Annotations
- Git and GitHub
- Microsoft GitHub Copilot

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/users | Get all users |
| GET | /api/users/{id} | Get user by ID |
| POST | /api/users | Create a new user |
| PUT | /api/users/{id} | Update user |
| DELETE | /api/users/{id} | Delete user |

## Validation

The API implements:

- Required field validation.
- Name length validation.
- Email format validation.
- Department length validation.
- Duplicate email prevention.

## Middleware

The application includes:

1. Bearer Token Authentication Middleware
2. Request/Response Logging Middleware
3. Centralized Exception Handling

## Authentication

All API endpoints require a valid bearer token.

Configure the following environment variable
before running the application:

Authentication__BearerToken

Use a strong secret and never commit it
to the repository.

## Database

Currently, the application uses in-memory
storage for user records.

Data is reset whenever the application stops.

## Running the Project

1. Clone the repository.

2. Navigate to the project directory.

3. Configure the authentication token.

4. Run the application:

   dotnet run

5. Open Swagger UI in Development:

   http://localhost:5221/swagger

## Microsoft Copilot

Microsoft GitHub Copilot was used to assist with:

- Initial project structure.
- API implementation.
- Input validation.
- Debugging and exception handling.
- Middleware implementation.
- Code optimization.

## Future Improvements

- Integrate SQL Server.
- Implement Entity Framework Core.
- Add automated unit and integration tests.
- Implement JWT authentication.
- Add additional API security features.

## Author

Sanket Shinde
