CREATE VIEW dbo.VW_Events
AS 
	SELECT evt.[version],
		   evt.AggregateTypeId,
		   agg.FriendlyName				as AggregateName,
		   evt.EventTypeId,
		   et.FriendlyName				as EventName,
		   evt.[data],
		   evt.metadata

	  FROM dbo.Events evt

	 INNER JOIN dbo.AggregateTypes agg
			 ON evt.AggregateTypeId = agg.Id

	 INNER JOIN dbo.EventTypes	et
			 ON evt.EventTypeId	= et.Id