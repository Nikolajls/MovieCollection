CREATE PROCEDURE [dbo].[AssignGenreMovie]
	@GenreId[int],
	@MovieId[int]
AS
BEGIN
	INSERT [GenreMovies](Genre_Id,Movie_Id)
	VALUES(@GenreId,@MovieId)
END 
