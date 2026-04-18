#nullable enable

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.LinuxTracepoints.Provider;

using System;
using System.Collections.Generic;
using System.Threading;
using EventLevel = System.Diagnostics.Tracing.EventLevel;

/// <summary>
/// Manages tracepoints that will be used to generate EventHeader-encoded events.
/// Typical usage:
/// <list type="bullet"><item>
/// At program start or component initialization, create a Provider.
/// </item><item>
/// Optional: Call provider.Register(...) for each combination of Level+Keyword that
/// you will need. Registering the tracepoints during component startup (rather than
/// at first use) helps tracepoint consumers get a complete list of the tracepoints
/// that your component can generate so they can correctly subscribe to all the
/// tracepoints they need.
/// </item><item>
/// When you need to log an event, call tracepoint = provider.FindOrRegister(level, keyword)
/// to get the tracepoint for the event's level+keyword (or you can cache the tracepoints
/// to avoid runtime lookup overhead). Then check tracepoint.IsEnabled to see whether
/// the tracepoint is connected to any consumers. If tracepoint.IsEnabled returns true,
/// use an <see cref="EventHeaderDynamicBuilder"/> to build the event, then call
/// tracepoint.Write(builder, ...) to emit the event.
/// </item><item>
/// Call provider.Dispose() at component cleanup to unregister all tracepoints.
/// </item></list>
/// </summary>
internal class EventHeaderDynamicProvider : IDisposable
{
    private const int EventHeaderNameMax = 256; // includes nul
    private const int SuffixMax = 21; // "_LffKffffffffffffffff"

    private readonly ReaderWriterLockSlim mutex;
    private readonly Dictionary<LevelKeyword, EventHeaderDynamicTracepoint> tracepoints;
    private int disposed;

    /// <summary>
    /// Initializes a new provider with the specified provider name and options.
    /// </summary>
    /// <param name="name">
    /// Provider name, e.g. "MyCompany_MyOrg_MyComponent".
    /// This must less than ~230 chars in length, must be ASCII only (no chars with value
    /// greater than 127), and must not contain ' ', ':', or '\0' chars.
    /// <br/>
    /// For best results, use only letters, numbers, '_', and ';' in the name (some
    /// components such as tracefs seem to ignore tracepoints that contain other chars).
    /// </param>
    /// <param name="providerOptions">Optional configuration for the provider.</param>
    /// <exception cref="ArgumentException">name contains invalid chars (non-ASCII, '\0', ' ', or ':').</exception>
    /// <exception cref="ArgumentOutOfRangeException">name is too long</exception>
    public EventHeaderDynamicProvider(string name, EventHeaderDynamicProviderOptions? providerOptions = null)
    {
        foreach (var ch in name)
        {
            if (ch == 0 || ch == ' ' || ch == ':' || ch > 127)
            {
                var charName = ch == 0 ? "\\0" : ch.ToString();
                throw new ArgumentException(
                    "Invalid char '" + charName + "' in providerName (must be ASCII and not nul, space, or colon).",
                    nameof(name));
            }
        }

        var options = providerOptions == null || providerOptions.GroupName.Length == 0
            ? ""
            : "G" + providerOptions.GroupName;
        if (name.Length + SuffixMax + options.Length >= EventHeaderNameMax)
        {
            throw new ArgumentOutOfRangeException(
                nameof(name),
                nameof(name) + ".Length + " + nameof(providerOptions) + ".GroupName.Length is too long (limit is ~234).");
        }

        this.mutex = new();
        this.tracepoints = new();
        this.Name = name;
        this.Options = options;
    }

    /// <summary>
    /// Gets the provider name provided to the constructor, e.g. "MyCompany_MyOrg_MyComponent".
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the provider options string. If no provider options were specified, this is "".
    /// If a provider group such as "mygroup" was specified, this is "Gmygroup".
    /// </summary>
    public string Options { get; }

    /// <summary>
    /// Returns e.g. "ProviderName_L*K*" or "ProviderName_L*K*Ggroup".
    /// </summary>
    public override string ToString()
    {
        return this.Name + "_L*K*" + this.Options;
    }

