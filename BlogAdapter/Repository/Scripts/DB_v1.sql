USE [master]
GO

DROP DATABASE IF EXISTS [BlogPosts_Master] 
GO
CREATE DATABASE [BlogPosts_Master]
GO

USE [BlogPosts_Master]
GO

DROP TABLE IF EXISTS [dbo].[Users]
GO
CREATE TABLE [dbo].[Users](
	[Id] int NOT NULL IDENTITY PRIMARY KEY,
	[Username] [nvarchar](16) NOT NULL UNIQUE,
	[Password] [nvarchar](150) NOT NULL,
	[RoleLevel] INT NOT NULL,
	[Token] INT NULL
)
GO

DROP TABLE IF EXISTS [dbo].[Posts]
GO
CREATE TABLE [dbo].[Posts](
	[Id] int NOT NULL IDENTITY PRIMARY KEY,
	[Title] [nvarchar](50) NOT NULL,
	[Body] [nvarchar](300) NOT NULL,
	[PublishDate] [datetime],
	[AuthorId] [int] NOT NULL,
	[Status] [nvarchar](10),
	CONSTRAINT FK_Author FOREIGN KEY (AuthorId)
    REFERENCES Users(Id)
)
GO

DROP TABLE IF EXISTS [dbo].[Comments]
GO
CREATE TABLE [dbo].[Comments](
	[Id] int NOT NULL IDENTITY PRIMARY KEY,
	[Text] [nvarchar](300) NOT NULL,
	[PostId] INT NOT NULL,
	CONSTRAINT FK_Post FOREIGN KEY (PostId)
    REFERENCES Posts(Id)
)
GO

DROP PROCEDURE IF EXISTS [dbo].[AddPost]
GO
CREATE PROCEDURE [dbo].[AddPost]
    ( 
      @Title [nvarchar](50),
      @Body [nvarchar](300),
	  @AuthorId int
    )
AS 
    BEGIN
        INSERT INTO [dbo].[Posts]
        VALUES  ( @Title, @Body, NULL, @AuthorId, 'Editable' )
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[UpdatePost]
GO
CREATE PROCEDURE [dbo].[UpdatePost]
    ( 
      @PostId int,
	  @Title [nvarchar](50) ,
      @Body [nvarchar](300)
    )
AS 
    BEGIN
        UPDATE [dbo].[Posts]
        SET Title = @Title, Body = @Body
		WHERE Id = @PostId
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[UpdatePostStatus]
GO
CREATE PROCEDURE [dbo].[UpdatePostStatus]
    ( 
      @PostId int,
	  @Status [nvarchar](10) 
    )
AS 
    BEGIN
        UPDATE [dbo].[Posts]
        SET Status = @Status
		WHERE Id = @PostId
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[GetPostsByStatus]
GO
CREATE PROCEDURE [dbo].[GetPostsByStatus]
    ( 
      @Status [nvarchar](10) 
    )
AS 
    BEGIN
        SELECT
			p.[Id],
			p.[Title],
			p.[Body],
			p.[PublishDate], 
			p.[AuthorId],
			p.[Status]
		FROM [dbo].[Posts] p
        WHERE p.[Status] = @Status
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[GetPosts]
GO
CREATE PROCEDURE [dbo].[GetPosts]
(
    @PostId int = 0,
	@AuthorId int = 0
)AS
        SELECT
			p.[Id],
			p.[Title],
			p.[Body],
			p.[PublishDate], 
			p.[AuthorId],
			p.[Status]
		FROM [dbo].[Posts] p
        WHERE @PostId = 0 AND (@AuthorId = 0 OR @AuthorId = p.AuthorId) OR
				p.Id = @PostId
GO

DROP PROCEDURE IF EXISTS [dbo].[GetPostsComments]
GO
CREATE PROCEDURE [dbo].[GetPostsComments]
    ( 
      @CommentId int = 0,
	  @PostId int = 0
	)
AS 
    SELECT
			c.[Id],
			c.[PostId],
			c.[Text]
		FROM [dbo].[Comments] c
        WHERE @CommentId = 0 AND (@PostId = 0 OR @PostId = c.PostId) 
			OR c.Id = @CommentId
GO

DROP PROCEDURE IF EXISTS [dbo].[AddPostComment]
GO
CREATE PROCEDURE [dbo].[AddPostComment]
    ( 
      @PostId int,
      @Text [nvarchar](300)
    )
AS 
    BEGIN
        INSERT INTO [dbo].[Comments]
        VALUES  ( @Text, @PostId )
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[GetUser]
GO
CREATE PROCEDURE [dbo].[GetUser]
    ( 
      @Token int
    )
AS 
    BEGIN
		SELECT u.Id, u.Username, u.RoleLevel FROM [dbo].[Users] u
        WHERE @Token = u.Token
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[AddUser]
GO
CREATE PROCEDURE [dbo].[AddUser]
    ( 
      @Username nvarchar(50),
      @Password [nvarchar](50),
	  @RoleLevel int
    )
AS 
    BEGIN
        INSERT INTO [dbo].[Users]
        VALUES  ( @Username, @Password, @RoleLevel, NULL )
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[LogIn]
GO
CREATE PROCEDURE [dbo].[LogIn]
    ( 
      @Username nvarchar(50),
      @Password [nvarchar](50),
	  @Token int
    )
AS 
    BEGIN
        UPDATE [dbo].[Users]
        SET Token = @Token
		WHERE Username = @Username
		AND Password = @Password
		
		SELECT Id, Username, RoleLevel, Token FROM Users
		WHERE Token = @Token
	END
GO

DROP PROCEDURE IF EXISTS [dbo].[LogOut]
GO
CREATE PROCEDURE [dbo].[LogOut]
    ( 
      @Username nvarchar(50),
      @Token int
    )
AS 
    BEGIN
        UPDATE [dbo].[Users]
        SET Token = NULL
		WHERE Username = @Username
		AND Token = @Token
	END
GO
-------------------------------------
-----------INIT----------------------
INSERT INTO [dbo].[Users] VALUES  ( 'mbustamante', 'passm', 1, 123 )
GO
INSERT INTO [dbo].[Users] VALUES  ( 'msanchez', 'passm2', 2 , 234)
GO
INSERT INTO [dbo].[Users] VALUES  ( 'jperez', 'passj', 3 , NULL)
GO
DECLARE @count INT = 1;
	
DECLARE @Title nvarchar(50);
DECLARE @Body nvarchar(300);
DECLARE @PublishDate DATETIME;
DECLARE @AuthorId int;
DECLARE @Status nvarchar(10);

WHILE @count<= 3
	BEGIN
		SET @Title = 'Title '+ CAST(@count as varchar)
		SET @Body = 'Body '+ CAST(@count as varchar)
		SET @PublishDate = GETDATE()
		SET @AuthorId = 1
		
		EXECUTE [dbo].[AddPost] @Title,@Body,@AuthorId
		SET @count = @count + 1
	END
GO
SELECT * FROM Posts
GO
SELECT * FROM Comments
GO
SELECT * FROM Users
GO