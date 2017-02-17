CREATE PROCEDURE dbo.Subscribe
	@SubscriptionId			UNIQUEIDENTIFIER,
	@AggregateId			UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO dbo.Subscriptions 
	(
		Id,
		AggregateId
	)
	SELECT @SubscriptionId,
		   @AggregateId
		
END