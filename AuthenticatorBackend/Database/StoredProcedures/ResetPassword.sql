CREATE PROCEDURE ResetPassword
    @Username NVARCHAR(50),
    @NewPassword NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET PasswordHash = HASHBYTES('SHA2_256', @NewPassword)
    WHERE Username = @Username;
END