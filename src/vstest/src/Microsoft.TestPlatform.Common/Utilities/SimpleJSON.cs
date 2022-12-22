#pragma warning disable IDE0073 // The file header does not match the required text
/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 *
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 *
 * Written by Bunny83
 * 2012-06-09
 *
 * Original link for this code: https://github.com/Bunny83/SimpleJSON
 * Modified in order to fix analyzer errors.
 *
 * Changelog now external. See Changelog.txt
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2012-2019 Markus Göbel (Bunny83)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * * * * */
#pragma warning restore IDE0073 // The file header does not match the required text

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.Common;

#nullable disable

namespace SimpleJSON;

internal enum JSONNodeType
{
    Array = 1,
    Object = 2,
    String = 3,
    Number = 4,
    NullValue = 5,
    Boolean = 6,
    None = 7,
    Custom = 0xFF,
}

internal enum JSONTextMode
{
    Compact,
    Indent
}

internal abstract partial class JSONNode
{
    #region Enumerators
    public struct Enumerator
    {
        private enum Type { None, Array, Object }

        private readonly Type _type;
        private Dictionary<string, JSONNode>.Enumerator _object;
        private List<JSONNode>.Enumerator _array;

        public bool IsValid { get { return _type != Type.None; } }

        public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
        {
            _type = Type.Array;
            _object = default;
            _array = aArrayEnum;
        }
        public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
        {
            _type = Type.Object;
            _object = aDictEnum;
            _array = default;
        }
        public KeyValuePair<string, JSONNode> Current
        {
            get
            {
                if (_type == Type.Array)
                    return new KeyValuePair<string, JSONNode>(string.Empty, _array.Current);
                else if (_type == Type.Object)
                    return _object.Current;
                return new KeyValuePair<string, JSONNode>(string.Empty, null);
            }
        }
        public bool MoveNext()
        {
            if (_type == Type.Array)
                return _array.MoveNext();
            else if (_type == Type.Object)
                return _object.MoveNext();
            return false;
        }
    }
    public struct ValueEnumerator
    {
        private Enumerator _enumerator;
        public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
        public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
        public ValueEnumerator(Enumerator aEnumerator) { _enumerator = aEnumerator; }
        public JSONNode Current { get { return _enumerator.Current.Value; } }
        public bool MoveNext() { return _enumerator.MoveNext(); }
        public ValueEnumerator GetEnumerator() { return this; }
    }
    public struct KeyEnumerator
    {
        private Enumerator _enumerator;
        public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
        public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
        public KeyEnumerator(Enumerator aEnumerator) { _enumerator = aEnumerator; }
        public string Current { get { return _enumerator.Current.Key; } }
        public bool MoveNext() { return _enumerator.MoveNext(); }
        public KeyEnumerator GetEnumerator() { return this; }
    }

    public class LinqEnumerator : IEnumerator<KeyValuePair<string, JSONNode>>, IEnumerable<KeyValuePair<string, JSONNode>>
    {
        private JSONNode _node;
        private Enumerator _enumerator;
        internal LinqEnumerator(JSONNode aNode)
        {
            _node = aNode;
            if (_node != null)
                _enumerator = _node.GetEnumerator();
        }
        public KeyValuePair<string, JSONNode> Current { get { return _enumerator.Current; } }
        object IEnumerator.Current { get { return _enumerator.Current; } }
        public bool MoveNext() { return _enumerator.MoveNext(); }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            _node = null;
            _enumerator = new Enumerator();
        }

        public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator()
        {
            return new LinqEnumerator(_node);
        }

