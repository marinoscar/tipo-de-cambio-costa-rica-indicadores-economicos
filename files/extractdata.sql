select
	Id, Bank,
	(Select Code FROM _Banks where _Banks.Bank = CompiledSet.Bank And Action = 'Compra') As BuyCode,
	(Select Code FROM _Banks where _Banks.Bank = CompiledSet.Bank And Action = 'Venta') As SaleCode,
	(Select Type FROM _Banks where _Banks.Bank = CompiledSet.Bank And Action = 'Compra') As Type
from(
select ROW_NUMBER() over (order by Bank ASC) As Id,
Bank
from(
select distinct bank As Bank from _Banks) As Banks
) As CompiledSet

/*

CREATE TABLE [dbo].[_Banks](
	[Code] [int] NULL,
	[Action] [nchar](100) NULL,
	[Type] [nchar](100) NULL,
	[Bank] [nchar](200) NULL
) ON [PRIMARY]
GO


*/