namespace AdventOfCode;

public class Day03 : BaseDay
{
	private readonly (HashSet<char>, HashSet<char>)[] _input;

	public Day03()
	{
		string text = File.ReadAllText(InputFilePath);
		_input = text.Split("\n")
			.Where(line => line.Length != 0)
			.Select(line => {
				int mid = line.Length / 2;
				return (new HashSet<char>(line[..mid]), new HashSet<char>(line[mid..]));
			})
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		int result = _input.Select(i => ItemToPriority(i.Item1.Intersect(i.Item2).First()))
			.Sum();
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int result = _input.Select(i => new HashSet<char>(i.Item1.Union(i.Item2)))
			.Batch(3)
			.Select(i => ItemToPriority(
				i.Aggregate((a, s) => new HashSet<char>(a.Intersect(s))).First())
			)
			.Sum();
		return new(result.ToString());
	}

	private static int ItemToPriority(char item) {
		if (Enumerable.Range('A', 'Z' - 'A' + 1).Contains(item)) {
			return item - 'A' + 27;
		} else {
			return item - 'a' + 1;
		}
	}
}