        public void Reset()
        {
            if (_node != null)
                _enumerator = _node.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinqEnumerator(_node);
        }
    }

    #endregion Enumerators

    #region common interface

    public static bool ForceASCII; // Use Unicode by default
    public static bool LongAsString; // lazy creator creates a JSONString instead of JSONNumber
    public static bool AllowLineComments = true; // allow "//"-style comments at the end of a line

    public abstract JSONNodeType Tag { get; }

    public virtual JSONNode this[int aIndex] { get { return null; } set { } }

    public virtual JSONNode this[string aKey] { get { return null; } set { } }

    public virtual string Value { get { return ""; } set { } }

    public virtual int Count { get { return 0; } }

    public virtual bool IsNumber { get { return false; } }
    public virtual bool IsString { get { return false; } }
    public virtual bool IsBoolean { get { return false; } }
    public virtual bool IsNull { get { return false; } }
    public virtual bool IsArray { get { return false; } }
    public virtual bool IsObject { get { return false; } }

    public virtual bool Inline { get { return false; } set { } }

    public virtual void Add(string aKey, JSONNode aItem)
    {
    }
    public virtual void Add(JSONNode aItem)
    {
        Add("", aItem);
    }

    public virtual JSONNode Remove(string aKey)
    {
        return null;
    }

    public virtual JSONNode Remove(int aIndex)
    {
        return null;
    }

    public virtual JSONNode Remove(JSONNode aNode)
    {
        return aNode;
    }
    public virtual void Clear() { }

    public virtual JSONNode Clone()
    {
        return null;
    }

    public virtual IEnumerable<JSONNode> Children
    {
        get
        {
            yield break;
        }
    }

    public IEnumerable<JSONNode> DeepChildren
    {
        get
        {
            foreach (var c in Children)
                foreach (var d in c.DeepChildren)
                    yield return d;
        }
    }

    public virtual bool HasKey(string aKey)
    {
        return false;
    }

    public virtual JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
    {
        return aDefault;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        WriteToStringBuilder(sb, 0, 0, JSONTextMode.Compact);
        return sb.ToString();
    }

    public virtual string ToString(int aIndent)
    {
        StringBuilder sb = new();
        WriteToStringBuilder(sb, 0, aIndent, JSONTextMode.Indent);
        return sb.ToString();
    }
    internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

    public abstract Enumerator GetEnumerator();
    public IEnumerable<KeyValuePair<string, JSONNode>> Linq { get { return new LinqEnumerator(this); } }
    public KeyEnumerator Keys { get { return new KeyEnumerator(GetEnumerator()); } }
    public ValueEnumerator Values { get { return new ValueEnumerator(GetEnumerator()); } }

    #endregion common interface

    #region typecasting properties


    public virtual double AsDouble
    {
        get
        {
            return double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) ? v : 0.0;
        }
        set
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public virtual int AsInt
    {
        get { return (int)AsDouble; }
        set { AsDouble = value; }
    }

    public virtual float AsFloat
    {
        get { return (float)AsDouble; }
        set { AsDouble = value; }
    }

    public virtual bool AsBool
    {
        get
        {
            return bool.TryParse(Value, out bool v) ? v : !Value.IsNullOrEmpty();
        }
        set
        {
            Value = (value) ? "true" : "false";
        }
    }

    public virtual long AsLong
    {
        get
        {
            return long.TryParse(Value, out long val) ? val : 0L;
        }
        set
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public virtual ulong AsULong
    {
        get
        {
            return ulong.TryParse(Value, out ulong val) ? val : 0;
        }
        set
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public virtual JSONArray AsArray
    {
        get
        {
            return this as JSONArray;
        }
    }

    public virtual JSONObject AsObject
    {
        get
        {
            return this as JSONObject;
        }
    }


    #endregion typecasting properties

    #region operators

    public static implicit operator JSONNode(string s)
    {
        return (s == null) ? (JSONNode)JSONNull.CreateOrGet() : new JSONString(s);
    }
    public static implicit operator string(JSONNode d)
    {
        return d?.Value;
    }

    public static implicit operator JSONNode(double n)
    {
        return new JSONNumber(n);
    }
    public static implicit operator double(JSONNode d)
    {
        return (d == null) ? 0 : d.AsDouble;
    }

    public static implicit operator JSONNode(float n)
    {
        return new JSONNumber(n);
    }
    public static implicit operator float(JSONNode d)
    {
        return (d == null) ? 0 : d.AsFloat;
    }

    public static implicit operator JSONNode(int n)
    {
        return new JSONNumber(n);
    }
    public static implicit operator int(JSONNode d)
    {
        return (d == null) ? 0 : d.AsInt;
    }

    public static implicit operator JSONNode(long n)
    {
        return LongAsString ? new JSONString(n.ToString(CultureInfo.InvariantCulture)) : new JSONNumber(n);
    }
    public static implicit operator long(JSONNode d)
    {
        return (d == null) ? 0L : d.AsLong;
    }

    public static implicit operator JSONNode(ulong n)
    {
        return LongAsString ? new JSONString(n.ToString(CultureInfo.InvariantCulture)) : new JSONNumber(n);
    }
    public static implicit operator ulong(JSONNode d)
    {
        return (d == null) ? 0 : d.AsULong;
    }

    public static implicit operator JSONNode(bool b)
    {
        return new JSONBool(b);
    }
    public static implicit operator bool(JSONNode d)
    {
        return d != null && d.AsBool;
    }

    public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue)
    {
        return aKeyValue.Value;
    }

    public static bool operator ==(JSONNode a, object b)
    {
        if (ReferenceEquals(a, b))
            return true;
        bool aIsNull = a is JSONNull or null or JSONLazyCreator;
        bool bIsNull = b is JSONNull or null or JSONLazyCreator;
        return aIsNull && bIsNull || !aIsNull && a.Equals(b);
    }

    public static bool operator !=(JSONNode a, object b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return ReferenceEquals(this, obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    #endregion operators

    [ThreadStatic]
    private static StringBuilder s_escapeBuilder;
    internal static StringBuilder EscapeBuilder
    {
        get
        {
            s_escapeBuilder ??= new StringBuilder();
            return s_escapeBuilder;
        }
    }
    internal static string Escape(string aText)
    {
        var sb = EscapeBuilder;
        sb.Length = 0;
        if (sb.Capacity < aText.Length + aText.Length / 10)
            sb.Capacity = aText.Length + aText.Length / 10;
        foreach (char c in aText)
        {
            switch (c)
            {
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                default:
                    if (c < ' ' || (ForceASCII && c > 127))
                    {
                        ushort val = c;
                        sb.Append("\\u").Append(val.ToString("X4", CultureInfo.InvariantCulture));
                    }
                    else
                        sb.Append(c);
                    break;
            }
        }
        string result = sb.ToString();
        sb.Length = 0;
        return result;
    }

    private static JSONNode ParseElement(string token, bool quoted)
    {
        if (quoted)
            return token;
        if (token.Length <= 5)
        {
            string tmp = token.ToLower(CultureInfo.InvariantCulture);
            if (tmp is "false" or "true")
                return tmp == "true";
            if (tmp == "null")
                return JSONNull.CreateOrGet();
        }
        return double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double val) ? (JSONNode)val : (JSONNode)token;
    }

    public static JSONNode Parse(string aJSON)
    {
        Stack<JSONNode> stack = new();
        JSONNode ctx = null;
        int i = 0;
        StringBuilder token = new();
        string tokenName = "";
        bool quoteMode = false;
        bool tokenIsQuoted = false;
        bool hasNewlineChar = false;
        while (i < aJSON.Length)
        {
            switch (aJSON[i])
            {
                case '{':
                    if (quoteMode)
                    {
                        token.Append(aJSON[i]);
                        break;
                    }
                    stack.Push(new JSONObject());
                    if (ctx != null)
                    {
                        ctx.Add(tokenName, stack.Peek());
                    }
                    tokenName = "";
                    token.Length = 0;
                    ctx = stack.Peek();
                    hasNewlineChar = false;
                    break;

                case '[':
                    if (quoteMode)
                    {
                        token.Append(aJSON[i]);
                        break;
                    }

                    stack.Push(new JSONArray());
                    if (ctx != null)
                    {
                        ctx.Add(tokenName, stack.Peek());
                    }
                    tokenName = "";
                    token.Length = 0;
                    ctx = stack.Peek();
                    hasNewlineChar = false;
                    break;

                case '}':
                case ']':
                    if (quoteMode)
                    {

                        token.Append(aJSON[i]);
                        break;
                    }
                    if (stack.Count == 0)
                        throw new Exception("JSON Parse: Too many closing brackets");

                    stack.Pop();
                    if (token.Length > 0 || tokenIsQuoted)
                        ctx.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
                    if (ctx != null)
                        ctx.Inline = !hasNewlineChar;
                    tokenIsQuoted = false;
                    tokenName = "";
                    token.Length = 0;
                    if (stack.Count > 0)
                        ctx = stack.Peek();
                    break;

                case ':':
                    if (quoteMode)
                    {
                        token.Append(aJSON[i]);
                        break;
                    }
                    tokenName = token.ToString();
                    token.Length = 0;
                    tokenIsQuoted = false;
                    break;

                case '"':
                    quoteMode ^= true;
                    tokenIsQuoted |= quoteMode;
                    break;

                case ',':
                    if (quoteMode)
                    {
                        token.Append(aJSON[i]);
                        break;
                    }
                    if (token.Length > 0 || tokenIsQuoted)
                        ctx.Add(tokenName, ParseElement(token.ToString(), tokenIsQuoted));
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                    tokenIsQuoted = false;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                    tokenName = "";
                    token.Length = 0;
                    tokenIsQuoted = false;
                    break;

                case '\r':
                case '\n':
                    hasNewlineChar = true;
                    break;

                case ' ':
                case '\t':
                    if (quoteMode)
                        token.Append(aJSON[i]);
                    break;

                case '\\':
                    ++i;
                    if (quoteMode)
                    {
                        char c = aJSON[i];
                        switch (c)
                        {
                            case 't':
                                token.Append('\t');
                                break;
                            case 'r':
                                token.Append('\r');
                                break;
                            case 'n':
                                token.Append('\n');
                                break;
                            case 'b':
                                token.Append('\b');
                                break;
                            case 'f':
                                token.Append('\f');
                                break;
                            case 'u':
                                {
                                    string s = aJSON.Substring(i + 1, 4);
                                    token.Append((char)int.Parse(
                                        s,
                                        System.Globalization.NumberStyles.AllowHexSpecifier,
                                        CultureInfo.InvariantCulture));
                                    i += 4;
                                    break;
                                }
                            default:
                                token.Append(c);
                                break;
                        }
                    }
                    break;
                case '/':
                    if (AllowLineComments && !quoteMode && i + 1 < aJSON.Length && aJSON[i + 1] == '/')
                    {
                        while (++i < aJSON.Length && aJSON[i] != '\n' && aJSON[i] != '\r') ;
                        break;
                    }
                    token.Append(aJSON[i]);
                    break;
                case '\uFEFF': // remove / ignore BOM (Byte Order Mark)
                    break;

                default:
                    token.Append(aJSON[i]);
                    break;
            }
            ++i;
        }
        if (quoteMode)
        {
            throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
        }
        if (ctx == null)
            return ParseElement(token.ToString(), tokenIsQuoted);
        return ctx;
    }

}
// End of JSONNode

internal partial class JSONArray : JSONNode
{
    private readonly List<JSONNode> _list = new();
    private bool _inline;
    public override bool Inline
    {
        get { return _inline; }
        set { _inline = value; }
    }

    public override JSONNodeType Tag { get { return JSONNodeType.Array; } }
    public override bool IsArray { get { return true; } }
    public override Enumerator GetEnumerator() { return new Enumerator(_list.GetEnumerator()); }

    public override JSONNode this[int aIndex]
    {
        get
        {
            return aIndex < 0 || aIndex >= _list.Count ? new JSONLazyCreator(this) : _list[aIndex];
        }
        set
        {
            if (value == null)
                value = JSONNull.CreateOrGet();
            if (aIndex < 0 || aIndex >= _list.Count)
                _list.Add(value);
            else
                _list[aIndex] = value;
        }
    }

    public override JSONNode this[string aKey]
    {
        get { return new JSONLazyCreator(this); }
        set
        {
            if (value == null)
                value = JSONNull.CreateOrGet();
            _list.Add(value);
        }
    }

    public override int Count
    {
        get { return _list.Count; }
    }

    public override void Add(string aKey, JSONNode aItem)
    {
        if (aItem == null)
            aItem = JSONNull.CreateOrGet();
        _list.Add(aItem);
    }

    public override JSONNode Remove(int aIndex)
    {
        if (aIndex < 0 || aIndex >= _list.Count)
            return null;
        JSONNode tmp = _list[aIndex];
        _list.RemoveAt(aIndex);
        return tmp;
    }

    public override JSONNode Remove(JSONNode aNode)
    {
        _list.Remove(aNode);
        return aNode;
    }

    public override void Clear()
    {
        _list.Clear();
    }

    public override JSONNode Clone()
    {
        var node = new JSONArray();
        node._list.Capacity = _list.Capacity;
        foreach (var n in _list)
        {
            if (n != null)
                node.Add(n.Clone());
            else
                node.Add(null);
        }
        return node;
    }

    public override IEnumerable<JSONNode> Children
    {
        get
        {
            foreach (JSONNode n in _list)
                yield return n;
        }
    }


    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append('[');
        int count = _list.Count;
        if (_inline)
            aMode = JSONTextMode.Compact;
        for (int i = 0; i < count; i++)
        {
            if (i > 0)
                aSB.Append(',');
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine();

            if (aMode == JSONTextMode.Indent)
                aSB.Append(' ', aIndent + aIndentInc);
            _list[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
        }
        if (aMode == JSONTextMode.Indent)
            aSB.AppendLine().Append(' ', aIndent);
        aSB.Append(']');
    }
}
// End of JSONArray

internal partial class JSONObject : JSONNode
{
    private readonly Dictionary<string, JSONNode> _dict = new();

    private bool _inline;
    public override bool Inline
    {
        get { return _inline; }
        set { _inline = value; }
    }

    public override JSONNodeType Tag { get { return JSONNodeType.Object; } }
    public override bool IsObject { get { return true; } }

    public override Enumerator GetEnumerator() { return new Enumerator(_dict.GetEnumerator()); }


    public override JSONNode this[string aKey]
    {
        get
        {
            return _dict.ContainsKey(aKey) ? _dict[aKey] : new JSONLazyCreator(this, aKey);
        }
        set
        {
            if (value == null)
                value = JSONNull.CreateOrGet();
            if (_dict.ContainsKey(aKey))
                _dict[aKey] = value;
            else
                _dict.Add(aKey, value);
        }
    }

    public override JSONNode this[int aIndex]
    {
        get
        {
            return aIndex < 0 || aIndex >= _dict.Count ? null : _dict.ElementAt(aIndex).Value;
        }
        set
        {
            if (value == null)
                value = JSONNull.CreateOrGet();
            if (aIndex < 0 || aIndex >= _dict.Count)
                return;
            string key = _dict.ElementAt(aIndex).Key;
            _dict[key] = value;
        }
    }

    public override int Count
    {
        get { return _dict.Count; }
    }

    public override void Add(string aKey, JSONNode aItem)
    {
        if (aItem == null)
            aItem = JSONNull.CreateOrGet();

        if (aKey != null)
        {
            if (_dict.ContainsKey(aKey))
                _dict[aKey] = aItem;
            else
                _dict.Add(aKey, aItem);
        }
        else
            _dict.Add(Guid.NewGuid().ToString(), aItem);
    }

    public override JSONNode Remove(string aKey)
    {
        if (!_dict.ContainsKey(aKey))
            return null;
        JSONNode tmp = _dict[aKey];
        _dict.Remove(aKey);
        return tmp;
    }

    public override JSONNode Remove(int aIndex)
    {
        if (aIndex < 0 || aIndex >= _dict.Count)
            return null;
        var item = _dict.ElementAt(aIndex);
        _dict.Remove(item.Key);
        return item.Value;
    }

    public override JSONNode Remove(JSONNode aNode)
    {
        try
        {
            var item = _dict.Where(k => k.Value == aNode).First();
            _dict.Remove(item.Key);
            return aNode;
        }
        catch
        {
            return null;
        }
    }

    public override void Clear()
    {
        _dict.Clear();
    }

    public override JSONNode Clone()
    {
        var node = new JSONObject();
        foreach (var n in _dict)
        {
            node.Add(n.Key, n.Value.Clone());
        }
        return node;
    }

    public override bool HasKey(string aKey)
    {
        return _dict.ContainsKey(aKey);
    }

    public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
    {
        return _dict.TryGetValue(aKey, out JSONNode res) ? res : aDefault;
    }

    public override IEnumerable<JSONNode> Children
    {
        get
        {
            foreach (var n in _dict)
                yield return n.Value;
        }
    }

    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append('{');
        bool first = true;
        if (_inline)
            aMode = JSONTextMode.Compact;
        foreach (var k in _dict)
        {
            if (!first)
                aSB.Append(',');
            first = false;
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine();
            if (aMode == JSONTextMode.Indent)
                aSB.Append(' ', aIndent + aIndentInc);
            aSB.Append('\"').Append(Escape(k.Key)).Append('\"');
            if (aMode == JSONTextMode.Compact)
                aSB.Append(':');
            else
                aSB.Append(" : ");
            k.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
        }
        if (aMode == JSONTextMode.Indent)
            aSB.AppendLine().Append(' ', aIndent);
        aSB.Append('}');
    }

}
// End of JSONObject

