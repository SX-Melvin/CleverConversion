using System.Threading.Tasks;
using CleverConversion.Common.Viewer.Entities;

namespace CleverConversion.Common.Viewer
{
    public interface IViewer
    {
        string PageExtension { get; }
        Page CreatePage(int pageNumber, byte[] data);
        Task<DocumentInfo> GetDocumentInfoAsync(FileCredentials fileCredentials);
        Task<Page> GetPageAsync(FileCredentials fileCredentials, int pageNumber);
        Task<Entities.Pages> GetPagesAsync(FileCredentials fileCredentials, int[] pageNumbers);
        Task<byte[]> GetPdfAsync(FileCredentials fileCredentials);
        Task<byte[]> GetPageResourceAsync(FileCredentials fileCredentials, int pageNumber, string resourceName);
    }
}