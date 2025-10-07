using CleverConversion.Common.Annotation.Annotator;
using CleverConversion.Common.Annotation.Common.Config;
using CleverConversion.Common.Annotation.Common.Entity.Web;
using CleverConversion.Common.Annotation.Common.Resources;
using CleverConversion.Common.Annotation.Common.Util.Comparator;
using CleverConversion.Common.Annotation.Config;
using CleverConversion.Common.Annotation.Entity.Web;
using CleverConversion.Common.Annotation.Util;
using GroupDocs.Annotation.Models;
using GroupDocs.Annotation.Models.AnnotationModels;
using GroupDocs.Annotation.Options;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using GroupDocs.Annotation;

namespace CleverConversion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnotationController : Controller
    {
        private static GlobalConfiguration globalConfiguration = new();
        private readonly List<string> SupportedImageFormats = [".bmp", ".jpeg", ".jpg", ".tiff", ".tif", ".png", ".dwg", ".dcm", ".dxf"];
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Load Annotation configuration
        /// </summary>
        /// <returns>Annotation configuration</returns>
        [HttpGet]
        [Route("loadConfig")]
        public AnnotationConfiguration LoadConfig()
        {
            return globalConfiguration.Annotation;
        }

        /// <summary>
        /// Get all files and directories from storage
        /// </summary>
        /// <param name="postedData">SignaturePostedDataEntity</param>
        /// <returns>List of files and directories</returns>
        [HttpPost]
        [Route("loadFileTree")]
        public List<FileDescriptionEntity> loadFileTree(PostedDataEntity postedData)
        {
            // get request body
            string relDirPath = postedData.path;
            // get file list from storage path
            try
            {
                // get all the files from a directory
                if (string.IsNullOrEmpty(relDirPath))
                {
                    relDirPath = globalConfiguration.Annotation.FilesDirectory;
                }
                else
                {
                    relDirPath = Path.Combine(globalConfiguration.Annotation.FilesDirectory, relDirPath);
                }

                List<FileDescriptionEntity> fileList = new List<FileDescriptionEntity>();
                List<string> allFiles = new List<string>(Directory.GetFiles(relDirPath));
                allFiles.AddRange(Directory.GetDirectories(relDirPath));

                allFiles.Sort(new FileNameComparator());
                allFiles.Sort(new FileTypeComparator());

                foreach (string file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    // check if current file/folder is hidden
                    if (!(fileInfo.Attributes.HasFlag(FileAttributes.Hidden) ||
                        Path.GetFileName(file).Equals(Path.GetFileName(globalConfiguration.Annotation.FilesDirectory)) ||
                        Path.GetFileName(file).StartsWith(".")))
                    {
                        FileDescriptionEntity fileDescription = new FileDescriptionEntity();
                        fileDescription.guid = Path.GetFullPath(file);
                        fileDescription.name = Path.GetFileName(file);
                        // set is directory true/false
                        fileDescription.isDirectory = fileInfo.Attributes.HasFlag(FileAttributes.Directory);
                        // set file size
                        if (!fileDescription.isDirectory)
                        {
                            fileDescription.size = fileInfo.Length;
                        }
                        // add object to array list
                        fileList.Add(fileDescription);
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return [];
        }

        /// <summary>
        /// Load document description
        /// </summary>
        /// <param name="postedData">Post data</param>
        /// <returns>Document info object</returns>
        [HttpPost]
        [Route("loadDocumentDescription")]
        public AnnotatedDocumentEntity LoadDocumentDescription(AnnotationPostedDataEntity postedData)
        {
            string password = "";
            try
            {
                AnnotatedDocumentEntity loadDocumentEntity = LoadDocument(postedData, globalConfiguration.Annotation.PreloadPageCount == 0);
                return loadDocumentEntity;
            }
            catch (Exception ex)
            {
                // set exception message
                _logger.Error(ex);
                return new();
            }
        }

        public AnnotatedDocumentEntity LoadDocument(AnnotationPostedDataEntity loadDocumentRequest, bool loadAllPages)
        {
            string password = loadDocumentRequest.password;
            AnnotatedDocumentEntity description = new();
            string documentGuid = loadDocumentRequest.guid;

            using (Annotator annotator = new(documentGuid, GetLoadOptions(password)))
            {
                IDocumentInfo info = annotator.Document.GetDocumentInfo();
                AnnotationBase[] annotations = annotator.Get().ToArray();
                description.guid = loadDocumentRequest.guid;
                string documentType = getDocumentType(info);

                description.supportedAnnotations = new SupportedAnnotations().GetSupportedAnnotations(documentType);

                List<string> pagesContent = new List<string>();

                if (loadAllPages)
                {
                    pagesContent = GetAllPagesContent(annotator, info);
                }

                for (int i = 0; i < info.PagesInfo.Count; i++)
                {
                    PageDataDescriptionEntity page = new PageDataDescriptionEntity
                    {
                        Number = i + 1,
                        Height = info.PagesInfo[i].Height,
                        Width = info.PagesInfo[i].Width,
                    };

                    if (annotations != null && annotations.Length > 0)
                    {
                        page.SetAnnotations(AnnotationMapper.MapForPage(annotations, i + 1, info.PagesInfo[i], documentType));
                    }

                    if (pagesContent.Count > 0)
                    {
                        page.Data = pagesContent[i];
                    }
                    description.pages.Add(page);
                }
            }

            description.guid = documentGuid;
            // return document description
            return description;
        }

        /// <summary>
        /// Get document page
        /// </summary>
        /// <param name="loadDocumentPageRequest"></param>
        /// <returns>Document page image</returns>
        [HttpPost]
        [Route("loadDocumentPage")]
        public PageDataDescriptionEntity LoadDocumentPage(AnnotationPostedDataEntity loadDocumentPageRequest)
        {
            string password = "";
            try
            {
                // get/set parameters
                string documentGuid = loadDocumentPageRequest.guid;
                int pageNumber = loadDocumentPageRequest.page;
                password = loadDocumentPageRequest.password;
                PageDataDescriptionEntity loadedPage = new();

                // get page image
                byte[] bytes;

                using (GroupDocs.Annotation.Annotator annotator = new GroupDocs.Annotation.Annotator(documentGuid, GetLoadOptions(password)))
                {
                    using (var memoryStream = RenderPageToMemoryStream(annotator, pageNumber))
                    {
                        bytes = memoryStream.ToArray();
                    }

                    IDocumentInfo info = annotator.Document.GetDocumentInfo();
                    AnnotationBase[] annotations = annotator.Get().ToArray();
                    string documentType = getDocumentType(info);

                    if (annotations != null && annotations.Length > 0)
                    {
                        loadedPage.SetAnnotations(AnnotationMapper.MapForPage(annotations, pageNumber, info.PagesInfo[pageNumber - 1], documentType));
                    }

                    string encodedImage = Convert.ToBase64String(bytes);
                    loadedPage.Data = encodedImage;

                    loadedPage.Height = info.PagesInfo[pageNumber - 1].Height;
                    loadedPage.Width = info.PagesInfo[pageNumber - 1].Width;
                    loadedPage.Number = pageNumber;
                }

                // return loaded page object
                return loadedPage;
            }
            catch (Exception ex)
            {
                // set exception message
                _logger.Error(ex);
                return new();
            }
        }

        private string getDocumentType(IDocumentInfo info)
        {
            string documentType = string.Empty;
            if (info.FileType != null)
            {
                documentType = SupportedImageFormats.Contains(info.FileType.Extension) ? "image" : info.FileType.ToString();
            }
            else
            {
                documentType = "Portable Document Format";
            }

            return documentType;
        }

        private static List<string> GetAllPagesContent(GroupDocs.Annotation.Annotator annotator, IDocumentInfo pages)
        {
            List<string> allPages = new List<string>();

            //get page HTML
            for (int i = 0; i < pages.PagesInfo.Count; i++)
            {
                byte[] bytes;
                using (var memoryStream = RenderPageToMemoryStream(annotator, i + 1))
                {
                    bytes = memoryStream.ToArray();
                }

                string encodedImage = Convert.ToBase64String(bytes);
                allPages.Add(encodedImage);
            }

            return allPages;
        }

        static MemoryStream RenderPageToMemoryStream(GroupDocs.Annotation.Annotator annotator, int pageNumberToRender)
        {
            MemoryStream result = new MemoryStream();

            PreviewOptions previewOptions = new PreviewOptions(pageNumber => result)
            {
                PreviewFormat = PreviewFormats.PNG,
                PageNumbers = new[] { pageNumberToRender },
                RenderComments = false
            };

            annotator.Document.GeneratePreview(previewOptions);

            return result;
        }

        /// <summary>
        /// Upload document
        /// </summary>      
        /// <returns>Uploaded document object</returns>
        [HttpPost]
        [Route("uploadDocument")]
        public async Task<UploadedDocumentEntity> UploadDocument()
        {
            try
            {
                var form = await Request.ReadFormAsync();
                string url = form["url"];
                // get documents storage path
                string documentStoragePath = globalConfiguration.Annotation.FilesDirectory;
                bool.TryParse(form["rewrite"], out bool rewrite);
                string fileSavePath = "";
                if (string.IsNullOrEmpty(url))
                {
                    var file = form.Files["file"];
                    if (file != null)
                    {
                        string targetFileName = rewrite
                            ? file.FileName
                            : Resources.GetFreeFileName(documentStoragePath, file.FileName);

                        fileSavePath = Path.Combine(documentStoragePath, targetFileName);

                        using (var stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                else
                {
                    using WebClient client = new();
                    // get file name from the URL
                    Uri uri = new Uri(url);
                    string fileName = Path.GetFileName(uri.LocalPath);
                    if (rewrite)
                    {
                        // Get the complete file path
                        fileSavePath = Path.Combine(documentStoragePath, fileName);
                    }
                    else
                    {
                        fileSavePath = Resources.GetFreeFileName(documentStoragePath, fileName);
                    }
                    // Download the Web resource and save it into the current filesystem folder.
                    client.DownloadFile(url, fileSavePath);
                }

                UploadedDocumentEntity uploadedDocument = new()
                {
                    guid = fileSavePath
                };

                return uploadedDocument;
            }
            catch (Exception ex)
            {
                // set exception message
                _logger.Error(ex);
                return new();
            }
        }

        /// <summary>
        /// Download document
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="annotated">bool</param>
        /// <returns></returns>
        [HttpGet]
        [Route("downloadDocument")]
        public HttpResponseMessage DownloadDocument(string path)
        {
            // add file into the response
            if (System.IO.File.Exists(path))
            {
                RemoveAnnotations(path, "");
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var fileStream = new FileStream(path, FileMode.Open);
                response.Content = new StreamContent(fileStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Download document
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="annotated">bool</param>
        /// <returns></returns>
        [HttpGet]
        [Route("downloadAnnotated")]
        public HttpResponseMessage DownloadAnnotated(string path)
        {
            // add file into the response
            if (System.IO.File.Exists(path))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var fileStream = new FileStream(path, FileMode.Open);
                response.Content = new StreamContent(fileStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        ///// <summary>
        ///// Annotate document
        ///// </summary>      
        ///// <returns>Annotated document info</returns>
        [HttpPost]
        [Route("annotate")]
        public AnnotatedDocumentEntity Annotate(AnnotationPostedDataEntity annotateDocumentRequest)
        {
            AnnotatedDocumentEntity annotatedDocument = new();

            try
            {
                // get/set parameters
                string documentGuid = annotateDocumentRequest.guid;
                string password = annotateDocumentRequest.password;
                string documentType = SupportedImageFormats.Contains(Path.GetExtension(annotateDocumentRequest.guid).ToLowerInvariant()) ? "image" : annotateDocumentRequest.DocumentType;
                string tempPath = GetTempPath(documentGuid);

                AnnotationDataEntity[] annotationsData = annotateDocumentRequest.AnnotationsData;
                // initiate list of annotations to add
                List<AnnotationBase> annotations = new List<AnnotationBase>();

                using (GroupDocs.Annotation.Annotator annotator = new GroupDocs.Annotation.Annotator(documentGuid, GetLoadOptions(password)))
                {
                    IDocumentInfo info = annotator.Document.GetDocumentInfo();

                    for (int i = 0; i < annotationsData.Length; i++)
                    {
                        AnnotationDataEntity annotationData = annotationsData[i];
                        PageInfo pageInfo = info.PagesInfo[annotationsData[i].PageNumber - 1];
                        // add annotation, if current annotation type isn't supported by the current document type it will be ignored
                        try
                        {
                            BaseAnnotator baseAnnotator = AnnotatorFactory.createAnnotator(annotationData, pageInfo);
                            if (baseAnnotator.IsSupported(documentType))
                            {
                                annotations.Add(baseAnnotator.GetAnnotationBase(documentType));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new GroupDocs.Annotation.Exceptions.AnnotatorException(ex.Message, ex);
                        }
                    }
                }

                // Add annotation to the document
                RemoveAnnotations(documentGuid, password);
                // check if annotations array contains at least one annotation to add
                if (annotations.Count != 0)
                {
                    using (GroupDocs.Annotation.Annotator annotator = new Annotator(documentGuid, GetLoadOptions(password)))
                    {
                        foreach (var annotation in annotations)
                        {
                            annotator.Add(annotation);
                        }

                        annotator.Save(tempPath);
                    }

                    if (System.IO.File.Exists(documentGuid))
                    {
                        System.IO.File.Delete(documentGuid);
                    }

                    System.IO.File.Move(tempPath, documentGuid);
                }

                annotatedDocument = new AnnotatedDocumentEntity();
                annotatedDocument.guid = documentGuid;
                if (annotateDocumentRequest.Print != null && annotateDocumentRequest.Print.Value)
                {
                    annotatedDocument.pages = GetAnnotatedPagesForPrint(password, documentGuid);
                    System.IO.File.Move(documentGuid, annotateDocumentRequest.guid);
                }
            }
            catch (Exception ex)
            {
                // set exception message
                _logger.Error(ex);
            }

            return annotatedDocument;
        }

        private static List<PageDataDescriptionEntity> GetAnnotatedPagesForPrint(string password, string documentGuid)
        {
            AnnotatedDocumentEntity description = new AnnotatedDocumentEntity();
            try
            {
                using (FileStream outputStream = System.IO.File.OpenRead(documentGuid))
                {
                    using Annotator annotator = new Annotator(outputStream, GetLoadOptions(password));
                    IDocumentInfo info = annotator.Document.GetDocumentInfo();
                    List<string> pagesContent = GetAllPagesContent(annotator, info);

                    for (int i = 0; i < info.PageCount; i++)
                    {
                        PageDataDescriptionEntity page = new();

                        if (pagesContent.Count > 0)
                        {
                            page.Data = pagesContent[i];
                        }

                        description.pages.Add(page);
                    }
                }

                return description.pages;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

        public static void RemoveAnnotations(string documentGuid, string password)
        {
            string tempPath = GetTempPath(documentGuid);

            try
            {
                using (Stream inputStream = System.IO.File.Open(documentGuid, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (GroupDocs.Annotation.Annotator annotator = new GroupDocs.Annotation.Annotator(inputStream, GetLoadOptions(password)))
                    {
                        annotator.Save(tempPath, new SaveOptions { AnnotationTypes = AnnotationType.None });
                    }
                }

                System.IO.File.Delete(documentGuid);
                System.IO.File.Move(tempPath, documentGuid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetTempPath(string documentGuid)
        {
            string tempFilename = Path.GetFileNameWithoutExtension(documentGuid) + "_tmp";
            string tempPath = Path.Combine(Path.GetDirectoryName(documentGuid), tempFilename + Path.GetExtension(documentGuid));
            return tempPath;
        }

        private static LoadOptions GetLoadOptions(string password)
        {
            LoadOptions loadOptions = new()
            {
                Password = password
            };

            return loadOptions;
        }
    }
}