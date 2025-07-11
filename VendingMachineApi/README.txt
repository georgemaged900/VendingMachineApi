
 FlapKap Vending Machine API

A RESTful API built with ASP.NET Core (.NET 7) that simulates a vending machine system with two user roles: Buyer and Seller. The system supports user registration/login, product management, deposits, and product purchases.

 Features

-  JWT Authentication + Role-based Authorization
-  Role Management (Buyer, Seller)
-  Product CRUD (Seller only)
-  Deposit & Purchase Logic (Buyer only)
-  Global Error Handling Middleware
-  Serilog for structured logging
-  API Testing with xUnit
-  Clean Code Structure + SOLID principles
-  In-Memory Database for demo/testing
- AutoMapper
- SwaggerUI

Project Structure

FlapKap/
├── Controllers/
├── Dto/
├── Middleware/
├── Models/
├── Repository/
├── Service/
├── Tests/
├── AutoMapperProfile.cs
├── Program.cs
└── README.md


# How to Run the Project

1. Prerequisites
- [.NET 8 SDK or later](https://dotnet.microsoft.com/en-us/download)
- (Optional) IDE: Visual Studio 2022 
- Git (if cloning from version control)



Getting Started

3 Run the App
dotnet run

- The API will start at: https://localhost:5001
- Swagger UI will be available at: https://localhost:5001/swagger/index.html

4.  Run API Tests
dotnet test

- Tests are written using xUnit and use WebApplicationFactory for integration testing.

Authentication

Use /register and /login endpoints to obtain a JWT token.
Call the login api with the below seeded users credentials in the request body then use the returned JWT Bearer Token from response and add to header.
If using SwaggerUi after running the project then click on authorize and add "Bearer {JwtToken}" without the quotations and curly bracket just Bearer then
the JWT Token. 

Seeded Users

| Username     | Password | Role   |
|--------------|----------|--------|
| testbuyer    | 123456   | buyer  |
| testseller   | 123456   | seller |

Seeded Roles

| Id   | Name    |    
|--------------  |
| 1    | buyer   |   
| 2    | seller  | 

Seeded Product

| Id     | ProductName | AmountAvailable| Cost | SellerId|
|----------------------|----------------|------|---------| 
| 1      | Kitkat        | 10  		  20        1    |

 

API Endpoints Summary

Authentication
- POST /register  
- POST /login

 Product (Seller only)
- GET /product  
- GET /product/{id}  
- POST /product  
- PUT /product{id}  
- DELETE /product{id}  

 Buyer Actions
- POST /deposit  
- POST /buy  
- POST /reset

 Authorization Rules

| Endpoint         | Role Required |
|------------------|----------------|
| /product (CRUD)  | Seller         |
| /deposit         | Buyer          |
| /buy             | Buyer          |
| /reset           | Buyer          |

 Logging

- Logging is configured using Serilog.
- Output is sent to both:
  - Console
  - Rolling log files in Logs/log.txt

 Technologies Used

- ASP.NET Core (.NET 7)
- Entity Framework Core (InMemory)
- JWT Authentication
- Serilog
- AutoMapper
- xUnit (Testing)
- Swagger/OpenAPI

 How to Extend

- Replace InMemory DB with SQL Server
- Add product image support
- Add transaction history for purchases
- Add refresh token mechanism for authentication
-- Add Fluent Validation

