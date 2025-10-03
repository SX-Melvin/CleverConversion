using CleverConversion.Configurations;
using CleverConversion.Dto.API;
using CleverConversion.Dtos;
using CleverConversion.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CleverConversion.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class ViewController(ViewService service, IOptions<OTCSConfiguration> otcsOptions) : Controller
    {
        private readonly ViewService _service = service;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet("node/{nodeID}/name/{fileName}")]
        public async Task<APIResponse<DownloadNodeResponse>> DownloadFile(long nodeID, string fileName)
        {
            APIResponse<DownloadNodeResponse> result = new();
            try
            {
                result = await _service.DownloadFile(nodeID, fileName);
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
