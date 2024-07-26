using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using System;
using System.Data.SqlClient;

namespace FF.Cockpit.Entity
{
    public static class DatabaseHelper
    {
        #region Trends
        public const string sp_GetExaminationTrendsByOneYear = "GetExaminationTrendsByOneYear";
        public const string sp_GetExaminationTrendsByFiveYear = "GetExaminationTrendsByFiveYear";
        public const string sp_GetExaminationTrendsByTwentyYear = "GetExaminationTrendsByTwentyYear";

        public const string sp_GetPatientTrendsByOneYear = "GetPatientTrendsByOneYear";
        public const string sp_GetPatientTrendsByFiveYear = "GetPatientTrendsByFiveYear";
        public const string sp_GetPatientTrendsByTwentyYear = "GetPatientTrendsByTwentyYear";


        public const string sp_GetSessionDurationTrendsByOneYear = "GetSessionDurationTrendsByOneYear";
        public const string sp_GetSessionDurationTrendsByFiveYear = "GetSessionDurationTrendsByFiveYear";
        public const string sp_GetSessionDurationTrendsByTwentyYear = "GetSessionDurationTrendsByTwentyYear";


        public const string sp_GetMicroImagesTrendsByOneYear = "GetMicroImagesTrendsByOneYear";
        public const string sp_GetMicroImagesTrendsByFiveYear = "GetMicroImagesTrendsByFiveYear";
        public const string sp_GetMicroImagesTrendsByTwentyYear = "GetMicroImagesTrendsByTwentyYear";

        public const string sp_GetAvgSessionDurationTrendsByOneYear = "GetAvgSessionDurationTrendsByOneYear";
        public const string sp_GetAvgSessionDurationTrendsByFiveYear = "GetAvgSessionDurationTrendsByFiveYear";
        public const string sp_GetAvgSessionDurationTrendsByTwentyYear = "GetAvgSessionDurationTrendsByTwentyYear";


        #endregion

        #region Datagrid Settings as per users
        public const string sp_GetDataGridSettings = "GetDataGridSettings";
        public const string sp_InsertOrUpdateGridSetting = "InsertOrUpdateGridSetting";
        #endregion

        #region Scheduler store procedure names

        public const string sp_GetRooms = "GetRooms";
        public const string sp_InsertOrUpdateRoom = "InsertOrUpdateRoom";
        public const string sp_DeleteRoom = "DeleteRoom";

        public const string sp_GetUsersInformation = "GetUsersInformation";
        public const string sp_InsertOrUpdateUserInformation = "InsertOrUpdateUserInformation";
        public const string sp_DeleteUserByRoleId = "DeleteUserByRoleId";


        public const string sp_GetAppointments = "GetAppointments";
        public const string sp_InsertOrUpdateAppointment = "InsertOrUpdateAppointment";
        public const string sp_DeleteAppointment = "DeleteAppointment";


        public const string sp_GetRoles = "GetRoles";
        public const string sp_InsertOrUpdateRoles = "InsertOrUpdateRole";
        public const string sp_DeleteRoles = "DeleteRole";

        public const string sp_GetModules = "GetModules";
        public const string sp_InsertOrUpdateModule = "InsertModule";
        public const string sp_DeleteModule = "DeleteModule";

        public const string sp_GetModulesPermissionInfoByRoleId = "GetModulesPermissionInfoByRoleId";
        public const string sp_InsertOrUpdateModulePermissionByRoleId = "InsertOrUpdateModulePermissionByRoleId";
        public const string sp_DeleteModulePermission = "DeleteModulePermission";

        public const string sp_GetAppointmentTypes = "GetAppointmentTypes";
        public const string sp_InsertOrUpdateAppointmentType = "InsertOrUpdateAppointmentType";
        public const string sp_DeleteAppointmentType = "DeleteAppointmentType";


        public const string sp_GetMiscellaneousData = "GetMiscellaneousData";
        public const string sp_InsertOrUpdateMiscellaneousData = "InsertOrUpdateMiscellaneousData";


        public const string sp_GetMiscellaneousAWSConfigurationDetails = "GetMiscellaneousAWSConfigurationDetails";
        public const string sp_InsertOrUpdateMiscellaneousAWSConfigurationDetails = "InsertOrUpdateMiscellaneousAWSConfigurationDetails";
      
        #endregion

