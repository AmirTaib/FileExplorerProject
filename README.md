## FileExplorerProject.

#### The database is in the bak file.

#### The code for building the DB:

```SQL

CREATE DATABASE [FileExplorer]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Search Details](
	[SearchDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[KeyWord] [nvarchar](50) NULL,
	[DirectoryPath] [nvarchar](200) NOT NULL,
	[SearchTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Search Details] PRIMARY KEY CLUSTERED 
(
	[SearchDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE FileExplorer
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Search Result](
	[SearchResultID] [int] IDENTITY(1,1) NOT NULL,
	[FullPath] [nvarchar](256) NOT NULL,
	[SearchDetailsID] [int] NOT NULL,
 CONSTRAINT [PK_SearchWordID] PRIMARY KEY CLUSTERED 
(
	[SearchResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Search Result]  WITH CHECK ADD  CONSTRAINT [FK_Search Result_Search Details] FOREIGN KEY([SearchDetailsID])
REFERENCES [dbo].[Search Details] ([SearchDetailsID])
GO

ALTER TABLE [dbo].[Search Result] CHECK CONSTRAINT [FK_Search Result_Search Details]
GO


USE FileExplorer
GO
SELECT * FROM [dbo].[Search Result]

SELECT * FROM [dbo].[Search Details]

```
