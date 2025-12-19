using GMG.Application.Common.Dtos;

namespace GMG.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<List<string>> SaveFilesAsync(List<FileUploadDto> files, string folder, string? subfolder = null);
        Task<bool> DeleteFileAsync(string filePath);
        Task<bool> DeleteFilesAsync(List<string> filePaths);
    }
}
