# 🌱 AgriHub

## 📝 DESCRIPTION

This project is a Agri-Energy Connect Platform built with ASP.NET Core MVC designed to bridge the gap between the agricultural sector and green energy technology providers. It provides functionality to manage farmers, products, and categories, while offering a clean UI and dashboard with real-time statistics.

### **Project Overview**
AgriHub serves as a comprehensive digital platform that facilitates seamless collaboration between agricultural producers and renewable energy solution providers. The system addresses the growing need for sustainable farming practices by connecting farmers with green energy technologies and resources.


### **Target Users**
- **Agricultural Employees**: System administrators who manage farmer accounts and oversee platform operations
- **Farmers**: Agricultural producers who use the platform to manage their products and connect with energy providers
- **Energy Providers**: Green technology companies seeking to connect with agricultural partners

### **Business Value**
- Streamlines the connection between agricultural and energy sectors
- Provides centralized management for farmer-product relationships
- Enables data-driven decision making through analytics and reporting
- Supports sustainable farming initiatives through technology integration
- Reduces administrative overhead through automated user and product management

### **Technical Architecture**
Built using modern web development practices with ASP.NET Core MVC, the platform features a scalable architecture that supports future growth and feature expansion. The system utilizes Entity Framework Core for data persistence and ASP.NET Core Identity for secure user authentication and authorization.

## 🎲 GETTING STARTED

### Prerequisites

- **Visual Studio 2022** or later
- **.NET 9.0 SDK** - Please ensure you have the latest .NET version (9.0) installed. Download from: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
- **SQL Server** or **LocalDB**
  
### Setup Instructions

**Important Notes:**
- Farmers can only sign in with credentials created by employees
- Employee accounts have full system access
- Farmer accounts have limited access to their own products

**Default Employee Credentials:**
- Email: `employee1@agri.com`
- Password: `Password123!`

**Installation Steps:**

1. Download the project from the repository
2. Extract the ZIP file to your local machine
3. Open `AgriHub.sln` in Visual Studio 2022
4. Navigate to **Tools > NuGet Package Manager > Package Manager Console**
5. Execute the following commands:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
6. Refresh your SQL Server Object Explorer to verify database creation
7. Click the HTTPS button to compile and run the application

## 💡 TECHNOLOGIES USED

- **ASP.NET Core MVC**: Modern web application framework
- **Entity Framework Core**: Object-relational mapping and data access
- **SQL Server**: Relational database management system
- **ASP.NET Core Identity**: Authentication and authorization system
- **C#**: Primary programming language
- **HTML5 & CSS3**: Frontend markup and styling

## 👾 FEATURES

### Authentication & Authorization
- **Role-based Access Control**: Secure employee and farmer role management
- **User Management**: Employee-created farmer accounts with controlled access
- **Session Security**: Protected authentication with secure session handling

### Farmer Management
- **Profile Creation**: Employees can create comprehensive farmer profiles
- **Contact Management**: Store and manage farmer contact information
- **Account Administration**: Full CRUD operations for farmer accounts

### Product Management
- **Product Catalog**: Farmers can add and manage their product listings
- **Category Organization**: Structured product categorization system
- **Inventory Tracking**: Complete product lifecycle management

### Dashboard & Analytics
- **Real-time Statistics**: Live dashboard with key performance metrics
- **Data Visualization**: Comprehensive overview of farmers and products
- **Filtering Capabilities**: Advanced search and filter functionality

### Data Management
- **Database Integration**: Robust SQL Server database with Entity Framework
- **Data Validation**: Comprehensive input validation and error handling
- **Performance Optimization**: Efficient data access and query optimization

## 🔗 LINKS

- **GitHub Repository**: https://github.com/Dessyx/ST10251981_DeslynnFenyes_PROG7311_Part2_Updated
