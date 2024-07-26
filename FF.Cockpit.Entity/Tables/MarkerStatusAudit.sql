IF OBJECT_ID('MarkerStatusAudit', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[MarkerStatusAudit](
	[MarkerStatusAuditId] [int] IDENTITY(1,1) NOT NULL,
	[MarkerId] [int] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Deactivated] [bit] NOT NULL,
	[Excised] [bit] NOT NULL,
	[StatusChangeDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MarkerStatusAudit] PRIMARY KEY CLUSTERED 
(
	[MarkerStatusAuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
