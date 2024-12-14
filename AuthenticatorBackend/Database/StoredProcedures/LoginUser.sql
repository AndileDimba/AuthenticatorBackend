CREATE OR ALTER PROCEDURE LoginUser
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Result INT = 0;
    
    IF EXISTS (
        SELECT 1 
        FROM Users 
        WHERE Username = @Username 
        AND PasswordHash = HASHBYTES('SHA2_256', @Password)
    )
    BEGIN
        SET @Result = 1;
    END
    
    SELECT @Result;
END