using MediatR;
using MvcApp.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MvcApp.Features.Todos.Complete
{
    public class CompleteTodoHandler : IRequestHandler<CompleteTodoCommand, bool>
    {
        private readonly TodoDbContext _context;
        public CompleteTodoHandler(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CompleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);
            if (todo == null) return false;
            todo.IsCompleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
