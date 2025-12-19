using GMG.Application.Common.Dtos;
using GMG.Application.Common.Interfaces;

namespace GMG.Infrastructure.Services
{
    public class LocalFileStorageService(IPathProvider _pathProvider) : IFileStorageService
    {

        public async Task<List<string>> SaveFilesAsync(
            List<FileUploadDto> files,
            string folder,
            string subFolder = null)
        {
            var savedPaths = new List<string>();

            // Usar la abstracción en lugar de IWebHostEnvironment
            var webRootPath = _pathProvider.GetWebRootPath();

            var basePath = subFolder != null
                ? _pathProvider.CombinePath(webRootPath, folder, subFolder)
                : _pathProvider.CombinePath(webRootPath, folder);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            foreach (var file in files)
            {
                if (!file.ContentType.StartsWith("image/"))
                {
                    continue;
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = _pathProvider.CombinePath(basePath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.Content.CopyToAsync(fileStream);
                }

                var relativePath = subFolder != null
                    ? $"{folder}/{subFolder}/{fileName}"
                    : $"{folder}/{fileName}";

                savedPaths.Add(relativePath);
            }

            return savedPaths;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var webRootPath = _pathProvider.GetWebRootPath();
                var fullPath = _pathProvider.CombinePath(webRootPath, filePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteFilesAsync(List<string> filePaths)
        {
            var allDeleted = true;

            foreach (var filePath in filePaths)
            {
                var deleted = await DeleteFileAsync(filePath);
                if (!deleted)
                {
                    allDeleted = false;
                }
            }

            return allDeleted;
        }
    }
}
