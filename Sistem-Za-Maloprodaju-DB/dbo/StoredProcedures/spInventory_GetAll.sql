CREATE PROCEDURE [dbo].[spInventory_GetAll]
	As
	begin 
	set nocount on;

	select  [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	from dbo.Inventory;


	end
