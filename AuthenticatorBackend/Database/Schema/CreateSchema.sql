-- If you need to create or modify the Users table
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE()
);