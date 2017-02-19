CREATE TABLE dbo.AccountSummaryView
(
	[AggregateId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[version] INT NOT NULL,
    [FriendlyName] VARCHAR(MAX) NULL, 
    [Balance] NUMERIC NOT NULL DEFAULT 0.0
)
