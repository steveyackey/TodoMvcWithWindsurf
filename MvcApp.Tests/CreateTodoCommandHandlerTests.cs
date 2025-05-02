using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcApp.Domain;
using MvcApp.Data;
using MvcApp.Features.Todos.Create;
using Xunit;

namespace MvcApp.Tests
{
    public class CreateTodoCommandHandlerTests
    {
        private TodoDbContext GetDbContext()
        {
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new TodoDbContext(options);
        }

        [Fact]
        public async Task Handle_CreatesTodo_WithAllFields()
        {
            var db = GetDbContext();
            var handler = new CreateTodoHandler(db);
            var due = DateTime.UtcNow.AddDays(2);
            var cmd = new CreateTodoCommand
            {
                Title = "Test Todo",
                DueDate = due,
                Priority = Features.Todos.Create.TodoPriority.High,
                Category = "Work"
            };
            var id = await handler.Handle(cmd, CancellationToken.None);
            var todo = await db.Todos.FindAsync(id);
            Assert.NotNull(todo);
            Assert.Equal("Test Todo", todo.Title);
            Assert.Equal(due, todo.DueDate);
            Assert.Equal(Domain.TodoPriority.High, todo.Priority);
            Assert.Equal("Work", todo.Category);
        }

        [Fact]
        public async Task Handle_CreatesTodo_WithDefaults()
        {
            var db = GetDbContext();
            var handler = new CreateTodoHandler(db);
            var cmd = new CreateTodoCommand { Title = "Default Todo" };
            var id = await handler.Handle(cmd, CancellationToken.None);
            var todo = await db.Todos.FindAsync(id);
            Assert.NotNull(todo);
            Assert.Equal(Domain.TodoPriority.Medium, todo.Priority);
            Assert.Null(todo.DueDate);
            Assert.Null(todo.Category);
        }
    }
}
