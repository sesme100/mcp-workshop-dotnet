using Microsoft.EntityFrameworkCore;

namespace McpTodoServer.ContainerApp;

/// <summary>
/// 할 일 목록 관리 서비스
/// </summary>
public class TodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> CreateAsync(string text)
    {
        var item = new TodoItem { Text = text, IsCompleted = false };
        await _context.TodoItems.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<List<TodoItem>> ListAsync()
    {
        return await _context.TodoItems.AsNoTracking().ToListAsync();
    }

    public async Task<bool> UpdateAsync(int id, string newText)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return false;
        item.Text = newText;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return false;
        item.IsCompleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return false;
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
