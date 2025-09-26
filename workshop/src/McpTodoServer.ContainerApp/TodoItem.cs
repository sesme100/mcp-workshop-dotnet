namespace McpTodoServer.ContainerApp;

/// <summary>
/// 할 일 항목 엔티티
/// </summary>
public class TodoItem
{
    /// <summary>
    /// 고유 ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 할 일 텍스트
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// 완료 여부
    /// </summary>
    public bool IsCompleted { get; set; }
}
