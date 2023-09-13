namespace TodoBackend.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
        public string Status { get; set; } = String.Empty;
        public int ListId { get; set; }

    }
}
