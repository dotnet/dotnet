// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Primitives")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Primitives shared by framework extensions. Commonly used types include:\r\n\r\nCommonly Used Types:\r\nMicrosoft.Extensions.Primitives.IChangeToken\r\nMicrosoft.Extensions.Primitives.StringValues\r\nMicrosoft.Extensions.Primitives.StringSegment")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Primitives")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Primitives
{
    public partial class CancellationChangeToken : IChangeToken
    {
        public CancellationChangeToken(System.Threading.CancellationToken cancellationToken) { }

        public bool ActiveChangeCallbacks { get { throw null; } }

        public bool HasChanged { get { throw null; } }

        public System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state) { throw null; }
    }

    public static partial class ChangeToken
    {
        public static System.IDisposable OnChange(System.Func<IChangeToken> changeTokenProducer, System.Action changeTokenConsumer) { throw null; }

        public static System.IDisposable OnChange<TState>(System.Func<IChangeToken> changeTokenProducer, System.Action<TState> changeTokenConsumer, TState state) { throw null; }
    }

    public partial class CompositeChangeToken : IChangeToken
    {
        public CompositeChangeToken(System.Collections.Generic.IReadOnlyList<IChangeToken> changeTokens) { }

        public bool ActiveChangeCallbacks { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<IChangeToken> ChangeTokens { get { throw null; } }

        public bool HasChanged { get { throw null; } }

        public System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state) { throw null; }
    }

    public static partial class Extensions
    {
        public static System.Text.StringBuilder Append(this System.Text.StringBuilder builder, StringSegment segment) { throw null; }
    }

    public partial interface IChangeToken
    {
        bool ActiveChangeCallbacks { get; }

        bool HasChanged { get; }

        System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state);
    }

    public readonly partial struct StringSegment : System.IEquatable<StringSegment>, System.IEquatable<string>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public static readonly StringSegment Empty;
        public StringSegment(string buffer, int offset, int length) { }

        public StringSegment(string buffer) { }

        public string Buffer { get { throw null; } }

        public bool HasValue { get { throw null; } }

        public char this[int index] { get { throw null; } }

        public int Length { get { throw null; } }

        public int Offset { get { throw null; } }

        public string Value { get { throw null; } }

        public readonly System.ReadOnlyMemory<char> AsMemory() { throw null; }

        public readonly System.ReadOnlySpan<char> AsSpan() { throw null; }

        public readonly System.ReadOnlySpan<char> AsSpan(int start, int length) { throw null; }

        public readonly System.ReadOnlySpan<char> AsSpan(int start) { throw null; }

        public static int Compare(StringSegment a, StringSegment b, System.StringComparison comparisonType) { throw null; }

        public readonly bool EndsWith(string text, System.StringComparison comparisonType) { throw null; }

        public static bool Equals(StringSegment a, StringSegment b, System.StringComparison comparisonType) { throw null; }

        public readonly bool Equals(StringSegment other, System.StringComparison comparisonType) { throw null; }

        public readonly bool Equals(StringSegment other) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(string text, System.StringComparison comparisonType) { throw null; }

        public readonly bool Equals(string text) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public readonly int IndexOf(char c, int start, int count) { throw null; }

        public readonly int IndexOf(char c, int start) { throw null; }

        public readonly int IndexOf(char c) { throw null; }

        public readonly int IndexOfAny(char[] anyOf, int startIndex, int count) { throw null; }

        public readonly int IndexOfAny(char[] anyOf, int startIndex) { throw null; }

        public readonly int IndexOfAny(char[] anyOf) { throw null; }

        public static bool IsNullOrEmpty(StringSegment value) { throw null; }

        public readonly int LastIndexOf(char value) { throw null; }

        public static bool operator ==(StringSegment left, StringSegment right) { throw null; }

        public static implicit operator System.ReadOnlyMemory<char>(StringSegment segment) { throw null; }

        public static implicit operator System.ReadOnlySpan<char>(StringSegment segment) { throw null; }

        public static implicit operator StringSegment(string value) { throw null; }

        public static bool operator !=(StringSegment left, StringSegment right) { throw null; }

        public readonly StringTokenizer Split(char[] chars) { throw null; }

        public readonly bool StartsWith(string text, System.StringComparison comparisonType) { throw null; }

        public readonly StringSegment Subsegment(int offset, int length) { throw null; }

        public readonly StringSegment Subsegment(int offset) { throw null; }

        public readonly string Substring(int offset, int length) { throw null; }

        public readonly string Substring(int offset) { throw null; }

        readonly bool System.IEquatable<string>.Equals(string other) { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly StringSegment Trim() { throw null; }

        public readonly StringSegment TrimEnd() { throw null; }

        public readonly StringSegment TrimStart() { throw null; }
    }

    public partial class StringSegmentComparer : System.Collections.Generic.IComparer<StringSegment>, System.Collections.Generic.IEqualityComparer<StringSegment>
    {
        public static StringSegmentComparer Ordinal { get { throw null; } }

        public static StringSegmentComparer OrdinalIgnoreCase { get { throw null; } }

        public int Compare(StringSegment x, StringSegment y) { throw null; }

        public bool Equals(StringSegment x, StringSegment y) { throw null; }

        public int GetHashCode(StringSegment obj) { throw null; }
    }

    public readonly partial struct StringTokenizer : System.Collections.Generic.IEnumerable<StringSegment>, System.Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringTokenizer(StringSegment value, char[] separators) { }

        public StringTokenizer(string value, char[] separators) { }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly System.Collections.Generic.IEnumerator<StringSegment> System.Collections.Generic.IEnumerable<StringSegment>.GetEnumerator() { throw null; }

        readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : System.Collections.Generic.IEnumerator<StringSegment>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Enumerator(ref StringTokenizer tokenizer) { }

            public StringSegment Current { get { throw null; } }

            object System.Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public readonly partial struct StringValues : System.Collections.Generic.IList<string>, System.Collections.Generic.ICollection<string>, System.Collections.Generic.IEnumerable<string>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyList<string>, System.Collections.Generic.IReadOnlyCollection<string>, System.IEquatable<StringValues>, System.IEquatable<string>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public static readonly StringValues Empty;
        public StringValues(string value) { }

        public StringValues(string[] values) { }

        public int Count { get { throw null; } }

        public string this[int index] { get { throw null; } }

        string System.Collections.Generic.IList<System.String>.this[int index] { get { throw null; } set { } }

        bool System.Collections.Generic.ICollection<string>.IsReadOnly { get { throw null; } }

        public static StringValues Concat(StringValues values1, StringValues values2) { throw null; }

        public static StringValues Concat(in StringValues values, string value) { throw null; }

        public static StringValues Concat(string value, in StringValues values) { throw null; }

        public static bool Equals(StringValues left, StringValues right) { throw null; }

        public static bool Equals(StringValues left, string right) { throw null; }

        public static bool Equals(StringValues left, string[] right) { throw null; }

        public readonly bool Equals(StringValues other) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public static bool Equals(string left, StringValues right) { throw null; }

        public readonly bool Equals(string other) { throw null; }

        public static bool Equals(string[] left, StringValues right) { throw null; }

        public readonly bool Equals(string[] other) { throw null; }

        public readonly Enumerator GetEnumerator() { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool IsNullOrEmpty(StringValues value) { throw null; }

        public static bool operator ==(StringValues left, StringValues right) { throw null; }

        public static bool operator ==(StringValues left, object right) { throw null; }

        public static bool operator ==(StringValues left, string right) { throw null; }

        public static bool operator ==(StringValues left, string[] right) { throw null; }

        public static bool operator ==(object left, StringValues right) { throw null; }

        public static bool operator ==(string left, StringValues right) { throw null; }

        public static bool operator ==(string[] left, StringValues right) { throw null; }

        public static implicit operator string(StringValues values) { throw null; }

        public static implicit operator string[](StringValues value) { throw null; }

        public static implicit operator StringValues(string value) { throw null; }

        public static implicit operator StringValues(string[] values) { throw null; }

        public static bool operator !=(StringValues left, StringValues right) { throw null; }

        public static bool operator !=(StringValues left, object right) { throw null; }

        public static bool operator !=(StringValues left, string right) { throw null; }

        public static bool operator !=(StringValues left, string[] right) { throw null; }

        public static bool operator !=(object left, StringValues right) { throw null; }

        public static bool operator !=(string left, StringValues right) { throw null; }

        public static bool operator !=(string[] left, StringValues right) { throw null; }

        readonly void System.Collections.Generic.ICollection<string>.Add(string item) { }

        readonly void System.Collections.Generic.ICollection<string>.Clear() { }

        readonly bool System.Collections.Generic.ICollection<string>.Contains(string item) { throw null; }

        readonly void System.Collections.Generic.ICollection<string>.CopyTo(string[] array, int arrayIndex) { }

        readonly bool System.Collections.Generic.ICollection<string>.Remove(string item) { throw null; }

        readonly System.Collections.Generic.IEnumerator<string> System.Collections.Generic.IEnumerable<string>.GetEnumerator() { throw null; }

        readonly int System.Collections.Generic.IList<string>.IndexOf(string item) { throw null; }

        readonly void System.Collections.Generic.IList<string>.Insert(int index, string item) { }

        readonly void System.Collections.Generic.IList<string>.RemoveAt(int index) { }

        readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }

        public readonly string[] ToArray() { throw null; }

        public override readonly string ToString() { throw null; }

        public partial struct Enumerator : System.Collections.Generic.IEnumerator<string>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Enumerator(ref StringValues values) { }

            public string Current { get { throw null; } }

            object System.Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void System.Collections.IEnumerator.Reset() { }
        }
    }
}