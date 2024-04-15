using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.RequestModels.BpmnDiagram
{
    public class BpmnDiagramCreateRequestForm
    {
        public string FileName { get; set; }

        public IFormFile BpmnDiagramFile { get; set; }
    }
}
