IF OBJECT_ID('CockpitSettings', 'U') IS  NULL 
BEGIN
Create Table CockpitSettings
(
  Id int Primary Key Identity(1,1) not null,
  LastSyncedOn DateTime, 
  CreatedBy nvarchar(10),
  CreatedDate DateTime  ,
  UpdatedBy nvarchar(10),
  UpdatedDate DateTime  ,
)
END 
GO
