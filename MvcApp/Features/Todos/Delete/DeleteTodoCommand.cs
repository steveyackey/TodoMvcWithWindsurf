using MediatR;

namespace MvcApp.Features.Todos.Delete
{
    public class DeleteTodoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
