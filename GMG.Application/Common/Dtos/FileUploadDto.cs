namespace GMG.Application.Common.Dtos
{
    public record FileUploadDto(string FileName, string ContentType, long Length, Stream Content);
}
