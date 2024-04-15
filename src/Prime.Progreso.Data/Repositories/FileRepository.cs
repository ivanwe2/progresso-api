using Microsoft.Extensions.Options;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Dtos.PathDtos;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Data.Repositories
{
    public class FileRepository<TPath> : IFileRepository
        where TPath : PathToDirectory
    {
        protected readonly string _directoryPath;

        public FileRepository(IOptions<TPath> pathOptions)
        {
            _directoryPath = pathOptions.Value.Path;
        }

        public async Task WriteContentToFileAsync(Stream fileStream, string fileName, string fileContentType)
        {
            if (fileStream == null)
            {
                throw new FileIsNullException();
            }

            string xmlContent;
            using (var reader = new StreamReader(fileStream))
            {
                xmlContent = await reader.ReadToEndAsync();
            }

            ValidateFileContent(xmlContent);

            string filePath = Path.Combine(_directoryPath, fileName + fileContentType);

            await File.WriteAllTextAsync(filePath, xmlContent);
        }

        public async Task<string> UpdateExistingFileAsync(Stream fileStream, string filePath)
        {
            if (fileStream == null)
            {
                throw new FileIsNullException();
            }

            string xmlContent;
            using (var reader = new StreamReader(fileStream))
            {
                xmlContent = await reader.ReadToEndAsync();
            }

            ValidateFileContent(xmlContent);

            DeleteFile(filePath);

            await File.WriteAllTextAsync(filePath, xmlContent);

            return filePath;
        }

        public StreamReader ReturnFileAsStream(string filePath)
        {
            CheckIfFileExists(filePath);

            var fileStream = new StreamReader(filePath);

            return fileStream;
        }

        public void DeleteFile(string filePath)
        {
            CheckIfFileExists(filePath);
            File.Delete(filePath);
        }

        private void CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File at {filePath} was not found!");
            }
        }

        private void ValidateFileContent(string xmlContent)
        {
            if (xmlContent == null || xmlContent == string.Empty)
            {
                throw new EmptyFileException();
            }
        }
    }
}
