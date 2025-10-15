using System;
using System.Collections.Generic;
using System.IO;
using CleverConversion.Common.Common.Entity.Web;
using CleverConversion.Common.Comparison.Comparison.Model.Response;
using CleverConversion.Common.Common.Config;
using CleverConversion.Common.Common.Util.Comparator;
using CleverConversion.Common.Comparison.Comparison.Model.Request;
using GroupDocs.Comparison.Result;
using GroupDocs.Comparison.Interfaces;
using GroupDocs.Comparison.Options;
using GroupDocs.Comparison;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace CleverConversion.Common.Comparison.Comparison.Service
{
    public class ComparisonServiceImpl
    {
        private readonly GlobalConfiguration globalConfiguration = new();
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public List<FileDescriptionEntity> LoadFiles(PostedDataEntity fileTreeRequest)
        {
            // get request body
            string relDirPath = fileTreeRequest.Path;

            // get file list from storage path
            try
            {
                // get all the files from a directory
                if (string.IsNullOrEmpty(relDirPath))
                {
                    relDirPath = globalConfiguration.Comparison.FilesDirectory;
                }
                else
                {
                    relDirPath = Path.Combine(globalConfiguration.Comparison.FilesDirectory, relDirPath);
                }

                List<string> allFiles = new List<string>(Directory.GetFiles(relDirPath));
                allFiles.AddRange(Directory.GetDirectories(relDirPath));
                List<FileDescriptionEntity> fileList = new List<FileDescriptionEntity>();

                allFiles.Sort(new FileNameComparator());
                allFiles.Sort(new FileTypeComparator());

                foreach (string file in allFiles)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                    // check if current file/folder is hidden
                    if (!(fileInfo.Attributes.HasFlag(FileAttributes.Hidden) ||
                        Path.GetFileName(file).StartsWith(".") ||
                        Path.GetFileName(file).Equals(Path.GetFileName(globalConfiguration.Comparison.FilesDirectory)) ||
                        Path.GetFileName(file).Equals(Path.GetFileName(globalConfiguration.Comparison.ResultDirectory))))
                    {
                        FileDescriptionEntity fileDescription = new FileDescriptionEntity
                        {
                            Guid = Path.GetFullPath(file),
                            Name = Path.GetFileName(file),
                            // set is directory true/false
                            IsDirectory = fileInfo.Attributes.HasFlag(FileAttributes.Directory)
                        };
                        // set file size
                        if (!fileDescription.IsDirectory)
                        {
                            fileDescription.Size = fileInfo.Length;
                        }
                        // add object to array list
                        fileList.Add(fileDescription);
                    }
                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Exception occurred while loading files", ex);
            }
        }

        public bool CheckFiles(CompareRequest files)
        {
            string extension = Path.GetExtension(files.Guids[0].Guid);

            _logger.Info(JsonConvert.SerializeObject(files.Guids[0]));
            _logger.Info(extension);
            _logger.Info(CheckSupportedFiles(extension));

            if (!CheckSupportedFiles(extension))
            {
                return false;
            }

            foreach (CompareFileDataRequest path in files.Guids)
            {
                if (!extension.Equals(Path.GetExtension(path.Guid)))
                {
                    return false;
                }
            }

            return true;
        }

        public ChangeInfo[] RejectChange(Comparer comparer, ChangeInfo[] changes, int changeNumber)
        {
            changes[changeNumber].ComparisonAction = ComparisonAction.Reject;
            return changes;
        }

        public ChangeInfo[] AcceptChange(Comparer comparer, ChangeInfo[] changes, int changeNumber)
        {
            changes[changeNumber].ComparisonAction = ComparisonAction.Accept;
            return changes;
        }

        public CompareResultResponse Compare(CompareRequest compareRequest)
        {
            CompareResultResponse compareResultResponse = CompareTwoDocuments(compareRequest);
            return compareResultResponse;
        }

        public CompareResultResponse SetChanges(SetChangesRequest setChangesRequest)
        {
            string extension = Path.GetExtension(setChangesRequest.guids[0].Guid);
            string guid = Guid.NewGuid().ToString();
            string resultGuid = Path.Combine(globalConfiguration.Comparison.ResultDirectory, guid + extension);
            
            string firstPath = setChangesRequest.guids[0].Guid;
            string secondPath = setChangesRequest.guids[1].Guid;

            Comparer compareResult = new Comparer(firstPath, GetLoadOptions(setChangesRequest.guids[0].Password));

            compareResult.Add(secondPath, GetLoadOptions(setChangesRequest.guids[1].Password));
            CompareOptions compareOptions = new CompareOptions { CalculateCoordinates = true };
            if (Path.GetExtension(resultGuid) == ".pdf")
            {
                compareOptions.DetalisationLevel = DetalisationLevel.High;
            }
            using (FileStream outputStream = File.Create(Path.Combine(resultGuid)))
            {
                compareResult.Compare(outputStream, compareOptions);
            }
            ChangeInfo[] changes = compareResult.GetChanges();

            for (int i = 0; i < setChangesRequest.changes.Length; i++)
            {
                ComparisonAction action = ComparisonAction.None;
                switch (setChangesRequest.changes[i])
                {
                    case 1:
                        action = ComparisonAction.Accept;
                        break;
                    case 2:
                        action = ComparisonAction.Reject;
                        break;
                    case 3:
                        action = ComparisonAction.None;
                        break;
                }
                changes[i].ComparisonAction = action;
            }

            compareResult.ApplyChanges(resultGuid, new SaveOptions(), new ApplyChangeOptions() { Changes = changes });

            CompareResultResponse compareResultResponse = GetCompareResultResponse(changes, resultGuid);
            compareResultResponse.Extension = extension;

            return compareResultResponse;
        }

        public static LoadDocumentEntity LoadDocumentPages(string documentGuid, string password, bool loadAllPages)
        {
            LoadDocumentEntity loadDocumentEntity = new LoadDocumentEntity();

            using Comparer comparer = new(documentGuid, GetLoadOptions(password));
            Dictionary<int, string> pagesContent = new Dictionary<int, string>();
            IDocumentInfo documentInfo = comparer.Source.GetDocumentInfo();
            if (documentInfo.PagesInfo == null)
            {
                throw new GroupDocs.Comparison.Common.Exceptions.ComparisonException("File is corrupted.");
            }

            if (loadAllPages)
            {
                for (int i = 0; i < documentInfo.PageCount; i++)
                {
                    string encodedImage = GetPageData(i, documentGuid, password);

                    pagesContent.Add(i, encodedImage);
                }
            }

            for (int i = 0; i < documentInfo.PageCount; i++)
            {
                PageDescriptionEntity pageData = new PageDescriptionEntity
                {
                    Height = documentInfo.PagesInfo[i].Height,
                    Width = documentInfo.PagesInfo[i].Width,
                    Number = i + 1
                };

                if (pagesContent.Count > 0)
                {
                    pageData.Data = pagesContent[i];
                }

                loadDocumentEntity.Pages.Add(pageData);
            }

            return loadDocumentEntity;
        }

        public PageDescriptionEntity LoadDocumentPage(PostedDataEntity postedData)
        {
            PageDescriptionEntity loadedPage = new PageDescriptionEntity();

            try
            {
                // get/set parameters
                string documentGuid = postedData.Guid;
                int pageNumber = postedData.Page;
                string password = (string.IsNullOrEmpty(postedData.Password)) ? null : postedData.Password;

                using (Comparer comparer = new Comparer(documentGuid, GetLoadOptions(password)))
                {
                    IDocumentInfo info = comparer.Source.GetDocumentInfo();

                    string encodedImage = GetPageData(pageNumber - 1, documentGuid, password);
                    loadedPage.Data = encodedImage;

                    loadedPage.Height = info.PagesInfo[pageNumber - 1].Height;
                    loadedPage.Width = info.PagesInfo[pageNumber - 1].Width;
                    loadedPage.Number = pageNumber;
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Exception occurred while loading result page", ex);
            }

            return loadedPage;
        }

        private static string GetPageData(int pageNumber, string documentGuid, string password)
        {
            string encodedImage = "";

            using (Comparer comparer = new Comparer(documentGuid, GetLoadOptions(password)))
            {
                byte[] bytes = RenderPageToMemoryStream(comparer, pageNumber).ToArray();
                encodedImage = Convert.ToBase64String(bytes);
            }

            return encodedImage;
        }

        static MemoryStream RenderPageToMemoryStream(Comparer comparer, int pageNumberToRender)
        {
            MemoryStream result = new MemoryStream();
            IDocumentInfo documentInfo = comparer.Source.GetDocumentInfo();

            PreviewOptions previewOptions = new PreviewOptions(pageNumber => result)
            {
                PreviewFormat = PreviewFormats.PNG,
                PageNumbers = new[] { pageNumberToRender + 1 },
                Height = documentInfo.PagesInfo[pageNumberToRender].Height,
                Width = documentInfo.PagesInfo[pageNumberToRender].Width
            };

            comparer.Source.GeneratePreview(previewOptions);

            return result;
        }

        private static LoadOptions GetLoadOptions(string password)
        {
            LoadOptions loadOptions = new LoadOptions
            {
                Password = password
            };

            return loadOptions;
        }

        private CompareResultResponse CompareTwoDocuments(CompareRequest compareRequest)
        {
            // to get correct coordinates we will compare document twice
            // this is a first comparing to get correct coordinates of the insertions and style changes
            string extension = Path.GetExtension(compareRequest.Guids[0].Guid);
            string guid = Guid.NewGuid().ToString();
            //save all results in file
            string resultGuid = Path.Combine(globalConfiguration.Comparison.ResultDirectory, guid + extension);

            Comparer compareResult = CompareFiles(compareRequest, resultGuid);
            ChangeInfo[] changes = compareResult.GetChanges();

            CompareResultResponse compareResultResponse = GetCompareResultResponse(changes, resultGuid);
            compareResultResponse.Extension = extension;
            return compareResultResponse;
        }

        private static Comparer CompareFiles(CompareRequest compareRequest, string resultGuid)
        {
            string firstPath = compareRequest.Guids[0].Guid;
            string secondPath = compareRequest.Guids[1].Guid;

            // create new comparer
            Comparer comparer = new Comparer(firstPath, GetLoadOptions(compareRequest.Guids[0].Password));

            comparer.Add(secondPath, GetLoadOptions(compareRequest.Guids[1].Password));
            CompareOptions compareOptions = new CompareOptions { CalculateCoordinates = true };

            if (Path.GetExtension(resultGuid) == ".pdf")
            {
                compareOptions.DetalisationLevel = DetalisationLevel.High;
            }

            using (FileStream outputStream = File.Create(Path.Combine(resultGuid)))
            {
                comparer.Compare(outputStream, compareOptions);
            }
            
            return comparer;
        }

        private static CompareResultResponse GetCompareResultResponse(ChangeInfo[] changes, string resultGuid)
        {
            CompareResultResponse compareResultResponse = new CompareResultResponse();
            compareResultResponse.Changes = changes;

            List<PageDescriptionEntity> pages = LoadDocumentPages(resultGuid, "", true).Pages;

            compareResultResponse.Pages = pages;
            compareResultResponse.Guid = resultGuid;
            return compareResultResponse;
        }

        /// <summary>
        /// Check support formats for comparing
        /// </summary>
        /// <param name="extension"></param>
        /// <returns>string</returns>
        private bool CheckSupportedFiles(string extension)
        {
            switch (extension)
            {
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".ppt":
                case ".pptx":
                case ".pdf":
                case ".txt":
                case ".html":
                case ".htm":
                case ".jpg":
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }
    }
}
