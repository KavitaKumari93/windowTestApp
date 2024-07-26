using FF.Cockpit.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FF.Cockpit.Entity.EntityModel
{
    public class ImageInfoModel
    {
        public int PatientId { get; set; }
        public int ImageNumber { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string FullImagePath { get; set; } = string.Empty;

        public async Task<IEnumerable<ImageInfoModel>> GetPatientImagesInfo(string patientIdXml)
        {
            try
            {
                using (var repo = new GenericRepository<ImageInfoModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetImageInfoByPatientId, new { @patientId = @patientIdXml });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
    }
}
