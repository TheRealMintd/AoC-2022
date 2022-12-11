using System.Data;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day08 : BaseDay
{
	private readonly Tree[,] _input;

	public Day08()
	{
		string[] text = File.ReadAllLines(InputFilePath).Where(line => line.Length > 0).ToArray();
		_input = new Tree[text.Length, text[0].Length];

		text.Index()
			.ForEach(line => line.Value
				.ToCharArray()
				.Index()
				.ForEach(c =>
					_input[line.Key, c.Key] = new(c.Value, line.Key == 0 || line.Key == text.Length - 1 || c.Key == 0 || c.Key == text[0].Length - 1 ? VisibleState.Visible : VisibleState.Unknown)
				)
			);
	}

	public void CheckVisibility(int row, int column)
	{
		Tree tree = _input[row, column];
		if (tree.state != VisibleState.Unknown) return;

		bool top = true;
		for (int r = row - 1; r >= 0; r--) {
			if (_input[r, column].height >= tree.height) {
				top = false;
				break;
			}
		}

		if (top) {
			tree.state = VisibleState.Visible;
			return;
		}

		bool bottom = true;
		for (int r = row + 1; r < _input.GetLength(0); r++) {
			if (_input[r, column].height >= tree.height) {
				bottom = false;
				break;
			}
		}

		if (bottom) {
			tree.state = VisibleState.Visible;
			return;
		}

		bool left = true;
		for (int c = column - 1; c >= 0; c--) {
			if (_input[row, c].height >= tree.height) {
				left = false;
				break;
			}
		}

		if (left) {
			tree.state = VisibleState.Visible;
			return;
		}

		bool right = true;
		for (int c = column + 1; c < _input.GetLength(1); c++) {
			if (_input[row, c].height >= tree.height) {
				right = false;
				break;
			}
		}

		if (right) {
			tree.state = VisibleState.Visible;
			return;
		}

		tree.state = VisibleState.Invisible;
	}

	public override ValueTask<string> Solve_1()
	{
		for (int row = 0; row < _input.GetLength(0); row++)
		{
			for (int column = 0; column < _input.GetLength(1); column++)
			{
				CheckVisibility(row, column);
			}
		}

		int result = 0;
		foreach (var tree in _input)
		{
			if (tree.state == VisibleState.Visible) result++;
		}

		return new(result.ToString());
	}

	public override ValueTask<string> Solve_2()
	{
		for (int row = 0; row < _input.GetLength(0); row++) {
			for (int column = 0; column < _input.GetLength(0); column++) {
				Tree tree = _input[row, column];

				tree.score = Enumerable.Range(0, row)
					.Reverse()
					.TakeUntil(r => _input[r, column] >= tree)
					.Count();

				tree.score *= Enumerable.Range(row + 1, Math.Clamp(_input.GetLength(0) - row - 1, 0, Int32.MaxValue))
					.TakeUntil(r => _input[r, column] >= tree)
					.Count();

				tree.score *= Enumerable.Range(0, column)
					.Reverse()
					.TakeUntil(c => _input[row, c] >= tree)
					.Count();

				tree.score *= Enumerable.Range(column + 1, Math.Clamp(_input.GetLength(1) - column - 1, 0, Int32.MaxValue))
					.TakeUntil(c => _input[row, c] >= tree)
					.Count();
			}
		}

		int result = 0;
		foreach (var tree in _input) {
			if (tree.score > result) result = tree.score;
		}
		return new(result.ToString());
	}
}


public enum VisibleState
{
	Unknown,
	Invisible,
	Visible,
}

public class Tree
{
	public readonly int height;
	public int score;
	public VisibleState state;

	public Tree(char height, VisibleState state)
	{
		this.height = height - 48;
		this.state = state;
	}

	public static bool operator <=(Tree a, Tree b) => a.height <= b.height;

	public static bool operator >=(Tree a, Tree b) => a.height >= b.height;

	public override string ToString()
	{
		return $"{height} = {state}, {score}";
	}
}
