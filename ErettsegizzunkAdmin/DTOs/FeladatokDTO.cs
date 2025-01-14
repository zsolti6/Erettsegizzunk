using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErettsegizzunkAdmin.DTOs
{
    public class FeladatokDeleteDTO
    {
        public string Token { get; set; }
        public List<int> Ids { get; set; }
    }
}
