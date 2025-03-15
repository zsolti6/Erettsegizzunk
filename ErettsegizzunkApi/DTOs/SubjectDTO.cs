using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class SubjectDTO : ParentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class SubjectPutDTO : ParentDTO
    {        
        public List<Subject> subjects { get; set; }
    }
}
