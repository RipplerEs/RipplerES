﻿CREATE TABLE dbo.Subscriptions
(
	channelId			UNIQUEIDENTIFIER		NOT NULL	PRIMARY KEY,
	Name				VARCHAR(255)			NOT NULL,
	LastEventId			BIGINT,
)
