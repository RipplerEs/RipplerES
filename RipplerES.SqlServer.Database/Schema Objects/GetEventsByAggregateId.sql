CREATE PROCEDURE GetEventsByAggregateId 
						@AggregateId		UNIQUEIDENTIFIER,
						@UseSnapshot		BIT = 0
AS 
BEGIN
	DECLARE @Version		INT
	DECLARE @Snapshot		VARCHAR(MAX)

	IF @UseSnapshot = 1
	BEGIN
		SELECT @Version		= [Version],
			   @Snapshot	= [Snapshot]
		  FROM Snapshots
		 WHERE AggregateId		= @AggregateId
	END

	SELECT evt.Id,
		   [version],

		   AggregateId,
		   AggregateType,
		   EventType,
		   [data],
		   metadata

	  FROM Events								AS Evt
	 INNER JOIN AggregateTypes					AS Atyp
			 ON Evt.AggregateTypeId		= Atyp.Id
	 INNER JOIN EventTypes						AS Etyp
			 ON Evt.EventTypeId			= Etyp.Id

	 WHERE AggregateId		= @AggregateId
	   AND (@UseSnapshot = 0
			OR @Version IS NULL
			OR [version] >= @Version)

	IF @UseSnapshot = 1
	BEGIN
		SELECT @Snapshot as [Snapshot], 
			   @Version as SnapshotVersion
	END
END