
using Microsoft.EntityFrameworkCore;

namespace McpTodoServer.ContainerApp;

public class TodoDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
}
