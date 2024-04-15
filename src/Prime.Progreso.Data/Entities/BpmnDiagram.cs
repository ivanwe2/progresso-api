using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Entities
{
    public class BpmnDiagram
    {
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int AuthorId { get; set; }

        public DateTime UploadDate { get; set; }

    }

}
