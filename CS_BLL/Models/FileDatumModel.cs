using System;
using System.Collections.Generic;
using System.Text;

namespace CS_BLL.Models
{
    public class FileDatumModel
    {
        public string UserName { get; set; }
        public string FileName { get; set; }
        public long? Size { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
