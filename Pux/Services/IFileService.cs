using Pux.Dto;

namespace Pux.Services
{
    public interface IFileService
    {
        List<FileInDirectoryDto> GetDirectoryContent(string path);
        List<FileInDirectoryDto> Compare(string path);
    }
}
