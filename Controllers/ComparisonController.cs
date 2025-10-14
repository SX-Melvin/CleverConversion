using CleverConversion.Common.Common.Entity.Web;
using CleverConversion.Common.Comparison.Comparison.Config;
using CleverConversion.Common.Common.LowercaseContractResolver;
using CleverConversion.Common.Comparison.Comparison.Model.Request;
using CleverConversion.Common.Comparison.Comparison.Model.Response;
using CleverConversion.Common.Comparison.Comparison.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using GroupDocs.Comparison.Common.Exceptions;
using CleverConversion.Common.Common.Resources;
using CleverConversion.Common.Common.Config;

namespace CleverConversion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComparisonController : Controller
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly GlobalConfiguration globalConfiguration = new();
        private readonly IComparisonService _comparisonService = new ComparisonServiceImpl(globalConfiguration);

        /// <summary>
        /// Load Comparison configuration
        /// </summary>
        /// <returns>Comparison configuration</returns>
        [HttpGet]
        [Route("loadConfig")]
        public ComparisonConfiguration LoadConfig()
        {
            return globalConfiguration.Comparison;
        }

        /// <summary>
        /// Get all files and directories from storage
        /// </summary>
        /// <param name="postedData">Post data</param>
        /// <returns>List of files and directories</returns>
        [HttpPost]
        [Route("loadFileTree")]
        public List<FileDescriptionEntity> loadFileTree(PostedDataEntity fileTreeRequest)
        {
            return _comparisonService.LoadFiles(fileTreeRequest);
        }

        /// <summary>
        /// Download results
        /// </summary>
        /// <param name=""></param>
        [HttpGet]
        [Route("downloadDocument")]
        public HttpResponseMessage DownloadDocument(string guid)
        {
            string filePath = guid;
            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var fileStream = new FileStream(filePath, FileMode.Open);
                response.Content = new StreamContent(fileStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = System.IO.Path.GetFileName(filePath);
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Upload document
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadDocument")]
        public async Task<UploadedDocumentEntity> UploadDocument()
        {
            UploadedDocumentEntity uploadedDocument = new UploadedDocumentEntity();

            try
            {
                var form = await Request.ReadFormAsync();
                string url = form["url"];
                // get documents storage path
                string documentStoragePath = globalConfiguration.Comparison.GetFilesDirectory();
                bool rewrite = bool.Parse(form["rewrite"]);
                string fileSavePath = "";
                if (string.IsNullOrEmpty(url))
                {
                    // Get the uploaded document from the Files collection
                    var httpPostedFile = form.Files["file"];
                    if (httpPostedFile != null)
                    {
                        if (rewrite)
                        {
                            // Get the complete file path
                            fileSavePath = System.IO.Path.Combine(documentStoragePath, httpPostedFile.FileName);
                        }
                        else
                        {
                            fileSavePath = Resources.GetFreeFileName(documentStoragePath, httpPostedFile.FileName);
                        }

                        // Save the uploaded file to "UploadedFiles" folder
                        using var stream = new FileStream(fileSavePath, FileMode.Create);
                        await httpPostedFile.CopyToAsync(stream);
                    }
                }
                else
                {
                    using (WebClient client = new WebClient())
                    {
                        // get file name from the URL
                        Uri uri = new Uri(url);
                        string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                        if (rewrite)
                        {
                            // Get the complete file path
                            fileSavePath = System.IO.Path.Combine(documentStoragePath, fileName);
                        }
                        else
                        {
                            fileSavePath = Resources.GetFreeFileName(documentStoragePath, fileName);
                        }
                        // Download the Web resource and save it into the current filesystem folder.
                        client.DownloadFile(url, fileSavePath);
                    }
                }
                uploadedDocument.Guid = fileSavePath;
                return uploadedDocument;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }

            return uploadedDocument;
        }

        /// <summary>
        /// Compare files from local storage
        /// </summary>
        /// <param name="compareRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("compare")]
        public object Compare(CompareRequest compareRequest)
        {
            try
            {
                // check formats
                if (_comparisonService.CheckFiles(compareRequest))
                {
                    // compare
                    CompareResultResponse result = _comparisonService.Compare(compareRequest);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.ContractResolver = new LowercaseContractResolver();
                    string json = JsonConvert.SerializeObject(result, Formatting.Indented, settings);
                    var compareResult = JsonConvert.DeserializeObject(json);
                    return compareResult;
                }
                else
                {
                    return new Resources().GenerateException(new Exception("Document types are different"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new Resources().GenerateException(ex);
            }
        }

        /// Set new changes in result file
        /// </summary>
        /// <param name="compareRequest"></param>
        /// <param name="listOfChanges"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("changes")]
        public object Changes(SetChangesRequest setChangesRequest)
        {
            try
            {
                CompareResultResponse result = _comparisonService.SetChanges(setChangesRequest);
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new LowercaseContractResolver();
                string json = JsonConvert.SerializeObject(result, Formatting.Indented, settings);
                var compareResult = JsonConvert.DeserializeObject(json);
                return compareResult;
            }
            catch (Exception ex)
            {
                return new Resources().GenerateException(ex);
            }
        }

        /// <summary>
        /// Get result page
        /// </summary>
        /// <param name="loadResultPageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("loadDocumentDescription")]
        public LoadDocumentEntity? LoadDocumentDescription(PostedDataEntity loadResultPageRequest)
        {
            try
            {
                LoadDocumentEntity document = ComparisonServiceImpl.LoadDocumentPages(loadResultPageRequest.Guid, loadResultPageRequest.Password, globalConfiguration.Comparison.GetPreloadResultPageCount() == 0);
                return document;
            }
            catch (PasswordProtectedFileException ex)
            {
                _logger.Error(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return null;
        }

        /// <summary>
        /// Get document page
        /// </summary>
        /// <param name="postedData">Post data</param>
        /// <returns>Document page object</returns>
        [HttpPost]
        [Route("loadDocumentPage")]
        public PageDescriptionEntity LoadDocumentPage(PostedDataEntity postedData)
        {
            return _comparisonService.LoadDocumentPage(postedData);
        }
    }
}