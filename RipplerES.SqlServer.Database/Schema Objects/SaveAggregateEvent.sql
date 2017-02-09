CREATE PROCEDURE SaveAggregateEvent 
						@AggregateId		UNIQUEIDENTIFIER,
						@ExpectedVersion	INT,
						@AggregateType		VARCHAR(MAX),
						@EventType			VARCHAR(255),
						@Data				VARCHAR(MAX),
						@Metadata			VARCHAR(MAX),
						@Snapshot			VARCHAR(MAX)	= NULL
AS 
BEGIN TRY 
	BEGIN TRAN
		DECLARE @currentVersion		INT
		DECLARE @newVersion			INT

		DECLARE @AggregateTypeId	INT
		EXEC  EnsureAggregateType @AggregateType, @AggregateTypeId OUTPUT

		DECLARE @EventTypeId	INT
		EXEC  EnsureEventType @EventType, @EventTypeId OUTPUT

		SELECT @currentVersion = max([version])
		  FROM Events
		 WHERE AggregateId = @AggregateId

		 IF @currentVersion is null AND @ExpectedVersion = -1
		 BEGIN
			SET @currentVersion		= 0
			SET @ExpectedVersion	= 0
		 END

		IF @currentVersion != @ExpectedVersion
		BEGIN
			;THROW	50000, 'version number did not match expected version number', 1
		END

		SET @newVersion = @currentVersion + 1
		INSERT INTO Events (  [version],
							  AggregateId,
							  AggregateTypeId,
							  EventTypeId,
							  [data],
							  metadata
					)
			 VALUES (		  @newVersion,
							  @AggregateId,
							  @AggregateTypeId,
							  @EventTypeId,
							  @Data,
							  @Metadata
					)

		IF @Snapshot IS NOT NULL
		BEGIN
			EXEC CreateOrUpdateSnapshot @AggregateId, @newVersion, @Snapshot 
		END

		SELECT @newVersion
	COMMIT
END TRY
BEGIN CATCH
	ROLLBACK

    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorState = ERROR_STATE();

	;THROW 50000, @ErrorMessage, @ErrorState
END CATCH;
