# Vehicle Registration System

Project Properties:
Asp.Net Core 3.1, Angular 10.2.0, Bootstrap,
Node.js is required,
Database: MSSQL

Summary:
This is a simple project where you can add, update and delete vehicle information. Also, you will be able to add, update and delete weight categories as well.
The tables are sortable and easy to use.

FOLLOW THESE STEPS TO SETUP AND RUN THE PROJECT

1. Clone the project to your local directory
2. Open the project with Visual Studio 2019 preferred
3. Build the project (do not run). This will prepare the dependencies using node package manager.
4. Next you need to setup up your database but dont worry because we will be using entity framework code first approach to do that.
5. You can either user your local/non-local MSSQL database server. The current connection string is :"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Vehicles"
6. The database schema and the migration file is already there for you. All you need to do is open the Package Manager Console and type "Update-Database". This will create a database called Vehicles and its tables.
7. Now you have all your tables. Time to run the application and play with it.

Note: This application does not apply any authentication/authorization method which means any user can access any of the elements or methods.
