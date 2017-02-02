CREATE PROCEDURE GetEventsByAggregateId 
						@AggregateId		UNIQUEIDENTIFIER
AS 
BEGIN
	SELECT Id,
		   [version],

		   AggregateId,
		   [type],
		   [data],
		   metadata

	  FROM Events
	 WHERE AggregateId		= @AggregateId
END