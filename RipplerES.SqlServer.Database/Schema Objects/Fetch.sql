CREATE PROCEDURE dbo.[Fetch]
	@channelId				UNIQUEIDENTIFIER,
	@maxNumberOfEvents		INT		= 0
AS
BEGIN
	DECLARE @lastEventId	BIGINT
	DECLARE @eventResult	TABLE
	(
		EventId				BIGINT,
		AggregateId			UNIQUEIDENTIFIER,
		[Version]			INT,
		EventTypeId			INT,
		EventType			VARCHAR(255),
		FriendlyName		VARCHAR(255),
		[Data]				VARCHAR(MAX),
		[MetaData]			VARCHAR(MAX)
	)

	
	SELECT @lastEventId			= LastEventId
	  FROM dbo.Subscriptions
	 WHERE channelId = @channelId

	IF @lastEventId IS NULL
	BEGIN
		SET @lastEventId = 0
	END

	IF @maxNumberOfEvents = 0 
	BEGIN
		INSERT INTO @eventResult
		SELECT evt.Id,
			   AggregateId,
			   [Version],
			   evt.EventTypeId,
			   evtType.EventType,
			   evtType.FriendlyName,
			   evt.data,
			   evt.metadata

		  FROM dbo.Events evt
		 INNER JOIN dbo.EventTypes evtType
				 ON evt.EventTypeId		= evtType.Id

		 WHERE evt.Id > @lastEventId

		 ORDER BY evt.Id asc
	END
	ELSE
	BEGIN
		INSERT INTO @eventResult
		SELECT TOP (@maxNumberOfEvents)
			   evt.Id,
			   AggregateId,
			   [Version],
			   evt.EventTypeId,
			   evtType.EventType,
			   evtType.FriendlyName,
			   evt.data,
			   evt.metadata

		  FROM dbo.Events evt
		 INNER JOIN dbo.EventTypes evtType
				 ON evt.EventTypeId		= evtType.Id

		 WHERE evt.Id > @lastEventId

		 ORDER BY evt.Id asc
	END

	UPDATE dbo.Subscriptions
	   SET lastEventId = (SELECT MAX(EventId) FROM @eventResult)
	 WHERE channelId	= @channelId

	 SELECT AggregateId,
			[Version],
			EventTypeId,
			EventType,
			FriendlyName,
			data,
			metadata
	   FROM @eventResult
END
