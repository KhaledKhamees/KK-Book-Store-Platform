# KK Book Store Platform

## Project Overview
KK Book Store is an e-commerce platform for purchasing books, with roles for customers, admins, companies, and employees. The platform features a payment gateway integrated with Stripe and offers login and registration options via Facebook accounts.

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap
- Dependency Injection (DI)
- Repository Pattern
- N-Tier Architecture

## Prerequisites
Before you begin, ensure you have the following installed:

- [.NET SDK 6.0 or higher](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Node.js](https://nodejs.org/en/download/) (for managing client-side dependencies)
- [Stripe Account](https://stripe.com/) (for payment gateway integration)
- [Facebook Developer Account](https://developers.facebook.com/) (for social login)

You will also need to configure Stripe API keys and Facebook app credentials in the project configuration.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/kk-book-store.git
   cd kk-book-store
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Set up the database:
   - Update the `appsettings.json` file with your SQL Server connection string.
   - Apply migrations to create the database:
     ```bash
     dotnet ef database update
     ```

4. Configure the Stripe and Facebook API keys in `appsettings.json`:
   ```json
   {
     "Stripe": {
       "PublishableKey": "your-publishable-key",
       "SecretKey": "your-secret-key"
     },
     "Facebook": {
       "AppId": "your-app-id",
       "AppSecret": "your-app-secret"
     }
   }
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. Navigate to `http://localhost:5000` to access the KK Book Store platform.

