# 🌱 AgriHub

## 📝 DESCRIPTION

This project is a Agri-Energy Connect Platform built with ASP.NET Core MVC designed to brudge the gap between the agricultural sector and green energy technology providers. It provides functionality to manage farmers, products, and categories, while offering a clean UI and dashboard with real-time statistics.



## 🎲 GETTING STARTED
### Prerequisites

- Visual Studio 2022 or later
- Please ensure you have the latest .NET version (9.0) installed. You can download it here: https://dotnet.microsoft.com/en-us/download/dotnet/9.0 
- SQL Server or LocalDB
  
### Setup Instructions

NOTE: To sign in as a Employee, please use Email: employee1@agri.com and Password: Password123!
- You need to add a farmer with their respective email and password.
- Using those credentials, sign in as a farmer.
- You can only sign in as a farmer with credentials a employee created.

1. Within the repository, click on the "<> Code" drop down on the far right 
   next to the "Go to file" and "+" buttons.
2. On the Local tab, click on the last option: "Download ZIP".
3. Once the zip file has downloaded, open your local file explorer.
4. Go to your Downloads.
5. Open the "AgriHub.zip" folder, should be most recent in Downloads.
6. Open the "AgriHub" folder, this folder is not a zip.
7. Open the AgriHUb.sln file.
8. The project should begin loading.
9. Navigate to Tools > Nuget Package Manager > Package Manager Console.
10. Open the package manager and type the command: Add-Migration InitialCreate
11. Once the build succeeded, type the command: update-database
12. In your SQL Server Object Explorer, refresh and it should be reflected. 
13. On the top in the middle, double click the https button.
14. The program will compile and you may use the program. 


## 💡 TECHNOLOGIES USED

- ASP.NET Core MVC: Framework for building the web application.
- Entity Framework Core (EF Core): Used for database access and management.
- SQL Server: (Local Database).
- Languages: C#, HTML & CSS

## 👾 FEATURES

- Role based authentication
- Employees can add farmer profiles specifying their name, email, contact number and password. 
- Farmers can then sign in with these credentials created by the employee.
- Dashboard feature to oversee farmers and products for Employees.
- Farmers have the ability to add and view their products.
- Employees can filter farmers and products effectively.

GitHub Link: https://github.com/Dessyx/AgriHub/tree/master
