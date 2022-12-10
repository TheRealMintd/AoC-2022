using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public partial class Day07 : BaseDay
{
	private readonly AocDir root;

	public Day07()
	{
		Stack<AocDir> stack = new();
		foreach (Match match in CommandRegex().Matches(File.ReadAllText(InputFilePath)).Cast<Match>()) {
			switch (match.Groups["command"].Value) {
				case "cd":
					string path = match.Groups["path"].Value;
					if (path == "..") {
						stack.Pop();
					} else {
						AocDir dir = new(path);
						if (stack.Count > 0) {
							stack.Peek().contents.Add(dir);
						} else {
							root = dir;
						}
						stack.Push(dir);
					}
					break;

				case "ls":
					match.Groups["output"]
						.Value
						.Split('\n')
						.Where(line => line.Length > 0)
						.ForEach(line => {
							var splitResult = line.Split(' ').ToArray();
							if (splitResult[0] != "dir") {
								stack.Peek().contents.Add(new(splitResult[1], Int32.Parse(splitResult[0])));
							}
						});
					break;

				default:
					throw new UnreachableException();
			}
		}
	}

	public override ValueTask<string> Solve_1()
	{
		int result = AccumulateSize(root).Where(size => size <= 100000).Sum();
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int currentFree = 70000000 - root.GetSize();
		int deficitSpace = 30000000 - currentFree;

		int result = AccumulateSize(root).Where(size => size >= deficitSpace).Min();
		return new(result.ToString());
	}

	private IEnumerable<int> AccumulateSize(AocDir dir) {
		var sizes = dir.contents
			.OfType<AocDir>()
			.Select(AccumulateSize)
			.Aggregate(Enumerable.Empty<int>(), (acc, list) => acc.Concat(list).ToList());

		return Enumerable.Append(sizes, dir.GetSize());
	}

	[GeneratedRegex(@"\$ (?<command>\w+)(?: (?<path>.+))?\n(?<output>[^$]*)", RegexOptions.Multiline)]
	private static partial Regex CommandRegex();
}

public class AocFile {
	public string name;
	public int size;

	public AocFile(string name, int size)
	{
		this.name = name;
		this.size = size;
	}

	public virtual int GetSize() {
		return size;
	}

	public override string ToString()
	{
		return $"{name}: {size}";
	}
}

public class AocDir : AocFile {
	public List<AocFile> contents;

	public AocDir(string name)
		: base(name, 0)
	{
		contents = new();
	}

	public override int GetSize() {
		return contents.Sum(item => item.GetSize());
	}

	public override string ToString()
	{
		return $"{name}";
	}
}
