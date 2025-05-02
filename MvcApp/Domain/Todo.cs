namespace MvcApp.Domain
{
    public enum TodoPriority
    {
        Low,
        Medium,
        High
    }

    public class TodoAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TodoPriority Priority { get; set; } = TodoPriority.Medium;
        public string? Category { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<TodoAttachment> Attachments { get; set; } = new();

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Title) || Title.Length > 100)
                return false;
            if (Tags.Distinct().Count() != Tags.Count)
                return false;
            if (DueDate != null && DueDate < DateTime.Today)
                return false;
            if (!Enum.IsDefined(typeof(TodoPriority), Priority))
                return false;
            return true;
        }
    }
}
