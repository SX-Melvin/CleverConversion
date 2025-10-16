using System.Threading.Tasks;
using CleverConversion.Common.Viewer.Entities;

namespace CleverConversion.Common.Viewer
{
    public interface IPageFormatter
    {
        Task<Page> FormatAsync(FileCredentials fileCredentials, Page page);
    }
}