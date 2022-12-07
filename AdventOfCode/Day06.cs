namespace AdventOfCode;

public class Day06 : BaseDay
{
	private readonly string _input;

	public Day06()
	{
		_input = File.ReadLines(InputFilePath).First();
	}

	public override ValueTask<string> Solve_1()
	{
		int result = _input.Window(4)
			.Index()
			.First(item => item.Value.Distinct().Exactly(4)).Key + 4;
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int result = _input.Window(14)
			.Index()
			.First(item => item.Value.Distinct().Exactly(14)).Key + 14;
		return new(result.ToString());
	}
}
