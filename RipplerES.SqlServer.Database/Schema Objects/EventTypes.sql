CREATE TABLE dbo.EventTypes (
    Id					INT					IDENTITY (1, 1) NOT NULL PRIMARY KEY
	, EventType			VARCHAR (255)		NOT NULL
	, FriendlyName		VARCHAR (255)		NULL
	, CONSTRAINT UX_EventTypes				UNIQUE(EventType)
);
GO;

CREATE UNIQUE NONCLUSTERED INDEX UX_EventTypes_FriendlyName		
	ON dbo.EventTypes(FriendlyName)
	WHERE FriendlyName IS NOT NULL;
GO;


