using Pux.Dto;
using Pux.Providers;

namespace Pux.Services
{
    public sealed class FileService : IFileService
    {
        private readonly IFileProvider _fileProvider;
        private readonly IHistoryProvider _historyProvider;

        public FileService(IFileProvider fileProvider, IHistoryProvider historyProvder)
        {
            _fileProvider = fileProvider;
            _historyProvider = historyProvder;
        }

        public IList<FileInDirectoryDto> GetDirectoryContent(string path)
        {
            var files = _fileProvider.LoadDirectoryContent(path);
            return files.Select(file => new FileInDirectoryDto()
            {
                Filename = file.Filename,
                Path = path,
                State = file.State,
                Version = file.Version
            }).ToList();
        }

        public IList<FileInDirectoryDto> Compare(string path)
        {
            var actualDirectoryContent = _fileProvider.LoadDirectoryContent(path);
            var changedDirectoryContent = _historyProvider.Compare(path, actualDirectoryContent);

            return changedDirectoryContent.Select(file => new FileInDirectoryDto()
            {
                Filename = file.Filename,
                Path = path,
                State = file.State,
                Version = file.Version
            }).ToList();
        }
    }
}
