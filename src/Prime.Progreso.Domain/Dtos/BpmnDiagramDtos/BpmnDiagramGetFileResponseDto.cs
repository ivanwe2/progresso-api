using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.BpmnDiagramDtos
{
    public class BpmnDiagramGetFileResponseDto
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public byte[] BpmnDiagramFile { get; set; }
    }
}
