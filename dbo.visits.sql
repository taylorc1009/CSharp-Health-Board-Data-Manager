CREATE TABLE [dbo].[visits]
(
	[staffID] NCHAR(3) PRIMARY KEY,
    [clientID] INTEGER NOT NULL,
    [vType] INTEGER NOT NULL,
	CONSTRAINT chk_Type CHECK (vType IN (0, 1, 2, 3)),
	[dateAndTime] DATETIME NOT NULL
)
