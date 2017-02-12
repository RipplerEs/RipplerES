CREATE TABLE dbo.AggregateTypes (
    Id					INT					IDENTITY (1, 1) NOT NULL PRIMARY KEY

	, AggregateType	    VARCHAR (255)		NOT NULL
	, FriendlyName		VARCHAR (255)		NULL

	, CONSTRAINT UX_AggregateType					UNIQUE (AggregateType)													
);

GO;

CREATE UNIQUE NONCLUSTERED INDEX UX_AggregateType_FriendlyName		
	ON dbo.AggregateTypes(FriendlyName)
	WHERE FriendlyName IS NOT NULL;
GO;


