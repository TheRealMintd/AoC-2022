namespace AdventOfCode;

public class Day04 : BaseDay
{
	private readonly (HashSet<int>, HashSet<int>)[] _input;

	public Day04()
	{
		string text = File.ReadAllText(InputFilePath);
		_input = text.Split("\n")
			.Where(line => line.Length != 0)
			.Select(line => {
				var ranges = line.Split(',').Select(range => {
					int[] startAndEnd = range.Split('-').Select(Int32.Parse).ToArray();
					return new HashSet<int>(Enumerable.Range(startAndEnd[0], startAndEnd[1] - startAndEnd[0] + 1));
				})
				.ToArray();
				return (ranges[0], ranges[1]);
			})
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		int result = _input.Select(w => {
				(var first, var second) = w;
				return first.IsSubsetOf(second) || second.IsSubsetOf(first);
			})
			.Count(subset => subset);
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int result = _input.Select(w => {
				(var first, var second) = w;
				return first.Intersect(second).Any();
			})
			.Count(overlap => overlap);
		return new(result.ToString());
	}
}
