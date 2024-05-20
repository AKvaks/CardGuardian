# CardGuardian
This project was made so I could try Blazor server and to solve the problem of tracking cards in my collection.

# Migrations
To create a new migration use the following console command
### Creating migration
>[!NOTE]
>Position yourself in the solution folder
```console
dotnet ef migrations add MIGRATION_NAME --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation" --context ApplicationDbContext --output-dir Persistance/Migrations
```
>[!NOTE]
>Update Db fom the console or just run the application
```console
dotnet ef database update --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation"
``` 

## Removing last migration 
>[!CAUTION]
>This should be done ONLY IF migration was not applied to the Db
```console
dotnet ef migrations remove --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation" --context ApplicationDbContext
``` 
