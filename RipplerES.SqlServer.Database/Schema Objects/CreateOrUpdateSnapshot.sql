CREATE PROCEDURE CreateOrUpdateSnapshot 
						@AggregateId		UNIQUEIDENTIFIER,
						@Version			INT,
						@Snapshot			VARCHAR(MAX)
AS 
BEGIN TRY 
	BEGIN TRAN
	
	DECLARE @SnashotId		INT

	SELECT @SnashotId = Id
	  FROM Snapshots
	 WHERE AggregateId = @AggregateId
	
	IF @Snapshot IS NULL
	BEGIN
		INSERT INTO Snapshots (AggregateId, [Version], [Snapshot])
		SELECT @AggregateId,
			   @Version,
			   @Snapshot
	END
	ELSE
	BEGIN
		UPDATE Snapshots
		   SET [Snapshot]		= @Snapshot,
			   [Version]		= @Version
		 WHERE Id		= @SnashotId	
	END
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
