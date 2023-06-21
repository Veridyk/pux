using Pux.Models;

namespace Pux.Providers
{
    public interface IHistoryProvider
    {
        List<FileInDirectory> Compare(string path, List<FileInDirectory> actualDirectoryContent);
    }
}
