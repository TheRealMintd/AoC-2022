using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class Day02 : BaseDay
{
	private readonly (Move, Move)[] _input;

	public Day02()
	{
		string text = File.ReadAllText(InputFilePath);
		_input = text.Split("\n")
			.Where(line => line.Length != 0)
			.Select(line => (line[0].ParseMove(), line[2].ParseMove()))
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		int result = _input.Select(moves => (int)moves.Item2 + 1 + moves.Play()).Sum();
		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		int result = _input.Select(moves => (moves.Item1, ((Target)moves.Item2).GetMove(moves.Item1)))
			.Select(moves => (int)moves.Item2 + 1 + moves.Play())
			.Sum();
		return new(result.ToString());
	}
}


public enum Move
{
	Rock,
	Paper,
	Scissors,
}


public enum Target
{
	Lose,
	Tie,
	Win,
}

public static class ExtensionMethods
{
	public static Move ParseMove(this char input)
	{
		return input switch
		{
			'A' or 'X' => Move.Rock,
			'B' or 'Y' => Move.Paper,
			'C' or 'Z' => Move.Scissors,
			_ => throw new UnreachableException(),
		};
	}

	public static Move GetMove(this Target target, Move move)
	{
		if (target == Target.Tie) return move;

		int mod = target == Target.Lose ? -1 : 1;
		return (Move)(((int)move + mod + 3) % 3);
	}

	public static int Play(this (Move, Move) move)
	{
		if (move.Item1 == move.Item2) return 3;

		return (move.Item1 == Move.Rock && move.Item2 == Move.Paper)
			|| (move.Item1 == Move.Paper && move.Item2 == Move.Scissors)
			|| (move.Item1 == Move.Scissors && move.Item2 == Move.Rock) ? 6 : 0;
	}
}
