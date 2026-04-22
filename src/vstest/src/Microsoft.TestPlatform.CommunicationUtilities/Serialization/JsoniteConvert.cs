// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !NETCOREAPP

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Jsonite;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

internal static class JsoniteConvert
{
    public static object? ToJsonValue(object? value, int version = 2)
    {
        return ToJsonValueCore(value, new HashSet<object>(ReferenceEqualityComparer.Instance), 0, version);
    }

    private const int MaxDepth = 64;
    private static bool IsV1(int version) => version == 0 || version == 1 || version == 3;

    private static object? ToJsonValueCore(object? value, HashSet<object> visited, int depth, int version)
    {
        if (value is null) return null;
        if (depth > MaxDepth) return null;
        var type = value.GetType();
        if (type.IsPrimitive || value is string || value is decimal) return value;
        if (type.IsEnum) return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        if (value is Guid g) return g.ToString("D");
        if (value is Uri u) return u.OriginalString;
        if (value is DateTimeOffset dto) return dto.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", CultureInfo.InvariantCulture);
        if (value is DateTime dt) return dt.ToString("o", CultureInfo.InvariantCulture);
        if (value is TimeSpan ts) return ts.ToString();
        if (value is Type t) return t.AssemblyQualifiedName;
        if (value is Delegate || value is MemberInfo || value is Assembly || value is Module) return null;
        if (!type.IsValueType && !visited.Add(value)) return null;

