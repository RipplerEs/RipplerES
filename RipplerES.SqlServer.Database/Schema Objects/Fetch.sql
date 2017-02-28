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
		[version]			INT,

		EventTypeId			INT,
		EventType			VARCHAR(255),
		EventName			VARCHAR(255),

		AggregateTypeId		INT,
		AggregateType		VARCHAR(255),
		AggregateName		VARCHAR(255),

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
			   [version],
			   
			   evt.EventTypeId,
			   evtType.EventType,
			   evtType.FriendlyName,

			   evt.AggregateTypeId,
			   aggType.AggregateType,
			   aggType.FriendlyName,

			   evt.data,
			   evt.metadata

		  FROM dbo.Events evt
		 INNER JOIN dbo.EventTypes		AS evtType
				 ON evt.EventTypeId			= evtType.Id

		 INNER JOIN dbo.AggregateTypes	AS aggType
				 ON evt.AggregateTypeId		= aggType.Id

		 WHERE evt.Id > @lastEventId

		 ORDER BY evt.Id asc
	END
	ELSE
	BEGIN
		INSERT INTO @eventResult
		SELECT TOP (@maxNumberOfEvents)
			   evt.Id,
			   AggregateId,
			   [version],
			   evt.EventTypeId,
			   evtType.EventType,
			   evtType.FriendlyName		EventName,

			   evt.AggregateTypeId,
			   aggType.AggregateType,
			   aggType.FriendlyName		AggregateName,

			   evt.data,
			   evt.metadata

		  FROM dbo.Events evt
		 INNER JOIN dbo.EventTypes evtType
				 ON evt.EventTypeId			= evtType.Id

		 INNER JOIN dbo.AggregateTypes aggType
				 ON aggType.Id		= evt.AggregateTypeId

		 WHERE evt.Id > @lastEventId

		 ORDER BY evt.Id asc
	END

	SET @lastEventId = (SELECT MAX(EventId) FROM @eventResult)
	IF  @lastEventId IS NOT NULL
	BEGIN
		UPDATE dbo.Subscriptions
		   SET lastEventId = (SELECT MAX(EventId) FROM @eventResult)
		 WHERE channelId		= @channelId
	 END

	 SELECT AggregateId,
			[Version],

			EventTypeId,
			EventType,
			EventName,

			AggregateTypeId,
			AggregateType,
			AggregateName,
			
			data,
			metadata
	   FROM @eventResult
END