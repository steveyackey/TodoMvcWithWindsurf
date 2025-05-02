using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Features.Todos.List;

namespace MvcApp.Controllers
{
    public class TodosController : Controller
    {
        private readonly IMediator _mediator;
        public TodosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var todos = await _mediator.Send(new GetTodosQuery());
            var vm = new TodoListViewModel { Todos = todos };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string title, DateTime? dueDate, string priority, string category)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var parsedPriority = MvcApp.Domain.TodoPriority.Medium;
                if (!string.IsNullOrEmpty(priority))
                    Enum.TryParse(priority, true, out parsedPriority);
                await _mediator.Send(new MvcApp.Features.Todos.Create.CreateTodoCommand {
                    Title = title,
                    DueDate = dueDate,
                    Priority = (MvcApp.Features.Todos.Create.TodoPriority)parsedPriority,
                    Category = category
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            await _mediator.Send(new MvcApp.Features.Todos.Complete.CompleteTodoCommand { Id = id });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new MvcApp.Features.Todos.Delete.DeleteTodoCommand { Id = id });
            return RedirectToAction("Index");
        }
    }
}
