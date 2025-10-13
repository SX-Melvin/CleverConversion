using CleverConversion.Configurations;
using CleverConversion.Dto.API;
using CleverConversion.Dtos;
using CleverConversion.Services.REST;
using Microsoft.Extensions.Options;

namespace CleverConversion.Services
{
    public class ViewService(OTCSRestService otcsService, IOptions<AppConfiguration> options)
    {
        private readonly OTCSRestService _otcsService = otcsService;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public AppConfiguration AppConfig { get; set; } = options.Value;

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

                if(AppConfig.Annotation.AllowedExts.Any(x => x == Path.GetExtension(fileName)))
                {
                    result.Data.RedirectLink = $"{AppConfig.BasePath}/annotate?guid={result.Data.AbsolutePath}";
                }
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
