using CleverConversion.Common.Common.Config;
using CleverConversion.Common.Viewer;
using CleverConversion.Common.Viewer.Utils;
using CleverConversion.Common.Comparison.Comparison.Service;
using GroupDocs.Viewer.UI.Api.Models;
using GroupDocs.Viewer.UI.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleverConversion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViewerController : Controller
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly UIConfig _config;
        private readonly IFileStorage _fileStorage;
        private readonly IViewer _viewer;

        public ViewerController(
            UIConfig uiConfig,
            IFileStorage fileStorage,
            IViewer viewer)
        {
            _config = uiConfig;
            _fileStorage = fileStorage;
            _viewer = viewer;
        }

        [HttpGet]
        [Route(Constants.LOAD_CONFIG_ACTION_NAME)]
        public IActionResult LoadConfig()
        {
            var config = new LoadConfigResponse
            {
                PageSelector = _config.PageSelector,
                Download = _config.Download,
                Upload = _config.Upload,
                Print = _config.Print,
                Browse = _config.Browse,
                Rewrite = _config.Rewrite,
                EnableRightClick = _config.EnableRightClick,
                DefaultDocument = _config.DefaultDocument,
                PreloadPageCount = _config.PreloadPageCount,
                Zoom = _config.Zoom,
                Search = _config.Search,
                Thumbnails = _config.Thumbnails,
                HtmlMode = _config.HtmlMode,
                PrintAllowed = _config.PrintAllowed,
                Rotate = _config.Rotate,
                SaveRotateState = _config.SaveRotateState,
                DefaultLanguage = _config.DefaultLanguage,
                SupportedLanguages = _config.SupportedLanguages,
                ShowLanguageMenu = _config.ShowLanguageMenu
            };

            return OkJsonResult(config);
        }

        [HttpPost]
        [Route(Constants.LOAD_FILE_TREE_ACTION_NAME)]
        public async Task<IActionResult> GetFileTree(LoadFileTreeRequest request)
        {
            if (!_config.Browse)
                return ErrorJsonResult("Browsing files is disabled.");

            try
            {
                var files = await _fileStorage.ListDirsAndFilesAsync(request.Path);

                var result = files
                    .Select(entity => new Common.Viewer.FileDescription(entity.FilePath, entity.FilePath, entity.IsDirectory, entity.Size))
                    .ToList();

                return OkJsonResult(result);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpGet]
        [Route(Constants.DOWNLOAD_DOCUMENT_ACTION_NAME)]
        public async Task<IActionResult> DownloadDocument(string path)
        {
            if (!_config.Download)
                return ErrorJsonResult("Downloading files is disabled.");

            try
            {
                var fileName = Path.GetFileName(path);
                var bytes = await _fileStorage.ReadFileAsync(path);

                return FileResult(bytes, fileName);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpGet]
        [Route(Constants.LOAD_DOCUMENT_PAGE_RESOURCE_ACTION_NAME)]
        public async Task<IActionResult> LoadDocumentPageResource([FromQuery] string guid, [FromQuery] int pageNumber, [FromQuery] string resourceName)
        {
            if (!_config.HtmlMode)
                return ErrorJsonResult("Loading page resources is disabled in image mode.");

            try
            {
                var fileCredentials = new Common.Viewer.Entities.FileCredentials(guid, "", "");
                var bytes = await _viewer.GetPageResourceAsync(fileCredentials, pageNumber, resourceName);

                if (bytes.Length == 0)
                    return NotFoundJsonResult($"Resource {resourceName} was not found");

                var contentType = resourceName.ContentTypeFromFileName();

                return ResourceFileResult(bytes, contentType);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route(Constants.UPLOAD_DOCUMENT_ACTION_NAME)]
        public async Task<IActionResult> UploadDocument()
        {
            if (!_config.Upload)
                return ErrorJsonResult("Uploading files is disabled.");

            try
            {
                var form = HttpContext.Request.Form;
                var url = form["url"].ToString();
                var rewrite = bool.TryParse(form["rewrite"], out var r) && r;

                var (fileName, bytes) = await ReadOrDownloadFile(url);

                var filePath = await _fileStorage.WriteFileAsync(fileName, bytes, rewrite);
                var result = new UploadFileResponse(filePath);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route(Constants.PRINT_PDF_ACTION_NAME)]
        public async Task<IActionResult> PrintPdf([FromBody] PrintPdfRequest request)
        {
            if (!_config.Print)
                return ErrorJsonResult("Printing files is disabled.");

            try
            {
                var fileCredentials = new Common.Viewer.Entities.FileCredentials(request.Guid, request.FileType, request.Password);

                var filename = Path.GetFileName(request.Guid);
                var pdfFileName = Path.ChangeExtension(filename, ".pdf");
                var pdfFileBytes = await _viewer.GetPdfAsync(fileCredentials);

                return FileResult(pdfFileBytes, pdfFileName, "application/pdf");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    var message = string.IsNullOrEmpty(request.Password)
                        ? "Password Required"
                        : "Incorrect Password";

                    return ForbiddenJsonResult(message);
                }

                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route(Constants.LOAD_DOCUMENT_DESCRIPTION_ACTION_NAME)]
        public async Task<IActionResult> LoadDocumentDescription([FromBody] LoadDocumentDescriptionRequest request)
        {
            try
            {
                var fileCredentials = new Common.Viewer.Entities.FileCredentials(request.Guid, request.FileType, request.Password);
                var documentDescription = await _viewer.GetDocumentInfoAsync(fileCredentials);

                var pageNumbers = GetPageNumbers(documentDescription.Pages.Count());
                var pagesData = await _viewer.GetPagesAsync(fileCredentials, pageNumbers);

                var pages = new List<PageDescription>();
                foreach (Common.Viewer.Entities.PageInfo pageInfo in documentDescription.Pages)
                {
                    var pageData = pagesData.FirstOrDefault(p => p.PageNumber == pageInfo.Number);
                    var pageDescription = new PageDescription
                    {
                        Width = pageInfo.Width,
                        Height = pageInfo.Height,
                        Number = pageInfo.Number,
                        SheetName = pageInfo.Name,
                        Data = pageData?.GetContent()
                    };

                    pages.Add(pageDescription);
                }

                var result = new LoadDocumentDescriptionResponse
                {
                    Guid = request.Guid,
                    FileType = documentDescription.FileType,
                    PrintAllowed = documentDescription.PrintAllowed,
                    Pages = pages
                };

                return OkJsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    var message = string.IsNullOrEmpty(request.Password)
                            ? "Password Required"
                            : "Incorrect Password";

                    return ForbiddenJsonResult(message);
                }

                return ErrorJsonResult(ex.Message);
            }
        }

        private int[] GetPageNumbers(int totalPageCount)
        {
            if (_config.PreloadPageCount == 0)
                return Enumerable.Range(1, totalPageCount).ToArray();

            var pageCount =
            Math.Min(totalPageCount, _config.PreloadPageCount);

            return Enumerable.Range(1, pageCount).ToArray();
        }

        [HttpPost]
        [Route(Constants.LOAD_DOCUMENT_PAGES_ACTION_NAME)]
        public async Task<IActionResult> LoadDocumentPages([FromBody] LoadDocumentPagesRequest request)
        {
            try
            {
                var fileCredentials = new Common.Viewer.Entities.FileCredentials(request.Guid, request.FileType, request.Password);
                var pages = await _viewer.GetPagesAsync(fileCredentials, request.Pages);
                var pageContents = pages
                    .Select(page => new Common.Viewer.PageContent { Number = page.PageNumber, Data = page.GetContent() })
                    .ToList();

                return OkJsonResult(pageContents);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    var message = string.IsNullOrEmpty(request.Password)
                        ? "Password Required"
                        : "Incorrect Password";

                    return ForbiddenJsonResult(message);
                }

                return ErrorJsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route(Constants.LOAD_DOCUMENT_PAGE_ACTION_NAME)]
        public async Task<IActionResult> LoadDocumentPage([FromBody] LoadDocumentPageRequest request)
        {
            try
            {
                var fileCredentials = new Common.Viewer.Entities.FileCredentials(request.Guid, request.FileType, request.Password);
                var page = await _viewer.GetPageAsync(fileCredentials, request.Page);
                var pageContent = new Common.Viewer.PageContent { Number = page.PageNumber, Data = page.GetContent() };

                return OkJsonResult(pageContent);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("password"))
                {
                    var message = string.IsNullOrEmpty(request.Password)
                        ? "Password Required"
                        : "Incorrect Password";

                    return ForbiddenJsonResult(message);
                }

                return ErrorJsonResult(ex.Message);
            }
        }

        private Task<(string, byte[])> ReadOrDownloadFile(string url)
        {
            return string.IsNullOrEmpty(url)
                ? ReadFileFromRequest()
                : DownloadFileAsync(url);
        }

        private async Task<(string FileName, byte[] Bytes)> ReadFileFromRequest()
        {
            var form = await HttpContext.Request.ReadFormAsync();
            var firstFile = form.Files.FirstOrDefault();

            if (firstFile == null)
                throw new InvalidOperationException("No file uploaded.");

            using var ms = new MemoryStream();
            await firstFile.CopyToAsync(ms);

            var fileName = PathUtils.RemoveInvalidFileNameChars(firstFile.FileName);
            var bytes = ms.ToArray();

            return (fileName, bytes);
        }


        private async Task<(string, byte[])> DownloadFileAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");

                Uri uri = new(url);
                string fileName = Path.GetFileName(uri.LocalPath);
                byte[] bytes = await httpClient.GetByteArrayAsync(uri);

                return (fileName, bytes);
            }
        }

        private IActionResult ErrorJsonResult(string message)
        {
            return new JsonActionResult(
                new ErrorResponse(message),
                StatusCodes.Status500InternalServerError
            );
        }

        private IActionResult ForbiddenJsonResult(string message)
        {
            return new JsonActionResult(
                new ErrorResponse(message),
                StatusCodes.Status403Forbidden
            );
        }

        private IActionResult NotFoundJsonResult(string message)
        {
            return new JsonActionResult(
                new ErrorResponse(message),
                StatusCodes.Status404NotFound
            );
        }

        private IActionResult OkJsonResult(object result) =>
            new JsonActionResult(result);

        private IActionResult FileResult(byte[] data, string fileName) =>
            new FileActionResult(data, fileName, "application/octet-stream");

        private IActionResult FileResult(byte[] data, string fileName, string contentType) =>
            new FileActionResult(data, fileName, contentType);

        private IActionResult ResourceFileResult(byte[] data, string contentType) =>
            new ResourceActionResult(data, contentType);
    }
}