internal partial class JSONString : JSONNode
{
    private string _data;

    public override JSONNodeType Tag { get { return JSONNodeType.String; } }
    public override bool IsString { get { return true; } }

    public override Enumerator GetEnumerator() { return new Enumerator(); }


    public override string Value
    {
        get { return _data; }
        set
        {
            _data = value;
        }
    }

    public JSONString(string aData)
    {
        _data = aData;
    }
    public override JSONNode Clone()
    {
        return new JSONString(_data);
    }

    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append('\"').Append(Escape(_data)).Append('\"');
    }
    public override bool Equals(object obj)
    {
        if (base.Equals(obj))
            return true;
        if (obj is string s)
            return _data == s;
        JSONString s2 = obj as JSONString;
        return s2 != null && _data == s2._data;
    }
    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }
    public override void Clear()
    {
        _data = "";
    }
}
// End of JSONString

internal partial class JSONNumber : JSONNode
{
    private double _data;

    public override JSONNodeType Tag { get { return JSONNodeType.Number; } }
    public override bool IsNumber { get { return true; } }
    public override Enumerator GetEnumerator() { return new Enumerator(); }

    public override string Value
    {
        get { return _data.ToString(CultureInfo.InvariantCulture); }
        set
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                _data = v;
        }
    }

    public override double AsDouble
    {
        get { return _data; }
        set { _data = value; }
    }
    public override long AsLong
    {
        get { return (long)_data; }
        set { _data = value; }
    }
    public override ulong AsULong
    {
        get { return (ulong)_data; }
        set { _data = value; }
    }

    public JSONNumber(double aData)
    {
        _data = aData;
    }

    public JSONNumber(string aData)
    {
        Value = aData;
    }

    public override JSONNode Clone()
    {
        return new JSONNumber(_data);
    }

    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append(Value);
    }
    private static bool IsNumeric(object value)
    {
        return value is int or uint
            or float or double
            or decimal
            or long or ulong
            or short or ushort
            or sbyte or byte;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (base.Equals(obj))
            return true;
        JSONNumber s2 = obj as JSONNumber;
        return s2 != null ? _data == s2._data : IsNumeric(obj) && Convert.ToDouble(obj, CultureInfo.InvariantCulture) == _data;
    }
    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }
    public override void Clear()
    {
        _data = 0;
    }
}
// End of JSONNumber

