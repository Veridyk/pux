using Pux.Models;

namespace Pux.Providers
{
    public sealed class HistoryProvider : IHistoryProvider
    {
        private Dictionary<string, IList<FileInDirectory>> History { get; set; } = new Dictionary<string, IList<FileInDirectory>>();

        public IList<FileInDirectory>? GetHistory(string path) => History.TryGetValue(path, out var lastDirectoryContent) ? lastDirectoryContent : null;

        public void SaveHistory(string path, IList<FileInDirectory> actualDirectoryContent)
        {
            if(!History.TryAdd(path, actualDirectoryContent))
            {
                History[path] = actualDirectoryContent;
            }
        }

        public IList<FileInDirectory> Compare(string path, IList<FileInDirectory> actualDirectoryContent)
        {
            var lastDirectoryContent = GetHistory(path);
            if (lastDirectoryContent == null)
            {
                SaveHistory(path, actualDirectoryContent);
                return new List<FileInDirectory>();
            }

            var difference = CompareFiles(actualDirectoryContent, lastDirectoryContent);

            SaveHistory(path, actualDirectoryContent);
            return difference;
        }

        private IList<FileInDirectory> CompareFiles(IList<FileInDirectory> actualDirectoryContent, IList<FileInDirectory> lastDirectoryContent)
        {
            var changedFiles = new List<FileInDirectory>();

            foreach (var file in actualDirectoryContent)
            {
                var lastFile = lastDirectoryContent.FirstOrDefault(f => f.Path == file.Path);
                if (lastFile != null)
                {
                    if (lastFile.Hash != file.Hash)
                    {
                        file.Version = lastFile.Version + 1;
                        changedFiles.Add(new FileInDirectory()
                        {
                            Filename = file.Filename,
                            Path = file.Path,
                            State = FileState.Modified,
                            Version = file.Version,
                            Hash = file.Hash,
                        });
                    }
                }
                else
                {
                    changedFiles.Add(new FileInDirectory()
                    {
                        Filename = file.Filename,
                        Path = file.Path,
                        State = FileState.Added,
                        Version = 1,
                        Hash = file.Hash,
                    });
                }
            }

            foreach (var file in lastDirectoryContent)
            {
                var lastExistedFile = actualDirectoryContent.FirstOrDefault(f => f.Path == file.Path);
                if (lastExistedFile == null)
                {
                    changedFiles.Add(new FileInDirectory()
                    {
                        Filename = file.Filename,
                        Path = file.Path,
                        State = FileState.Deleted,
                        Version = file.Version,
                        Hash = file.Hash,
                    });
                }
            }

            return changedFiles;
        }
    }
}
