namespace ErettsegizzunkApi.DTOs
{
    public class TokenRefreshDTO : LoggedUserForCheckDTO
    {
        public string OldName { get; set; }

        public string NewName { get; set; }
    }
}