        #region HighRisk Patients Tile Procedures
        public const string sp_GetHighRiskPatientTileDataCount = "GetHighRiskPatientTileDataCount";
        public const string sp_GetHighRiskPatientTileDataByDateRange = "GetHighRiskPatientTileDataByDateRange";
        public const string sp_GetHighRiskPatientBodyScanCount = "GetHighRiskPatientBodyScanCount";
        public const string sp_GetHighRiskPatientMarkerCount = "GetHighRiskPatientMarkerCount";
        public const string sp_GetHighRiskPatientMicroImageCount = "GetHighRiskPatientMicroImageCount";
        public const string sp_GetHighRiskPatientExcisionCount = "GetHighRiskPatientExcisionCount";
        public const string sp_MergeHighRiskPatientTileData = "SP_MergeHighRiskPatientTileData";
        public const string SP_MergeMasterTableData = "SP_MergeMasterTableData";
        #endregion

        #region Operational DashBoard Tile Procedures

        public const string sp_GetAppointedPatients = "GetAppointedPatients";
        public const string sp_GetAppointedDoctors = "GetAppointedDoctors";
        public const string sp_GetOperationalFollowUpData = "GetOperationalFollowUpDataByDateRange";
        public const string sp_GetOperationalTreatmentBreakdown = "GetOperationalTreatmentBreakDownByDateRange";
        public const string sp_GetScheduledAppointmentTimeByDateRange = "GetScheduledAppointmentTimeByDateRange";
        public const string sp_GetRoomOccupancyReportByDateRange = "GetRoomOccupancyReportByDateRange";

        #endregion

        #region Cloud Sync
        public const string sp_GetImageInfoByPatientId = "GetImageInfoByPatientId";
        public const string sp_GetUpcomingAppointments = "GetUpcomingAppointments";
        public const string sp_InsertOrUpdateCloudSyncInfo = "InsertOrUpdateCloudSyncInfo";
        public const string sp_UpdateDownloadStatus = "UpdateDownloadStatus";
        public const string sp_GetTrackUploadStatusLogs = "GetTrackUploadStatusLogs";
        public const string sp_GetTrackDownloadStatusLogs = "GetTrackDownloadStatusLogs";
        public const string sp_GetPatientImagesStatusFollowUp = "GetPatientImagesStatusFollowUp";
        #endregion

        #region Cockpit Settings
        public const string sp_InsertOrUpdateCockpitSettings = "InsertOrUpdateCockpitSettings";
        public const string sp_GetCockpitSettings = "GetCockpitSettings";
        #endregion

        #region Dashboard,Performance and Hardware details from universe tab 
        public const string sp_GetLowFrequentPatients = "GetLowFrequentPatients";
        public const string sp_GetExcisedMarker = "GetExcisedMarkerList";
        public const string sp_GetHardwareCounterTileData = "GetHardwareCounterTileData";
        public const string sp_GetPerformanceTilesDataByDateRange = "GetPerformanceTilesDataByDateRange";
        public const string sp_GetTreatmentBreakdownDataByDateRange = "GetTreatmentBreakdownDataByDateRange";
        public const string sp_GetExaminationHistoryDataByPatientId = "GetExaminationHistoryDataByPatientId";
        public const string sp_UpdateSessionIntervalTileData = "UpdateSessionIntervalTileData";
        public const string sp_GetSessionIntervalTileDataByPatientId = "GetSessionIntervalTileDataByPatientId";
        public const string sp_GetDashboardTilesDataByPatientId = "GetDashboardTilesDataByPatientId";
        public const string sp_GetFollowUpSessionTileDataByPatientId = "GetFollowUpSessionTileDataByPatientId";
        public const string sp_GetSessionYearsDataByPatientId = "GetSessionYearsDataByPatientId";
        public const string sp_InsertPatientMedicalHistoryOperationsById = "InsertMedicalHistoryDatabyPatientId";
        public const string sp_GetPatientMedicalHistoryOperationsById = "GetMedicalHistorybyPatientId";
        public const string sp_UpdateDashboardSkinTypeByPatientId = "UpdateDashboardSkinTypeByPatientId";
        public const string sp_GetPatients = "GetPatients";
        public const string sp_GetServiceTileVersionData = "GetServiceTileVersionData";
        public const string sp_GetFotoFinderDevicesInformation = "GetFotoFinderDevicesInformation";
        #endregion

        #region DatabaseConnection
        public static SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder();

        public static bool IsConnectionOpen(LoginModel model)
        {
            string connectionString = string.Empty;
            if (model!=null)
            {
                if (model.UseWindowsSecurity)
                    connectionString = $"Data Source={model.ServerInstance};Initial Catalog={model.DatabaseName};Integrated Security=True;";
                else
                    connectionString = $"Data Source={model.ServerInstance};Initial Catalog={model.DatabaseName};User ID={model.UserName};Password={model.UserPassword};";
            }
            try
            {
                using (var con = new SqlConnection(connectionString))
                {

                    ConnectionStringBuilder.ConnectionString = connectionString;
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return false;
            }


        }
        #endregion
    }
}