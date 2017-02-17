CREATE VIEW dbo.VW_Events
AS 
	SELECT evt.[version],
		   evt.AggregateTypeId,
		   agg.FriendlyName				as AggregateName,
		   agg.AggregateType,
		   evt.EventTypeId,
		   et.FriendlyName				as EventName,
		   et.EventType,
		   evt.[data],
		   evt.metadata

	  FROM dbo.Events evt

	 INNER JOIN dbo.AggregateTypes agg
			 ON evt.AggregateTypeId = agg.Id

	 INNER JOIN dbo.EventTypes	et
			 ON evt.EventTypeId	= et.Id