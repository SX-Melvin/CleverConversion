namespace CleverConversion.Dto.OTCS
{
    public class DownloadFileResponse: CommonResponse
    {
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
    }
}