internal partial class JSONBool : JSONNode
{
    private bool _data;

    public override JSONNodeType Tag { get { return JSONNodeType.Boolean; } }
    public override bool IsBoolean { get { return true; } }
    public override Enumerator GetEnumerator() { return new Enumerator(); }

    public override string Value
    {
        get { return _data.ToString(); }
        set
        {
            if (bool.TryParse(value, out bool v))
                _data = v;
        }
    }
    public override bool AsBool
    {
        get { return _data; }
        set { _data = value; }
    }

    public JSONBool(bool aData)
    {
        _data = aData;
    }

    public JSONBool(string aData)
    {
        Value = aData;
    }

    public override JSONNode Clone()
    {
        return new JSONBool(_data);
    }

    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append((_data) ? "true" : "false");
    }
    public override bool Equals(object obj)
    {
        return obj != null && obj is bool boolean && _data == boolean;
    }
    public override int GetHashCode()
    {
        return _data.GetHashCode();
    }
    public override void Clear()
    {
        _data = false;
    }
}
// End of JSONBool

internal partial class JSONNull : JSONNode
{
    private static readonly JSONNull StaticInstance = new();
    public static bool ReuseSameInstance = true;
    public static JSONNull CreateOrGet()
    {
        return ReuseSameInstance ? StaticInstance : new JSONNull();
    }
    private JSONNull() { }

