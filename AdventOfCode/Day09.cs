using System.Diagnostics;

namespace AdventOfCode;

public class Day09 : BaseDay
{
	private readonly Movement[] input;

	public Day09()
	{
		input = File.ReadAllLines(InputFilePath)
			.Select(line => new Movement(line[0], line[2..]))
			.ToArray();
	}

	public override ValueTask<string> Solve_1()
	{
		(int, int) head = (0, 0);
		(int, int) tail = (0, 0);
		HashSet<(int, int)> visitedPositions = new() { (0, 0) };

		foreach (var move in input)
		{
			for (int i = 0; i < move.magnitude; i++)
			{
				head = move.direction switch
				{
					Direction.Up => (head.Item1, head.Item2 + 1),
					Direction.Down => (head.Item1, head.Item2 - 1),
					Direction.Left => (head.Item1 - 1, head.Item2),
					Direction.Right => (head.Item1 + 1, head.Item2),
				};
				tail = FollowHead(head, tail);
				visitedPositions.Add(tail);
			}
		}

		return new(visitedPositions.Count.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		(int, int)[] knots = new (int, int)[10];
		Array.Fill(knots, (0, 0));
		HashSet<(int, int)> visitedPositions = new() { (0, 0) };

		foreach (var move in input)
		{
			for (int i = 0; i < move.magnitude; i++)
			{
				var head = knots[0];
				knots[0] = move.direction switch
				{
					Direction.Up => (head.Item1, head.Item2 + 1),
					Direction.Down => (head.Item1, head.Item2 - 1),
					Direction.Left => (head.Item1 - 1, head.Item2),
					Direction.Right => (head.Item1 + 1, head.Item2),
				};

				for (int k = 1; k < knots.Length; k++)
				{
					knots[k] = FollowHead(knots[k - 1], knots[k]);
				}

				visitedPositions.Add(knots[9]);
			}
		}

		return new(visitedPositions.Count.ToString());
	}

	public static (int, int) FollowHead((int, int) head, (int, int) tail)
	{
		if (Math.Abs(head.Item1 - tail.Item1) <= 1 && Math.Abs(head.Item2 - tail.Item2) <= 1) return tail;

		if (head.Item1 == tail.Item1)
		{
			return (head.Item1, tail.Item2 + head.Item2 - tail.Item2 + (head.Item2 < tail.Item2 ? 1 : -1));
		}
		else if (head.Item2 == tail.Item2)
		{
			return (tail.Item1 + head.Item1 - tail.Item1 + (head.Item1 < tail.Item1 ? 1 : -1), head.Item2);
		}
		else
		{
			return (tail.Item1 + (head.Item1 < tail.Item1 ? -1 : 1), tail.Item2 + (head.Item2 < tail.Item2 ? -1 : 1));
		}
	}
}


enum Direction
{
	Up,
	Down,
	Left,
	Right,
}

struct Movement
{
	public readonly Direction direction;
	public readonly int magnitude;

	public Movement(char direction, string magnitude)
	{
		this.direction = direction switch
		{
			'U' => Direction.Up,
			'D' => Direction.Down,
			'L' => Direction.Left,
			'R' => Direction.Right,
			_ => throw new UnreachableException(),
		};
		this.magnitude = Int32.Parse(magnitude);
	}
}
