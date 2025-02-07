namespace ErettsegizzunkApi.DTOs
{
    public class ErrorDTO : Exception
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public ErrorDTO(int id, string message)
        {
            Id = id;
            Message = message;
        }

        public ErrorDTO(string message)
        {
            Message = message;
        }

        public ErrorDTO() { }

        public override string ToString()
        {
            return $"Hiba történt\n\nKód:\t{Id} \nÜzenet:\t{Message}";
        }
    }
}
