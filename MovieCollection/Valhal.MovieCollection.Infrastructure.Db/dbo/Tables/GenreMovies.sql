CREATE TABLE [dbo].[GenreMovies] (
    [Genre_Id] INT NOT NULL,
    [Movie_Id] INT NOT NULL,
    CONSTRAINT [PK_dbo.GenreMovies] PRIMARY KEY CLUSTERED ([Genre_Id] ASC, [Movie_Id] ASC),
    CONSTRAINT [FK_dbo.GenreMovies_Movies.Genre_Genre_Id] FOREIGN KEY ([Genre_Id]) REFERENCES [Movies].[Genre] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.GenreMovies_Movies.Movie_Movie_Id] FOREIGN KEY ([Movie_Id]) REFERENCES [Movies].[Movie] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Movie_Id]
    ON [dbo].[GenreMovies]([Movie_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Genre_Id]
    ON [dbo].[GenreMovies]([Genre_Id] ASC);

