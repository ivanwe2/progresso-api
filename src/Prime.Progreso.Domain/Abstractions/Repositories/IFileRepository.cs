namespace Prime.Progreso.Domain.Abstractions.Repositories
{
    public interface IFileRepository
    {
        Task WriteContentToFileAsync(Stream fileStream, string fileName, string fileContentType);
        Task<string> UpdateExistingFileAsync(Stream fileStream, string filePath);
        void DeleteFile(string filePath);
        StreamReader ReturnFileAsStream(string filePath);
    }
}
