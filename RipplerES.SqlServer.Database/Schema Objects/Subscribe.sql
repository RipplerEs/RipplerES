CREATE PROCEDURE dbo.Subscribe
	@SubscriptionId			UNIQUEIDENTIFIER,
	@AggregateId			UNIQUEIDENTIFIER
AS
BEGIN
	IF NOT EXISTS (
		SELECT 1 
		  FROM dbo.Subscriptions 
		 WHERE Id		= @SubscriptionId )
	BEGIN

		INSERT INTO dbo.Subscriptions 
		(
			Id,
			AggregateId
		)
		SELECT @SubscriptionId,
			   @AggregateId
	
	END
END