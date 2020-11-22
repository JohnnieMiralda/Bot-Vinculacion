# VinculacionUnitec-SPS

### Migracion linux base de datos

#### Connection
Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword; 

dotnet ef --startup-project ../HoursTracker.Web/ migrations add InitialModel
dotnet ef database update 
