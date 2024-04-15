using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Abstractions.Repositories;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Constants;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using Prime.Progreso.Domain.Exceptions;
using Prime.Progreso.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Services
{
    public class QuizAssignmentService : IQuizAssignmentService
    {
        private readonly IQuizAssignmentRepository _repository;
        private readonly IValidationProvider _validationProvider;
        private readonly IQuizRepository _quizRepository;
        private readonly IUserDetailsProvider _userDetails;

        public QuizAssignmentService(IQuizAssignmentRepository quizAssignmentRepository, 
            IValidationProvider validationProvider, IQuizRepository quizRepository, IUserDetailsProvider userDetails)
        {
            _repository = quizAssignmentRepository;
            _validationProvider = validationProvider;
            _quizRepository = quizRepository;
            _userDetails = userDetails;
        }

        public async Task<QuizAssignmentResponseDto> CreateAsync(QuizAssignmentRequestDto dto)
        {
            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            return await _repository.CreateAsync<QuizAssignmentRequestDto, QuizAssignmentResponseDto>(dto);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(Guid id, QuizAssignmentRequestDto dto)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _validationProvider.TryValidate(dto);

            await ValidateRequestDto(dto);

            await _repository.UpdateAsync(id, dto);
        }

        public async Task<PaginatedResult<QuizAssignmentResponseDto>> GetPageAsync(PagingInfo pagingInfo)
        {
            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _repository.GetPageByFilterAsync(pagingInfo, _userDetails.GetUserId());
            }

            return await _repository.GetPageAsync<QuizAssignmentResponseDto>(pagingInfo.Page, pagingInfo.Size);
        }

        public async Task<QuizAssignmentResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (_userDetails.GetUserRole() == RoleAuthorizationConstants.Intern)
            {
                return await _repository.GetByIdAsync<QuizAssignmentResponseDto>(id, _userDetails.GetUserId());
            }

            return await _repository.GetByIdAsync<QuizAssignmentResponseDto>(id);
        }

        private async Task ValidateRequestDto(QuizAssignmentRequestDto dto)
        {
            var quiz = await _quizRepository.GetByIdAsync(dto.QuizId);

            if (quiz is null)
            {
                throw new NotFoundException("Invalid quiz ID.");
            }

            if (dto.StartTime.Add(new TimeSpan(0, quiz.Duration, 0)) > dto.EndTime)
            {
                throw new ValidationException("Specified time period will not be enough for given Quiz!");
            }
        }
    }
}
