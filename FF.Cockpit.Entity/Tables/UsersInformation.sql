---Create UsersInformation Table
IF OBJECT_ID('UsersInformation', 'U') IS  NULL 
BEGIN
Create Table UsersInformation
(
	UserId int Identity(1,1) not null Primary Key,
	UserName nvarchar(100),
	PhoneNumber nvarchar(20),
	UserRoleId int Foreign Key References RolesTable(RoleId),
	Email nvarchar(50),
	[Password] varbinary(8000),
	Photo varbinary(max),
	Specialization nvarchar(1000),
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
