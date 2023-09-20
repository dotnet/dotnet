// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin factory.
    /// </summary>
    public sealed class PluginFactory : IPluginFactory
    {
        private bool _isDisposed;
        private readonly IPluginLogger _logger;
        private readonly TimeSpan _pluginIdleTimeout;
        private readonly ConcurrentDictionary<string, Lazy<Task<IPlugin>>> _plugins;

        /// <summary>
        /// Instantiates a new <see cref="PluginFactory" /> class.
        /// </summary>
        /// <param name="pluginIdleTimeout">The plugin idle timeout.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pluginIdleTimeout" />
        /// is less than <see cref="Timeout.InfiniteTimeSpan" />.</exception>
        public PluginFactory(TimeSpan pluginIdleTimeout)
        {
            if (pluginIdleTimeout < Timeout.InfiniteTimeSpan)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pluginIdleTimeout),
                    pluginIdleTimeout,
                    Strings.Plugin_IdleTimeoutMustBeGreaterThanOrEqualToInfiniteTimeSpan);
            }

            _logger = PluginLogger.DefaultInstance;
            _pluginIdleTimeout = pluginIdleTimeout;
            _plugins = new ConcurrentDictionary<string, Lazy<Task<IPlugin>>>();
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            foreach (var entry in _plugins)
            {
                var lazyTask = entry.Value;

                if (lazyTask.IsValueCreated && lazyTask.Value.Status == TaskStatus.RanToCompletion)
                {
                    var plugin = lazyTask.Value.Result;

                    plugin.Dispose();
                }
            }

            _logger.Dispose();

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        /// <summary>
        /// Asynchronously gets an existing plugin instance or creates a new instance and connects to it.
        /// </summary>
        /// <param name="filePath">The file path of the plugin.</param>
        /// <param name="arguments">Command-line arguments to be supplied to the plugin.</param>
        /// <param name="requestHandlers">Request handlers.</param>
        /// <param name="options">Connection options.</param>
        /// <param name="sessionCancellationToken">A cancellation token for the plugin's lifetime.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Plugin" />
        /// instance.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arguments" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandlers" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="sessionCancellationToken" />
        /// is cancelled.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this object is disposed.</exception>
        /// <exception cref="ProtocolException">Thrown if a plugin protocol error occurs.</exception>
        /// <exception cref="PluginException">Thrown for a plugin failure during creation.</exception>
        /// <remarks>This is intended to be called by NuGet client tools.</remarks>
        public async Task<IPlugin> GetOrCreateAsync(
            string filePath,
            IEnumerable<string> arguments,
            IRequestHandlers requestHandlers,
            ConnectionOptions options,
            CancellationToken sessionCancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(PluginFactory));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(filePath));
            }

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (requestHandlers == null)
            {
                throw new ArgumentNullException(nameof(requestHandlers));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            sessionCancellationToken.ThrowIfCancellationRequested();

            var lazyTask = _plugins.GetOrAdd(
                filePath,
                (path) => new Lazy<Task<IPlugin>>(
                    () => CreatePluginAsync(filePath, arguments, requestHandlers, options, sessionCancellationToken)));

            await lazyTask.Value;

            // Manage plugin lifetime by its idleness.  Thus, don't allow callers to prematurely dispose of a plugin.
            return new NoOpDisposePlugin(lazyTask.Value.Result);
        }

        private async Task<IPlugin> CreatePluginAsync(
            string filePath,
            IEnumerable<string> arguments,
            IRequestHandlers requestHandlers,
            ConnectionOptions options,
            CancellationToken sessionCancellationToken)
        {
            var args = string.Join(" ", arguments);
#if IS_DESKTOP
            var startInfo = new ProcessStartInfo(filePath)
            {
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                WindowStyle = ProcessWindowStyle.Hidden,
            };
#else
            var startInfo = new ProcessStartInfo
            {
                FileName = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ??
                           (NuGet.Common.RuntimeEnvironmentHelper.IsWindows ?
                                "dotnet.exe" :
                                "dotnet"),
                Arguments = $"\"{filePath}\" " + args,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                WindowStyle = ProcessWindowStyle.Hidden,
            };
#endif
            var pluginProcess = new PluginProcess(startInfo);
            string pluginId = Plugin.CreateNewId();

            using (var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(sessionCancellationToken))
            {
                // Process ID is unavailable until we start the process; however, we want to wire up this event before
                // attempting to start the process in case the process immediately exits.
                EventHandler<IPluginProcess> onExited = null;
                Connection connection = null;

                onExited = (object eventSender, IPluginProcess exitedProcess) =>
                {
                    exitedProcess.Exited -= onExited;

                    OnPluginProcessExited(exitedProcess, pluginId);

                    if (connection?.State == ConnectionState.Handshaking)
                    {
                        combinedCancellationTokenSource.Cancel();
                    }
                };

                pluginProcess.Exited += onExited;

                var stopwatch = Stopwatch.StartNew();

                pluginProcess.Start();

                if (_logger.IsEnabled)
                {
                    WriteCommonLogMessages(_logger);
                }

                var sender = new Sender(pluginProcess.StandardInput);
                var receiver = new StandardOutputReceiver(pluginProcess);
                var processingHandler = new InboundRequestProcessingHandler(new HashSet<MessageMethod> { MessageMethod.Handshake, MessageMethod.Log });
                var messageDispatcher = new MessageDispatcher(requestHandlers, new RequestIdGenerator(), processingHandler, _logger);
                connection = new Connection(messageDispatcher, sender, receiver, options, _logger);

                var plugin = new Plugin(
                    filePath,
                    connection,
                    pluginProcess,
                    isOwnProcess: false,
                    idleTimeout: _pluginIdleTimeout,
                    id: pluginId);

                if (_logger.IsEnabled)
                {
                    _logger.Write(new PluginInstanceLogMessage(_logger.Now, plugin.Id, PluginState.Started, pluginProcess.Id));
                }

                try
                {
                    // Wire up handlers before calling ConnectAsync(...).
                    plugin.Faulted += OnPluginFaulted;
                    plugin.Idle += OnPluginIdle;

                    await connection.ConnectAsync(combinedCancellationTokenSource.Token);

                    // It's critical that this be registered after ConnectAsync(...).
                    // If it's registered before, stuff could be disposed, which would lead to unexpected exceptions.
                    // If the plugin process exited before this event is registered, an exception should be caught below.
                    plugin.Exited += OnPluginExited;
                }
                catch (ProtocolException ex)
                {
                    plugin.Dispose();

                    throw new ProtocolException(
                        string.Format(CultureInfo.CurrentCulture, Strings.Plugin_Exception, plugin.Name, ex.Message));
                }
                catch (TaskCanceledException ex)
                {
                    stopwatch.Stop();

                    plugin.Dispose();

                    throw new PluginException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.Plugin_FailedOnCreation,
                            plugin.Name,
                            stopwatch.Elapsed.TotalSeconds,
                            pluginProcess.ExitCode),
                        ex);
                }
                catch (Exception)
                {
                    plugin.Dispose();

                    throw;
                }

                return plugin;
            }
        }

        /// <summary>
        /// Asynchronously creates a plugin instance and connects to it.
        /// </summary>
        /// <param name="requestHandlers">Request handlers.</param>
        /// <param name="options">Connection options.</param>
        /// <param name="sessionCancellationToken">A cancellation token for the plugin's lifetime.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="Plugin" />
        /// instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestHandlers" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options" />
        /// is either <see langword="null" /> or empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="sessionCancellationToken" />
        /// is cancelled.</exception>
        /// <remarks>This is intended to be called by a plugin.</remarks>
        public static async Task<IPlugin> CreateFromCurrentProcessAsync(
            IRequestHandlers requestHandlers,
            ConnectionOptions options,
            CancellationToken sessionCancellationToken)
        {
            if (requestHandlers == null)
            {
                throw new ArgumentNullException(nameof(requestHandlers));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            sessionCancellationToken.ThrowIfCancellationRequested();

            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            var standardInput = new StreamReader(Console.OpenStandardInput(), encoding);
            var standardOutput = new StreamWriter(Console.OpenStandardOutput(), encoding);
            var sender = new Sender(standardOutput);
            var receiver = new StandardInputReceiver(standardInput);
            var logger = PluginLogger.DefaultInstance;

            if (logger.IsEnabled)
            {
                WriteCommonLogMessages(logger);
            }

            var messageDispatcher = new MessageDispatcher(requestHandlers, new RequestIdGenerator(), new InboundRequestProcessingHandler(), logger);
            var connection = new Connection(messageDispatcher, sender, receiver, options, logger);
            var pluginProcess = new PluginProcess();

            // Wire up handlers before calling ConnectAsync(...).
            var plugin = new Plugin(
                pluginProcess.FilePath,
                connection,
                pluginProcess,
                isOwnProcess: true,
                idleTimeout: Timeout.InfiniteTimeSpan);

            requestHandlers.TryAdd(MessageMethod.Close, new CloseRequestHandler(plugin));
            requestHandlers.TryAdd(MessageMethod.MonitorNuGetProcessExit, new MonitorNuGetProcessExitRequestHandler(plugin));

            try
            {
                await connection.ConnectAsync(sessionCancellationToken);
            }
            catch (Exception)
            {
                plugin.Dispose();

                throw;
            }

            return plugin;
        }

        private void Dispose(IPlugin plugin)
        {
            if (_logger.IsEnabled)
            {
                _logger.Write(new PluginInstanceLogMessage(_logger.Now, plugin.Id, PluginState.Disposing));
            }

            UnregisterEventHandlers(plugin as Plugin);

            Lazy<Task<IPlugin>> lazyTask;

            if (_plugins.TryRemove(plugin.FilePath, out lazyTask))
            {
                if (lazyTask.IsValueCreated && lazyTask.Value.Status == TaskStatus.RanToCompletion)
                {
                    using (var pluginSingleton = lazyTask.Value.Result)
                    {
                        SendCloseRequest(pluginSingleton);
                    }
                }
            }

            plugin.Dispose();

            if (_logger.IsEnabled)
            {
                _logger.Write(new PluginInstanceLogMessage(_logger.Now, plugin.Id, PluginState.Disposed));
            }
        }

        private void OnPluginFaulted(object sender, FaultedPluginEventArgs e)
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                Strings.Plugin_Fault,
                e.Plugin.Name,
                e.Exception.ToString());

            Console.WriteLine(message);

            Dispose(e.Plugin);
        }

        private void OnPluginExited(object sender, PluginEventArgs e)
        {
            Dispose(e.Plugin);
        }

        private void OnPluginIdle(object sender, PluginEventArgs e)
        {
            if (_logger.IsEnabled)
            {
                _logger.Write(new PluginInstanceLogMessage(_logger.Now, e.Plugin.Id, PluginState.Idle));
            }

            Dispose(e.Plugin);
        }

        // This is more reliable than OnPluginExited as this even handler is wired up before the process
        // has even started, while OnPluginExited is wired up after.
        private void OnPluginProcessExited(IPluginProcess pluginProcess, string pluginId)
        {
            if (_logger.IsEnabled)
            {
                _logger.Write(new PluginInstanceLogMessage(_logger.Now, pluginId, PluginState.Exited, pluginProcess.Id));
            }
        }

        private static void SendCloseRequest(IPlugin plugin)
        {
            var message = plugin.Connection.MessageDispatcher.CreateMessage(
                MessageType.Request,
                MessageMethod.Close);

            using (var cancellationTokenSource = new CancellationTokenSource(PluginConstants.CloseTimeout))
            {
                try
                {
                    plugin.Connection.SendAsync(message, cancellationTokenSource.Token).Wait();
                }
                catch (Exception)
                {
                }
            }
        }

        private void UnregisterEventHandlers(Plugin plugin)
        {
            if (plugin != null)
            {
                plugin.Exited -= OnPluginExited;
                plugin.Faulted -= OnPluginFaulted;
                plugin.Idle -= OnPluginIdle;
            }
        }

        private static void WriteCommonLogMessages(IPluginLogger logger)
        {
            logger.Write(new AssemblyLogMessage(logger.Now));
            logger.Write(new MachineLogMessage(logger.Now));
            logger.Write(new EnvironmentVariablesLogMessage(logger.Now));
            logger.Write(new ProcessLogMessage(logger.Now));
            logger.Write(new ThreadPoolLogMessage(logger.Now));
        }
    }
}
