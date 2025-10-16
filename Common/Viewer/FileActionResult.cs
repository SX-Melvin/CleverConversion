using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CleverConversion.Common.Viewer
{
    internal class FileActionResult : IActionResult
    {
        private readonly byte[] _data;
        private readonly string _fileName;
        private readonly string _contentType;

        public FileActionResult(byte[] data, string fileName, string contentType = "application/octet-stream")
        {
            _data = data;
            _fileName = fileName;
            _contentType = string.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = _contentType;
            response.Headers["Content-Disposition"] = $"attachment; filename=\"{_fileName}\"";

            await response.Body.WriteAsync(_data, 0, _data.Length);
        }
    }
}