using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.RequestModels.BpmnDiagram
{
    public class BpmnDiagramUpdateRequestForm
    {
        public IFormFile BpmnDiagramFile { get; set; }
    }
}
