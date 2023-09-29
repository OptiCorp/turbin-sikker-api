-- User Admin Roles

CREATE USER [bjorn.goa@bouvet.no] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA = dbo;
ALTER ROLE db_datareader ADD MEMBER [bjorn.goa@bouvet.no];
ALTER ROLE db_datawriter ADD MEMBER [bjorn.goa@bouvet.no];
ALTER ROLE db_owner ADD MEMBER [bjorn.goa@bouvet.no];

CREATE USER [malin.svela@bouvet.no] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA = dbo;
ALTER ROLE db_datareader ADD MEMBER [malin.svela@bouvet.no];
ALTER ROLE db_datawriter ADD MEMBER [malin.svela@bouvet.no];
ALTER ROLE db_ddladmin ADD MEMBER [malin.svela@bouvet.no];

-- Tables

CREATE TABLE User (
    id varchar(500) PRIMARY KEY NOT NULL DEFAULT newid(),
    userRoleId varchar(500) NOT NULL,
    firstName varchar(250) NOT NULL,
    lastName varchar(250) NOT NULL,
    username varchar(250) NOT NULL,
    email varchar(300) NOT NULL,
    password varchar(300) NOT NULL
    
);
ALTER TABLE [User]
    ADD CONSTRAINT FK_User_Role_Id FOREIGN KEY (userRoleId)
    REFERENCES User_Role (id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
;

CREATE TABLE User_Role (
    id varchar(500) NOT NULL PRIMARY KEY,
    Name varchar(100) NOT NULL,
);


CREATE TABLE Form_Task (
    description VARCHAR (1500) NOT NULL,
    category_id VARCHAR (500)  NOT NULL,
    id VARCHAR (500) NOT NULL PRIMARY KEY,
);

ALTER TABLE Form_Task
    ADD CONSTRAINT FK_Category_Id FOREIGN KEY (category_id)
    REFERENCES Category (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

CREATE TABLE Category (
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
);

CREATE TABLE Checklist (
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    checklistStatus INT,
    createdDate DATE,
    updatedDate DATE,
    createdBy VARCHAR(500)
);

ALTER TABLE Form
    ADD CONSTRAINT FK_Created_By FOREIGN KEY (created_by)
    REFERENCES User (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

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

ALTER TABLE Punch
    ADD CONSTRAINT FK_Punch_User_Id FOREIGN KEY (user_id)
    REFERENCES User (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

ALTER TABLE Punch
    ADD CONSTRAINT FK_Punch_Form_Id FOREIGN KEY (form_id)
    REFERENCES Form (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

CREATE TABLE Upload(
    id VARCHAR(500) NOT NULL PRIMARY KEY,
    punchId VARCHAR(500) NOT NULL,
    blobRef VARCHAR(1500) NOT NULL,
);

ALTER TABLE Upload
    ADD CONSTRAINT FK_Upload_Punch_Id FOREIGN KEY (punch_id)
    REFERENCES Punch (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

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

ALTER TABLE Form_Handler
    ADD CONSTRAINT FK_Form_Handler_Form_Id FOREIGN KEY (form_id)
    REFERENCES Form (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

ALTER TABLE Form_Handler
    ADD CONSTRAINT FK_Form_Handler_User_Id FOREIGN KEY (user_id)
    REFERENCES User (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

CREATE TABLE Ad_Link(
    user_id VARCHAR(500),
    ad_identifier VARCHAR(500)
);

ALTER TABLE Ad_Link
    ADD CONSTRAINT FK_Ad_Link_User FOREIGN KEY (user_id)
    REFERENCES User (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

CREATE TABLE Task_Form_Link(
    task_id VARCHAR(500),
    form_id VARCHAR(500)
)

ALTER TABLE Task_Form_Link
    ADD CONSTRAINT FK_Task_Id FOREIGN KEY (task_id)
    REFERENCES Form_Task (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;

ALTER TABLE Task_Form_Link
    ADD CONSTRAINT FK_Form_Id FOREIGN KEY (form_id)
    REFERENCES Form (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
;
