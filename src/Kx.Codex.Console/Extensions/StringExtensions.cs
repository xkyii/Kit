using System;
using System.Linq;
using Ks.Core.Naming;

namespace Kx.Codex.Console.Extensions;

public static class StringExtensions
{
	public static string ToBeanName(this string s)
	{
		if (s.IsNullOrEmpty())
		{
			return string.Empty;
		}

		string[] splits = s.Split('_');
		if (splits.IsNullOrEmpty())
		{
			return string.Empty;
		}

		if (splits.Length == 1)
		{
			return splits[0].FirstWordUpper();
		}

		return splits.Skip(1).ToPascal();
	}
}
