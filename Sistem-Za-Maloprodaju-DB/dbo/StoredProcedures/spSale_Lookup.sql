﻿CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
	begin
	set nocount on;

	select Id 
	from dbo.Sale 
	where CashierId = @CashierId and SaleDate = @SaleDate;

	end
