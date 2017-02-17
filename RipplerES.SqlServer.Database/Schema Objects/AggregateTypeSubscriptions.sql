CREATE TABLE dbo.AggregateTypeSubscriptions
(
	SubscriptionId		UNIQUEIDENTIFIER	NOT NULL , 
    AggregateTypeId		INT					NOT NULL, 
    FirstEventOnly		BIT					NOT NULL DEFAULT 1,
	 
    [LastEventId] INT NOT NULL DEFAULT -1, 
    PRIMARY KEY (AggregateTypeId, SubscriptionId),
	CONSTRAINT FK_AggregateSubscription_SubscriptionType 
								   FOREIGN KEY (SubscriptionId)
								   REFERENCES Subscriptions(Id),
	CONSTRAINT FK_AggregateSubscription_AggregateType
								   FOREIGN KEY (AggregateTypeId)
								   REFERENCES AggregateTypes(Id)
)
