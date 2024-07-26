---Create Roles Table
IF OBJECT_ID('RolesTable', 'U') IS  NULL 
BEGIN 
Create table RolesTable
  (
  RoleId int not null primary key IDENTITY(1,1) ,
  RoleName nvarchar(100) ,
  IsDeleted bit not null default 0  ,
  IsActive bit not null default 0  ,
  CreatedDate datetime ,
  CreatedBy datetime ,
  UpdatedDate datetime  ,
  UpdatedBy datetime 
  )
  END
  INSERT INTO RolesTable(RoleName,IsActive) values ('Admin',1);
  INSERT INTO RolesTable(RoleName,IsActive)values ('Doctor',1);
  GO
