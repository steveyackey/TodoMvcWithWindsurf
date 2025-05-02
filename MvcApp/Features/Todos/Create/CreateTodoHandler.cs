using MediatR;
using MvcApp.Data;
using MvcApp.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvcApp.Features.Todos.Create
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, int>
    {
        private readonly TodoDbContext _context;
        public CreateTodoHandler(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = new Todo
            {
                Title = request.Title,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false,
                DueDate = request.DueDate,
                Priority = (Domain.TodoPriority)request.Priority,
                Category = request.Category
            };
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync(cancellationToken);
            return todo.Id;
        }
    }
}
