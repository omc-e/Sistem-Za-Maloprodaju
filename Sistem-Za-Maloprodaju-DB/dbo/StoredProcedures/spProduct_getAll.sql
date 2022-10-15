CREATE PROCEDURE [dbo].[spProduct_getAll]
	
AS
	begin		
		set nocount on;


		select Id, ProductName, [Description], RetailPrice, QuantityInStock
		from dbo.Product
		order by ProductName;
	end
