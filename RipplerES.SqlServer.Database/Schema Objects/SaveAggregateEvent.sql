CREATE PROCEDURE SaveAggregateEvent 
						@AggregateId		UNIQUEIDENTIFIER,
						@expectedVersion	INT,
						@type				VARCHAR(255),
						@data				VARCHAR(max),
						@metadata			VARCHAR(max)
AS 
BEGIN TRY 
	BEGIN TRAN
		DECLARE @currentVersion		INT
		DECLARE @newVersion			INT

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
							  [type],
							  [data],
							  metadata
					)
			 VALUES (		  @newVersion,
							  @AggregateId,
							  @type,
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
