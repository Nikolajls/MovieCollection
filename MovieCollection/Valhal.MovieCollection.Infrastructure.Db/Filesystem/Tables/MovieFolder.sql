CREATE TABLE [Filesystem].[MovieFolder] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Folderpath]      NVARCHAR (MAX) NULL,
    [Fanartpath]      NVARCHAR (MAX) NULL,
    [Posterpath]      NVARCHAR (MAX) NULL,
    [MovieFilePath]   NVARCHAR (MAX) NULL,
    [Subtitlepath]    NVARCHAR (MAX) NULL,
    [MovieId]         INT            NULL,
    [DirectoryPath]   NVARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [SoftDeletedDate] DATETIME       NULL,
    [Nfofile_Id]      INT            NULL,
    CONSTRAINT [PK_Filesystem.MovieFolder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Filesystem.MovieFolder_Filesystem.NfoFile_Nfofile_Id] FOREIGN KEY ([Nfofile_Id]) REFERENCES [Filesystem].[NfoFile] ([Id]),
    CONSTRAINT [FK_Filesystem.MovieFolder_Movies.Movie_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies].[Movie] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Nfofile_Id]
    ON [Filesystem].[MovieFolder]([Nfofile_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MovieId]
    ON [Filesystem].[MovieFolder]([MovieId] ASC);

