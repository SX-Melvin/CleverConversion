namespace CleverConversion.Dto.API
{
    public class DownloadNodeResponse
    {
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
        public string? RedirectLink { get; set; } = null;
    }
}
