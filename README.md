# SimpleCrud API

A simple CRUD (Create, Read, Update, Delete) API for managing products built with ASP.NET Core 8.0.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor (Visual Studio, Visual Studio Code, or Rider)
- Docker (optional, for containerized deployment)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/rodrigo-web-developer/products-simple-crud.git
cd SimpleCrud
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

## Running the Application

### Option 1: Using .NET CLI

Navigate to the API project directory and run:

```bash
cd SimpleCrud.Api
dotnet run
```

The application will start and be available at:
- HTTP: `http://localhost:5133`
- HTTPS: `https://localhost:7023`

### Option 2: Using Visual Studio

1. Open `SimpleCrud.sln` in Visual Studio
2. Set `SimpleCrud.Api` as the startup project
3. Press `F5` or click the "Run" button

### Option 3: Using Docker

Build and run the Docker container:

```bash
docker build -t simplecrud-api -f SimpleCrud.Api/Dockerfile .
docker run -p 8080:8080 -p 8081:8081 simplecrud-api
```

The application will be available at:
- HTTP: `http://localhost:8080`
- HTTPS: `https://localhost:8081`

## API Documentation

Once the application is running, you can access the Swagger UI documentation at:

```
https://localhost:7023/swagger
```

or

```
http://localhost:5133/swagger
```

## Available Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get a product by ID |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product by ID |

### Example Requests

#### Create a Product

```bash
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "price": 999.99
}
```

#### Update a Product

```bash
PUT /api/products
Content-Type: application/json

{
  "id": 1,
  "name": "Gaming Laptop",
  "description": "Add a long description for the product"
  "price": 1299.99
}
```

#### Get All Products

```bash
GET /api/products
```

#### Get Product by ID

```bash
GET /api/products/1
```

#### Delete a Product

```bash
DELETE /api/products/1
```

## Running Tests

To run the unit tests:

```bash
dotnet test
```

Or to run tests with detailed output:

```bash
dotnet test --verbosity normal
```

## Project Structure

```
SimpleCrud/
├── SimpleCrud/              # Core business logic library
│   ├── Entities/            # Domain entities
│   ├── Repositories/        # Data access layer
│   ├── Services/            # Business logic services
│   └── Validations/         # Custom validation attributes
├── SimpleCrud.Api/          # Web API project
│   ├── Controllers/         # API controllers
│   ├── Requests/            # Request DTOs
│   └── Program.cs           # Application entry point
└── SimpleCrud.Tests/        # Unit tests
    └── Controllers/         # Controller tests
```

## Technologies Used

- **ASP.NET Core 8.0** - Web framework
- **Swagger/OpenAPI** - API documentation
- **In-Memory Repository** - Data storage (for demonstration purposes)
- **xUnit** - Unit testing framework

## Configuration

Application settings can be modified in:
- `appsettings.json` - General settings
- `appsettings.Development.json` - Development environment settings

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is open source and available for educational purposes.
