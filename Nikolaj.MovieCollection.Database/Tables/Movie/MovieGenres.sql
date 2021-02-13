CREATE TABLE [Movie].[MovieGenres]
(
	[MovieId] INT NOT NULL,
	[GenreId] INT NOT NULL,
	CONSTRAINT [FK_MovieGenres_Movies.Id] FOREIGN KEY ([MovieId]) REFERENCES [Movie].[Movies]([Id]),
	CONSTRAINT [FK_MovieGenres_Genres.Id] FOREIGN KEY ([GenreId]) REFERENCES [Movie].[Genres]([Id])
)
