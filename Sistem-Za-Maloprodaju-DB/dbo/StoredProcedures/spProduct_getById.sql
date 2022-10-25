CREATE PROCEDURE [dbo].[spProduct_getById]
	@Id int
AS
begin
set nocount on;

select Id, ProductName, [Description], RetailPrice, QuantityInStock
from dbo.Product
where Id = @Id;

	end
