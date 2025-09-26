
using System;
using System.Threading.Tasks;

class Program
{
	private static readonly string[] AsciiArts = new[]
	{
		@"  (o o)
 (  V  )
/--m-m-",
		@"   w  c( .. )o   ( - )
	\\__(  -   )    (   )
		",
		@"   (o.o)
  <(   )>
   ^^ ^^",
		@"   ("""")
  ( o o )
 (  V  )
/--m-m-",
		@"   (o o)
  (  V  )
 /--m-m-
  Monkey!"
	};

	static async Task Main()
	{
		while (true)
		{
			Console.Clear();
			PrintRandomAsciiArt();
			Console.WriteLine("============================");
			Console.WriteLine(" Monkey Console App");
			Console.WriteLine("============================");
			Console.WriteLine("1. 모든 원숭이 나열");
			Console.WriteLine("2. 이름으로 특정 원숭이의 세부 정보 가져오기");
			Console.WriteLine("3. 무작위 원숭이 가져오기");
			Console.WriteLine("4. 앱 종료");
			Console.Write("메뉴를 선택하세요: ");
			var input = Console.ReadLine();
			Console.WriteLine();

			switch (input)
			{
				case "1":
					await ListAllMonkeys();
					break;
				case "2":
					await ShowMonkeyByName();
					break;
				case "3":
					await ShowRandomMonkey();
					break;
				case "4":
					Console.WriteLine("앱을 종료합니다.");
					return;
				default:
					Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
					break;
			}
			Console.WriteLine("\n아무 키나 누르면 계속합니다...");
			Console.ReadKey();
		}
	}

	static void PrintRandomAsciiArt()
	{
		var rand = new Random();
		var art = AsciiArts[rand.Next(AsciiArts.Length)];
		Console.WriteLine(art);
		Console.WriteLine();
	}

	static async Task ListAllMonkeys()
	{
		var monkeys = await MonkeyHelper.GetAllMonkeysAsync();
		Console.WriteLine("이름                | 위치                | 개체수");
		Console.WriteLine("---------------------------------------------------");
		foreach (var m in monkeys)
		{
			Console.WriteLine($"{m.Name,-20} | {m.Location,-20} | {m.Population}");
		}
	}

	static async Task ShowMonkeyByName()
	{
		Console.Write("원숭이 이름을 입력하세요: ");
		var name = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(name))
		{
			Console.WriteLine("이름을 입력해야 합니다.");
			return;
		}
		var monkey = await MonkeyHelper.GetMonkeyByNameAsync(name);
		if (monkey == null)
		{
			Console.WriteLine("해당 이름의 원숭이를 찾을 수 없습니다.");
			return;
		}
		PrintRandomAsciiArt();
		Console.WriteLine($"이름: {monkey.Name}");
		Console.WriteLine($"위치: {monkey.Location}");
		Console.WriteLine($"개체수: {monkey.Population}");
		Console.WriteLine($"설명: {monkey.Details}");
		if (!string.IsNullOrEmpty(monkey.Image))
			Console.WriteLine($"이미지: {monkey.Image}");
	}

	static async Task ShowRandomMonkey()
	{
		var monkey = await MonkeyHelper.GetRandomMonkeyAsync();
		if (monkey == null)
		{
			Console.WriteLine("원숭이 데이터가 없습니다.");
			return;
		}
		PrintRandomAsciiArt();
		Console.WriteLine($"이름: {monkey.Name}");
		Console.WriteLine($"위치: {monkey.Location}");
		Console.WriteLine($"개체수: {monkey.Population}");
		Console.WriteLine($"설명: {monkey.Details}");
		if (!string.IsNullOrEmpty(monkey.Image))
			Console.WriteLine($"이미지: {monkey.Image}");
		var count = MonkeyHelper.GetRandomAccessCount(monkey.Name);
		Console.WriteLine($"이 원숭이는 무작위로 {count}번 선택되었습니다.");
	}
}
