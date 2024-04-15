using Microsoft.AspNetCore.Http;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using Prime.Progreso.Domain.RequestModels.BpmnDiagram;

namespace Prime.Progreso.Services
{
    public class BpmnDiagramService : IBpmnDiagramService
    {
        private readonly IBpmnDiagramRepository _repository;
        private readonly IBpmnDiagramFileRepository _fileHandler;
        private readonly IUserDetailsProvider _userDetails;

        public BpmnDiagramService(IBpmnDiagramRepository repository, IBpmnDiagramFileRepository bpmnDiagramFileHandler,
            IUserDetailsProvider userDetailsHelper)
        {
            _repository = repository;
            _fileHandler = bpmnDiagramFileHandler;
            _userDetails = userDetailsHelper;
        }

        public async Task<BpmnDiagramGetMetadataResponseDto> CreateAsync(BpmnDiagramCreateRequestForm request)
        {
            TryValidateFile(request.BpmnDiagramFile);

            string fileType = '.' + request.BpmnDiagramFile.ContentType.Substring(request.BpmnDiagramFile.ContentType.IndexOf('/') + 1);

            await _repository.TryValidateNewFileName(request.FileName, fileType);

            await _fileHandler.WriteContentToFileAsync(request.BpmnDiagramFile.OpenReadStream(),
                 request.FileName, fileType);

            var dto = new BpmnDiagramCreateRequestDto
            {
                FileName = request.FileName,
                AuthorId = _userDetails.GetUserId()
            };

            return await _repository.CreateAsync<BpmnDiagramGetMetadataResponseDto>(dto, request.FileName+fileType);
        }

        public async Task DeleteAsync(Guid id)
        {
            TryValidateId(id);

            await CheckAuthorization(id, _userDetails.GetUserId(), _userDetails.GetUserRole());

            string filePath = await _repository.DeleteAsync(id);

            _fileHandler.DeleteFile(filePath);
        }

        public async Task<StreamReader> GetFileByIdAsync(Guid id)
        {
            TryValidateId(id);

            await CheckAuthorization(id, _userDetails.GetUserId(), _userDetails.GetUserRole());

            var dto = await _repository.GetByIdAsync(id);

            return _fileHandler.ReturnFileAsStream(dto.FilePath);
        }

        public async Task<PaginatedResult<BpmnDiagramGetMetadataResponseDto>> GetMetadataPageAsync(int pageIndex,
                                                                                                   int pageSize)
        {
            if(_userDetails.GetUserRole() == RoleAuthorizationConstants.Mentor)
            {
                return await _repository.GetPageAndFilterByUserIdAsync(pageIndex, pageSize, _userDetails.GetUserId());
            }

            return await _repository.GetPageAsync<BpmnDiagramGetMetadataResponseDto>(pageIndex, pageSize);
        }

        public async Task UpdateAsync(Guid id, BpmnDiagramUpdateRequestForm request)
        {
            TryValidateId(id);

            await CheckAuthorization(id, _userDetails.GetUserId(), _userDetails.GetUserRole());

            TryValidateFile(request.BpmnDiagramFile);

            var dto = await _repository.GetByIdAsync(id);

            await _fileHandler.UpdateExistingFileAsync(request.BpmnDiagramFile.OpenReadStream(), dto.FilePath);

            var dtoNew = new BpmnDiagramUpdateRequestDto()
            {
                AuthorId = _userDetails.GetUserId()
            };

            await _repository.UpdateAsync(id, dtoNew);
        }

        private void TryValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
        }

        private void TryValidateFile(IFormFile file)
        {
            if(file is null || file.ContentType == null)
            {
                throw new FileIsNullException();
            }
        }

        private async Task CheckAuthorization(Guid id, int userId, string role)
        {
            if (role == RoleAuthorizationConstants.Mentor && (!await _repository.IsAccessAllowedAsync(id, userId)))
            {
                throw new UnauthorizedAccessException("You do not have access to this BPMN diagram.");
            }
        }
    }
}
