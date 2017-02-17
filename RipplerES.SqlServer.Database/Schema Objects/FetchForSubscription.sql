CREATE PROCEDURE [dbo].[FetchForSubscription]
	@AggregateId		UNIQUEIDENTIFIER
AS
BEGIN TRAN
	DECLARE @lastVersion	INT = -1
	DECLARE @nextVersion	INT

	SELECT @lastVersion = LastVersion
	  FROM dbo.Subscriptions
	 WHERE AggregateId		= @AggregateId

	 SELECT @nextVersion = max([version])
	   FROM dbo.Events
	  WHERE AggregateId		= @AggregateId

	  UPDATE dbo.Subscriptions
	     SET LastVersion = @nextVersion
	   WHERE AggregateId	= @AggregateId

	SELECT * 
	  FROM dbo.VW_Events
	 WHERE AggregateId		= @AggregateId
	   AND [Version] > @lastVersion
	   AND [Version] <= @nextVersion

COMMIT TRAN