    /// <summary>
    /// Releases all resources, unregisters all tracepoints, clears the
    /// list of registered tracepoints. After Dispose, Find and Register will
    /// throw ObjectDisposedException.
    /// </summary>
    /// <exception cref="SynchronizationLockException">
    /// A call to Find or Register is in progress.
    /// </exception>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finds and returns a tracepoint that was previously registered by
    /// Register(level, keyword). If no such tracepoint has been registered, returns
    /// null.
    /// <br/>
    /// Throws if level > 255 or if the provider is disposed.
    /// <br/>
    /// This method is thread-safe (uses a reader lock).
    /// </summary>
    /// <exception cref="OverflowException">level is greater than 255.</exception>
    /// <exception cref="ObjectDisposedException">The provider has been disposed.</exception>
    public EventHeaderDynamicTracepoint? Find(EventLevel level, UInt64 keyword)
    {
        var key = new LevelKeyword(checked((byte)level), keyword); // OverflowException

        EventHeaderDynamicTracepoint? existingTp;
        this.mutex.EnterReadLock(); // ObjectDisposedException
        try
        {
            this.tracepoints.TryGetValue(key, out existingTp);
        }
        finally
        {
            this.mutex.ExitReadLock();
        }

        return existingTp;
    }

    /// <summary>
    /// Finds and returns a tracepoint that was previously registered by
    /// Register(level, keyword). If no such tracepoint has been registered, registers
    /// a new tracepoint with the specified level and keyword and returns it.
    /// <br/>
    /// Throws if level > 255 or if provider is disposed.
    /// <br/>
    /// This method is thread-safe (find uses a reader lock, register uses a writer lock).
    /// </summary>
    /// <exception cref="OverflowException">level is greater than 255.</exception>
    /// <exception cref="ObjectDisposedException">The provider has been disposed.</exception>
    public EventHeaderDynamicTracepoint FindOrRegister(EventLevel level, UInt64 keyword)
    {
        return this.Find(level, keyword) ?? this.Register(level, keyword, true);
    }

    /// <summary>
    /// Registers a new tracepoint with the specified level and keyword and returns it.
    /// <br/>
    /// Throws if level > 255, if the provider is disposed, or if a tracepoint with
    /// the specified level and keyword is already registered on this provider.
    /// <br/>
    /// This method is thread-safe (uses a writer lock).
    /// </summary>
    /// <exception cref="OverflowException">level is greater than 255.</exception>
    /// <exception cref="ObjectDisposedException">The provider has been disposed.</exception>
    /// <exception cref="ArgumentException">
    /// A tracepoint with the specified level and keyword is already registered on this provider.
    /// </exception>
    public EventHeaderDynamicTracepoint Register(EventLevel level, UInt64 keyword)
    {
        return this.Register(level, keyword, false);
    }

    private EventHeaderDynamicTracepoint Register(EventLevel level, UInt64 keyword, bool tryAdd)
    {
        var key = new LevelKeyword(checked((byte)level), keyword); // OverflowException
        var newTracepoint = new EventHeaderDynamicTracepoint(this, key.LevelByte, key.Keyword, PerfUserEventReg.None);
        try
        {
            this.mutex.EnterWriteLock(); // ObjectDisposedException
            try
            {
                if (!tryAdd)
                {
                    this.tracepoints.Add(key, newTracepoint); // ArgumentException
                }
                else if (!this.tracepoints.TryAdd(key, newTracepoint))
                {
                    // FindOrRegister: Find returned null, but it must have been added on
                    // another thread. Return the existing one. (Finally will clean up the
                    // new one.)
                    return this.tracepoints[key];
                }

                // Successfully added new one. Keep and return it.
                var result = newTracepoint;
                newTracepoint = null; // So Finally doesn't clean it up.
                return result;
            }
            finally
            {
                this.mutex.ExitWriteLock();
            }
        }
        finally
        {
            ((IDisposable?)newTracepoint)?.Dispose();
        }
    }

    /// <summary>
    /// If disposing: calls mutex.Dispose(), unregisters all tracepoints,
    /// clears the tracepoint list.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    /// <exception cref="SynchronizationLockException">
    /// disposing is true and a call to Find or Register is in progress.
    /// </exception>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && 0 == Interlocked.Exchange(ref this.disposed, 1))
        {
            this.mutex.Dispose();
            foreach (var kv in this.tracepoints)
            {
                ((IDisposable)kv.Value).Dispose();
            }
            this.tracepoints.Clear();
        }
    }

    private readonly struct LevelKeyword : IEquatable<LevelKeyword>
    {
        public readonly UInt64 Keyword;
        public readonly byte LevelByte;

        public LevelKeyword(byte levelByte, UInt64 keyword)
        {
            this.Keyword = keyword;
            this.LevelByte = levelByte;
        }

        public override bool Equals(object? obj)
        {
            if (obj is LevelKeyword other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(LevelKeyword other)
        {
            return this.Keyword == other.Keyword && this.LevelByte == other.LevelByte;
        }

        public override int GetHashCode()
        {
            return unchecked((this.Keyword.GetHashCode() ^ this.LevelByte) * 0x01000193);
        }
    }
}
