﻿using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos
{
    public class KeywordDescriptionResponseDto : BaseResponseDto
    {
        public string Description { get; set; }

        public Difficulty Difficulty { get; set; }

        public Guid KeywordId { get; set; }
    }
}