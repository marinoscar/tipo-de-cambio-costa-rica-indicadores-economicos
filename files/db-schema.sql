DROP TABLE [ExchangeRate]
GO
DROP TABLE [Bank]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Bank](
	[Id] [int] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[BuyCode] [int] NULL,
	[SaleCode] [int] NULL,
	[Type] [varchar](50) NULL,
	[Url] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ExchangeRate](
	[BankId] [int] NOT NULL,
	[DayOfYear] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Month] [tinyint] NOT NULL,
	[MonthName] [varchar](50) NOT NULL,
	[DayName] [varchar](50) NOT NULL,
	[UtcLastUpdateOn] [smalldatetime] NOT NULL,
	[BuyRate] [float] NULL,
	[SaleRate] [float] NULL,
 CONSTRAINT [PK_ExchangeRate] PRIMARY KEY CLUSTERED 
(
	[BankId] ASC,
	[DayOfYear] ASC,
	[Year] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
