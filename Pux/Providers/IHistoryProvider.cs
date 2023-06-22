using Pux.Models;

namespace Pux.Providers
{
    public interface IHistoryProvider
    {
        IList<FileInDirectory> Compare(string path, IList<FileInDirectory> actualDirectoryContent);
    }
}
