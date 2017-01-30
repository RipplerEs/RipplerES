CREATE TABLE dbo.Events (
    Id					INT					IDENTITY (1, 1) NOT NULL,
    version				INT					NOT NULL,
    AggregateId			UNIQUEIDENTIFIER	NOT NULL,
    [type]				VARCHAR (MAX)		NOT NULL,
    [data]				VARCHAR (MAX)		NOT NULL,
    metadata			VARCHAR (MAX)		NULL
);


