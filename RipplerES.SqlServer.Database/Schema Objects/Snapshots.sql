CREATE TABLE dbo.Snapshots(
    Id						INT					IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	
	AggregateId				UNIQUEIDENTIFIER	NOT NULL,
    [Version]				INT		NOT NULL,
    [Snapshot]				VARCHAR (MAX)		NOT NULL,

	CONSTRAINT UX_Snapshots_AggregateId UNIQUE (AggregateId)
);


