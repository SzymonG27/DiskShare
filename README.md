#DiskShare v1.0

DiskShare is an ASP.NET 6.0 application that allows you to create and manage an internet drive without using a dataabase for files. You can also share the drive for another user


##Why did I made this application?

A lot of web application like zippyshare allows to share only one file. I wanted to make an application that user can collect a lot of files in web storage, and another user which has a link to the disk can choose what file he wants to download.


##Instalation

All you had to do for correct working this application is create a database from context for users.

In command prompt (for local usage) you must go to the directory with project and write:
```
dotnet ef database update --context AppIdentityDbContext
```
And that's it! For implement this project on web hosting like Azure you can check [This tutorial](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources)
