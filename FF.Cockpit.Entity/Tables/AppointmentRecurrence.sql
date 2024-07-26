IF OBJECT_ID('AppointmentRecurrence', 'U') IS  NULL 
BEGIN
Create Table AppointmentRecurrence
(
	RecursiveId int Identity(1,1) not null Primary Key,
	RecurrenceMonth int,
	IsDeleted bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
)
END
GO