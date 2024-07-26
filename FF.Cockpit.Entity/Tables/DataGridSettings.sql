

IF OBJECT_ID('DataGridSettings', 'U') IS  NULL 
BEGIN
Create Table DataGridSettings
(
	Id int Identity(1,1) not null Primary Key,
	UserId int,
	ModuleId int,
	GridName nvarchar(20),
	GridConfigName nvarchar(20),
	GridConfigValue nvarchar(max),
	
	IsDeleted bit default 0,
	CreatedDate DateTime,
	CreatedBy nvarchar(10),
	UpdatedDate DateTime,
	UpdatedBy nvarchar(10),
)
END
GO