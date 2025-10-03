using CleverConversion.Dto.API;
using CleverConversion.Dtos;
using CleverConversion.Services.REST;
using System.Threading.Tasks;

namespace CleverConversion.Services
{
    public class ViewService(OTCSRestService otcsService)
    {
        private readonly OTCSRestService _otcsService = otcsService;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<APIResponse<DownloadNodeResponse>> DownloadFile(long nodeID, string fileName)
        {
            APIResponse<DownloadNodeResponse> result = new();
            try
            {
                var path = await _otcsService.DownloadFile(nodeID, fileName);
                result.Data = new()
                {
                    AbsolutePath = path.AbsolutePath,
                    RelativePath = path.RelativePath,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result.ErrorMessage = ex.Message;
                result.Message = "ERROR";
            }

            return result;
        }
    }
}
