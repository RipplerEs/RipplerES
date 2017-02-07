CREATE PROCEDURE GetEventsByAggregateId 
						@AggregateId		UNIQUEIDENTIFIER
AS 
BEGIN
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
END