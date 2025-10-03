namespace CleverConversion.Dtos
{
    public class APIResponse<T>
    {
        public string Message { get; set; } = "SUCCESS";
        public string? ErrorMessage { get; set; } = null;
        public T Data { get; set; }
    }
}
