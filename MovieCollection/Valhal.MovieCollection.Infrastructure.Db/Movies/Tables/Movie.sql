CREATE TABLE [Movies].[Movie] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ImdbId]          NVARCHAR (MAX) NULL,
    [Title]           NVARCHAR (MAX) NULL,
    [OriginalTitle]   NVARCHAR (MAX) NULL,
    [Plot]            NVARCHAR (MAX) NULL,
    [PlotOutline]     NVARCHAR (MAX) NULL,
    [RuntimeMinutes]  INT            NOT NULL,
    [TagLine]         NVARCHAR (MAX) NULL,
    [DirectorId]      INT            NULL,
    [TrailerKey]      NVARCHAR (MAX) NULL,
    [Rating]          FLOAT (53)     NOT NULL,
    [Released]        DATETIME       NULL,
    [Votes]           INT            NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [SoftDeletedDate] DATETIME       NULL,
    CONSTRAINT [PK_Movies.Movie] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Movies.Movie_Person.Person_DirectorId] FOREIGN KEY ([DirectorId]) REFERENCES [Person].[Person] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_DirectorId]
    ON [Movies].[Movie]([DirectorId] ASC);

