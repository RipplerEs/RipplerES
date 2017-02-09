CREATE TABLE dbo.Snapshots(
	AggregateId				UNIQUEIDENTIFIER	NOT NULL PRIMARY KEY,
    [Version]				INT		NOT NULL,
    [Snapshot]				VARCHAR (MAX)		NOT NULL,

	CONSTRAINT UX_Snapshots_AggregateId UNIQUE (AggregateId)
);


