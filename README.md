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
    category_id VARCHAR (500)            NULL,
    id VARCHAR (500) NOT NULL PRIMARY KEY
);

CREATE TABLE Category (
    id int NOT NULL PRIMARY KEY,
    name VARCHAR(100),
    id VARCHAR(500),
);

CREATE TABLE Form (
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    title VARCHAR(100),
    created_date DATE,
    created_by varchar(500),
);

CREATE TABLE Punch(
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    form_id VARCHAR(500),
    user_id VARCHAR(500),
    created_date DATE,
    punch_description VARCHAR(1500),
    severity int, 1 = minor, 2 = major, 3 = critical ???
    punch_status int, 1 = pending, 2 = approved, 3 = rejected
    active TINYINT,
    edited_date DATE,
);

CREATE TABLE Upload(
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    punch_id VARCHAR(500),
    blob_ref VARCHAR(1500)
);

CREATE TABLE Form_Handler(
    id int NOT NULL PRIMARY KEY,
    form_id int,
    created_date DATE,
    expire_interval DATE,
    user_id INT,
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
