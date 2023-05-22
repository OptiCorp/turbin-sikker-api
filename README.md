# turbin-sikker-api


##sql queries

```

// User Admin Roles

CREATE USER [bjorn.goa@bouvet.no] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA = dbo;
ALTER ROLE db_datareader ADD MEMBER [bjorn.goa@bouvet.no];
ALTER ROLE db_datawriter ADD MEMBER [bjorn.goa@bouvet.no];
ALTER ROLE db_owner ADD MEMBER [bjorn.goa@bouvet.no];

CREATE USER [malin.svela@bouvet.no] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA = dbo;
ALTER ROLE db_datareader ADD MEMBER [malin.svela@bouvet.no];
ALTER ROLE db_datawriter ADD MEMBER [malin.svela@bouvet.no];
ALTER ROLE db_ddladmin ADD MEMBER [malin.svela@bouvet.no];

// Tables

CREATE TABLE User (
    id varchar(500) NOT NULL PRIMARY KEY,
    role_id int NOT NULL,
    first_name varchar(250) NOT NULL,
    last_name varchar(250) NOT NULL,
    username varchar(250) NOT NULL,
    email varchar(300) NOT NULL
)

CREATE TABLE Task (
    description VARCHAR (1500) NULL,
    category_id INT            NULL
);

CREATE TABLE Category (
    id int NOT NULL PRIMARY KEY,
    name VARCHAR(100)
);
```
