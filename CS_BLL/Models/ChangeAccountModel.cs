using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_BLL.Models
{
    public class ChangeAccountModel
    {
        public string UserName { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? NewEmail { get; set; }
    }
}
