USE [master]
GO
/****** Object:  Database [LibraryDb]    Script Date: 08.11.2021 14:00:17 ******/
CREATE DATABASE [LibraryDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\LibraryDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\LibraryDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LibraryDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LibraryDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LibraryDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryDb] SET RECOVERY FULL 
GO
ALTER DATABASE [LibraryDb] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LibraryDb', N'ON'
GO
ALTER DATABASE [LibraryDb] SET QUERY_STORE = OFF
GO
USE [LibraryDb]
GO
/****** Object:  User [lib_reader]    Script Date: 08.11.2021 14:00:17 ******/
CREATE USER [lib_reader] FOR LOGIN [lib_reader] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [lib_librarian]    Script Date: 08.11.2021 14:00:17 ******/
CREATE USER [lib_librarian] FOR LOGIN [lib_librarian] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [lib_admin]    Script Date: 08.11.2021 14:00:17 ******/
CREATE USER [lib_admin] FOR LOGIN [lib_admin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [library_reader]    Script Date: 08.11.2021 14:00:17 ******/
CREATE ROLE [library_reader]
GO
/****** Object:  DatabaseRole [library_admin]    Script Date: 08.11.2021 14:00:17 ******/
CREATE ROLE [library_admin]
GO
/****** Object:  DatabaseRole [librarian]    Script Date: 08.11.2021 14:00:17 ******/
CREATE ROLE [librarian]
GO
ALTER ROLE [library_reader] ADD MEMBER [lib_reader]
GO
ALTER ROLE [library_reader] ADD MEMBER [lib_librarian]
GO
ALTER ROLE [librarian] ADD MEMBER [lib_librarian]
GO
ALTER ROLE [library_reader] ADD MEMBER [lib_admin]
GO
ALTER ROLE [librarian] ADD MEMBER [lib_admin]
GO
ALTER ROLE [library_admin] ADD MEMBER [lib_admin]
GO
/****** Object:  UserDefinedFunction [dbo].[GetBookAuthors]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetBookAuthors]
(
	@id_Book uniqueidentifier
)
RETURNS nvarchar(MAX)
AS
BEGIN
	DECLARE @authors nvarchar(MAX)

	SELECT @authors = STRING_AGG(FirstName + ',' + LastName, '|')
	FROM Author INNER JOIN AuthorBook
	ON Author.Id = AuthorBook.Id_Author
	WHERE AuthorBook.Id_Book = @id_Book

	RETURN @authors
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetPatentAuthors]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetPatentAuthors]
(
	@id_Patent uniqueidentifier
)
RETURNS nvarchar(MAX)
AS
BEGIN
	DECLARE @authors nvarchar(MAX)

	SELECT @authors = STRING_AGG(FirstName + ',' + LastName, '|')
	FROM Author INNER JOIN AuthorPatent
	ON Author.Id = AuthorPatent.Id_Author
	WHERE AuthorPatent.Id_Patent = @id_Patent

	RETURN @authors
END
GO
/****** Object:  UserDefinedFunction [dbo].[IsInRole]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[IsInRole] 
(
	@role nvarchar(20),
	@login nvarchar(50)
)
RETURNS bit
AS
BEGIN
	DECLARE @foundedRole nvarchar(20) = (SELECT TOP 1 DbRole = g.name
										 from sys.database_principals u, sys.database_principals g, sys.database_role_members m
										 where g.name = @role
										   and u.name = @login
										   and g.principal_id = m.role_principal_id
										   and u.principal_id = m.member_principal_id)

	IF @foundedRole IS NULL
	BEGIN
		RETURN 0
	END

	RETURN 1
END
GO
/****** Object:  Table [dbo].[Note]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Note](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[PublicationDate] [date] NOT NULL,
	[PagesCount] [smallint] NOT NULL,
	[ObjectNotes] [nvarchar](2000) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[Id] [uniqueidentifier] NOT NULL,
	[Id_Note] [uniqueidentifier] NOT NULL,
	[PublicationPlace] [nvarchar](200) NOT NULL,
	[Publisher] [nvarchar](300) NOT NULL,
	[ISBN] [char](18) NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[BookView]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[BookView]
AS
SELECT        dbo.Book.Id, dbo.GetBookAuthors(dbo.Book.Id) as Authors, dbo.Book.PublicationPlace, dbo.Book.Publisher, dbo.Book.ISBN, dbo.Note.Name, dbo.Note.PublicationDate, dbo.Note.PagesCount, dbo.Note.ObjectNotes, 
                         dbo.Note.IsDeleted
FROM            dbo.Book INNER JOIN dbo.Note ON dbo.Book.Id_Note = dbo.Note.Id
GO
/****** Object:  Table [dbo].[Patent]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patent](
	[Id] [uniqueidentifier] NOT NULL,
	[Id_Note] [uniqueidentifier] NOT NULL,
	[Country] [nvarchar](200) NOT NULL,
	[RegistrationNumber] [varchar](9) NOT NULL,
	[ApplicationDate] [date] NULL,
 CONSTRAINT [PK_Patent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[PatentView]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[PatentView]
AS
SELECT dbo.Patent.Id, dbo.GetPatentAuthors(dbo.Patent.Id) as Authors, dbo.Patent.Country, dbo.Patent.RegistrationNumber, dbo.Patent.ApplicationDate, dbo.Note.Name, dbo.Note.PublicationDate, dbo.Note.PagesCount, dbo.Note.ObjectNotes, 
                         dbo.Note.IsDeleted
FROM dbo.Patent INNER JOIN dbo.Note ON dbo.Patent.Id_Note = dbo.Note.Id
GO
/****** Object:  Table [dbo].[Author]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Author](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthorBook]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthorBook](
	[Id_Author] [uniqueidentifier] NOT NULL,
	[Id_Book] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AuthorBook] PRIMARY KEY CLUSTERED 
(
	[Id_Author] ASC,
	[Id_Book] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthorPatent]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthorPatent](
	[Id_Author] [uniqueidentifier] NOT NULL,
	[Id_Patent] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id_Author] ASC,
	[Id_Patent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Newspaper]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Newspaper](
	[Id] [uniqueidentifier] NOT NULL,
	[Id_Note] [uniqueidentifier] NOT NULL,
	[PublicationPlace] [nvarchar](200) NOT NULL,
	[Publisher] [nvarchar](300) NOT NULL,
	[Number] [nvarchar](50) NULL,
	[ReleaseDate] [date] NOT NULL,
	[ISSN] [char](13) NULL,
 CONSTRAINT [PK_Newspaper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteLog]    Script Date: 08.11.2021 14:00:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteLog](
	[Id] [uniqueidentifier] NOT NULL,
	[Id_Note] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[NoteType] [nvarchar](50) NOT NULL,
	[OperationName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[JsonDescription] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_NoteLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Author] ADD  CONSTRAINT [DF_Author_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Book] ADD  CONSTRAINT [DF_Book_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Newspaper] ADD  CONSTRAINT [DF_Newspaper_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Note] ADD  CONSTRAINT [DF_Note_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Note] ADD  CONSTRAINT [DF_Note_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[NoteLog] ADD  CONSTRAINT [DF_NoteLog_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Patent] ADD  CONSTRAINT [DF_Patent_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[AuthorBook]  WITH CHECK ADD  CONSTRAINT [FK_AuthorBook_Author] FOREIGN KEY([Id_Author])
REFERENCES [dbo].[Author] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorBook] CHECK CONSTRAINT [FK_AuthorBook_Author]
GO
ALTER TABLE [dbo].[AuthorBook]  WITH CHECK ADD  CONSTRAINT [FK_AuthorBook_Book] FOREIGN KEY([Id_Book])
REFERENCES [dbo].[Book] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorBook] CHECK CONSTRAINT [FK_AuthorBook_Book]
GO
ALTER TABLE [dbo].[AuthorPatent]  WITH CHECK ADD  CONSTRAINT [FK_AuthorPatent_Author] FOREIGN KEY([Id_Author])
REFERENCES [dbo].[Author] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorPatent] CHECK CONSTRAINT [FK_AuthorPatent_Author]
GO
ALTER TABLE [dbo].[AuthorPatent]  WITH CHECK ADD  CONSTRAINT [FK_AuthorPatent_Patent] FOREIGN KEY([Id_Patent])
REFERENCES [dbo].[Patent] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorPatent] CHECK CONSTRAINT [FK_AuthorPatent_Patent]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_Book_Book] FOREIGN KEY([Id_Note])
REFERENCES [dbo].[Note] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_Book_Book]
GO
ALTER TABLE [dbo].[Newspaper]  WITH CHECK ADD  CONSTRAINT [FK_Newspaper_Note] FOREIGN KEY([Id_Note])
REFERENCES [dbo].[Note] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Newspaper] CHECK CONSTRAINT [FK_Newspaper_Note]
GO
ALTER TABLE [dbo].[Patent]  WITH CHECK ADD  CONSTRAINT [FK_Patent_Note] FOREIGN KEY([Id_Note])
REFERENCES [dbo].[Note] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Patent] CHECK CONSTRAINT [FK_Patent_Note]
GO
/****** Object:  StoredProcedure [dbo].[Add_Author]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Add_Author]
	@firstName nvarchar(50),
	@lastName nvarchar(200),
	@id uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT OFF;
	
	DECLARE @addId uniqueidentifier = NEWID()

	DECLARE @firstNameRepeat nvarchar(50) = (SELECT TOP 1 FirstName FROM [dbo].[Author] WHERE FirstName = @firstName)
	DECLARE @lastNameRepeat nvarchar(200) = (SELECT TOP 1 LastName FROM [dbo].[Author] WHERE LastName = @lastName)

	IF ((@firstNameRepeat IS NULL) OR (@lastNameRepeat = NULL))
	BEGIN
		SET @id = @addId
		INSERT INTO [dbo].[Author](Id, FirstName, LastName)
		VALUES(@addId, @firstName, @lastName)
		RETURN
	END

	SET @id = (SELECT TOP 1 Id FROM [dbo].[Author] WHERE FirstName = @firstName AND LastName = @lastName)
END
GO
/****** Object:  StoredProcedure [dbo].[Add_Book]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Add_Book]
	@id_Note uniqueidentifier,
	@publicationPlace nvarchar(200),
	@publisher nvarchar(300),
	@iSBN char(18),

	@id uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT OFF;

	SET @id = (NEWID())
	INSERT INTO dbo.Book(Id, Id_Note, PublicationPlace, Publisher, ISBN)
	VALUES(@id, @id_Note, @publicationPlace, @publisher, @iSBN)
END
GO
/****** Object:  StoredProcedure [dbo].[Add_Newspaper]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Add_Newspaper]
	@id_Note uniqueidentifier,
	@publicationPlace nvarchar(200),
	@publisher nvarchar(300),
	@iSSN char(18),
	@number nvarchar(50),
	@releaseDate date,

	@id uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT OFF;

	SET @id = (NEWID())
	INSERT INTO dbo.Newspaper(Id, Id_Note, PublicationPlace, Publisher, ISSN, Number, ReleaseDate)
	VALUES(@id, @id_Note, @publicationPlace, @publisher, @iSSN, @number, @releaseDate)
END
GO
/****** Object:  StoredProcedure [dbo].[Add_Note]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Add_Note]
	@name nvarchar(300),
	@publicationDate date,
	@pagesCount int,
	@objectNotes nvarchar(2000),
	@id uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT OFF;

	SET @id = (NEWID())

	INSERT INTO [dbo].[Note](Id, [Name], PublicationDate, PagesCount, ObjectNotes)
	VALUES(@id, @name, @publicationDate, @pagesCount, @objectNotes)
END
GO
/****** Object:  StoredProcedure [dbo].[Add_Patent]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Add_Patent]
	@id_Note uniqueidentifier,
	@country nvarchar(200),
	@registrationNumber varchar(9),
	@applicationDate date,

	@id uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT OFF;

    DECLARE	@noteId uniqueidentifier

	SET @id = (NEWID())
	INSERT INTO dbo.Patent(Id, Id_Note, Country, RegistrationNumber, ApplicationDate)
	VALUES(@id, @id_Note, @country, @registrationNumber, @applicationDate)
END
GO
/****** Object:  StoredProcedure [dbo].[Book_GetNoteId]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_GetNoteId]
	@id uniqueidentifier,
	@id_Note uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    SET @id_Note = (SELECT TOP 1 Id_Note FROM Book WHERE Id = @id)
END
GO
/****** Object:  StoredProcedure [dbo].[Book_Set_Author]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Book_Set_Author]
	@id_Author uniqueidentifier,
	@id_Note uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[AuthorBook](Id_Author, Id_Book)
	VALUES(@id_Author, @id_Note)
END
GO
/****** Object:  StoredProcedure [dbo].[CompleteDelete_Book]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CompleteDelete_Book]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @id_Note uniqueidentifier = (SELECT TOP 1 Id FROM [dbo].[Book] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Book] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Book.Id = @id)

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [Delete.NoteId],
		'Book' as [Delete.Type],
		@noteName as [Delete.Name],
		'Data was completely deleted from table' as [Delete.Description]
		FROM [dbo].[Book] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	DELETE FROM [dbo].[Book]
	WHERE Id = @id;

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@id_Note, CURRENT_TIMESTAMP, N'Book', N'Complete delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[CompleteDelete_Newspaper]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CompleteDelete_Newspaper]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

	DECLARE @noteId uniqueidentifier = (SELECT TOP 1 Id_Note FROM [dbo].[Newspaper] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Newspaper] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Newspaper.Id = @id)

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [Delete.NoteId],
		'Newspaper' as [Delete.Type],
		@noteName as [Delete.Name],
		'Data was completely deleted from table' as [Delete.Description]
		FROM [dbo].[Newspaper] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	DELETE FROM [dbo].[Newspaper]
	WHERE Id = @id

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Newspaper', N'Complete delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[CompleteDelete_Patent]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CompleteDelete_Patent]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

    DECLARE @noteId uniqueidentifier = (SELECT TOP 1 Id_Note FROM [dbo].[Patent] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Patent] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Patent.Id = @id)

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [Delete.NoteId],
		'Patent' as [Delete.Type],
		@noteName as [Delete.Name],
		'Data was completely deleted from table' as [Delete.Description]
		FROM [dbo].[Patent] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	DELETE FROM [dbo].[Patent]
	WHERE Id = @id

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Patent', N'Complete delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_AllMarked]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete_AllMarked]

AS
BEGIN
	SET NOCOUNT OFF;

	DELETE FROM [dbo].[Book] FROM [dbo].[Note]
	WHERE (Id_Note = Note.Id AND IsDeleted = 1)

	DELETE FROM [dbo].Newspaper FROM [dbo].[Note]
	WHERE (Id_Note = Note.Id AND IsDeleted = 1)

	DELETE FROM [dbo].[Patent] FROM [dbo].[Note]
	WHERE (Id_Note = Note.Id AND IsDeleted = 1)

	DELETE FROM [dbo].[Note]
	WHERE IsDeleted = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Book]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete_Book]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

    DELETE FROM [dbo].[Book]
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Newspaper]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete_Newspaper]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

    DELETE FROM [dbo].[Newspaper]
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Note]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete_Note]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [dbo].[Note]
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_Patent]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete_Patent]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

    DELETE FROM [dbo].[Patent]
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllBooks]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllBooks]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Authors, PublicationPlace, Publisher, ISBN, [Name], PublicationDate, PagesCount, ObjectNotes, IsDeleted
	FROM dbo.BookView
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllNewspapers]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllNewspapers]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Newspaper.Id, [Name], PublicationDate, PagesCount, ObjectNotes, PublicationPlace, Publisher, ISSN, Number, ReleaseDate, IsDeleted
	FROM Note INNER JOIN Newspaper ON Note.Id = Id_Note
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllNotes]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllNotes]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Book.Id, [Name], PublicationDate, PagesCount, ObjectNotes, PublicationPlace, Publisher, ISBN
	FROM Note INNER JOIN Book ON Note.Id = Id_Note

	SELECT Newspaper.Id, [Name], PublicationDate, PagesCount, ObjectNotes, PublicationPlace, Publisher, ISSN, Number, ReleaseDate
	FROM Note INNER JOIN Newspaper ON Note.Id = Id_Note

	SELECT Patent.Id, [Name], PublicationDate, PagesCount, ObjectNotes, Country, RegistrationNumber, ApplicationDate
	FROM Note INNER JOIN Patent ON Note.Id = Id_Note
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllNotMarked_Books]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllNotMarked_Books]

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Id, Authors, PublicationPlace, Publisher, ISBN, [Name], PublicationDate, PagesCount, ObjectNotes, IsDeleted
	FROM dbo.BookView
	WHERE IsDeleted = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllNotMarked_Newspapers]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllNotMarked_Newspapers]

AS
BEGIN
	SET NOCOUNT ON;

    SELECT Newspaper.Id, [Name], PublicationDate, PagesCount, ObjectNotes, PublicationPlace, Publisher, ISSN, Number, ReleaseDate, IsDeleted
	FROM Note INNER JOIN Newspaper ON Note.Id = Id_Note
	WHERE IsDeleted = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllNotMarked_Patents]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllNotMarked_Patents]

AS
BEGIN

	SET NOCOUNT ON;

    SELECT Id, Authors, Country, RegistrationNumber, ApplicationDate, [Name], PublicationDate, PagesCount, ObjectNotes, IsDeleted
	FROM dbo.PatentView
	WHERE IsDeleted = 0
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AllPatents]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_AllPatents]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Authors, Country, RegistrationNumber, ApplicationDate, [Name], PublicationDate, PagesCount, ObjectNotes, IsDeleted
	FROM dbo.PatentView
END
GO
/****** Object:  StoredProcedure [dbo].[Get_BookAuthors]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_BookAuthors]
	@id_Note uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT FirstName, LastName
	FROM Author INNER JOIN AuthorBook ON Id = Id_Author
	WHERE AuthorBook.Id_Book = @id_Note
END
GO
/****** Object:  StoredProcedure [dbo].[Get_PatentAuthors]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_PatentAuthors]
	@id_Note uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT FirstName, LastName
	FROM Author INNER JOIN AuthorPatent ON Id = Id_Author
	WHERE AuthorPatent.Id_Patent = @id_Note
END
GO
/****** Object:  StoredProcedure [dbo].[Get_UserRoles]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get_UserRoles] 
	@login nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT [Role] = g.name
			FROM sys.database_principals as u, sys.database_principals as g, sys.database_role_members as m
			WHERE u.name = @login
				and g.principal_id = m.role_principal_id
				and u.principal_id = m.member_principal_id
END
GO
/****** Object:  StoredProcedure [dbo].[MarkForDelete_Book]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MarkForDelete_Book]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @noteId uniqueidentifier = (SELECT TOP 1 Id_Note FROM [dbo].[Book] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Book] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Book.Id = @id)

	UPDATE [dbo].[Note] SET
	IsDeleted = 1
	WHERE Id = @noteId

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [MarkForDel.NoteId],
		'Book' as [MarkForDel.Type],
		@noteName as [MarkForDel.Name],
		'Book was marked for future delete' as [MarkForDel.Description]
		FROM [dbo].[Book] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Book', N'Mark for delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[MarkForDelete_Newspaper]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MarkForDelete_Newspaper]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @noteId uniqueidentifier = (SELECT TOP 1 Id_Note FROM [dbo].[Newspaper] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Newspaper] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Newspaper.Id = @id)

	UPDATE [dbo].[Note] SET
	IsDeleted = 1
	WHERE Id = @noteId

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [MarkForDel.NoteId],
		'Newspaper' as [MarkForDel.Type],
		@noteName as [MarkForDel.Name],
		'Newspaper was marked for future delete' as [MarkForDel.Description]
		FROM [dbo].[Newspaper] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Newspaper', N'Mark for delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[MarkForDelete_Patent]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MarkForDelete_Patent]
	@id uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    DECLARE @noteId uniqueidentifier = (SELECT TOP 1 Id_Note FROM [dbo].[Patent] WHERE Id = @id)
	DECLARE @noteName nvarchar(300) = (SELECT TOP 1 [Name] FROM [dbo].[Patent] INNER JOIN [dbo].[Note] ON Id_Note = Note.Id WHERE Patent.Id = @id)

	UPDATE [dbo].[Note] SET
	IsDeleted = 1
	WHERE Id = @noteId

	DECLARE @jsonDescription nvarchar(MAX) = (
		SELECT TOP 1
		Id as [MarkForDel.NoteId],
		'Patent' as [MarkForDel.Type],
		@noteName as [MarkForDel.Name],
		'Patent was marked for future delete' as [MarkForDel.Description]
		FROM [dbo].[Patent] WHERE Id = @id
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Patent', N'Mark for delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
/****** Object:  StoredProcedure [dbo].[Newspaper_GetNoteId]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Newspaper_GetNoteId]
	@id uniqueidentifier,
	@id_Note uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    SET @id_Note = (SELECT TOP 1 Id_Note FROM Newspaper WHERE Id = @id)
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_GetNoteId]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_GetNoteId]
	@id uniqueidentifier,
	@id_Note uniqueidentifier OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

    SET @id_Note = (SELECT TOP 1 Id_Note FROM Patent WHERE Id = @id)
END
GO
/****** Object:  StoredProcedure [dbo].[Patent_Set_Author]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Patent_Set_Author]
	@id_Author uniqueidentifier,
	@id_Note uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

	INSERT INTO [dbo].[AuthorPatent](Id_Author, Id_Patent)
	VALUES(@id_Author, @id_Note)
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Book]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update_Book]
	@id uniqueidentifier OUTPUT,
	@publicationPlace nvarchar(200),
	@publisher nvarchar(300),
	@iSBN char(18)
AS
BEGIN
	SET NOCOUNT OFF;

    UPDATE [dbo].[Book] SET
	PublicationPlace = @publicationPlace,
	Publisher = @publisher,
	ISBN = @iSBN
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Newspaper]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update_Newspaper]
	@id uniqueidentifier OUTPUT,
	@publicationPlace nvarchar(200),
	@publisher nvarchar(300),
	@iSSN char(18),
	@number nvarchar(50),
	@releaseDate date
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Newspaper] SET
	PublicationPlace = @publicationPlace,
	Publisher = @publisher,
	ISSN = @iSSN,
	Number = @number,
	ReleaseDate = @releaseDate
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Note]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update_Note]
	@id uniqueidentifier OUTPUT,
	@name nvarchar(300),
	@publicationDate date,
	@pagesCount int,
	@objectNotes nvarchar(2000)
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Note] SET
	Name = @name,
	PublicationDate = @publicationDate,
	PagesCount = @pagesCount,
	ObjectNotes = @objectNotes
	WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Patent]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update_Patent]
	@id uniqueidentifier OUTPUT,
	@country nvarchar(200),
	@registrationNumber varchar(9),
	@applicationDate date
AS
BEGIN
	SET NOCOUNT OFF;

	UPDATE [dbo].[Patent] SET
	Country = @country,
	RegistrationNumber = @registrationNumber,
	ApplicationDate = @applicationDate
	WHERE Id = @id
END
GO
/****** Object:  Trigger [dbo].[Book_AfterDelete]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Book_AfterDelete] 
   ON [dbo].[Book]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT OFF;

	DECLARE @id_Note uniqueidentifier = (SELECT Id FROM deleted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM deleted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Delete.NoteId],
		'Book' as [Delete.Type],
		@noteName as [Delete.Name],
		'Data was completely deleted from table' as [Delete.Description]
		FROM deleted
		FOR JSON PATH, ROOT('Delete'))

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@id_Note, CURRENT_TIMESTAMP, N'Book', N'Complete delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Book] DISABLE TRIGGER [Book_AfterDelete]
GO
/****** Object:  Trigger [dbo].[Book_Insert]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Book_Insert]
ON [dbo].[Book]
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @id UNIQUEIDENTIFIER = (SELECT Id FROM inserted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Insert.NoteId],
		'Book' as [Insert.Type],
		@noteName as [Insert.Name],
		'New data was inserted' as [Insert.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog] (Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@id, CURRENT_TIMESTAMP ,N'Book', N'Insert', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Book] ENABLE TRIGGER [Book_Insert]
GO
/****** Object:  Trigger [dbo].[Book_InsteadOfDelete]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[Book_InsteadOfDelete]
   ON  [dbo].[Book] 
   INSTEAD OF DELETE
AS 
BEGIN
	SET NOCOUNT OFF;

    DECLARE @noteId uniqueidentifier = (SELECT Id_Note FROM deleted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM deleted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	UPDATE [dbo].[Note] SET
	IsDeleted = 1
	WHERE Id = @noteId

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [MarkForDel.NoteId],
		'Book' as [MarkForDel.Type],
		@noteName as [MarkForDel.Name],
		'Book was marked for future delete' as [MarkForDel.Description]
		FROM deleted
		FOR JSON PATH, ROOT('MarkForDel'))

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES(@noteId, CURRENT_TIMESTAMP, N'Book', N'Mark for delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Book] DISABLE TRIGGER [Book_InsteadOfDelete]
GO
/****** Object:  Trigger [dbo].[Book_Update]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Book_Update]
ON [dbo].[Book]
AFTER UPDATE
AS 
BEGIN
	IF (COLUMNS_UPDATED() & 28) > 0
	BEGIN
		SET NOCOUNT ON;

		DECLARE @id UNIQUEIDENTIFIER = (SELECT Id FROM deleted)
		DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

		DECLARE @jsonDescription nvarchar(MAX) = (SELECT
			Id as [Update.NoteId],
			'Book' as [Update.Type],
			@noteName as [Update.Name],
			'Data in book was changed' as [Update.Description]
			FROM inserted
			FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

		IF (SELECT PublicationPlace FROM deleted) <> (SELECT PublicationPlace FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublicationPlace', (SELECT PublicationPlace FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublicationPlace', (SELECT PublicationPlace FROM inserted))
		END

		IF (SELECT Publisher FROM deleted) <> (SELECT Publisher FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublisher', (SELECT Publisher FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublisher', (SELECT Publisher FROM inserted))
		END

		IF (SELECT ISBN FROM deleted) <> (SELECT ISBN FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldISBN', (SELECT ISBN FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewISBN', (SELECT ISBN FROM inserted))
		END

		INSERT INTO [dbo].[NoteLog](Id_Note, [Date], OperationName, NoteType, UserName, JsonDescription)
		VALUES(@id, CURRENT_TIMESTAMP, N'Update', N'Book', ORIGINAL_LOGIN(), @jsonDescription)
	END
END
GO
ALTER TABLE [dbo].[Book] ENABLE TRIGGER [Book_Update]
GO
/****** Object:  Trigger [dbo].[Newspaper_Insert]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Newspaper_Insert]
   ON  [dbo].[Newspaper]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT OFF;

    DECLARE @id_Note uniqueidentifier = (SELECT Id FROM inserted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Insert.NoteId],
		'Newspaper' as [Insert.Type],
		@noteName as [Insert.Name],
		'New data was inserted' as [Insert.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Newspaper', N'Insert', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Newspaper] ENABLE TRIGGER [Newspaper_Insert]
GO
/****** Object:  Trigger [dbo].[Newspaper_Update]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Newspaper_Update]
   ON [dbo].[Newspaper]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

    DECLARE @id_Note uniqueidentifier = (SELECT Id FROM deleted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Update.NoteId],
		'Newspaper' as [Update.Type],
		@noteName as [Update.Name],
		'Data in newspaper was changed' as [Update.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	IF (SELECT PublicationPlace FROM deleted) <> (SELECT PublicationPlace FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublicationPlace', (SELECT PublicationPlace FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublicationPlace', (SELECT PublicationPlace FROM inserted))
	END

	IF (SELECT Publisher FROM deleted) <> (SELECT Publisher FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublisher', (SELECT Publisher FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublisher', (SELECT Publisher FROM inserted))
	END

	IF (SELECT Number FROM deleted) <> (SELECT Number FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldNumber', (SELECT Number FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewNumber', (SELECT Number FROM inserted))
	END

	IF (SELECT ReleaseDate FROM deleted) <> (SELECT ReleaseDate FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldReleaseDate', (CAST((SELECT ReleaseDate FROM deleted) AS char(10))))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewReleaseDate', (CAST((SELECT ReleaseDate FROM inserted) AS char(10))))
	END

	IF (SELECT ISSN FROM deleted) <> (SELECT ISSN FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldISSN', (SELECT ISSN FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewISSN', (SELECT ISSN FROM inserted))
	END

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Newspaper', N'Update', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Newspaper] ENABLE TRIGGER [Newspaper_Update]
GO
/****** Object:  Trigger [dbo].[Note_Delete]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Note_Delete]
   ON [dbo].[Note]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT OFF;

	DECLARE @id_Note uniqueidentifier = (SELECT Id FROM deleted)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Delete.NoteId],
		'Note' as [Delete.Type],
		[Name] as [Delete.Name],
		'Data was completely deleted from table' as [Delete.Description]
		FROM deleted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Note', N'Complete delete', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Note] ENABLE TRIGGER [Note_Delete]
GO
/****** Object:  Trigger [dbo].[Note_Insert]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Note_Insert]
   ON [dbo].[Note]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT OFF;

	DECLARE @id_Note uniqueidentifier = (SELECT Id FROM inserted)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Insert.NoteId],
		'Note' as [Insert.Type],
		[Name] as [Insert.Name],
		'New data was inserted' as [Insert.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Note', N'Insert', ORIGINAL_LOGIN(), @jsonDescription)

	
END
GO
ALTER TABLE [dbo].[Note] ENABLE TRIGGER [Note_Insert]
GO
/****** Object:  Trigger [dbo].[Note_Update]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Note_Update]
   ON [dbo].[Note]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

    DECLARE @id_Note uniqueidentifier = (SELECT Id FROM deleted)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Update.NoteId],
		'Note' as [Update.Type],
		[Name] as [Update.Name],
		'Data in note was changed' as [Update.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


	IF (SELECT [Name] FROM deleted) <> (SELECT [Name] FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublicationPlace', (SELECT [Name] FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublicationPlace', (SELECT [Name] FROM inserted))
		END

		IF (SELECT PublicationDate FROM deleted) <> (SELECT PublicationDate FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldPublisher', (CAST((SELECT PublicationDate FROM deleted) AS char(10))))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewPublisher', (CAST((SELECT PublicationDate FROM inserted) AS char(10))))
		END

		IF (SELECT PagesCount FROM deleted) <> (SELECT PagesCount FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldISBN', (SELECT PagesCount FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewISBN', (SELECT PagesCount FROM inserted))
		END

		IF (SELECT ObjectNotes FROM deleted) <> (SELECT ObjectNotes FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldISBN', (SELECT ObjectNotes FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewISBN', (SELECT ObjectNotes FROM inserted))
		END

		IF (SELECT IsDeleted FROM deleted) <> (SELECT IsDeleted FROM inserted)
		BEGIN
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldISBN', (SELECT IsDeleted FROM deleted))
			SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewISBN', (SELECT IsDeleted FROM inserted))
		END

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Note', N'Update', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Note] ENABLE TRIGGER [Note_Update]
GO
/****** Object:  Trigger [dbo].[Patent_Insert]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Patent_Insert]
   ON [dbo].[Patent]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT OFF;

    DECLARE @id_Note uniqueidentifier = (SELECT Id FROM inserted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Insert.NoteId],
		'Patent' as [Insert.Type],
		@noteName as [Insert.Name],
		'New data was inserted' as [Insert.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Patent', N'Insert', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Patent] ENABLE TRIGGER [Patent_Insert]
GO
/****** Object:  Trigger [dbo].[Patent_Update]    Script Date: 08.11.2021 14:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Patent_Update]
   ON [dbo].[Patent]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;

    DECLARE @id_Note uniqueidentifier = (SELECT Id FROM deleted)
	DECLARE @noteName nvarchar(300) = (SELECT [Name] FROM inserted INNER JOIN [dbo].[Note] ON Id_Note = Note.Id)

	DECLARE @jsonDescription nvarchar(MAX) = (SELECT
		Id as [Update.NoteId],
		'Patent' as [Update.Type],
		@noteName as [Update.Name],
		'Data in patent was changed' as [Update.Description]
		FROM inserted
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)


	IF (SELECT Country FROM deleted) <> (SELECT Country FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldCountry', (SELECT Country FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewCountry', (SELECT Country FROM inserted))
	END

	IF (SELECT RegistrationNumber FROM deleted) <> (SELECT RegistrationNumber FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldRegistrationNumber', (SELECT RegistrationNumber FROM deleted))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewRegistrationNumber', (SELECT RegistrationNumber FROM inserted))
	END

	IF (SELECT ApplicationDate FROM deleted) <> (SELECT ApplicationDate FROM inserted)
	BEGIN
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.OldApplicationDate', (CAST((SELECT ApplicationDate FROM deleted) AS char(10))))
		SET @jsonDescription = JSON_MODIFY(@jsonDescription, '$.NewApplicationDate', (CAST((SELECT ApplicationDate FROM inserted) AS char(10))))
	END

	INSERT INTO [dbo].[NoteLog](Id_Note, [Date], NoteType, OperationName, UserName, JsonDescription)
	VALUES (@id_Note, CURRENT_TIMESTAMP, N'Patent', N'Insert', ORIGINAL_LOGIN(), @jsonDescription)
END
GO
ALTER TABLE [dbo].[Patent] ENABLE TRIGGER [Patent_Update]
GO
USE [master]
GO
ALTER DATABASE [LibraryDb] SET  READ_WRITE 
GO
