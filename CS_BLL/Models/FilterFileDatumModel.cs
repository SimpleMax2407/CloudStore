using System;
using System.Collections.Generic;
using System.Text;

namespace CS_BLL.Models
{
    public class FilterFileDatumModel
    {
        public DateTime? StartCreationDate { get; set; }
        public DateTime? EndCreationDate { get; set; }
        public DateTime? StartEditDate { get; set; }
        public DateTime? EndEditDate { get; set; }
        public string SearchString { get; set; }
    }
}
