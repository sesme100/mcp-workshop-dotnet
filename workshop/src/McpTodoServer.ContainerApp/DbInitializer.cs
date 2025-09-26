using Microsoft.EntityFrameworkCore;

namespace McpTodoServer.ContainerApp;

public static class DbInitializer
{
    public static void Initialize(TodoDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
