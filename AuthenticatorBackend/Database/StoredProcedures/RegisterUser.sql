CREATE OR ALTER PROCEDURE RegisterUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Hash the password
        DECLARE @PasswordHash VARBINARY(64) = HASHBYTES('SHA2_256', @Password);

        -- Insert the new user
        INSERT INTO Users (Username, PasswordHash)
        VALUES (@Username, @PasswordHash);
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        RETURN ERROR_NUMBER();
    END CATCH
END