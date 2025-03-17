using ErettsegizzunkApi.Models;

namespace ErettsegizzunkApi.DTOs
{
    public class PutPermissionDTO : ParentDTO
    {
        public List<Permission> Permissions { get; set; }
    }

    public class PostPermissionDTO : ParentDTO
    {
        public Permission Permission { get; set; }
    }
}
