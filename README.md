
# 🛒 E-Commerce WebMVC Application

A robust e-commerce platform built with ASP.NET Core MVC, featuring product management, order processing, Firebase push notifications, dynamic reporting using Stimulsoft, and designed using Clean Architecture principles. The application also includes unit and integration testing to ensure reliable functionality.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Firebase Integration](#firebase-integration)
- [Reporting with Stimulsoft](#reporting-with-stimulsoft)
- [Testing](#testing)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)

## 🚀 Features

- **Product Management**: Create, edit, and delete products with multiple options (size, color, stock).
- **Image Handling**: Upload and display multiple product images.
- **Order Processing**: Manage customer orders with detailed item tracking.
- **Push Notifications**: Send real-time notifications to sales managers via Firebase.
- **Dynamic Reporting**: Generate sales and stock reports using Stimulsoft Designer.
- **User Authentication**: Secure login and role-based access control.
- **Responsive Design**: Mobile-friendly interface using Bootstrap.

## 🛠 Tech Stack

- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
- **Backend**: ASP.NET Core MVC
- **Database**: Entity Framework Core with SQL Server
- **Notifications**: Firebase Cloud Messaging (FCM)
- **Reporting**: Stimulsoft Reports
- **Authentication**: ASP.NET Identity
- **Architecture**: Clean Architecture for better separation of concerns and maintainability
- **Testing**: Unit and Integration Testing using xUnit and TUnit

## ⚙️ Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Node.js](https://nodejs.org/) (for frontend dependencies)
- [Stimulsoft Designer](https://designer.stimulsoft.com/) (for report designing)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/ecommerce-webmvc.git
   cd ecommerce-webmvc
   ```

2. **Install Backend Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Install Frontend Dependencies**:
   ```bash
   npm install
   ```

4. **Run Database Migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**:
   ```bash
   dotnet run
   ```

---

### 🧪 Testing

The application includes both unit and integration tests to ensure that the core business logic and features are working as expected. The tests follow the principles of Clean Architecture, with clear separation of concerns between the application layers.

- **Unit Tests**: Focus on testing individual components or services in isolation.
- **Integration Tests**: Test the interaction between different parts of the system (e.g., database, API, and external services).

To run the tests, use the following command:

```bash
dotnet test
```

---

This setup provides a solid foundation for scalable development, testing, and maintenance. Let me know if you need any further adjustments!
