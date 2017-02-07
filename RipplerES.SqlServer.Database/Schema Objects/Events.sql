CREATE TABLE dbo.Events (
    Id						INT					IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    version					INT					NOT NULL,
    [AggregateTypeId]		INT					NOT NULL,
    [EventTypeId]			INT					NOT NULL,
	
	AggregateId				UNIQUEIDENTIFIER	NOT NULL,
    [data]					VARCHAR (MAX)		NOT NULL,
    metadata				VARCHAR (MAX)		NULL,

	CONSTRAINT		FK_AggregateType_Id		FOREIGN KEY (AggregateTypeId) 
											REFERENCES AggregateTypes (Id),

	CONSTRAINT		FK_EventType_Id			FOREIGN KEY (EventTypeId) 
											REFERENCES EventTypes (Id)
);


