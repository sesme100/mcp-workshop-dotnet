namespace McpTodoServer.ContainerApp.Tools;

/// <summary>
/// Todo 항목을 나타내는 모델 클래스입니다.
/// </summary>
public class TodoItem
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
