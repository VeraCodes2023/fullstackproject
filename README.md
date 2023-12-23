## Ecommerce Fullstack Project Documentation
### Introduction
This documentation aims to provide comprehensive insights into the Fullstack Project, detailing the implementation and functionalities of both the frontend and backend components. The project is designed to deliver a seamless user experience with robust administrative management to the Ecommerce web application. The Fullstack Project comprises a frontend developed using TypeScript, React, and Redux Toolkit, and a backend built on .NET Core 7, Entity Framework Core, and PostgreSQL. The primary aim is to create a functional and user-friendly application with user management, product browsing, cart functionality, and seamless checkout for users, while providing administrators with robust user, product, and order management capabilities.

### Table of Contents
- Introduction
- Technology Stack
- Project Overview
- Architecture
- ERD
- File Structure
- Backend Implementation
- Frontend Connection
- Testing
- Deployment
- Conclusion

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