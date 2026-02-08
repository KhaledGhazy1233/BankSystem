🧱 Architecture Overview
The system follows *Clean Architecture*, ensuring clear separation of concerns and long-term scalability
 🔹 Domain Layer
* Core business entities (Users, BankAccounts, Transactions)
* Business rules & enums
* Completely independent from frameworks

 🔹 Application / Core Layer
* Feature-based structure (Authorization, Accounts, Transactions, Users, Audit Logs)
* CQRS-style separation (Commands & Queries)
* Clean handlers with validation & business rules
* FluentValidation & AutoMapper
* Unified & normalized API responses

🔹 Infrastructure Layer
* Entity Framework Core + SQL Server
* ASP.NET Identity
* Repository pattern (abstraction-first)
* Database migrations & seeding
* Centralized logging using Serilog

 🔹 API Layer
* Thin controllers (no business logic)
* JWT Authentication & Authorization
* Global Exception Handling Middleware
* Localization & Swagger configuration

🔐 Authentication & Authorization
* JWT Authentication (Access & Refresh Tokens)
* Login / Logout / Logout from all devices
* Role-based & Claims-based authorization
* Full role & claims management APIs

 👥 User Management
* Create, update, delete users
* Change password securely
* Paginated users listing
* Identity-based access control

 🏦 Bank Account Management APIs
* Create bank accounts
* Get all accounts
* Get user-owned accounts
* Get account by ID or account number
* Update account type
* Secure account deletion
* *Lock/unlock accounts automatically tracked via audit logs*

 💸 Transaction APIs
* Deposit
* Withdraw
* Transfer between accounts
* Transaction history per user
* Balance validation & consistency checks
* *All transactions are automatically audited*

 📜 Audit Logging & Monitoring
* *Automated auditing for all database changes*
* Tracks:
 * User performing the action
 * IP address
 * Timestamp
 * Old & new values
 * Action type (Create, Update, Delete)
* Structured logging using *Serilog*:
 * Console
 * Rolling files
 * SQL Server (centralized logs)

 🛡️ Global Error Handling
* Centralized global exception handling middleware
* Normalized API error responses
* Clean & frontend-friendly error messages

 🌍 Localization Support
* Multi-language support
* Configured cultures:
 * en-US
 * ar-EG
 * fr-FR
 * de-DE

 🛠️ Tech Stack
* ASP.NET Core Web API
* Entity Framework Core
* ASP.NET Identity
* SQL Server
* JWT (Access & Refresh Tokens)
* Serilog
* Swagger (OpenAPI)

💡 Why This Project?
This project reflects how *real banking backends are structured in production environments*, focusing on:
* Security
* Scalability
* Clean code
* Maintainability
* Automated monitoring & audit logging for maximum traceability*
