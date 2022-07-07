CREATE PROCEDURE [dbo].[spUserLookup]
	@UserId nvarchar(128)
	
AS
BEGIN
set nocount on;
	SELECT UserId, FirstName, LastName, EmailAddress, CreateDate
	FROM [dbo].[User]
	WHERE UserId = @UserId;
END
