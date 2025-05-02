using MediatR;

namespace MvcApp.Features.Todos.Complete
{
    public class CompleteTodoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
