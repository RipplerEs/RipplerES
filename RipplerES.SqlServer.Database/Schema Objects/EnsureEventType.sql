CREATE PROCEDURE EnsureEventType 
						@EventType				VARCHAR(255),
						@EventFriendlyName		VARCHAR(255),
						@EventTypeId			INT					OUTPUT
AS 
BEGIN TRY 
	BEGIN TRAN
		SELECT @EventTypeId			= Id
		  FROM EventTypes
		 WHERE EventType			= @EventType

		 IF @EventTypeId IS NULL
		 BEGIN
			INSERT INTO EventTypes
			SELECT @EventType, @EventFriendlyName

			SET @EventTypeId =  SCOPE_IDENTITY()
		 END
		 ELSE
		 BEGIN
			UPDATE EventTypes
			   SET EventType		= @EventType,
				   FriendlyName		= @EventFriendlyName
			 WHERE Id	= @EventTypeId
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
