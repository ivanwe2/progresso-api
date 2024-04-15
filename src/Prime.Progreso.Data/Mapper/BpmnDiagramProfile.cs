using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Dtos.BpmnDiagramDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Mapper
{
    public class BpmnDiagramProfile : Profile
    {
        public BpmnDiagramProfile()
        {
            CreateMap<BpmnDiagramCreateRequestDto, BpmnDiagram>()
                .ForMember(
                    dest => dest.FileName, opt => opt.MapFrom(src => src.FileName + ".xml"))
                .ForMember(
                    dest => dest.UploadDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<BpmnDiagram, BpmnDiagramGetFileResponseDto>();
            CreateMap<BpmnDiagram, BpmnDiagramGetMetadataResponseDto>();
        }
    }
}
