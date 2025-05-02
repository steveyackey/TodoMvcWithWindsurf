using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcApp.Data;
using MvcApp.Domain;
using MvcApp.Features.Todos.List;
using Xunit;

namespace MvcApp.Tests
{
    public class GetTodosQueryHandlerTests
    {
        private TodoDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new TodoDbContext(options);
        }

        [Fact]
        public async Task Handle_ReturnsFilteredAndSortedTodos()
        {
            var db = GetDbContext();
            db.Todos.AddRange(
                new Todo { Title = "A", CreatedAt = DateTime.UtcNow.AddDays(-2), Priority = TodoPriority.Low, Category = "Home", DueDate = DateTime.UtcNow.AddDays(3) },
                new Todo { Title = "B", CreatedAt = DateTime.UtcNow.AddDays(-1), Priority = TodoPriority.High, Category = "Work", DueDate = DateTime.UtcNow.AddDays(1) },
                new Todo { Title = "C", CreatedAt = DateTime.UtcNow, Priority = TodoPriority.Medium, Category = "Work", DueDate = DateTime.UtcNow.AddDays(2) }
            );
            await db.SaveChangesAsync();
            var handler = new GetTodosHandler(db);
            var query = new GetTodosQuery { Category = "Work", Priority = TodoPriority.High, SortByDueDate = true };
            var result = await handler.Handle(query, CancellationToken.None);
            Assert.Single(result);
            Assert.Equal("B", result.First().Title);
        }

        [Fact]
        public async Task Handle_ReturnsAll_WhenNoFilters()
        {
            var db = GetDbContext();
            db.Todos.AddRange(
                new Todo { Title = "A", CreatedAt = DateTime.UtcNow },
                new Todo { Title = "B", CreatedAt = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();
            var handler = new GetTodosHandler(db);
            var query = new GetTodosQuery();
            var result = await handler.Handle(query, CancellationToken.None);
            Assert.Equal(2, result.Count);
        }
    }
}
