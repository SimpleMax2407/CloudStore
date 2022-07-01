using CS_DAL.Authentification;
using System;

namespace CS_DAL.Entities
{
    public class FileDatum : BaseEntity
    {
        public string UserName { get; set; }
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
