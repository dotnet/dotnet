using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using Microsoft.Build.Framework;

namespace NestedReentrancyProbe;

[MSBuildMultiThreadableTask]
public sealed class PacketObserverTask : Microsoft.Build.Utilities.Task
{
    private static readonly List<Timer> Timers = new();
    private static readonly HashSet<string> TraversalFields = new(StringComparer.Ordinal)
    {
        "_host", "_componentHost", "_buildComponentHost", "_sharedHost",
        "_sharedComponentHost", "_parentHost", "_componentFactories",
        "_componentFactoryCollection", "_componentEntriesByType", "_singleton",
        "_outOfProcTaskHostNodeProvider"
    };
    [Required] public string RunDir { get; set; } = "";

    public override bool Execute()
    {
        if (!Path.IsPathFullyQualified(RunDir))
            throw new ArgumentException("RunDir must be absolute.");
        Directory.CreateDirectory(RunDir);
        object host = ReadField(BuildEngine, "_host")
            ?? throw new InvalidOperationException("No existing BuildEngine host reference.");
        string output = Path.Combine(RunDir, $"packet-ownership-{Environment.ProcessId}.json");
        var timer = new Timer(_ => Capture(host, output), null, 20000, Timeout.Infinite);
        lock (Timers) { Timers.Add(timer); }
        Log.LogMessage(MessageImportance.High, $"PACKET OBSERVER armed in PID {Environment.ProcessId}, host {host.GetType().FullName}");
        return true;
    }

    private static object? ReadField(object? instance, string name)
    {
        for (Type? type = instance?.GetType(); type is not null; type = type.BaseType)
        {
            var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (field is not null)
                return field.GetValue(instance);
        }
        return null;
    }

    private static object? ReadProperty(object? instance, string name) =>
        instance?.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public |
            BindingFlags.NonPublic)?.GetValue(instance);

    private static void Capture(object host, string output)
    {
        var providers = new List<object>();
        var visitedTypes = new List<string>();
        try
        {
            var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
            var pending = new Queue<(object Value, int Depth)>();
            pending.Enqueue((host, 0));
            while (pending.Count != 0 && seen.Count < 5000)
            {
                var (value, depth) = pending.Dequeue();
                if (!seen.Add(value) || depth > 12)
                    continue;
                string name = value.GetType().FullName ?? value.GetType().Name;
                visitedTypes.Add(name);
                if (name.EndsWith(".NodeProviderOutOfProcTaskHost", StringComparison.Ordinal))
                {
                    providers.Add(value);
                    continue;
                }
                if (value is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                        if (entry.Value is not null)
                            pending.Enqueue((entry.Value, depth + 1));
                    continue;
                }
                if (!name.StartsWith("Microsoft.Build.", StringComparison.Ordinal))
                    continue;
                for (Type? type = value.GetType(); type is not null; type = type.BaseType)
                {
                    foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                    {
                        if (!TraversalFields.Contains(field.Name) &&
                            field.FieldType.Name != "BuildComponentFactoryCollection" &&
                            field.FieldType.Name != "IBuildComponentHost")
                            continue;
                        object? child = field.GetValue(value);
                        if (child is not null)
                            pending.Enqueue((child, depth + 1));
                    }
                }
            }

            var snapshots = new List<object>();
            foreach (object provider in providers)
            {
                var stacks = ReadField(provider, "_nodeIdToPacketHandlerStack") as IEnumerable
                    ?? throw new InvalidOperationException("Existing provider has no handler-stack map.");
                foreach (object pair in stacks)
                {
                    var handlers = new List<object>();
                    var stack = ReadProperty(pair, "Value") as IEnumerable
                        ?? throw new InvalidOperationException("Handler stack is not enumerable.");
                    int index = 0;
                    foreach (object handler in stack)
                    {
                        var parameters = ReadField(handler, "_setParameters") as IDictionary;
                        object? role = parameters?.Contains("Role") == true ? parameters["Role"] : null;
                        object? context = ReadProperty(ReadField(handler, "_taskLoggingContext"), "BuildEventContext");
                        var packets = new List<object>();
                        var queue = ReadField(handler, "_receivedPackets") as IEnumerable
                            ?? throw new InvalidOperationException("Handler has no packet queue.");
                        foreach (object packet in queue)
                        {
                            string packetType = packet.GetType().Name;
                            string? probeMessage = null;
                            if (packetType == "LogMessagePacket")
                            {
                                var buildEvent = ReadProperty(packet, "BuildEvent") ??
                                    ReadField(packet, "_buildEvent") ?? ReadProperty(packet, "BuildEventArgs");
                                string? message = (buildEvent as BuildEventArgs)?.Message;
                                if (message?.Contains("PROBE ", StringComparison.Ordinal) == true)
                                    probeMessage = message;
                            }
                            packets.Add(new
                            {
                                type = packetType,
                                probeMessage,
                                taskResult = packetType == "TaskHostTaskComplete"
                                    ? ReadProperty(packet, "TaskResult")?.ToString() : null
                            });
                        }
                        handlers.Add(new
                        {
                            stackIndex = index++,
                            objectIdentity = RuntimeHelpers.GetHashCode(handler),
                            role = role?.ToString(),
                            node = ReadProperty(context, "NodeId"),
                            project = ReadProperty(context, "ProjectContextId"),
                            task = ReadProperty(context, "TaskId"),
                            packets
                        });
                    }
                    snapshots.Add(new { communicationNode = ReadProperty(pair, "Key"), handlers });
                }
            }
            File.WriteAllText(output, JsonSerializer.Serialize(new
            {
                utc = DateTime.UtcNow,
                pid = Environment.ProcessId,
                hostType = host.GetType().FullName,
                providerCount = providers.Count,
                readOnly = true,
                traversalTypes = visitedTypes,
                snapshots
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception error)
        {
            File.WriteAllText(output, JsonSerializer.Serialize(new
            {
                utc = DateTime.UtcNow,
                pid = Environment.ProcessId,
                error = error.ToString(),
                traversalTypes = visitedTypes
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
