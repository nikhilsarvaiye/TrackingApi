
CREATE TABLE [TrackerRequest] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Type] [int] NOT NULL,
	[MetaDeta1] [nvarchar](max) NULL,
	[MetaDeta2] [nvarchar](max) NULL,
	[MetaDeta3] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Result] [int] NULL,
	[ResultDetails] [nvarchar](max) NULL,
	[TotalSteps] [int] NULL,
	[CurrentStep] [int] NULL,
	[CurrentStepDescription] [nvarchar](max) NULL,
	[CreatedDateTime] [datetime2](7) NULL,
	[UpdatedDateTime] [datetime2](7) NULL,
) 
GO

CREATE TABLE [Notification] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NULL,
	[UserId] [int] NULL,
	[Type] [int] NOT NULL,
	[Severity] [int] NULL,
	[MetaDeta1] [nvarchar](max) NULL,
	[MetaDeta2] [nvarchar](max) NULL,
	[MetaDeta3] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedDateTime] [datetime2](7) NULL,
	[UpdatedDateTime] [datetime2](7) NULL,
) 
GO


CREATE TABLE [UserNotification] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[NotificationId] [int] NOT NULL,
	[IsRead] [bit] NULL,
	[ReadDateTime] [datetime2](7) NULL,
	[CreatedDateTime] [datetime2](7) NULL,
	[UpdatedDateTime] [datetime2](7) NULL,
) 
GO

--- DROP TABLE [TrackerRequest]
--- DROP TABLE [Notification]
--- DROP TABLE [UserNotification]
