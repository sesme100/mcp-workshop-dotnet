using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

/// <summary>
/// 원숭이 데이터 관리를 위한 정적 헬퍼 클래스입니다.
/// </summary>
public static class MonkeyHelper
{
    private static List<Monkey>? monkeys;
    private static readonly Dictionary<string, int> randomAccessCounts = new();
    private static readonly object lockObj = new();

    /// <summary>
    /// MCP 서버에서 원숭이 데이터를 비동기로 가져옵니다.
    /// </summary>
    public static async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (monkeys != null)
            return monkeys;

        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync("https://monkeyapi.azurewebsites.net/api/monkeys");
        monkeys = JsonSerializer.Deserialize<List<Monkey>>(response) ?? new List<Monkey>();
        return monkeys;
    }

    /// <summary>
    /// 모든 원숭이 목록을 반환합니다.
    /// </summary>
    public static async Task<IEnumerable<Monkey>> GetAllMonkeysAsync()
    {
        return await GetMonkeysAsync();
    }

    /// <summary>
    /// 이름으로 원숭이를 찾습니다.
    /// </summary>
    public static async Task<Monkey?> GetMonkeyByNameAsync(string name)
    {
        var list = await GetMonkeysAsync();
        return list.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 무작위 원숭이를 반환하고, 해당 원숭이의 액세스 횟수를 1 증가시킵니다.
    /// </summary>
    public static async Task<Monkey?> GetRandomMonkeyAsync()
    {
        var list = await GetMonkeysAsync();
        if (!list.Any()) return null;
        var random = new Random();
        var monkey = list[random.Next(list.Count)];
        lock (lockObj)
        {
            if (!randomAccessCounts.ContainsKey(monkey.Name))
                randomAccessCounts[monkey.Name] = 0;
            randomAccessCounts[monkey.Name]++;
        }
        return monkey;
    }

    /// <summary>
    /// 무작위로 선택된 원숭이의 액세스 횟수를 반환합니다.
    /// </summary>
    public static int GetRandomAccessCount(string name)
    {
        lock (lockObj)
        {
            return randomAccessCounts.TryGetValue(name, out var count) ? count : 0;
        }
    }
}
