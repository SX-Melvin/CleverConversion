using System.Threading.Tasks;

namespace CleverConversion.Common.Viewer
{
    public interface IFileCache
    {
        TEntry TryGetValue<TEntry>(string cacheKey, string filePath);

        Task<TEntry> TryGetValueAsync<TEntry>(string cacheKey, string filePath);

        void Set<TEntry>(string cacheKey, string filePath, TEntry entry);

        Task SetAsync<TEntry>(string cacheKey, string filePath, TEntry entry);
    }
}