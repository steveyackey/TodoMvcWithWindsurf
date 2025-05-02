using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcApp.Data;
using MvcApp.Domain;

namespace MvcApp.Features.Todos.List
{
    public class GetTodosHandler : IRequestHandler<GetTodosQuery, List<Todo>>
    {
        private readonly TodoDbContext _context;
        public GetTodosHandler(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Todos.AsQueryable();
            if (!string.IsNullOrEmpty(request.Category))
                query = query.Where(t => t.Category == request.Category);
            if (request.Priority.HasValue)
                query = query.Where(t => t.Priority == request.Priority);
            if (request.SortByDueDate == true)
                query = query.OrderBy(t => t.DueDate);
            else
                query = query.OrderByDescending(t => t.CreatedAt);
            return await query.ToListAsync(cancellationToken);
        }
    }
}
