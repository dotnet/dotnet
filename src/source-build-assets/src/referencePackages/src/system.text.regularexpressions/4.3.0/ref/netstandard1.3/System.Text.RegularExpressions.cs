// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Text.RegularExpressions")]
[assembly: System.Reflection.AssemblyDescription("System.Text.RegularExpressions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.RegularExpressions")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.23123.00")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.23123.00 built by: PROJECTKREL")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("", "")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text.RegularExpressions
{
    public partial class Capture
    {
        internal Capture() { }

        public int Index { get { throw null; } }

        public int Length { get { throw null; } }

        public string Value { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public partial class CaptureCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal CaptureCollection() { }

        public int Count { get { throw null; } }

        public Capture this[int i] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int arrayIndex) { }
    }

    public partial class Group : Capture
    {
        internal Group() { }

        public CaptureCollection Captures { get { throw null; } }

        public bool Success { get { throw null; } }
    }

    public partial class GroupCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal GroupCollection() { }

        public int Count { get { throw null; } }

        public Group this[int groupnum] { get { throw null; } }

        public Group this[string groupname] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int arrayIndex) { }
    }

    public partial class Match : Group
    {
        internal Match() { }

        public static Match Empty { get { throw null; } }

        public virtual GroupCollection Groups { get { throw null; } }

        public Match NextMatch() { throw null; }

        public virtual string Result(string replacement) { throw null; }
    }

    public partial class MatchCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal MatchCollection() { }

        public int Count { get { throw null; } }

        public virtual Match this[int i] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int arrayIndex) { }
    }

    public delegate string MatchEvaluator(Match match);
    public partial class Regex
    {
        public static readonly TimeSpan InfiniteMatchTimeout;
        protected Regex() { }

        public Regex(string pattern, RegexOptions options, TimeSpan matchTimeout) { }

        public Regex(string pattern, RegexOptions options) { }

        public Regex(string pattern) { }

        public static int CacheSize { get { throw null; } set { } }

        public TimeSpan MatchTimeout { get { throw null; } }

        public RegexOptions Options { get { throw null; } }

        public bool RightToLeft { get { throw null; } }

        public static string Escape(string str) { throw null; }

        public string[] GetGroupNames() { throw null; }

        public int[] GetGroupNumbers() { throw null; }

        public string GroupNameFromNumber(int i) { throw null; }

        public int GroupNumberFromName(string name) { throw null; }

        public bool IsMatch(string input, int startat) { throw null; }

        public static bool IsMatch(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static bool IsMatch(string input, string pattern, RegexOptions options) { throw null; }

        public static bool IsMatch(string input, string pattern) { throw null; }

        public bool IsMatch(string input) { throw null; }

        public Match Match(string input, int beginning, int length) { throw null; }

        public Match Match(string input, int startat) { throw null; }

        public static Match Match(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static Match Match(string input, string pattern, RegexOptions options) { throw null; }

        public static Match Match(string input, string pattern) { throw null; }

        public Match Match(string input) { throw null; }

        public MatchCollection Matches(string input, int startat) { throw null; }

        public static MatchCollection Matches(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static MatchCollection Matches(string input, string pattern, RegexOptions options) { throw null; }

        public static MatchCollection Matches(string input, string pattern) { throw null; }

        public MatchCollection Matches(string input) { throw null; }

        public string Replace(string input, string replacement, int count, int startat) { throw null; }

        public string Replace(string input, string replacement, int count) { throw null; }

        public static string Replace(string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static string Replace(string input, string pattern, string replacement, RegexOptions options) { throw null; }

        public static string Replace(string input, string pattern, string replacement) { throw null; }

        public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options) { throw null; }

        public static string Replace(string input, string pattern, MatchEvaluator evaluator) { throw null; }

        public string Replace(string input, string replacement) { throw null; }

        public string Replace(string input, MatchEvaluator evaluator, int count, int startat) { throw null; }

        public string Replace(string input, MatchEvaluator evaluator, int count) { throw null; }

        public string Replace(string input, MatchEvaluator evaluator) { throw null; }

        public string[] Split(string input, int count, int startat) { throw null; }

        public string[] Split(string input, int count) { throw null; }

        public static string[] Split(string input, string pattern, RegexOptions options, TimeSpan matchTimeout) { throw null; }

        public static string[] Split(string input, string pattern, RegexOptions options) { throw null; }

        public static string[] Split(string input, string pattern) { throw null; }

        public string[] Split(string input) { throw null; }

        public override string ToString() { throw null; }

        public static string Unescape(string str) { throw null; }
    }

    public partial class RegexMatchTimeoutException : TimeoutException
    {
        public RegexMatchTimeoutException() { }

        public RegexMatchTimeoutException(string message, Exception inner) { }

        public RegexMatchTimeoutException(string regexInput, string regexPattern, TimeSpan matchTimeout) { }

        public RegexMatchTimeoutException(string message) { }

        public string Input { get { throw null; } }

        public TimeSpan MatchTimeout { get { throw null; } }

        public string Pattern { get { throw null; } }
    }

    [Flags]
    public enum RegexOptions
    {
        None = 0,
        IgnoreCase = 1,
        Multiline = 2,
        ExplicitCapture = 4,
        Compiled = 8,
        Singleline = 16,
        IgnorePatternWhitespace = 32,
        RightToLeft = 64,
        ECMAScript = 256,
        CultureInvariant = 512
    }
}