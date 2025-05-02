using MediatR;

namespace MvcApp.Features.Todos.Create
{
    public enum TodoPriority
    {
        Low,
        Medium,
        High
    }

    public class CreateTodoCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public TodoPriority Priority { get; set; } = TodoPriority.Medium;
        public string? Category { get; set; }
    }
}
