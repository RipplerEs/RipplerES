CREATE PROCEDURE EnsureAggregateType 
						@AggregateType		VARCHAR(MAX),
						@AggregateTypeId	INT					OUTPUT
AS 
BEGIN TRY 
	BEGIN TRAN
		SELECT @AggregateTypeId			= Id
		  FROM AggregateTypes
		 WHERE AggregateType		= @AggregateType

		 IF @AggregateTypeId IS NULL
		 BEGIN
			INSERT INTO AggregateTypes
			SELECT @AggregateType

			SET @AggregateTypeId =  SCOPE_IDENTITY()
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
