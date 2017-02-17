CREATE TABLE dbo.Subscriptions
(
	Id				UNIQUEIDENTIFIER	NOT NULL	PRIMARY KEY, 
	AggregateId		UNIQUEIDENTIFIER	NOT NULL, 
	[LastVersion]	INT					NOT NULL	DEFAULT		 1,
	[Disabled]		BIT					NOT NULL	DEFAULT		 0, 
    CONSTRAINT FK_Subscriptions_Event_AggregateId
								   FOREIGN KEY (AggregateId, LastVersion)
								   REFERENCES Events(AggregateId, [version])
)
