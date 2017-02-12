CREATE PROCEDURE EnsureAggregateType 
						@AggregateType				VARCHAR(255),
						@AggregateFriendlyName		VARCHAR(255),
						@AggregateTypeId			INT					OUTPUT
AS 
BEGIN TRY 
	BEGIN TRAN
		SELECT @AggregateTypeId			= Id
		  FROM AggregateTypes
		 WHERE AggregateType		= @AggregateType

		 IF @AggregateTypeId IS NULL
		 BEGIN
			INSERT INTO AggregateTypes
			SELECT @AggregateType, @AggregateFriendlyName

			SET @AggregateTypeId =  SCOPE_IDENTITY()
		 END
		 ELSE
		 BEGIN
			UPDATE AggregateTypes
			   SET AggregateType	= @AggregateType,
				   FriendlyName		= @AggregateFriendlyName
			 WHERE Id	= @AggregateTypeId
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
