using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public partial class Day05 : BaseDay
{
	private readonly Stack<char>[] stacks;
	private readonly Instruction[] instructions;

	public Day05()
	{
		string text = File.ReadAllText(InputFilePath);
		var lines = text.Split("\n")
			.Where(line => line.Length != 0)
			.ToArray();

		stacks = lines[..8].Reverse()
			.Aggregate(Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray(),
				(acc, line) => {
					for (int i = 1; i < line.Length; i += 4) {
						if (line[i] != ' ') acc[i / 4].Push(line[i]);
					}
					return acc;
			});

		instructions = lines[9..].Select(line => {
				int[] matches = InstructionRegex()
					.Matches(line)[0]
					.Groups
					.Values
					.Skip(1)
					.Select(group => Int32.Parse(group.Value))
					.ToArray();
				return new Instruction(matches[0], matches[1], matches[2]);
			})
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		// foreach (Instruction instruction in instructions) {
		// 	for (int i = 0; i < instruction.amount; i++) {
		// 		stacks[instruction.destination].Push(stacks[instruction.source].Pop());
		// 	}
		// }

		// return new(string.Join("", stacks.Select(stack => stack.Peek())));

		return new("Uncomment me");
	}

	public override ValueTask<string> Solve_2()
	{
		foreach (Instruction instruction in instructions) {
			var temp = new List<char>();
			for (int i = 0; i < instruction.amount; i++) {
				temp.Add(stacks[instruction.source].Pop());
			}
			temp.AsEnumerable()
				.Reverse()
				.ForEach(crate => stacks[instruction.destination].Push(crate));
		}
		return new(string.Join("", stacks.Select(stack => stack.Peek())));
	}

	[GeneratedRegex(@".+ (\d+) .+ (\d+) .+ (\d+)")]
	private static partial Regex InstructionRegex();
}

public struct Instruction {
	public readonly int amount;
	public readonly int source;
	public readonly int destination;

	public Instruction(int amount, int source, int destination)
	{
		this.amount = amount;
		this.source = source - 1;
		this.destination = destination - 1;
	}
}
