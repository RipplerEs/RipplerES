CREATE PROCEDURE SaveAggregateEvent 
						@AggregateId		UNIQUEIDENTIFIER,
						@expectedVersion	INT,
						@AggregateType		VARCHAR(MAX),
						@EventType			VARCHAR(255),
						@data				VARCHAR(MAX),
						@metadata			VARCHAR(MAX)
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

		 IF @currentVersion is null AND @expectedVersion = -1
		 BEGIN
			SET @currentVersion		= 0
			SET @expectedVersion	= 0
		 END

		IF @currentVersion != @expectedVersion
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
							  @data,
							  @metadata
					)

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
