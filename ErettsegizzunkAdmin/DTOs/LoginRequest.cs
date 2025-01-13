using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErettsegizzunkAdmin.DTOs
{
    class LoginRequest
    {
        public string LoginName { get; set; }
        public string TmpHash { get; set; }
    }
}
