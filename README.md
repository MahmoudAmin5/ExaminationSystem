OnlineExam API
A comprehensive and robust Online Exam System built with .NET 8 Web API. This application follows Clean Architecture principles and implements Vertical Slice Architecture with CQRS pattern to ensure scalability, maintainability, and performance.

🚀 Features
🔐 Authentication & Authorization
JWT Authentication: Secure login and registration.
Identity Management: Role-based access control (Admin/User).
Account Recovery: Forgot password, reset password, email verification.
📚 Exam Management
CRUD Operations: Create, update, delete, and list exams.
Exam Logic: Start exam attempts, timed exams, and auto-submission.
Categorization: Organize exams by categories.
❓ Question Bank
Question Management: Add, update, and remove questions for exams.
Types: Support for various question types (implied by architecture).
📊 Dashboard & Analytics
Admin Dashboard: Statistics on exams, categories, and user activity.
Performance: View most active exams and categories.
📝 User Results
Attempt Tracking: Store user answers and calculate scores.
Detailed Reports: Review answers and performance after exam completion.
🛠 Tech Stack
Framework: .NET 8.0
Database: SQL Server (via Entity Framework Core)
Caching: Redis (StackExchange.Redis)
Architecture Patterns:
CQRS (using MediatR)
Vertical Slice Architecture
Repository & Unit of Work Pattern
Validation: FluentValidation
Mapping: AutoMapper
Logging: Serilog
Documentation: Swagger / OpenAPI
Email: MailKit / MimeKit
📂 Project Structure
The project is organized by features (Vertical Slices) rather than technical layers:

OnlineExam/
├── Domain/                 # Core Entities and Interfaces
├── Features/               # Feature Slices (Command, Query, Endpoint)
│   ├── Accounts/           # Authentication & User Management
│   ├── Categories/         # Exam Categories
│   ├── Dashboard/          # Admin Stats
│   ├── Exams/              # Exam Logic
│   ├── Profile/            # User Profile
│   ├── Questions/          # Question Management
│   └── UserAnswers/        # Grading & Results
├── Infrastructure/         # DB Context, Repositories, External Services
├── Middlewares/            # Custom Middlewares (Error Handling, Transaction, etc.)
├── Migrations/             # EF Core Migrations
├── Shared/                 # Common DTOs, Responses, Helpers
└── Program.cs              # App Entry Point & Service Configuration
⚙️ Getting Started
Prerequisites
.NET 8.0 SDK
SQL Server
Redis (Optional, but recommended if caching is enabled)
Installation
Clone the repository

git clone https://github.com/your-username/OnlineExam.git
cd OnlineExam
Configure Application Settings Update appsettings.json with your local configuration:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OnlineExam_DB;Integrated Security=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "Secretkey": "your_super_secret_key_must_be_long_enough",
    "ExpiryInMinutes": 60
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SenderEmail": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
Run Migrations Apply database migrations to create the schema:

dotnet ef database update
Run the Application

dotnet run
Access Documentation Once running, navigate to the Swagger UI to explore endpoints:

https://localhost:7251/swagger/index.html
(Port may vary based on your launch settings)

🧪 API Endpoints Overview
Feature	Method	Endpoint	Description
Auth	POST	/api/accounts/register	Register a new user
Auth	POST	/api/accounts/login	Login and get JWT
Exams	GET	/api/exams	List all available exams
Exams	POST	/api/exams	Create a new exam (Admin)
Exams	POST	/api/exams/start/{id}	Start an exam attempt
Exams	POST	/api/exams/submit	Submit exam answers
(See Swagger for the full list of endpoints)

🤝 Contributing
Fork the repository.
Create a new feature branch (git checkout -b feature/AmazingFeature).
Commit your changes (git commit -m 'Add some AmazingFeature').
Push to the branch (git push origin feature/AmazingFeature).
Open a Pull Request.
