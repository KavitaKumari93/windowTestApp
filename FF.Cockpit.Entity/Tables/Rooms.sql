IF OBJECT_ID('Rooms', 'U') IS  NULL 
BEGIN 
Create Table Rooms
(
	RoomId int Identity(1,1) not null Primary Key,
	RoomName nvarchar(100),
	Description nvarchar(1000),
	DarkThemeColor nvarchar(10),
	LightThemeColor nvarchar(10),
	IsDeleted bit default 0,
	IsActive bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
)
End
GO