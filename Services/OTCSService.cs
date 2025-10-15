using CleverConversion.Configurations;
using CleverConversion.Dto.API;
using CleverConversion.Dtos;
using CleverConversion.Services.REST;
using Microsoft.Extensions.Options;

namespace CleverConversion.Services
{
    public class OTCSService(OTCSRestService otcsService, IOptions<AppConfiguration> options)
    {
        private readonly OTCSRestService _otcsService = otcsService;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public AppConfiguration AppConfig { get; set; } = options.Value;

        public async Task<APIResponse<AddNodeVersionResponse>> AddNodeVersion(long nodeID, string filePath)
        {
            APIResponse<AddNodeVersionResponse> result = new()
            {
                Data = new()
            };

            try
            {
                var data = await _otcsService.AddNodeVersion(nodeID, filePath);
                result.Data.Results = data.Results;
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
