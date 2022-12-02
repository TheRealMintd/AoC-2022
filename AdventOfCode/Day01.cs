namespace AdventOfCode;

public class Day01 : BaseDay
{
	private readonly int[][] _input;

	public Day01()
	{
		string text = File.ReadAllText(InputFilePath);
		string[] inventories = text.Split("\n\n");
		_input = inventories.Select(inv => inv.Trim().Split('\n').Select(Int32.Parse).ToArray())
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		int result = _input.Select(inv => inv.Sum()).Max();
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int result = _input.Select(inv => inv.Sum()).OrderDescending().Take(3).Sum();
		return new(result.ToString());
	}
}
