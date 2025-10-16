using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace CleverConversion.Common.Viewer
{
    internal class JsonActionResult : IActionResult
    {
        private readonly object _value;
        private readonly int _statusCode;

        public JsonActionResult(object value, int statusCode = StatusCodes.Status200OK)
        {
            _value = value;
            _statusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = _statusCode;
            response.ContentType = "application/json; charset=utf-8";

            var json = JsonSerializer.Serialize(_value, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await response.WriteAsync(json, Encoding.UTF8);
        }
    }
}