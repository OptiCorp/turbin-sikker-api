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
    role_id varchar(500) NOT NULL,
    first_name varchar(250) NOT NULL,
    last_name varchar(250) NOT NULL,
    username varchar(250) NOT NULL,
    email varchar(300) NOT NULL
)

CREATE TABLE Task (
    description VARCHAR (1500) NOT NULL,
    category_id VARCHAR (500)  NOT NULL,
    id VARCHAR (500) NOT NULL PRIMARY KEY,
);

CREATE TABLE Category (
    id int NOT NULL PRIMARY KEY,
    name VARCHAR(100),
    id VARCHAR(500),
);

CREATE TABLE Form (
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    created_date DATE,
    created_by varchar(500),
);

CREATE TABLE Punch(
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    form_id VARCHAR(500) NOT NULL,
    user_id VARCHAR(500) NOT NULL,
    created_date DATE,
    punch_description VARCHAR(1500),
    severity int NOT NULL, (1 = minor, 2 = major, 3 = critical ???)
    punch_status int NOT NULL, (1 = pending, 2 = approved, 3 = rejected)
    active TINYINT,
    edited_date DATE,
);

CREATE TABLE Upload(
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    punch_id VARCHAR(500) NOT NULL,
    blob_ref VARCHAR(1500) NOT NULL,
);

CREATE TABLE Form_Handler(
    id VARCHAR NOT NULL PRIMARY KEY,
    form_id VARCHAR(500) NOT NULL,
    created_date DATE,
    expire_interval DATE,
    user_id VARCHAR(500) NOT NULL,
    form_status int,
    form_data NVARCHAR(4000),
    active TINYINT,
    edited_date DATE,
);

CREATE TABLE Ad_Link(
    user_id VARCHAR(500),
    ad_identifier VARCHAR(500)
);

CREATE TABLE Task_Form_Link(
    task_id VARCHAR(500),
    form_id VARCHAR(500)
)
```
