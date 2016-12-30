CREATE PROCEDURE [dbo].[AssignMovieToMovieFolder]
	@Id[int],
	@MovieId[int]
AS
BEGIN
		update Filesystem.MovieFolder SET [MovieId] = @MovieId WHERE Id = @Id
END 
