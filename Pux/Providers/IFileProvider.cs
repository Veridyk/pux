using Pux.Models;

namespace Pux.Providers
{
    public interface IFileProvider
    {
        List<FileInDirectory> LoadDirectoryContent(string path);
    }
}
