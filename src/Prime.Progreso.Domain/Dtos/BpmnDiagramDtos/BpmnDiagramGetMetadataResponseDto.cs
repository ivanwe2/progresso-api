﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.BpmnDiagramDtos
{
    public class BpmnDiagramGetMetadataResponseDto
    {
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public int AuthorId { get; set; }

        public DateTime UploadDate { get; set; }

    }
}
