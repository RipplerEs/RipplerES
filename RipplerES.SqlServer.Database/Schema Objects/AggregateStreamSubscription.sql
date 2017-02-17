CREATE TABLE dbo.AggregateStreamSubscription
(
	SubscriptionId		UNIQUEIDENTIFIER	NOT NULL , 
    [AggregateId]		UNIQUEIDENTIFIER	NOT NULL, 
    FirstEventOnly		BIT					NOT NULL DEFAULT 1,
	 
    [LastEventId] INT NOT NULL DEFAULT -1, 
    PRIMARY KEY ([AggregateId], SubscriptionId),
	CONSTRAINT FK_AggregateStreamSubscription_SubscriptionType 
								   FOREIGN KEY (SubscriptionId)
								   REFERENCES Subscriptions(Id),

	CONSTRAINT FK_AggregateStreamSubscription_AggregateType
								   FOREIGN KEY (AggregateId)
								   REFERENCES Events(AggregateId)

)
