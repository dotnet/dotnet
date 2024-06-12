#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xunit.Internal
{
	// Adapted from ExceptionUtility (xunit.v3.common) and StackFrameTransformer (xunit.v3.runner.common)
	internal static class ExceptionUtility
	{
		static readonly Regex transformRegex;

		static ExceptionUtility()
		{
			transformRegex = new Regex(@"^\s*at (?<method>.*) in (?<file>.*):(line )?(?<line>\d+)$");
		}

		static bool FilterStackFrame(string stackFrame)
		{
			Assert.GuardArgumentNotNull(nameof(stackFrame), stackFrame);

#if DEBUG
			return false;
#else
			return stackFrame.StartsWith("at Xunit.", StringComparison.Ordinal);
#endif
		}

#if XUNIT_NULLABLE
		public static string? FilterStackTrace(string? stack)
#else
		public static string FilterStackTrace(string stack)
#endif
		{
			if (stack == null)
				return null;

			var results = new List<string>();

			foreach (var line in stack.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
			{
				var trimmedLine = line.TrimStart();
				if (!FilterStackFrame(trimmedLine))
					results.Add(line);
			}

			return string.Join(Environment.NewLine, results.ToArray());
		}

#if XUNIT_NULLABLE
		public static string? TransformStackFrame(
			string? stackFrame,
#else
		public static string TransformStackFrame(
			string stackFrame,
#endif
			string indent = "")
		{
			if (stackFrame == null)
				return null;

			var match = transformRegex.Match(stackFrame);
			if (match == Match.Empty)
				return stackFrame;

			var file = match.Groups["file"].Value;
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}({2},0): at {3}", indent, file, match.Groups["line"].Value, match.Groups["method"].Value);
		}

#if XUNIT_NULLABLE
		public static string? TransformStackTrace(
			string? stack,
#else
		public static string TransformStackTrace(
			string stack,
#endif
			string indent = "")
		{
			if (stack == null)
				return null;

			return string.Join(
				Environment.NewLine,
				stack
					.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
					.Select(frame => TransformStackFrame(frame, indent))
					.ToArray()
			);
		}
	}
}
