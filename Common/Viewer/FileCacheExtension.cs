namespace CleverConversion.Common.Viewer
{
    public static class FileCacheExtension
    {
        public static async Task<TEntry> GetOrAddAsync<TEntry>(
            this IFileCache cache,
            string cacheKey,
            string filePath,
            Func<Task<TEntry>> valueFactory)
        {
            var existing = await cache.TryGetValueAsync<TEntry>(cacheKey, filePath);
            if (existing != null)
                return existing;

            var newValue = await valueFactory();
            await cache.SetAsync(cacheKey, filePath, newValue);
            return newValue;
        }

    }
}
