CREATE TABLE [Movie].[Movies]
(
	[Id]						INT							NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name]					NVARCHAR(512)		NOT NULL,
	[OriginalName]	NVARCHAR(512)		NULL,
	[IMDBId]				NVARCHAR(12)		NULL,
	[TMDBId]				NVARCHAR(12)		NULL,
	[Year]					INT							NOT NULL,
	[ReleaseDate]		DATETIME				NOT NULL,
	[Rating]				FLOAT						NULL,
	[Votes]					INT							NULL,
	[Runtime]				INT							NULL,
	[Plot]					NVARCHAR(2048)	NULL,
	[Outline]				NVARCHAR(2048)	NULL,
	[Tagline]				NVARCHAR(2048)	NULL,
	[Country]				NVARCHAR(256)		NULL,
	[AddedDate]			DATETIME				NULL,
	[ModifiedDate]	DATETIME				NULL
)
