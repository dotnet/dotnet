// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net9.0 --filter Microsoft.IdentityModel.Benchmarks.LogSanitizationBenchmarks*

    [MemoryDiagnoser]
    public class LogSanitizationBenchmarks
    {
        // Test strings with varying characteristics
        private const string NoSpecialChars = "This is a normal log message with no special characters at all";
        private const string WithCommonControlChars = "Log entry\r\nwith\ttabs\rand\nnewlines";
        private const string WithUnicodeFormatChars = "Text with\u200Bzero\u200Cwidth\u200Dcharacters\uFEFFand\u202Adirectional\u202Emarks";
        private const string WithMixedChars = "Mixed: normal\r\ntext\twith\u200Bformat\u0000control\u007Fchars";
        private const string LongStringNoSpecialChars = "This is a much longer log message that contains no special characters but is long enough to test performance with larger strings. " +
            "It includes multiple sentences and spans several lines when formatted, but contains only normal ASCII characters that do not need sanitization. " +
            "The purpose is to measure the overhead of checking for special characters when none are present.";
        private const string LongStringWithSparseChars = "This is a much longer log message\r\nwith occasional\tspecial characters\u200Bscattered throughout the text. " +
            "It includes multiple\nsentences and spans\rseveral lines, testing\u200Cthe performance\u200Dwhen special characters\uFEFFare rare but present.";

        [Benchmark(Baseline = true)]
        public void Sanitize_NoSpecialChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(NoSpecialChars));
        }

        [Benchmark]
        public void Sanitize_WithCommonControlChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(WithCommonControlChars));
        }

        [Benchmark]
        public void Sanitize_WithUnicodeFormatChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(WithUnicodeFormatChars));
        }

        [Benchmark]
        public void Sanitize_WithMixedChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(WithMixedChars));
        }

        [Benchmark]
        public void Sanitize_LongStringNoSpecialChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(LongStringNoSpecialChars));
        }

        [Benchmark]
        public void Sanitize_LongStringWithSparseChars()
        {
            LogHelper.FormatInvariant("Message: {0}", LogHelper.MarkAsNonPII(LongStringWithSparseChars));
        }
    }
}
