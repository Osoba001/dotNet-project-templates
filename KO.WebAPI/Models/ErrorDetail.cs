namespace KO.WebAPI.Models
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return $"Error code: {StatusCode}\n Message: {Message}";
        }
    }
}
