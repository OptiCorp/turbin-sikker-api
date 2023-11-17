# turbin-sikker-api

## Key Features

- [CRUD Checklist]
- [CRUD Tasks for the Checklists]
- [CRUD Workflows from the Checklists]
- [CRUD Punches for the Checklist Tasks]
- [CRUD Uploads for the Punches]
- [Create Invoices with help from [Invoice Function App](https://github.com/OptiCorp/invoice-function-app)]
- [Read Notifications created in the [Invoice Function App](https://github.com/OptiCorp/invoice-function-app)]

## Run API

Clone and Run the API Application:

1. Clone repo: `gh repo clone OptiCorp/turbin-sikker-api`
2. Navigate to project folder: `cd turbin-sikker-api/`
3. Run API: `cd turbin.sikker.core && dotnet run`

## Branch name convention 
1. type (feat, fix, chore, refactor)
2. issue number
3. Descriptive text.

### Example:

```
feat/#1/users-endpoint
```

## ER Diagram

```mermaid
    erDiagram
        CATEGORY ||--o{ TASK : "belongs to"
        CHECKLIST }o--o{ TASK : "is a part of"
        CHECKLIST ||--o{ WORKFLOW : "is used in"
        TASK ||--o{ PUNCH : "can include"
        WORKFLOW ||--o{ PUNCH : "is associated with"
        USER ||--o{ PUNCH : "creates"
        USER ||--o{ CHECKLIST : "creates"
        USER ||--o{ WORKFLOW : "is assigned to"
        PUNCH ||--o{ UPLOAD : "contains"
        ROLE ||--o{ USER : "has"
```

## Database Migration

After changing models, services, contollers or context, run the following command:

```dotnet
dotnet ef migrations add <NameOfChanges>
dotnet ef database update (or start the solution)
```

To undo migration, run the following command:

```dotnet
dotnet ef migrations remove
dotnet ef database update (or start the solution)
```

or:

```dotnet
dotnet ef database update <NameofMigrationYouWantToRevertTo>
dotnet ef migrations remove (will remove all migration after the one you reverted to)
```
