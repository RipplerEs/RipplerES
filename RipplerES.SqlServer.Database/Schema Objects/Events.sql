CREATE TABLE dbo.Events (
    version					INT					NOT NULL,
    [AggregateTypeId]		INT					NOT NULL,
    [EventTypeId]			INT					NOT NULL,
	
	AggregateId				UNIQUEIDENTIFIER	NOT NULL,
    [data]					VARCHAR (MAX)		NOT NULL,
    metadata				VARCHAR (MAX)		NULL,

	CONSTRAINT		FK_AggregateType_Id		FOREIGN KEY (AggregateTypeId) 
											REFERENCES AggregateTypes (Id),

	CONSTRAINT		FK_EventType_Id			FOREIGN KEY (EventTypeId) 
											REFERENCES EventTypes (Id), 
    PRIMARY KEY ([version], [AggregateId])
);


