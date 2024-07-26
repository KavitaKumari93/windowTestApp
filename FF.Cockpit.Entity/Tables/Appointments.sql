----Appointments
IF OBJECT_ID('Appointments', 'U') IS  NULL 
BEGIN
Create Table Appointments
(
	Id int Identity(1,1) not null Primary Key,
	Notes nvarchar(1000),
	StartTime DateTime,
	EndTime DateTime,
	
	--custom fields
	PatientId int FOREIGN KEY REFERENCES MasterTable(ObjectID),
	AppointmentTypeId int FOREIGN KEY REFERENCES AppointmentTypes(AppointmentTypeId),
	RoomId int FOREIGN KEY REFERENCES Rooms(RoomId),
	DoctorId int FOREIGN KEY REFERENCES UsersInformation(UserId),
	ViewType nvarchar(50),
	IsRecurrence bit default 0,
	RecursiveId int FOREIGN KEY REFERENCES AppointmentRecurrence(RecursiveId),
	IsDeleted bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
)
END
GO