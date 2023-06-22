using Pux.Dto;

namespace Pux.Services
{
    public interface IFileService
    {
        IList<FileInDirectoryDto> GetDirectoryContent(string path);
        IList<FileInDirectoryDto> Compare(string path);
    }
}
