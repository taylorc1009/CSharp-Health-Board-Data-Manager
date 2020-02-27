CREATE TABLE [dbo].[clients]
(
	[id] INT NOT NULL PRIMARY KEY,
	[forename] NCHAR(20) NOT NULL,
    [surname] NCHAR(20) NOT NULL,
	[address1] NCHAR(30) NOT NULL,
	[address2] NCHAR(30) NOT NULL,
	[longitude] FLOAT NOT NULL,
	[latitude] FLOAT NOT NULL
)
