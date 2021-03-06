
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ClusterDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ClusterDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ClusterDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ClusterDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ClusterDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ClusterDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [ClusterDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ClusterDB] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [ClusterDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ClusterDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ClusterDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ClusterDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ClusterDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ClusterDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ClusterDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ClusterDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ClusterDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ClusterDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ClusterDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ClusterDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ClusterDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ClusterDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ClusterDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ClusterDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ClusterDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ClusterDB] SET  MULTI_USER 
GO

ALTER DATABASE [ClusterDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ClusterDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [ClusterDB] SET  READ_WRITE 
GO


USE [ClusterDB]
GO

/****** Object:  Table [dbo].[Tags]    Script Date: 21.01.2015 22:01:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tags](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Tags] ON
INSERT [dbo].[Tags] ([id], [name]) VALUES (1, N'a')
INSERT [dbo].[Tags] ([id], [name]) VALUES (2, N'div')
INSERT [dbo].[Tags] ([id], [name]) VALUES (3, N'h1')
INSERT [dbo].[Tags] ([id], [name]) VALUES (4, N'h2')
INSERT [dbo].[Tags] ([id], [name]) VALUES (5, N'h3')
INSERT [dbo].[Tags] ([id], [name]) VALUES (6, N'h4')
INSERT [dbo].[Tags] ([id], [name]) VALUES (7, N'h5')
INSERT [dbo].[Tags] ([id], [name]) VALUES (8, N'h6')
INSERT [dbo].[Tags] ([id], [name]) VALUES (9, N'b')
INSERT [dbo].[Tags] ([id], [name]) VALUES (10, N'i')
INSERT [dbo].[Tags] ([id], [name]) VALUES (11, N'p')
INSERT [dbo].[Tags] ([id], [name]) VALUES (12, N'table')
INSERT [dbo].[Tags] ([id], [name]) VALUES (13, N'td')
INSERT [dbo].[Tags] ([id], [name]) VALUES (14, N'tr')
INSERT [dbo].[Tags] ([id], [name]) VALUES (15, N'th')
INSERT [dbo].[Tags] ([id], [name]) VALUES (16, N'title')
INSERT [dbo].[Tags] ([id], [name]) VALUES (17, N'ul')
INSERT [dbo].[Tags] ([id], [name]) VALUES (18, N'li')
INSERT [dbo].[Tags] ([id], [name]) VALUES (22, N'u')
INSERT [dbo].[Tags] ([id], [name]) VALUES (23, N'ol')
INSERT [dbo].[Tags] ([id], [name]) VALUES (24, N'link')
INSERT [dbo].[Tags] ([id], [name]) VALUES (25, N'em')
INSERT [dbo].[Tags] ([id], [name]) VALUES (27, N'dfn')
INSERT [dbo].[Tags] ([id], [name]) VALUES (28, N'cite')
INSERT [dbo].[Tags] ([id], [name]) VALUES (29, N'big')
INSERT [dbo].[Tags] ([id], [name]) VALUES (30, N'span')
SET IDENTITY_INSERT [dbo].[Tags] OFF

GO

/****** Object:  Table [dbo].[Pages]    Script Date: 21.01.2015 22:03:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[url] [nvarchar](max) NOT NULL,
	[count_lemm] [int] NOT NULL,
	[date] [date] NOT NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF_Pages_date]  DEFAULT (getdate()) FOR [date]
GO


/****** Object:  Table [dbo].[Words]    Script Date: 21.01.2015 22:04:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Words](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[word] [nvarchar](max) NOT NULL,
	[id_page] [int] NOT NULL,
	[frequency] [float] NOT NULL,
 CONSTRAINT [PK_Words] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Words]  WITH CHECK ADD  CONSTRAINT [FK_Words_Words] FOREIGN KEY([id_page])
REFERENCES [dbo].[Pages] ([id])
GO

ALTER TABLE [dbo].[Words] CHECK CONSTRAINT [FK_Words_Words]
GO

/****** Object:  StoredProcedure [dbo].[GetVectorForPages]    Script Date: 21.01.2015 22:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVectorForPages]
@idPage1 INT, 
@idPage2 INT
AS
BEGIN	
	SELECT
  w.word,
  ISNULL(leftw.frequency, 0) AS freq
FROM (SELECT
  wd.word
FROM Words AS wd
WHERE wd.id_page = @idPage1
OR wd.id_page = @idPage2
GROUP BY wd.word) AS w
LEFT JOIN (SELECT
  *
FROM Words AS w2
WHERE w2.id_page = @idPage1) leftw
  ON leftw.word = w.word
END
