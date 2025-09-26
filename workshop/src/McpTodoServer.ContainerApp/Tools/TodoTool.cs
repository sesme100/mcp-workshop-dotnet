
using System;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using System.ComponentModel;

#if DEBUG
public class TodoToolTest
{
    public static async Task Main()
    {
        var tool = new TodoTool();

        // 1. 할 일 목록 조회
        var list = await tool.ListAsync();
        Console.WriteLine("[초기 할 일 목록]");
        foreach (var t in list)
            Console.WriteLine($"{t.Id}: {t.Text} (완료: {t.IsCompleted})");

        // 2. '오후 12시 점심' 추가
        var lunch = await tool.CreateAsync("오후 12시 점심");
        Console.WriteLine($"\n[추가됨] {lunch.Id}: {lunch.Text}");

        // 3. 최종 목록 출력
        var list2 = await tool.ListAsync();
        Console.WriteLine("\n[최종 할 일 목록]");
        foreach (var t in list2)
            Console.WriteLine($"{t.Id}: {t.Text} (완료: {t.IsCompleted})");
    }
}
#endif

namespace McpTodoServer.ContainerApp.Tools;

/// <summary>
/// 할 일(Todo) 관련 기능을 제공하는 도구 클래스입니다.
/// </summary>
/// 
// 👇👇👇 제거: [McpServerToolType] 특성은 클래스에 붙일 수 없음 👇👇👇
// 👆👆👆 제거 👆👆👆
public class TodoTool
{
    private static List<TodoItem> _todos = new List<TodoItem>
    {
        new TodoItem { Id = 1, Text = "샘플 할 일 1", IsCompleted = false },
        new TodoItem { Id = 2, Text = "샘플 할 일 2", IsCompleted = true },
        new TodoItem { Id = 3, Text = "오후 12시 점심", IsCompleted = false }
    };
    private static int _nextId = 4;

    public async Task<TodoItem> CreateAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            text = $"샘플 할 일 {_nextId}";
        }
        var todo = new TodoItem { Id = _nextId++, Text = text, IsCompleted = false };
        _todos.Add(todo);
        return await Task.FromResult(todo);
    }

    // 👇👇👇 추가 👇👇👇
    [McpServerTool(Name = "get_todo_items", Title = "Get a list of to-do items")]
    [Description("Gets a list of to-do items from database.")]
    // 👆👆👆 추가 👆👆👆
    public async Task<List<TodoItem>> ListAsync()
    {
        if (_todos.Count == 0)
        {
            _todos.Add(new TodoItem { Id = _nextId++, Text = "샘플 할 일", IsCompleted = false });
        }
        return await Task.FromResult(_todos.ToList());
    }

    // 👇👇👇 추가 👇👇👇
    //[McpServerTool(Name = "update_todo_item", Title = "Update a to-do item")]
    //[Description("Updates a to-do item in the database.")]
    // 👆👆👆 추가 👆👆👆
   

    

    // 👇👇👇 추가 👇👇👇
    [McpServerTool(Name = "complete_todo_item", Title = "Complete a to-do item")]
    [Description("Completes a to-do item in the database.")]
    // 👆👆👆 추가 👆👆👆
    public async Task<TodoItem?> CompleteAsync(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            // 샘플 데이터 생성 및 반환
            todo = new TodoItem { Id = id, Text = $"샘플 할 일 {id}", IsCompleted = true };
            _todos.Add(todo);
            return await Task.FromResult(todo);
        }
        todo.IsCompleted = true;
        return await Task.FromResult(todo);
    }

    // 👇👇👇 추가 👇👇👇
    [McpServerTool(Name = "delete_todo_item", Title = "Delete a to-do item")]
    [Description("Deletes a to-do item from the database.")]
    // 👆👆👆 추가 👆👆👆
    public async Task<bool> DeleteAsync(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            // 샘플 데이터 삭제 성공으로 처리
            return await Task.FromResult(true);
        }
        _todos.Remove(todo);
        return await Task.FromResult(true);
    }
}
