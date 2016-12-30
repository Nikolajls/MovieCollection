CREATE PROCEDURE [dbo].[AddMovie]
@CreatedDate [datetime],
@ModifedDate[datetime],
@Title [nvarchar](255),
@OriginalTitle [nvarchar](255),
@ImdbId [nvarchar](30),
@Votes [int],
@TrailerKey [nvarchar](30),
@Released [datetime],
@Rating [float],
@TagLine [nvarchar](255),
@Plot [nvarchar](MAX),
@PlotOutline [nvarchar](MAX),
@Runtime [int]
AS
BEGIN
 INSERT [Movies].Movie([CreatedDate],[ModifiedDate],[Title],[OriginalTitle],[ImdbId],[Votes],[TrailerKey],[Released],[Rating],[TagLine],[Plot],[PlotOutline],[RuntimeMinutes])
 VALUES(@CreatedDate,@ModifedDate,@Title,@OriginalTitle,@ImdbId,@Votes,@TrailerKey,@Released,@Rating,@TagLine,@Plot,@PlotOutline,@Runtime)
  DECLARE @shipmentId AS INT;
    SET @shipmentId = SCOPE_IDENTITY()
  SELECT @shipmentId as SHIPMENTID
END