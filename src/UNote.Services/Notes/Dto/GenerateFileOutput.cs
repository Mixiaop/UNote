using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNote.Services.Notes.Dto
{
    public class GenerateFileOutput : U.Application.Services.Dto.IOutputDto
    {
        public string Url { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }
    }
}