        try
        {
            if (value is TestProperty tp) return SerializeTestProperty(tp);
            if (value is TestCase tc) return SerializeTestCase(tc, visited, depth, version);
            if (value is TestResult tr) return SerializeTestResult(tr, visited, depth, version);
            if (value is TestObject to) return SerializeTestObject(to, visited, depth, version);
            if (value is ITestRunStatistics runStats) return SerializeTestRunStatistics(runStats);
            if (value is TestRunCompleteEventArgs trce) return SerializeTestRunCompleteEventArgs(trce, visited, depth, version);
            if (value is TestRunChangedEventArgs trch) return SerializeTestRunChangedEventArgs(trch, visited, depth, version);
            if (value is AttachmentSet att) return SerializeAttachmentSet(att, visited, depth, version);
            if (value is UriDataAttachment uda) return SerializeUriDataAttachment(uda);
            if (value is TestSessionInfo tsi) return SerializeTestSessionInfo(tsi);
            if (value is DiscoveryCriteria dc) return SerializeDiscoveryCriteria(dc, visited, depth, version);
            if (value is TestExecutionContext tec) return SerializeTestExecutionContext(tec, visited, depth, version);
            if (value is TestProcessAttachDebuggerPayload tpad) return SerializeTestProcessAttachDebuggerPayload(tpad);

            if (value is IDictionary dict)
            {
                var obj = new JsonObject();
                foreach (DictionaryEntry entry in dict)
                    obj[Convert.ToString(entry.Key, CultureInfo.InvariantCulture) ?? string.Empty] = ToJsonValueCore(entry.Value, visited, depth + 1, version)!;
                return obj;
            }
            if (value is IEnumerable enumerable)
            {
                var arr = new JsonArray();
                foreach (var item in enumerable) arr.Add(ToJsonValueCore(item, visited, depth + 1, version)!);
                return arr;
            }
            var result = new JsonObject();
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || prop.GetIndexParameters().Length > 0) continue;
                if (prop.GetCustomAttribute<IgnoreDataMemberAttribute>() != null) continue;
                try { result[prop.Name] = ToJsonValueCore(prop.GetValue(value), visited, depth + 1, version)!; }
                catch (Exception ex) { EqtTrace.Warning("JsoniteConvert: Failed to serialize property '{0}' on type '{1}': {2}", prop.Name, type.FullName, ex.Message); }
            }
            return result;
        }
        finally { if (!type.IsValueType) visited.Remove(value); }
    }

    private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();
        bool IEqualityComparer<object>.Equals(object? x, object? y) => ReferenceEquals(x, y);
        int IEqualityComparer<object>.GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
    }

    // ==================== SERIALIZERS ====================

    private static object SerializeTestProperty(TestProperty tp) => new JsonObject
    {
        ["Id"] = tp.Id, ["Label"] = tp.Label, ["Category"] = tp.Category ?? "", ["Description"] = tp.Description ?? "",
        ["Attributes"] = (int)tp.Attributes, ["ValueType"] = tp.ValueType,
    };

    private static object SerializeKV(TestProperty key, object? val, HashSet<object> visited, int depth, int version) => new JsonObject
    {
        ["Key"] = SerializeTestProperty(key), ["Value"] = ToJsonValueCore(val, visited, depth + 1, version)!,
    };

    private static object SerializeTestCase(TestCase tc, HashSet<object> visited, int depth, int version)
    {
        if (IsV1(version))
        {
            var props = new JsonArray();
            props.Add(SerializeKV(TestCaseProperties.FullyQualifiedName, tc.FullyQualifiedName, visited, depth, version));
            props.Add(SerializeKV(TestCaseProperties.ExecutorUri, tc.ExecutorUri?.OriginalString, visited, depth, version));
            props.Add(SerializeKV(TestCaseProperties.Source, tc.Source, visited, depth, version));
            props.Add(SerializeKV(TestCaseProperties.CodeFilePath, tc.CodeFilePath, visited, depth, version));
            props.Add(SerializeKV(TestCaseProperties.DisplayName, tc.DisplayName, visited, depth, version));
            props.Add(SerializeKV(TestCaseProperties.Id, tc.Id.ToString("D"), visited, depth, version));
            props.Add(new JsonObject { ["Key"] = SerializeTestProperty(TestCaseProperties.LineNumber), ["Value"] = tc.LineNumber });
            foreach (var kvp in tc.GetProperties()) props.Add(SerializeKV(kvp.Key, kvp.Value, visited, depth, version));
            return new JsonObject { ["Properties"] = props };
        }
        var r = new JsonObject { ["Id"] = tc.Id.ToString("D"), ["FullyQualifiedName"] = tc.FullyQualifiedName, ["DisplayName"] = tc.DisplayName,
            ["ExecutorUri"] = tc.ExecutorUri?.OriginalString!, ["Source"] = tc.Source, ["CodeFilePath"] = tc.CodeFilePath!, ["LineNumber"] = tc.LineNumber };
        var p2 = new JsonArray();
        foreach (var kvp in tc.GetProperties()) p2.Add(SerializeKV(kvp.Key, kvp.Value, visited, depth, version));
        r["Properties"] = p2;
        return r;
    }

    private static object SerializeTestResult(TestResult tr, HashSet<object> visited, int depth, int version)
    {
        if (IsV1(version))
        {
            var r = new JsonObject { ["TestCase"] = ToJsonValueCore(tr.TestCase, visited, depth + 1, version)!,
                ["Attachments"] = ToJsonValueCore(tr.Attachments, visited, depth + 1, version)!, ["Messages"] = ToJsonValueCore(tr.Messages, visited, depth + 1, version)! };
            var p = new JsonArray();
            p.Add(new JsonObject { ["Key"] = SerializeTestProperty(TestResultProperties.Outcome), ["Value"] = (int)tr.Outcome });
            p.Add(SerializeKV(TestResultProperties.ErrorMessage, tr.ErrorMessage, visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.ErrorStackTrace, tr.ErrorStackTrace, visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.DisplayName, tr.DisplayName, visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.ComputerName, tr.ComputerName ?? "", visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.Duration, tr.Duration.ToString(), visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.StartTime, tr.StartTime, visited, depth, version));
            p.Add(SerializeKV(TestResultProperties.EndTime, tr.EndTime, visited, depth, version));
            foreach (var kvp in tr.GetProperties()) p.Add(SerializeKV(kvp.Key, kvp.Value, visited, depth, version));
            r["Properties"] = p;
            return r;
        }
        var r2 = new JsonObject { ["TestCase"] = ToJsonValueCore(tr.TestCase, visited, depth + 1, version)!,
            ["Attachments"] = ToJsonValueCore(tr.Attachments, visited, depth + 1, version)!, ["Outcome"] = (int)tr.Outcome,
            ["ErrorMessage"] = tr.ErrorMessage!, ["ErrorStackTrace"] = tr.ErrorStackTrace!, ["DisplayName"] = tr.DisplayName!,
            ["Messages"] = ToJsonValueCore(tr.Messages, visited, depth + 1, version)!, ["ComputerName"] = tr.ComputerName!,
            ["Duration"] = tr.Duration.ToString(), ["StartTime"] = tr.StartTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", CultureInfo.InvariantCulture),
            ["EndTime"] = tr.EndTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", CultureInfo.InvariantCulture) };
        var p2 = new JsonArray();
        foreach (var kvp in tr.GetProperties()) p2.Add(SerializeKV(kvp.Key, kvp.Value, visited, depth, version));
        r2["Properties"] = p2;
        return r2;
    }

    private static object SerializeTestObject(TestObject to, HashSet<object> visited, int depth, int version)
    {
        var props = new JsonArray();
        foreach (var kvp in to.GetProperties()) props.Add(SerializeKV(kvp.Key, kvp.Value, visited, depth, version));
        return new JsonObject { ["Properties"] = props };
    }

    private static object SerializeTestRunStatistics(ITestRunStatistics stats)
    {
        var r = new JsonObject { ["ExecutedTests"] = stats.ExecutedTests };
        if (stats is TestRunStatistics c && c.Stats is not null)
        {
            var s = new JsonObject(); foreach (var kvp in c.Stats) s[kvp.Key.ToString()] = kvp.Value; r["Stats"] = s;
        }
        else r["Stats"] = null!;
        return r;
    }

    private static object SerializeTestRunCompleteEventArgs(TestRunCompleteEventArgs a, HashSet<object> v, int d, int ver) => new JsonObject
    {
        ["TestRunStatistics"] = ToJsonValueCore(a.TestRunStatistics, v, d + 1, ver)!, ["IsCanceled"] = a.IsCanceled, ["IsAborted"] = a.IsAborted,
        ["Error"] = ToJsonValueCore(a.Error, v, d + 1, ver)!, ["AttachmentSets"] = ToJsonValueCore(a.AttachmentSets, v, d + 1, ver)!,
        ["InvokedDataCollectors"] = ToJsonValueCore(a.InvokedDataCollectors, v, d + 1, ver)!, ["ElapsedTimeInRunningTests"] = a.ElapsedTimeInRunningTests.ToString(),
        ["Metrics"] = ToJsonValueCore(a.Metrics, v, d + 1, ver)!, ["DiscoveredExtensions"] = ToJsonValueCore(a.DiscoveredExtensions, v, d + 1, ver)!,
    };

    private static object SerializeTestRunChangedEventArgs(TestRunChangedEventArgs a, HashSet<object> v, int d, int ver) => new JsonObject
    {
        ["NewTestResults"] = ToJsonValueCore(a.NewTestResults, v, d + 1, ver)!, ["TestRunStatistics"] = ToJsonValueCore(a.TestRunStatistics, v, d + 1, ver)!,
        ["ActiveTests"] = ToJsonValueCore(a.ActiveTests, v, d + 1, ver)!,
    };

    private static object SerializeAttachmentSet(AttachmentSet a, HashSet<object> v, int d, int ver) => new JsonObject
    {
        ["Uri"] = a.Uri.OriginalString, ["DisplayName"] = a.DisplayName, ["Attachments"] = ToJsonValueCore(a.Attachments, v, d + 1, ver)!,
    };

    private static object SerializeUriDataAttachment(UriDataAttachment u) => new JsonObject { ["Description"] = u.Description!, ["Uri"] = u.Uri.OriginalString };
    private static object SerializeTestSessionInfo(TestSessionInfo s) => new JsonObject { ["Id"] = s.Id.ToString("D") };

    private static object SerializeDiscoveryCriteria(DiscoveryCriteria dc, HashSet<object> v, int d, int ver) => new JsonObject
    {
        ["Package"] = dc.Package!, ["AdapterSourceMap"] = ToJsonValueCore(dc.AdapterSourceMap, v, d + 1, ver)!,
        ["FrequencyOfDiscoveredTestsEvent"] = dc.FrequencyOfDiscoveredTestsEvent, ["DiscoveredTestEventTimeout"] = dc.DiscoveredTestEventTimeout.ToString(),
        ["RunSettings"] = dc.RunSettings!, ["TestCaseFilter"] = dc.TestCaseFilter!, ["TestSessionInfo"] = ToJsonValueCore(dc.TestSessionInfo, v, d + 1, ver)!,
    };

    private static object SerializeTestExecutionContext(TestExecutionContext c, HashSet<object> v, int d, int ver)
    {
        var r = new JsonObject { ["FrequencyOfRunStatsChangeEvent"] = c.FrequencyOfRunStatsChangeEvent, ["RunStatsChangeEventTimeout"] = c.RunStatsChangeEventTimeout.ToString(),
            ["InIsolation"] = c.InIsolation, ["KeepAlive"] = c.KeepAlive, ["AreTestCaseLevelEventsRequired"] = c.AreTestCaseLevelEventsRequired,
            ["IsDebug"] = c.IsDebug, ["TestCaseFilter"] = c.TestCaseFilter! };
        r["FilterOptions"] = ToJsonValueCore(c.FilterOptions, v, d + 1, ver)!;
        return r;
    }

    private static object SerializeTestProcessAttachDebuggerPayload(TestProcessAttachDebuggerPayload p) => new JsonObject
    { ["ProcessID"] = p.ProcessID, ["TargetFramework"] = p.TargetFramework! };

    // ==================== DESERIALIZERS ====================

    private static object? DeserializeTestProperty(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var id = dict.TryGetValue("Id", out var iv) ? iv?.ToString() : null;
        var label = dict.TryGetValue("Label", out var lv) ? lv?.ToString() : null;
        if (id is null || label is null) return null;
        var cat = dict.TryGetValue("Category", out var cv) ? cv?.ToString() ?? "" : "";
        var desc = dict.TryGetValue("Description", out var dv) ? dv?.ToString() ?? "" : "";
        var attr = dict.TryGetValue("Attributes", out var av) ? (TestPropertyAttributes)Convert.ToInt32(av, CultureInfo.InvariantCulture) : default;
        var vt = dict.TryGetValue("ValueType", out var vtv) ? vtv?.ToString() : null;
        var tp = TestProperty.Find(id);
        if (tp is not null) return tp;
        var resolvedType = vt is not null ? (Type.GetType(vt) ?? typeof(string)) : typeof(string);
        tp = TestProperty.Register(id, label, cat, desc, resolvedType, null, attr, typeof(TestObject));
        if (vt is not null) tp.ValueType = vt;
        return tp;
    }

    private static object? DeserializeTestCase(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var tc = new TestCase();
        bool flat = dict.ContainsKey("FullyQualifiedName");
        if (flat)
        {
            if (dict.TryGetValue("FullyQualifiedName", out var fqn) && fqn != null) tc.FullyQualifiedName = fqn.ToString()!;
            if (dict.TryGetValue("ExecutorUri", out var uri) && uri != null) tc.ExecutorUri = new Uri(uri.ToString()!);
            if (dict.TryGetValue("Source", out var src) && src != null) tc.Source = src.ToString()!;
            if (dict.TryGetValue("Id", out var id) && id != null) tc.Id = Guid.Parse(id.ToString()!);
            if (dict.TryGetValue("DisplayName", out var dn) && dn != null) tc.DisplayName = dn.ToString()!;
            if (dict.TryGetValue("CodeFilePath", out var cfp) && cfp != null) tc.CodeFilePath = cfp.ToString();
            if (dict.TryGetValue("LineNumber", out var ln) && ln != null) tc.LineNumber = Convert.ToInt32(ln, CultureInfo.InvariantCulture);
        }
        if (dict.TryGetValue("Properties", out var po) && po is IList pl)
            foreach (var item in pl)
            {
                if (item is not IDictionary<string, object> pd) continue;
                if (!pd.TryGetValue("Key", out var ko)) continue;
                var tp = (TestProperty?)DeserializeTestProperty(ko); if (tp is null) continue;
                pd.TryGetValue("Value", out var rv); var data = ConvertPropertyValueToString(rv);
                if (!flat) switch (tp.Id)
                {
                    case "TestCase.Id": tc.Id = Guid.Parse(data!); continue;
                    case "TestCase.ExecutorUri": tc.ExecutorUri = new Uri(data!); continue;
                    case "TestCase.FullyQualifiedName": tc.FullyQualifiedName = data!; continue;
                    case "TestCase.DisplayName": tc.DisplayName = data!; continue;
                    case "TestCase.Source": tc.Source = data!; continue;
                    case "TestCase.CodeFilePath": tc.CodeFilePath = data; continue;
                    case "TestCase.LineNumber": tc.LineNumber = int.Parse(data!, CultureInfo.InvariantCulture); continue;
                }
                tp = TestProperty.Register(tp.Id, tp.Label, tp.GetValueType(), tp.Attributes, typeof(TestObject));
                tc.SetPropertyValue(tp, data);
            }
        return tc;
    }

    private static object? DeserializeTestResult(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        if (!dict.TryGetValue("TestCase", out var tco)) return null;
        var tc = (TestCase?)DeserializeTestCase(tco) ?? new TestCase();
        var tr = new TestResult(tc);
        if (dict.TryGetValue("Attachments", out var ao) && ao is IList al) foreach (var i in al) if (i != null && DeserializeAttachmentSet(i) is AttachmentSet a) tr.Attachments.Add(a);
        if (dict.TryGetValue("Messages", out var mo) && mo is IList ml) foreach (var i in ml) if (i != null && ConvertTo(i, typeof(TestResultMessage)) is TestResultMessage m) tr.Messages.Add(m);
        bool flat = dict.ContainsKey("Outcome");
        if (flat)
        {
            if (dict.TryGetValue("Outcome", out var oc)) tr.Outcome = (TestOutcome)Convert.ToInt32(oc, CultureInfo.InvariantCulture);
            if (dict.TryGetValue("ErrorMessage", out var em) && em != null) tr.ErrorMessage = em.ToString();
            if (dict.TryGetValue("ErrorStackTrace", out var es) && es != null) tr.ErrorStackTrace = es.ToString();
            if (dict.TryGetValue("DisplayName", out var dn) && dn != null) tr.DisplayName = dn.ToString();
            if (dict.TryGetValue("ComputerName", out var cn) && cn != null) tr.ComputerName = cn.ToString();
            if (dict.TryGetValue("Duration", out var du) && du != null) tr.Duration = TimeSpan.Parse(du.ToString()!, CultureInfo.InvariantCulture);
            if (dict.TryGetValue("StartTime", out var st) && st != null) tr.StartTime = DateTimeOffset.Parse(st.ToString()!, CultureInfo.InvariantCulture);
            if (dict.TryGetValue("EndTime", out var et) && et != null) tr.EndTime = DateTimeOffset.Parse(et.ToString()!, CultureInfo.InvariantCulture);
        }
        if (dict.TryGetValue("Properties", out var po) && po is IList pl)
            foreach (var item in pl)
            {
                if (item is not IDictionary<string, object> pd) continue;
                if (!pd.TryGetValue("Key", out var ko)) continue;
                var tp = (TestProperty?)DeserializeTestProperty(ko); if (tp is null) continue;
                pd.TryGetValue("Value", out var rv); var data = ConvertPropertyValueToString(rv);
                if (!flat) switch (tp.Id)
                {
                    case "TestResult.DisplayName": tr.DisplayName = data; continue;
                    case "TestResult.ComputerName": tr.ComputerName = data ?? ""; continue;
                    case "TestResult.Outcome": tr.Outcome = (TestOutcome)Enum.Parse(typeof(TestOutcome), data!); continue;
                    case "TestResult.Duration": tr.Duration = TimeSpan.Parse(data!, CultureInfo.InvariantCulture); continue;
                    case "TestResult.StartTime": tr.StartTime = DateTimeOffset.Parse(data!, CultureInfo.InvariantCulture); continue;
                    case "TestResult.EndTime": tr.EndTime = DateTimeOffset.Parse(data!, CultureInfo.InvariantCulture); continue;
                    case "TestResult.ErrorMessage": tr.ErrorMessage = data; continue;
                    case "TestResult.ErrorStackTrace": tr.ErrorStackTrace = data; continue;
                }
                tp = TestProperty.Register(tp.Id, tp.Label, tp.GetValueType(), tp.Attributes, typeof(TestObject));
                tr.SetPropertyValue(tp, data);
            }
        return tr;
    }

    private static object? DeserializeTestObject(object? value, Type targetType)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var ctor = targetType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        var inst = ctor is not null ? (TestObject)ctor.Invoke(Array.Empty<object>()) : (TestObject)FormatterServices.GetUninitializedObject(targetType);
        if (dict.TryGetValue("Properties", out var po) && po is IList pl)
            foreach (var item in pl)
            {
                if (item is not IDictionary<string, object> pd) continue;
                if (!pd.TryGetValue("Key", out var ko)) continue;
                var tp = (TestProperty?)DeserializeTestProperty(ko); if (tp is null) continue;
                pd.TryGetValue("Value", out var rv); var data = ConvertPropertyValueToString(rv);
                tp = TestProperty.Register(tp.Id, tp.Label, tp.GetValueType(), tp.Attributes, typeof(TestObject));
                inst.SetPropertyValue(tp, (object?)data, CultureInfo.InvariantCulture);
            }
        return inst;
    }

    private static object? DeserializeTestRunStatistics(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        long et = dict.TryGetValue("ExecutedTests", out var e) ? Convert.ToInt64(e, CultureInfo.InvariantCulture) : 0;
        IDictionary<TestOutcome, long>? stats = null;
        if (dict.TryGetValue("Stats", out var so) && so is IDictionary<string, object> sd)
        { stats = new Dictionary<TestOutcome, long>(); foreach (var kvp in sd) if (Enum.TryParse<TestOutcome>(kvp.Key, out var o)) stats[o] = Convert.ToInt64(kvp.Value, CultureInfo.InvariantCulture); }
        return new TestRunStatistics(et, stats);
    }

    private static object? DeserializeTestRunCompleteEventArgs(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var stats = dict.TryGetValue("TestRunStatistics", out var s) && s != null ? (ITestRunStatistics?)DeserializeTestRunStatistics(s) : null;
        var cancel = dict.TryGetValue("IsCanceled", out var ic) && Convert.ToBoolean(ic, CultureInfo.InvariantCulture);
        var abort = dict.TryGetValue("IsAborted", out var ia) && Convert.ToBoolean(ia, CultureInfo.InvariantCulture);
        var error = dict.TryGetValue("Error", out var er) && er != null ? (Exception?)ConvertTo(er, typeof(Exception)) : null;
        var att = dict.TryGetValue("AttachmentSets", out var ao) && ao != null ? (Collection<AttachmentSet>?)ConvertTo(ao, typeof(Collection<AttachmentSet>)) ?? new Collection<AttachmentSet>() : new Collection<AttachmentSet>();
        var idc = dict.TryGetValue("InvokedDataCollectors", out var io) && io != null ? (Collection<InvokedDataCollector>?)ConvertTo(io, typeof(Collection<InvokedDataCollector>)) ?? new Collection<InvokedDataCollector>() : new Collection<InvokedDataCollector>();
        var elapsed = dict.TryGetValue("ElapsedTimeInRunningTests", out var el) && el != null ? TimeSpan.Parse(el.ToString()!, CultureInfo.InvariantCulture) : default;
        var r = new TestRunCompleteEventArgs(stats, cancel, abort, error, att, idc, elapsed);
        if (dict.TryGetValue("Metrics", out var m) && m != null) r.Metrics = (IDictionary<string, object>?)ConvertTo(m, typeof(IDictionary<string, object>));
        if (dict.TryGetValue("DiscoveredExtensions", out var ext) && ext != null) r.DiscoveredExtensions = (Dictionary<string, HashSet<string>>?)ConvertTo(ext, typeof(Dictionary<string, HashSet<string>>));
        return r;
    }

    private static object? DeserializeTestRunChangedEventArgs(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var stats = dict.TryGetValue("TestRunStatistics", out var s) && s != null ? (ITestRunStatistics?)DeserializeTestRunStatistics(s) : null;
        var results = dict.TryGetValue("NewTestResults", out var nr) && nr != null ? (IEnumerable<TestResult>?)ConvertTo(nr, typeof(List<TestResult>)) : null;
        var active = dict.TryGetValue("ActiveTests", out var at) && at != null ? (IEnumerable<TestCase>?)ConvertTo(at, typeof(List<TestCase>)) : null;
        return new TestRunChangedEventArgs(stats, results, active);
    }

    private static object? DeserializeAttachmentSet(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var uri = dict.TryGetValue("Uri", out var u) && u != null ? new Uri(u.ToString()!) : new Uri("unknown://");
        var dn = dict.TryGetValue("DisplayName", out var d) && d != null ? d.ToString()! : "";
        var s = new AttachmentSet(uri, dn);
        if (dict.TryGetValue("Attachments", out var ao) && ao is IList al) foreach (var i in al) if (i != null && DeserializeUriDataAttachment(i) is UriDataAttachment a) s.Attachments.Add(a);
        return s;
    }

    private static object? DeserializeUriDataAttachment(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var uri = dict.TryGetValue("Uri", out var u) && u != null ? new Uri(u.ToString()!) : new Uri("unknown://");
        var desc = dict.TryGetValue("Description", out var d) && d != null ? d.ToString() : null;
        return new UriDataAttachment(uri, desc);
    }

    private static object? DeserializeTestSessionInfo(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var id = dict.TryGetValue("Id", out var o) && o != null ? Guid.Parse(o.ToString()!) : Guid.NewGuid();
        var info = new TestSessionInfo();
        typeof(TestSessionInfo).GetProperty(nameof(TestSessionInfo.Id))!.SetValue(info, id);
        return info;
    }

    private static object? DeserializeDiscoveryCriteria(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var c = new DiscoveryCriteria(); var tp = typeof(DiscoveryCriteria);
        if (dict.TryGetValue("AdapterSourceMap", out var asm) && asm != null) tp.GetProperty(nameof(DiscoveryCriteria.AdapterSourceMap))!.SetValue(c, ConvertTo(asm, typeof(Dictionary<string, IEnumerable<string>>)));
        if (dict.TryGetValue("FrequencyOfDiscoveredTestsEvent", out var f) && f != null) tp.GetProperty(nameof(DiscoveryCriteria.FrequencyOfDiscoveredTestsEvent))!.SetValue(c, Convert.ToInt64(f, CultureInfo.InvariantCulture));
        if (dict.TryGetValue("DiscoveredTestEventTimeout", out var t) && t != null) tp.GetProperty(nameof(DiscoveryCriteria.DiscoveredTestEventTimeout))!.SetValue(c, TimeSpan.Parse(t.ToString()!, CultureInfo.InvariantCulture));
        if (dict.TryGetValue("RunSettings", out var rs) && rs != null) tp.GetProperty(nameof(DiscoveryCriteria.RunSettings))!.SetValue(c, rs.ToString());
        if (dict.TryGetValue("Package", out var p) && p != null) c.Package = p.ToString();
        if (dict.TryGetValue("TestCaseFilter", out var tcf) && tcf != null) c.TestCaseFilter = tcf.ToString();
        if (dict.TryGetValue("TestSessionInfo", out var tsi) && tsi != null) c.TestSessionInfo = (TestSessionInfo?)DeserializeTestSessionInfo(tsi);
        return c;
    }

    private static object? DeserializeTestExecutionContext(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var c = new TestExecutionContext();
        if (dict.TryGetValue("FrequencyOfRunStatsChangeEvent", out var f)) c.FrequencyOfRunStatsChangeEvent = Convert.ToInt64(f, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("RunStatsChangeEventTimeout", out var t) && t != null) c.RunStatsChangeEventTimeout = TimeSpan.Parse(t.ToString()!, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("InIsolation", out var iso)) c.InIsolation = Convert.ToBoolean(iso, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("KeepAlive", out var ka)) c.KeepAlive = Convert.ToBoolean(ka, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("AreTestCaseLevelEventsRequired", out var tcle)) c.AreTestCaseLevelEventsRequired = Convert.ToBoolean(tcle, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("IsDebug", out var dbg)) c.IsDebug = Convert.ToBoolean(dbg, CultureInfo.InvariantCulture);
        if (dict.TryGetValue("TestCaseFilter", out var fi) && fi != null) c.TestCaseFilter = fi.ToString();
        if (dict.TryGetValue("FilterOptions", out var fo) && fo != null) c.FilterOptions = (FilterOptions?)ConvertTo(fo, typeof(FilterOptions));
        return c;
    }

    private static object? DeserializeTestProcessAttachDebuggerPayload(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var pid = dict.TryGetValue("ProcessID", out var p) ? Convert.ToInt32(p, CultureInfo.InvariantCulture) : 0;
        var tf = dict.TryGetValue("TargetFramework", out var t) && t != null ? t.ToString() : null;
        return new TestProcessAttachDebuggerPayload(pid) { TargetFramework = tf };
    }

    private static object? DeserializeAfterTestRunEndResult(object? value)
    {
        if (value is not IDictionary<string, object> dict) return null;
        var att = dict.TryGetValue("AttachmentSets", out var ao) && ao != null ? (Collection<AttachmentSet>?)ConvertTo(ao, typeof(Collection<AttachmentSet>)) ?? new Collection<AttachmentSet>() : new Collection<AttachmentSet>();
        var idc = dict.TryGetValue("InvokedDataCollectors", out var io) && io != null ? (Collection<InvokedDataCollector>?)ConvertTo(io, typeof(Collection<InvokedDataCollector>)) : null;
        var met = dict.TryGetValue("Metrics", out var m) && m != null ? (IDictionary<string, object>?)ConvertTo(m, typeof(Dictionary<string, object>)) ?? new Dictionary<string, object>() : new Dictionary<string, object>();
        var ctor = typeof(AfterTestRunEndResult).GetConstructor(new[] { typeof(Collection<AttachmentSet>), typeof(Collection<InvokedDataCollector>), typeof(IDictionary<string, object>) });
        return ctor is not null ? ctor.Invoke(new object?[] { att, idc, met }) : FormatterServices.GetUninitializedObject(typeof(AfterTestRunEndResult));
    }

    // ==================== HELPERS ====================

    private static string? ConvertPropertyValueToString(object? rawValue)
    {
        if (rawValue is null) return null;
        if (rawValue is string s) return s;
        if (rawValue is bool b) return b ? "True" : "False";
        return Json.Serialize(rawValue);
    }

    // ==================== GENERIC DESERIALIZATION ====================

    public static T? To<T>(object? value) => (T?)ConvertTo(value, typeof(T));

    // Overload accepting version for API symmetry with ToJsonValue.
    // Currently version is unused during deserialization (format is auto-detected),
    // but having the plumbing here allows version-specific logic in the future.