    public override JSONNodeType Tag { get { return JSONNodeType.NullValue; } }
    public override bool IsNull { get { return true; } }
    public override Enumerator GetEnumerator() { return new Enumerator(); }

    public override string Value
    {
        get { return "null"; }
        set { }
    }
    public override bool AsBool
    {
        get { return false; }
        set { }
    }

    public override JSONNode Clone()
    {
        return CreateOrGet();
    }

    public override bool Equals(object obj)
    {
        return object.ReferenceEquals(this, obj) || obj is JSONNull;
    }
    public override int GetHashCode()
    {
        return 0;
    }

    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append("null");
    }
}
// End of JSONNull

internal partial class JSONLazyCreator : JSONNode
{
    private JSONNode _node;
    private readonly string _key;
    public override JSONNodeType Tag { get { return JSONNodeType.None; } }
    public override Enumerator GetEnumerator() { return new Enumerator(); }

    public JSONLazyCreator(JSONNode aNode)
    {
        _node = aNode;
        _key = null;
    }

    public JSONLazyCreator(JSONNode aNode, string aKey)
    {
        _node = aNode;
        _key = aKey;
    }

    private T Set<T>(T aVal) where T : JSONNode
    {
        if (_key == null)
            _node.Add(aVal);
        else
            _node.Add(_key, aVal);
        _node = null; // Be GC friendly.
        return aVal;
    }

