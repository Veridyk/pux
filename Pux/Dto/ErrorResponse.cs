namespace Pux.Dto
{
    public sealed record ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Directory { get; set; } = string.Empty;
    }
}
