CREATE TABLE [Filesystem].[MovieFolders]
(
	[Id]						INT	NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[FileSourceId]	INT NOT NULL,
	[MovieID]				INT NULL
	CONSTRAINT [FK_MovieFolders_FileSourceId_FileSources.Id] FOREIGN KEY ([FileSourceID]) REFERENCES [FileSystem].[Filesources] (Id)
	CONSTRAINT [FK_MovieFolders_MovieId_Movie_Movies.Id] FOREIGN KEY ([FileSourceID]) REFERENCES [Movie].[Movies] (Id)	
)
