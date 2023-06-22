using Pux.Models;

namespace Pux.Dto
{
    public sealed record FileInDirectoryDto
    {
        public string Filename { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public FileState State { get; set; }
        public int Version { get; set; }
    }
}
