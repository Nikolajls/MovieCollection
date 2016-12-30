CREATE TABLE [Person].[Person] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [SoftDeletedDate] DATETIME       NULL,
    CONSTRAINT [PK_Person.Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);

