CREATE TABLE [Filesystem].[FileSources]
(
	[Id]					INT							NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Path]				NVARCHAR(2048)	NOT NULL,
	[Recursive]		BIT							NOT NULL	DEFAULT(1),
	[CreatedDate]	DATETIME				NOT NULL,
	[Enabled]			BIT							NOT NULL	DEFAULT(1),
	[Name]				NVARCHAR(128)		NOT NULL
)
