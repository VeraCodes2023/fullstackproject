## Ecommerce Fullstack Project Documentation

### Table of Contents
- [Introduction](#1-Introduction)
- [Technology Stack](#1-Technology-Stack)
- [Project Overview](#1-Project-Overview)
- [Architecture](#1-Architecture)
- [Features](#1-Features)
- [File Structure](#1-File-Structure)
- [Backend Implementation](#1-Backend-Implementation)
- [Frontend Connection](#1-Frontend-Connection)
- [Testing](#1-Testing)
- [Deployment](#1-Deployment)
- [Conclusion](#1-Conclusion)

### Introduction
This documentation aims to provide comprehensive insights into the Fullstack Project, detailing the implementation and functionalities of both the frontend and backend components. The project is designed to deliver a seamless user experience with robust administrative management to the Ecommerce web application. The Fullstack Project comprises a frontend developed using TypeScript, React, and Redux Toolkit, and a backend built on .NET Core 7, Entity Framework Core, and PostgreSQL. The primary aim is to create a functional and user-friendly application with user management, product browsing, cart functionality, and seamless checkout for users, while providing administrators with robust user, product, and order management capabilities.

### Technology Stack
#### Frontend:
- TypeScript
- SASS
- React
- Redux Toolkit
#### Backend:
- .NET Core 7
- Entity Framework Core
- PostgreSQL

### Architecture
The project follows the CLEAN architecture pattern in the backend to ensure separation of concerns and maintainability. The architecture consists of distinct layers:

- Presentation Layer: Includes the API controllers.
- Application Layer: Contains use cases and business logic.
- Domain Layer: Defines the domain models and entities.
- Infrastructure Layer: Manages data access and external services.

### File Structure
The file structure adheres to a modular and organized approach, separating concerns based on functionality and domain.
.
└── README.md
└── .gitignore
└── Ecommerce.Business
└── Ecommerce.Controller
└── Ecommerce.Core
└── Ecommerce.ERD
└── Ecommerce.Test
└── Ecommerce.WebAPI

### Features 
#### User Functionalities
User Management: Allows user registration and login.
Product Browsing: Enables users to view, search, and sort products.
Shopping Cart: Facilitates adding and managing products in the cart.
Checkout: Allows users to place orders.

#### Admin Functionalities
User Management: Admins can view and delete users.
Product Management: Admins can view, edit, delete, and add products.
Order Management: Admins can view all orders.

### Backend Implementation
#### Database Schema 
Before coding, a well-planned database schema was designed to ensure proper data organization and relationships.
![Alt text](ECommerce.ERD/ERD.png)

### Error Handling
The Fullstack Project employs .NET Core's built-in exception handling capabilities to manage exceptions gracefully throughout the application. While not implementing a separate custom error handling middleware, our backend leverages the inherent exception capture system provided by the .NET Core framework.

### Swagger API endpoints 
API endpoints are annotated for Swagger documentation, providing an interactive UI for testing and documentation. http://localhost:5000/swagger/index.html

### Frontend Connection
The modified frontend project is connected to this backend server to create a cohesive fullstack application. Refer to the frontend repository here for frontend-related details.

### Testing
Unit tests are implemented, primarily focusing on the Service layer using xUnit. Additional tests cover entities, repositories, and controllers.
Swagger Documentation. Both frontend and backend code undergo unit testing to achieve high test coverage and ensure all major functionalities are covered.

### Deployment
The live servers host frontend, backend, and database servers for comprehensive access and functionality.

###  Conclusion
The Fullstack Project is a robust, user-friendly application that offers seamless user experiences and efficient administrative capabilities. It demonstrates the integration of React, Redux Toolkit in the frontend and .NET Core 7, Entity Framework Core, PostgreSQL in the backend to deliver a complete fullstack solution.