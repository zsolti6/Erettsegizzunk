using ErettsegizzunkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErettsegizzunkAdmin.DTOs
{
    public class TantargyDTO
    {
        public int Id { get; set; } = -1;

        public string Name { get; set; } = null!;

        public string Token { get; set; } = null!;
    }

    public class TantargyDeleteDTO
    {
        public string Token { get; set; } = null!;

        public List<int> Ids { get; set; }
    }

    public class TantargyPutDTO
    {
        public string Token { get; set; } = null!;
        public List<Subject> subjects { get; set; }
    }
}
