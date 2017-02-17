CREATE PROCEDURE dbo.Unsubscribe
	@SubscriptionId			UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE dbo.Subscriptions
		SET [Disabled] = 0
	  WHERE Id = @SubscriptionId
END