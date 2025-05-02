using System.Collections.Generic;
using MvcApp.Domain;

namespace MvcApp.Features.Todos.List
{
    public class TodoListViewModel
    {
        public List<Todo> Todos { get; set; } = new();
    }
}
