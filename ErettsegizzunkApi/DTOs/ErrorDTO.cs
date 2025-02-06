namespace ErettsegizzunkApi.DTOs
{
    public class ErrorDTO : Exception
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
