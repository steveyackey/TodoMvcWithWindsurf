using MediatR;
using System.Collections.Generic;
using MvcApp.Domain;

namespace MvcApp.Features.Todos.List
{
    public class GetTodosQuery : IRequest<List<Todo>>
    {
        public string? Category { get; set; }
        public TodoPriority? Priority { get; set; }
        public bool? SortByDueDate { get; set; }
    }
}
