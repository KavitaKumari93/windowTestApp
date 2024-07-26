USE [FotoFinder.Universe]
GO
CREATE TABLE RolePermission
(
	Id int NOT NULL primary key IDENTITY(1,1),
	RoleId int NOT NULL FOREIGN KEY REFERENCES RolesTable(RoleId),
	ModuleId int NOT NULL FOREIGN KEY REFERENCES Modules(ModuleId),
	Permission bit NOT NULL default 0,
	IsDeleted bit NOT null default 0,
	CreatedDate datetime NULL,
	CreatedBy nvarchar(50) NULL,
	UpdatedDate datetime NULL,
	UpdatedBy nvarchar(50) NULL
)
GO