#pragma warning disable IDE0060 // Remove unused parameter
    public static T? To<T>(object? value, int version) => (T?)ConvertTo(value, typeof(T));
#pragma warning restore IDE0060

    private static object? ConvertTo(object? value, Type targetType)
    {
        if (value is null) return targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null ? Activator.CreateInstance(targetType) : null;
        var ut = Nullable.GetUnderlyingType(targetType); if (ut is not null) return ConvertTo(value, ut);
        if (targetType == typeof(object)) return value;
        if (targetType.IsInstanceOfType(value)) return value;

        if (targetType == typeof(TestProperty)) return DeserializeTestProperty(value);
        if (targetType == typeof(TestCase)) return DeserializeTestCase(value);
        if (targetType == typeof(TestResult)) return DeserializeTestResult(value);
        if (targetType == typeof(ITestRunStatistics) || targetType == typeof(TestRunStatistics)) return DeserializeTestRunStatistics(value);
        if (targetType == typeof(TestRunCompleteEventArgs)) return DeserializeTestRunCompleteEventArgs(value);
        if (targetType == typeof(TestRunChangedEventArgs)) return DeserializeTestRunChangedEventArgs(value);
        if (targetType == typeof(AttachmentSet)) return DeserializeAttachmentSet(value);
        if (targetType == typeof(UriDataAttachment)) return DeserializeUriDataAttachment(value);
        if (targetType == typeof(TestSessionInfo)) return DeserializeTestSessionInfo(value);
        if (targetType == typeof(DiscoveryCriteria)) return DeserializeDiscoveryCriteria(value);
        if (targetType == typeof(TestExecutionContext)) return DeserializeTestExecutionContext(value);
        if (targetType == typeof(TestProcessAttachDebuggerPayload)) return DeserializeTestProcessAttachDebuggerPayload(value);
        if (targetType == typeof(AfterTestRunEndResult)) return DeserializeAfterTestRunEndResult(value);
        if (typeof(TestObject).IsAssignableFrom(targetType) && targetType != typeof(TestObject)) return DeserializeTestObject(value, targetType);

        if (targetType == typeof(string)) return Convert.ToString(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(bool)) { if (value is bool bv) return bv; if (value is string bs) return bool.Parse(bs); return Convert.ToBoolean(value, CultureInfo.InvariantCulture); }
        if (targetType.IsEnum) { if (value is string sv) return Enum.Parse(targetType, sv, true); return Enum.ToObject(targetType, Convert.ToInt64(value, CultureInfo.InvariantCulture)); }
        if (targetType == typeof(int)) return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(long)) return Convert.ToInt64(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(double)) return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(float)) return Convert.ToSingle(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(short)) return Convert.ToInt16(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(byte)) return Convert.ToByte(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(decimal)) return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(uint)) return Convert.ToUInt32(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(ulong)) return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(ushort)) return Convert.ToUInt16(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(sbyte)) return Convert.ToSByte(value, CultureInfo.InvariantCulture);
        if (targetType == typeof(Guid)) return Guid.Parse(Convert.ToString(value, CultureInfo.InvariantCulture)!);
        if (targetType == typeof(Uri)) { var s = Convert.ToString(value, CultureInfo.InvariantCulture); return s is null ? null : new Uri(s); }
        if (targetType == typeof(DateTime)) return DateTime.Parse(Convert.ToString(value, CultureInfo.InvariantCulture)!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        if (targetType == typeof(DateTimeOffset)) return DateTimeOffset.Parse(Convert.ToString(value, CultureInfo.InvariantCulture)!, CultureInfo.InvariantCulture);
        if (targetType == typeof(TimeSpan)) return TimeSpan.Parse(Convert.ToString(value, CultureInfo.InvariantCulture)!, CultureInfo.InvariantCulture);

        if (targetType.IsArray && value is IList sa) { var et = targetType.GetElementType()!; var a = Array.CreateInstance(et, sa.Count); for (int i = 0; i < sa.Count; i++) a.SetValue(ConvertTo(sa[i], et), i); return a; }
        if (targetType.IsGenericType && value is IList sl)
        {
            var gd = targetType.GetGenericTypeDefinition();
            if (gd == typeof(List<>) || gd == typeof(IList<>) || gd == typeof(IEnumerable<>) || gd == typeof(ICollection<>) || gd == typeof(IReadOnlyList<>) || gd == typeof(IReadOnlyCollection<>))
            { var et = targetType.GetGenericArguments()[0]; var lt = typeof(List<>).MakeGenericType(et); var rl = (IList)Activator.CreateInstance(lt)!; foreach (var i in sl) rl.Add(ConvertTo(i, et)); return rl; }
            if (gd == typeof(Collection<>))
            { var et = targetType.GetGenericArguments()[0]; var lt = typeof(List<>).MakeGenericType(et); var tl = (IList)Activator.CreateInstance(lt)!; foreach (var i in sl) tl.Add(ConvertTo(i, et)); return Activator.CreateInstance(typeof(Collection<>).MakeGenericType(et), tl); }
            if (gd == typeof(HashSet<>))
            { var et = targetType.GetGenericArguments()[0]; var st = typeof(HashSet<>).MakeGenericType(et); var se = Activator.CreateInstance(st)!; var am = st.GetMethod("Add")!; foreach (var i in sl) am.Invoke(se, new[] { ConvertTo(i, et) }); return se; }
        }
        if (targetType.IsGenericType && value is IDictionary<string, object> sd)
        {
            var gd = targetType.GetGenericTypeDefinition();
            if (gd == typeof(Dictionary<,>) || gd == typeof(IDictionary<,>))
            {
                var kt = targetType.GetGenericArguments()[0]; var vt = targetType.GetGenericArguments()[1];
                var rdt = typeof(Dictionary<,>).MakeGenericType(kt, vt); var rd = (IDictionary)Activator.CreateInstance(rdt)!;
                foreach (var kvp in sd) rd[ConvertTo(kvp.Key, kt)!] = ConvertTo(kvp.Value, vt);
                return rd;
            }
        }
        if (value is IDictionary<string, object> od)
        {
            var inst = CreateInstance(targetType, od, out var usedKeys);
            var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var kvp in od)
            {
                // Skip keys already consumed by the constructor
                if (usedKeys != null && usedKeys.Contains(kvp.Key)) continue;
                var prop = FindProperty(props, kvp.Key); if (prop is null) continue;
                if (prop.GetCustomAttribute<IgnoreDataMemberAttribute>() != null) continue;
                try
                {
                    if (prop.GetSetMethod() is not null) prop.SetValue(inst, ConvertTo(kvp.Value, prop.PropertyType));
                    else if (kvp.Value is IList ia && prop.GetValue(inst) is IList tl)
                    { var et = prop.PropertyType.IsGenericType ? prop.PropertyType.GetGenericArguments()[0] : typeof(object); foreach (var i in ia) tl.Add(ConvertTo(i, et)); }
                    else
                    {
                        var cv = ConvertTo(kvp.Value, prop.PropertyType);
                        var bf = targetType.GetField($"<{prop.Name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (bf is not null) bf.SetValue(inst, cv); else prop.GetSetMethod(nonPublic: true)?.Invoke(inst, new[] { cv });
                    }
                }
                catch (Exception ex) { EqtTrace.Warning("JsoniteConvert: Failed to set property '{0}' on type '{1}': {2}", prop.Name, targetType.FullName, ex.Message); }
            }
            return inst;
        }
        try { return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture); }
        catch (Exception ex) { EqtTrace.Warning("JsoniteConvert: Failed to convert value of type '{0}' to '{1}': {2}", value.GetType().FullName, targetType.FullName, ex.Message); return targetType.IsValueType ? Activator.CreateInstance(targetType) : null; }
    }

    private static PropertyInfo? FindProperty(PropertyInfo[] props, string name)
    {
        foreach (var p in props) if (string.Equals(p.Name, name, StringComparison.Ordinal)) return p;
        foreach (var p in props) if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)) return p;
        return null;
    }

    private static object CreateInstance(Type type, IDictionary<string, object> data, out HashSet<string>? usedKeys)
    {
        usedKeys = null;
        var pc = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        if (pc is not null) return pc.Invoke(Array.Empty<object>());
        foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
        {
            var pars = ctor.GetParameters(); var args = new object?[pars.Length]; bool ok = true;
            var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < pars.Length; i++)
            {
                var k = data.Keys.FirstOrDefault(x => string.Equals(x, pars[i].Name, StringComparison.OrdinalIgnoreCase));
                if (k is not null) { args[i] = ConvertTo(data[k], pars[i].ParameterType); keys.Add(k); }
                else if (pars[i].HasDefaultValue) args[i] = pars[i].DefaultValue;
                else { ok = false; break; }
            }
            if (ok) { usedKeys = keys; return ctor.Invoke(args); }
        }
        return FormatterServices.GetUninitializedObject(type);
    }
}

#endif