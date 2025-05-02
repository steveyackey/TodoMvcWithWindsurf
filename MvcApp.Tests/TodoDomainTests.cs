using System;
using MvcApp.Domain;
using Xunit;
using FluentAssertions;

namespace MvcApp.Tests
{
    public class TodoDomainTests
    {
        [Fact]
        public void Todo_DefaultValues_AreCorrect()
        {
            var todo = new Todo { Title = "Test" };
            Assert.Equal("Test", todo.Title);
            Assert.False(todo.IsCompleted);
            Assert.Equal(TodoPriority.Medium, todo.Priority);
            Assert.Null(todo.DueDate);
            Assert.Null(todo.Category);
        }

        [Fact]
        public void Todo_CanAssign_AllProperties()
        {
            var due = DateTime.Today.AddDays(1);
            var todo = new Todo
            {
                Title = "Test2",
                IsCompleted = true,
                CreatedAt = DateTime.Today,
                DueDate = due,
                Priority = TodoPriority.High,
                Category = "Work"
            };
            Assert.Equal("Test2", todo.Title);
            Assert.True(todo.IsCompleted);
            Assert.Equal(DateTime.Today, todo.CreatedAt);
            Assert.Equal(due, todo.DueDate);
            Assert.Equal(TodoPriority.High, todo.Priority);
            Assert.Equal("Work", todo.Category);
        }

        [Fact]
        public void Todo_WithPastDueDate_ShouldBeInvalid()
        {
            // Arrange
            var todo = new Todo
            {
                Title = "Test Past Due",
                DueDate = DateTime.Today.AddDays(-1)
            };

            // Act & Assert
            todo.IsValid().Should().BeFalse("due date should not be in the past");
        }

        [Fact]
        public void Todo_Title_IsRequired_AndMaxLength()
        {
            var todo = new Todo { Title = "" };
            todo.IsValid().Should().BeFalse("title is required");

            todo.Title = new string('a', 101);
            todo.IsValid().Should().BeFalse("title max length is 100");

            todo.Title = "Valid title";
            todo.IsValid().Should().BeTrue();
        }

        [Fact]
        public void Todo_Tags_MustBeUnique()
        {
            var todo = new Todo { Title = "Tag Test", Tags = new() { "work", "work" } };
            todo.IsValid().Should().BeFalse("tags must be unique");

            todo.Tags = new() { "work", "personal" };
            todo.IsValid().Should().BeTrue();
        }

        [Fact]
        public void Todo_Attachments_CanBeAdded()
        {
            var todo = new Todo { Title = "Attach Test" };
            todo.Attachments.Add(new TodoAttachment { FileName = "file.txt", Size = 123 });
            todo.Attachments.Should().HaveCount(1);
            todo.Attachments[0].FileName.Should().Be("file.txt");
        }

        [Fact]
        public void Todo_Priority_MustBeValidEnum()
        {
            var todo = new Todo { Title = "Enum Test", Priority = (TodoPriority)999 };
            todo.IsValid().Should().BeFalse("priority must be a valid enum value");

            todo.Priority = TodoPriority.Low;
            todo.IsValid().Should().BeTrue();
        }
    }
}
