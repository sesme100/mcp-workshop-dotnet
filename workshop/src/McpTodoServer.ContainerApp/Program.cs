
using Microsoft.EntityFrameworkCore;
using McpTodoServer.ContainerApp;

var builder = WebApplication.CreateBuilder(args);

// SQLite In-Memory 데이터베이스 구성
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite("Data Source=:memory:"));

// 서비스 등록
builder.Services.AddScoped<TodoService>();


// 👇👇👇 추가 👇👇👇
builder.Services.AddMcpServer()
                .WithHttpTransport(o => o.Stateless = true)
                .WithToolsFromAssembly();
// 👆👆👆 추가 👆👆👆

var app = builder.Build();

// DB 초기화 및 In-Memory DB 연결 유지
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.OpenConnection();
    DbInitializer.Initialize(db);
}

// API 엔드포인트는 구현하지 않음
// 👇👇👇 추가 👇👇👇
app.MapMcp("/mcp");
// 👆👆👆 추가 👆👆👆
app.Run();
