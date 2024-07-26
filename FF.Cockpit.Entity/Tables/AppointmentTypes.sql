---AppointmentTypes---
IF OBJECT_ID('AppointmentTypes', 'U') IS  NULL 
BEGIN
Create Table AppointmentTypes
(
	[AppointmentTypeId] [int] IDENTITY(1,1) NOT NULL Primary Key,
	[AppointmentTypeName] [nvarchar](100) NOT NULL,
	[AppointmentTypeColor] nvarchar(10),
	[IsDeleted] [bit] default 0,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] nvarchar(10),
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] nvarchar(10),
)

INSERT INTO AppointmentTypes (AppointmentTypeName,AppointmentTypeColor, [IsDeleted])
VALUES
('Dermoscopy','#ff0000', 0),
('BodyMapping','#964B00',0);
END
GO