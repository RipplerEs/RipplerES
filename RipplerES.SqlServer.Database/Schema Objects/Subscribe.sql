CREATE PROCEDURE dbo.Subscribe
	@channelId		UNIQUEIDENTIFIER,
	@name			VARCHAR(255)
AS
BEGIN
	DECLARE @currentName	VARCHAR(255)

	SELECT @currentName		= name
	  FROM dbo.Subscriptions
	 WHERE channelId			= @channelId

	 IF @currentName IS NULL
	 BEGIN
		INSERT INTO dbo.Subscriptions
		SELECT @channelId,
			   @name,
			   NULL
	 END

	 IF @currentName IS NOT NULL 
			AND @currentName != @name
	BEGIN
		UPDATE dbo.Subscriptions
		   SET name		= @name
		 WHERE @channelId	= @channelId
	END
END


