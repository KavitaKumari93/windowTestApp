USE [FotoFinder.Universe]
GO
 
Create Table AppointmentDurations
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [Duration] int null,
	IsDeleted bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
)

GO