    public override JSONNode this[int aIndex]
    {
        get { return new JSONLazyCreator(this); }
        set { Set(new JSONArray()).Add(value); }
    }

    public override JSONNode this[string aKey]
    {
        get { return new JSONLazyCreator(this, aKey); }
        set { Set(new JSONObject()).Add(aKey, value); }
    }

    public override void Add(JSONNode aItem)
    {
        Set(new JSONArray()).Add(aItem);
    }

    public override void Add(string aKey, JSONNode aItem)
    {
        Set(new JSONObject()).Add(aKey, aItem);
    }

    public static bool operator ==(JSONLazyCreator a, object b)
    {
        return b == null || System.Object.ReferenceEquals(a, b);
    }

    public static bool operator !=(JSONLazyCreator a, object b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj == null || System.Object.ReferenceEquals(this, obj);
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public override int AsInt
    {
        get { Set(new JSONNumber(0)); return 0; }
        set { Set(new JSONNumber(value)); }
    }

    public override float AsFloat
    {
        get { Set(new JSONNumber(0.0f)); return 0.0f; }
        set { Set(new JSONNumber(value)); }
    }

    public override double AsDouble
    {
        get { Set(new JSONNumber(0.0)); return 0.0; }
        set { Set(new JSONNumber(value)); }
    }

    public override long AsLong
    {
        get
        {
            if (LongAsString)
                Set(new JSONString("0"));
            else
                Set(new JSONNumber(0.0));
            return 0L;
        }
        set
        {
            if (LongAsString)
                Set(new JSONString(value.ToString(CultureInfo.InvariantCulture)));
            else
                Set(new JSONNumber(value));
        }
    }

    public override ulong AsULong
    {
        get
        {
            if (LongAsString)
                Set(new JSONString("0"));
            else
                Set(new JSONNumber(0.0));
            return 0L;
        }
        set
        {
            if (LongAsString)
                Set(new JSONString(value.ToString(CultureInfo.InvariantCulture)));
            else
                Set(new JSONNumber(value));
        }
    }

    public override bool AsBool
    {
        get { Set(new JSONBool(false)); return false; }
        set { Set(new JSONBool(value)); }
    }

    public override JSONArray AsArray
    {
        get { return Set(new JSONArray()); }
    }

    public override JSONObject AsObject
    {
        get { return Set(new JSONObject()); }
    }
    internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
    {
        aSB.Append("null");
    }
}
// End of JSONLazyCreator

internal static class JSON
{
    public static JSONNode Parse(string aJSON)
    {
        return JSONNode.Parse(aJSON);
    }
}
