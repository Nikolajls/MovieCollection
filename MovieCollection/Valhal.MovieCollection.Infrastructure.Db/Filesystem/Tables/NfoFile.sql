CREATE TABLE [Filesystem].[NfoFile] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Filepath]        NVARCHAR (MAX) NULL,
    [Content]         NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [SoftDeletedDate] DATETIME       NULL,
    CONSTRAINT [PK_Filesystem.NfoFile] PRIMARY KEY CLUSTERED ([Id] ASC)
);

