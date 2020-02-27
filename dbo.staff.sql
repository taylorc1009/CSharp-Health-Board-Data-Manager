CREATE TABLE [dbo].[staff] (
    [id]        INT        NOT NULL,
    [forename]  NCHAR (20) NOT NULL,
    [surname]   NCHAR (20) NOT NULL,
    [address1]  NCHAR (30) NOT NULL,
    [address2]  NCHAR (30) NOT NULL,
    [category]  NCHAR (20) NOT NULL,
    [longitude] FLOAT (53) NOT NULL,
    [latitude]  FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [chk_Category] CHECK ([category]='Care Worker' OR [category]='Social Worker' OR [category]='Community Nurse' OR [category]='General Practitioner')
);

