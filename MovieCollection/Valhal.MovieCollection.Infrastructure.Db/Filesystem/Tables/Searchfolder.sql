CREATE TABLE [Filesystem].[Searchfolder] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Path]            NVARCHAR (MAX) NULL,
    [Title]           NVARCHAR (MAX) NULL,
    [Active]          BIT            NOT NULL,
    [Recursive]       BIT            NOT NULL,
    [LastScan]        DATETIME       NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [SoftDeletedDate] DATETIME       NULL,
    CONSTRAINT [PK_Filesystem.Searchfolder] PRIMARY KEY CLUSTERED ([Id] ASC)
);

