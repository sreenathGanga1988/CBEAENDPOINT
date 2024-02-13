using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class FileUploadDetail
    {

        public int Id { get; set; }

        public  string Filename { get; set; }

        public string FileDesription { get; set; }

        public string FileType { get; set; }

        public string FileLocation { get; set; }

        public String FileRelativeLocation { get; set; }
        public Boolean isPublic{ get; set; }

        public Byte[] FileContent { get; set; }


    }
}
