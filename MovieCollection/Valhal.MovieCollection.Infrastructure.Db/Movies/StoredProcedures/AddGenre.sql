CREATE PROCEDURE [dbo].[AddGenre]
@CreatedDate [datetime],
@ModifedDate[datetime],
@Name [nvarchar](255)
AS
BEGIN
INSERT [Movies].Genre([CreatedDate],[ModifiedDate],[Name])
VALUES(@CreatedDate,@ModifedDate,@Name)
  DECLARE @shipmentId AS INT;
    SET @shipmentId = SCOPE_IDENTITY()
  SELECT @shipmentId as SHIPMENTID
END