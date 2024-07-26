IF OBJECT_ID('CloudSyncInfo', 'U') IS  NULL 
BEGIN
Create Table CloudSyncInfo
(
	Id int Identity(1,1) not null Primary Key,
	AppointmentId int FOREIGN KEY REFERENCES Appointments(Id),
	PatientId int FOREIGN KEY REFERENCES MasterTable(ObjectID),
	DownloadDateTime DateTime,
	UploadDateTime DateTime,
	SyncType nvarchar(20),
	SyncUploadStatus int FOREIGN KEY REFERENCES MasterSyncStatus(Id) default(1),
	SyncDownloadStatus int FOREIGN KEY REFERENCES MasterSyncStatus(Id) default(6),
	IsLocalCleared bit default(0),
	IsDeleted bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
	InProgress bit NULL default(0)
)

END
GO
