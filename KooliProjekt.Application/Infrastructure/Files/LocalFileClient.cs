using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Infrastructure.Files
{
    public class LocalFileClient : IFileClient
    {
        private readonly string _filesRoot;

        public LocalFileClient(string filesRoot)
        {
            if (string.IsNullOrWhiteSpace(filesRoot))
            {
                throw new ArgumentException("FilesRoot is not configured. Add the FilesRoot setting to appsettings.json.", nameof(filesRoot));
            }

            _filesRoot = Path.GetFullPath(filesRoot);
        }

        public async Task Save(string fileName, string fileStore, Stream inputStream)
        {
            var folderPath = GetFolderPath(fileStore);
            var filePath = GetFilePath(folderPath, fileName);

            // Kui kaust on juba olemas, ei tee see midagi
            Directory.CreateDirectory(folderPath);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await inputStream.CopyToAsync(fileStream);
        }

        public Task<bool> Delete(string fileName, string fileStore)
        {
            var folderPath = GetFolderPath(fileStore);
            var filePath = GetFilePath(folderPath, fileName);

            if (!File.Exists(filePath))
            {
                return Task.FromResult(false);
            }

            File.Delete(filePath);

            return Task.FromResult(true);
        }

        public Task<Stream> Get(string fileName, string fileStore)
        {
            var folderPath = GetFolderPath(fileStore);
            var filePath = GetFilePath(folderPath, fileName);

            if (!File.Exists(filePath))
            {
                return Task.FromResult<Stream>(null);
            }

            Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

            return Task.FromResult(stream);
        }

        public Task<IList<string>> List(string fileStore)
        {
            var folderPath = GetFolderPath(fileStore);

            // Kausta tekib alles siis, kui sinna esimene fail salvestatakse
            if (!Directory.Exists(folderPath))
            {
                return Task.FromResult<IList<string>>(new List<string>());
            }

            IList<string> fileNames = Directory
                .GetFiles(folderPath)
                .Select(Path.GetFileName)
                .OrderBy(name => name)
                .ToList();

            return Task.FromResult(fileNames);
        }

        private string GetFolderPath(string fileStore)
        {
            var folderPath = Path.GetFullPath(Path.Combine(_filesRoot, fileStore));

            // Kaust peab jääma FilesRoot'i sisse
            var root = _filesRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (!(folderPath + Path.DirectorySeparatorChar).StartsWith(root, comparison))
            {
                throw new ArgumentException("File store must be inside the files root.", nameof(fileStore));
            }

            return folderPath;
        }

        private static string GetFilePath(string folderPath, string fileName)
        {
            if (!FileNameHelper.IsSafeFileName(fileName))
            {
                throw new ArgumentException("Invalid file name.", nameof(fileName));
            }

            return Path.Combine(folderPath, fileName);
        }
    }
}
