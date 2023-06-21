using Pux.Models;

namespace Pux.Providers
{
    public sealed class HistoryProvider : IHistoryProvider
    {
        public Dictionary<string, List<FileInDirectory>> History { get; set; } = new Dictionary<string, List<FileInDirectory>>();

        public List<FileInDirectory>? GetHistory(string path)
        {
            List<FileInDirectory>? lastDirectoryContent;
            History.TryGetValue(path, out lastDirectoryContent);
            return lastDirectoryContent;
        }

        public void SaveHistory(string path, List<FileInDirectory> actualDirectoryContent)
        {
            if (History.ContainsKey(path))
            {
                History[path] = actualDirectoryContent;
            }
            else
            {
                History.Add(path, actualDirectoryContent);
            }
        }

        public List<FileInDirectory> Compare(string path, List<FileInDirectory> actualDirectoryContent)
        {
            var lastDirectoryContent = GetHistory(path);
            if (lastDirectoryContent == null)
            {
                SaveHistory(path, actualDirectoryContent);
                return new List<FileInDirectory>();
            }

            List<FileInDirectory> difference = CompareFiles(actualDirectoryContent, lastDirectoryContent);

            SaveHistory(path, actualDirectoryContent);
            return difference;
        }

        private List<FileInDirectory> CompareFiles(List<FileInDirectory> actualDirectoryContent, List<FileInDirectory> lastDirectoryContent)
        {
            List<FileInDirectory> changedFiles = new List<FileInDirectory>();

            foreach (var file in actualDirectoryContent)
            {
                var lastFile = lastDirectoryContent.Find(f => f.Path == file.Path);
                if (lastFile != null)
                {
                    if (lastFile.Hash != file.Hash)
                    {
                        file.Version = lastFile.Version + 1;
                        changedFiles.Add(new FileInDirectory()
                        {
                            Filename = file.Filename,
                            Path = file.Path,
                            State = FileState.MODIFIED,
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
                        State = FileState.ADDED,
                        Version = 1,
                        Hash = file.Hash,
                    });
                }
            }

            foreach (var file in lastDirectoryContent)
            {
                var lastExistedFile = actualDirectoryContent.Find(f => f.Path == file.Path);
                if (lastExistedFile == null)
                {
                    changedFiles.Add(new FileInDirectory()
                    {
                        Filename = file.Filename,
                        Path = file.Path,
                        State = FileState.DELETED,
                        Version = file.Version,
                        Hash = file.Hash,
                    });
                }
            }

            return changedFiles;
        }
    }
}
