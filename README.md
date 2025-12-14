ProductionGrade API

ProductionGrade is a clean, production-ready ASP.NET Core Web API designed to demonstrate best practices in architecture, validation, authentication, and maintainability. It provides a foundation for building scalable applications with features like repositories, services, unit of work, FluentValidation, JWT authentication, and Swagger documentation.

Features

- Entity Framework Core with SQL Server integration
- Repository and Unit of Work pattern for clean data access
- FluentValidation for DTO validation
- Global Exception Middleware for consistent error handling
- Swagger and OpenAPI for interactive API documentation
- JWT Authentication with ASP.NET Identity
- Role-based Authorization for Admin and User
- Seeded Data for quick testing of categories and products
- Checkout flow from Cart to Order

 Project Structure

ProductionGrade/
│
├── Controllers/         
├── Data/                
├── DTOs/                
├── Extensions/          
├── Middleware/          
├── Models/              
├── Repositories/        
├── Services/            
├── Validation/          
└── Program.cs           

 Getting Started

 Prerequisites
- .NET 8 SDK
- SQL Server or LocalDB
- Postman or Swagger UI for testing

Setup
1. Clone the repository
   git clone https://github.com/yourusername/ProductionGrade.git
   cd ProductionGrade

2. Update the connection string in appsettings.json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductionGradeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }

3. Run migrations and update the database
   dotnet ef migrations add InitialCreate
   dotnet ef database update

4. Launch the API
   dotnet run

5. Open Swagger UI
   https://localhost:5001/swagger

 Authentication

ProductionGrade uses ASP.NET Identity with JWT tokens.

 Register
POST /api/auth/register
{
  "username": "bestuser",
  "email": "best@example.com",
  "password": "StrongPass123"
}

 Login
POST /api/auth/login
{
  "username": "bestuser",
  "password": "StrongPass123"
}

Response
{
  "token": "JWT_TOKEN_HERE"
}

Use the token in headers
Authorization: Bearer JWT_TOKEN_HERE

Example Flow

1. View Products
   GET /api/products

2. Add to Cart
   POST /api/cart/{userId}/add?productId={productId}&quantity=2

3. View Cart
   GET /api/cart/{userId}

4. Checkout
   POST /api/orders/checkout
   {
     "userId": "USER_GUID"
   }

5. View Orders
   GET /api/orders

 Roles

- User can register, login, manage cart, checkout, and view orders
- Admin can create products, categories, and manage inventory

Endpoints requiring admin access are decorated with
[Authorize(Roles = "Admin")]

 Testing

Unit tests are located in ProductionGrade.Tests  
Run tests with
dotnet test

 Summary

ProductionGrade is more than a demo. It is a blueprint for building real-world APIs. With authentication, validation, clean architecture, and seeded data, you can start experimenting immediately and extend it into a full e-commerce or inventory system.
