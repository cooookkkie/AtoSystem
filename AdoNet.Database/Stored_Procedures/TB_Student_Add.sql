CREATE PROCEDURE [dbo].[TB_Student_Add]
	@StudentName varchar(50),
	@Address varchar(200)
AS
	INSERT INTO
		TB_Student
		(
			StudentName.
			Address
		)
		VALUES
		(
			@StudentName ,
			@Address 
		)
	SELECT SCOPE_IDENTITY()
