using Pux.Models;
using System.Security.Cryptography;

namespace Pux.Providers
{
    public sealed class FileProvider : IFileProvider
    {
        public FileProvider()
        {
            
        }

        public IList<FileInDirectory> LoadDirectoryContent(string path)
        {
            var files = new List<FileInDirectory>();
            var filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                string hash = CalculateFileHash(filePath);
                string filename = Path.GetRelativePath(path, filePath);
                files.Add(new FileInDirectory()
                {
                    Filename = filename,
                    Path = filePath,
                    State = FileState.Actual,
                    Version = 1,
                    Hash = hash
                });

            }

            return files;
        }

        public static string CalculateFileHash(string path)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(path);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
