# turbin-sikker-api


##sql queries

```
CREATE USER [bjorn.goa@bouvet.no] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA = dbo;
ALTER ROLE db_datareader ADD MEMBER [bjorn.goa@bouvet.no];
ALTER ROLE db_datawriter ADD MEMBER [bjorn.goa@bouvet.no];
```
