/*
AdoNet.Database_1의 배포 스크립트

이 코드는 도구를 사용하여 생성되었습니다.
파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
변경 내용이 손실됩니다.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AdoNet.Database_1"
:setvar DefaultFilePrefix "AdoNet.Database_1"
:setvar DefaultDataPath "C:\Users\user\AppData\Local\Microsoft\VisualStudio\SSDT\AdoNetWindow"
:setvar DefaultLogPath "C:\Users\user\AppData\Local\Microsoft\VisualStudio\SSDT\AdoNetWindow"

GO
:on error exit
GO
/*
SQLCMD 모드가 지원되지 않으면 SQLCMD 모드를 검색하고 스크립트를 실행하지 않습니다.
SQLCMD 모드를 설정한 후에 이 스크립트를 다시 사용하려면 다음을 실행합니다.
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'이 스크립트를 실행하려면 SQLCMD 모드를 사용하도록 설정해야 합니다.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'키가 8dba6a3b-e91e-4720-be32-236e49b72704인 이름 바꾸기 리팩터링 작업을 건너뜁니다. 요소 [dbo].[TB_Student].[Id](SqlSimpleColumn)의 이름이 StudentId(으)로 바뀌지 않습니다.';


GO
PRINT N'프로시저 [dbo].[TB_Student_Add]을(를) 변경하는 중...';


GO
ALTER PROCEDURE [dbo].[TB_Student_Add]
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
GO
-- 배포된 트랜잭션 로그를 사용하여 대상 서버를 업데이트하는 리팩터링 단계

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8dba6a3b-e91e-4720-be32-236e49b72704')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8dba6a3b-e91e-4720-be32-236e49b72704')

GO

GO
PRINT N'업데이트가 완료되었습니다.';


GO
