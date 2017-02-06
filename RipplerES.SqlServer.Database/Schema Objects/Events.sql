﻿CREATE TABLE dbo.Events (
    Id					INT					IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    version				INT					NOT NULL,
    AggregateId			UNIQUEIDENTIFIER	NOT NULL,
	AggregateType	    VARCHAR (MAX)		NOT NULL,
    [type]				VARCHAR (MAX)		NOT NULL,
    [data]				VARCHAR (MAX)		NOT NULL,
    metadata			VARCHAR (MAX)		NULL
);


