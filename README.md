# CardGuardian
This project was made so I could try Blazor server and to solve the problem of tracking cards in my collection.

# Migrations
To create new migration use this command in solution folder:

## Creating migration
    ```console
    dotnet ef migrations add MIGRATION_NAME --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation" --context ApplicationDbContext --output-dir Persistance/Migrations
    ```
## To apply migration to Db run application or update Db fom console
    ```console
    dotnet ef database update --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation"
    ``` 

## Removing last migration
    ```console
    dotnet ef migrations remove --project "CardGuardian.Infrastructure" --startup-project "CardGuardian.Presentation" --context ApplicationDbContext
    ``` 