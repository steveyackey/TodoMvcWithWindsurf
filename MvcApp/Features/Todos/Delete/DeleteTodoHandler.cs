using MediatR;
using MvcApp.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MvcApp.Features.Todos.Delete
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        private readonly TodoDbContext _context;
        public DeleteTodoHandler(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);
            if (todo == null) return false;
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
