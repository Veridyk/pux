namespace Pux.Models
{
    public sealed class FileInDirectory
    {
        public string Filename { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public FileState State { get; set; }
        public int Version { get; set; } = 1;
        public string Hash { get; set; } = string.Empty;
    }
}
