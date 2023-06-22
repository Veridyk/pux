using Pux.Models;

namespace Pux.Providers
{
    public interface IFileProvider
    {
        IList<FileInDirectory> LoadDirectoryContent(string path);
    }
}
