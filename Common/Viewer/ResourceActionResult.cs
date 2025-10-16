using Microsoft.AspNetCore.Mvc;

namespace CleverConversion.Common.Viewer
{
    internal class ResourceActionResult : IActionResult
    {
        private readonly byte[] _data;
        private readonly string _contentType;

        public ResourceActionResult(byte[] data, string contentType = "application/octet-stream")
        {
            _data = data;
            _contentType = string.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = _contentType;

            // Force browser to render inline instead of download
            response.Headers["Content-Disposition"] = "inline";

            await response.Body.WriteAsync(_data, 0, _data.Length);
        }
    }
}