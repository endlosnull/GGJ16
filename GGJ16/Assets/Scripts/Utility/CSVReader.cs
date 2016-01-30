using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

public static class CSVReader
{
	public static string ToString(string[,] grid)
	{
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < grid.GetUpperBound(1); ++y)
		{
			for (int x = 0; x < grid.GetUpperBound(0); ++x)
			{
				sb.Append(grid[x, y]);
				sb.Append("|");
			}
			sb.AppendLine();
		}
		return sb.ToString();
	}

	public static string[,] SplitCSV(string text)
	{
		string[] lines = text.Split("\n"[0]);
		List<string[]> rows = new List<string[]>();
		int width = 0;
		for (int i = 0; i < lines.Length; ++i)
		{
			string[] row = SplitCSVLine(lines[i]);
			rows.Add(row);
			width = Mathf.Max(width, row.Length);
		}
		string[,] grid = new string[width + 1, rows.Count + 1];
		for (int y = 0; y < rows.Count; ++y)
		{
			string[] row = rows[y];
			for (int x = 0; x < row.Length; ++x)
			{
				grid[x, y] = row[x];
				grid[x, y] = grid[x, y].Replace("\"\"", "\"");
			}
		}
		return grid;
	}

	static string[] SplitCSVLine(string line)
	{
		return (from Match m in Regex.Matches(line,
			@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
			RegexOptions.ExplicitCapture)
			select m.Groups[1].Value).ToArray();
	}
}
