# 🎓 OnlineExam API

![.NET Core](https://img.shields.io/badge/.NET%2010.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)

A comprehensive and robust Online Exam System built with **.NET 8 Web API**. This application strictly follows **Clean Architecture** principles and implements **Vertical Slice Architecture** alongside the **CQRS pattern** to ensure superior scalability, maintainability, and performance.

---

## 🚀 Features

### 🔐 Authentication & Authorization
* **JWT Authentication:** Secure login, registration, and token validation.
* **Identity Management:** Robust role-based access control (Admin/User).
* **Account Recovery:** Secure forgot password flow, password resets, and email verification.

### 📚 Exam Management
* **CRUD Operations:** Create, update, delete, and list exams.
* **Exam Logic:** Engine to handle starting exam attempts, managing timed exams, and auto-submission.
* **Categorization:** Organize and filter exams by distinct categories.

### ❓ Question Bank
* **Question Management:** Dynamically add, update, and remove questions linked to specific exams.
* **Flexible Types:** Architecture supports various question types and formats.

### 📊 Dashboard & Analytics
* **Admin Dashboard:** High-level statistics on exams, categories, and system-wide user activity.
* **Performance Metrics:** View most active exams, popular categories, and engagement trends.

### 📝 User Results
* **Attempt Tracking:** Securely store user answers and automatically calculate final scores.
* **Detailed Reports:** Allow users to review their submitted answers and performance metrics post-exam.

---

## 🛠 Tech Stack

* **Framework:** .NET 10.0
* **Database:** SQL Server (via Entity Framework Core)
* **Caching:** Redis (StackExchange.Redis)
* **Architecture Patterns:**
  * CQRS (via [MediatR](https://github.com/jbogard/MediatR))
  * Vertical Slice Architecture
  * Repository & Unit of Work Pattern
* **Validation:** FluentValidation
* **Object Mapping:** AutoMapper
* **Logging:** Serilog
* **API Documentation:** Swagger / OpenAPI
* **Email Services:** MailKit / MimeKit

---

## 📂 Project Structure

This project is organized by features (**Vertical Slices**) rather than traditional technical layers, keeping related logic cohesive and easy to navigate:

```text
OnlineExam/
├── Domain/                 # Core Entities, Enums, and Interfaces
├── Features/               # Feature Slices (Commands, Queries, Endpoints)
│   ├── Accounts/           # Authentication & User Management
│   ├── Categories/         # Exam Categories
│   ├── Dashboard/          # Admin Stats & Analytics
│   ├── Exams/              # Exam Logic & Lifecycle
│   ├── Profile/            # User Profile Management
│   ├── Questions/          # Question Bank Management
│   └── UserAnswers/        # Grading & Result Generation
├── Infrastructure/         # DB Context, Repositories, External Services
├── Middlewares/            # Custom Middlewares (Error Handling, Transactions, etc.)
├── Migrations/             # EF Core Migrations
├── Shared/                 # Common DTOs, Standard Responses, and Helpers
└── Program.cs              # App Entry Point & Service Configuration
