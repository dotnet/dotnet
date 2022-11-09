// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Credentials
{
    /// <summary>
    /// Provider that handles calling command line credential providers
    /// </summary>
    public class PluginCredentialProvider : ICredentialProvider
    {
        private readonly Common.ILogger _logger;
        private readonly string _verbosity;
        private const string NormalVerbosity = "normal";
        private const string CrossPlatformPluginLink = "https://aka.ms/nuget-cross-platform-authentication-plugin";
        private int _deprecationMessageWarningLogged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">IConsole logger to use for debug logging. No secrets should ever be written to this log.</param>
        /// <param name="path">Fully qualified plugin application path.</param>
        /// <param name="timeoutSeconds">Max timeout to wait for the plugin application
        /// to return credentials.</param>
        /// <param name="verbosity">Verbosity string to pass to the plugin.</param>
        public PluginCredentialProvider(Common.ILogger logger, string path, int timeoutSeconds, string verbosity)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (verbosity == null)
            {
                throw new ArgumentNullException(nameof(verbosity));
            }

            _logger = logger;
            _verbosity = verbosity;
            Path = path;
            TimeoutSeconds = timeoutSeconds;
            var filename = System.IO.Path.GetFileName(path);
            Id = $"{typeof(PluginCredentialProvider).Name}_{filename}_{Guid.NewGuid()}";
        }

        /// <summary>
        /// Unique identifier of this credential provider
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Call the plugin credential provider application to acquire credentials.
        /// The request will be passed to the plugin on standard input as a json serialized
        /// PluginCredentialRequest.
        /// The plugin will return credentials as a json serialized PluginCredentialResponse.
        /// Valid credentials will be returned, or null if the provide cannot provide credentials
        /// for the given request.  If the plugin returns an Abort message, an exception will be thrown to
        /// fail the current request.
        /// </summary>
        /// <param name="uri">The uri of a web resource for which credentials are needed.</param>
        /// <param name="proxy">Ignored.  Proxy information will not be passed to plugins.</param>
        /// <param name="type">
        /// The type of credential request that is being made. Note that this implementation of
        /// <see cref="ICredentialProvider"/> does not support providing proxy credenitials and treats
        /// all other types the same.
        /// </param>
        /// <param name="isRetry">If true, credentials were previously supplied by this
        /// provider for the same uri.</param>
        /// <param name="message">A message provided by NuGet to show to the user when prompting.</param>
        /// <param name="nonInteractive">If true, the plugin must not prompt for credentials.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A credential object.</returns>
        public Task<CredentialResponse> GetAsync(
            Uri uri,
            IWebProxy proxy,
            CredentialRequestType type,
            string message,
            bool isRetry,
            bool nonInteractive,
            CancellationToken cancellationToken)
        {
            CredentialResponse taskResponse;
            if (type == CredentialRequestType.Proxy)
            {
                taskResponse = new CredentialResponse(CredentialStatus.ProviderNotApplicable);
                return Task.FromResult(taskResponse);
            }

            try
            {
                var request = new PluginCredentialRequest
                {
                    Uri = uri.AbsoluteUri,
                    IsRetry = isRetry,
                    NonInteractive = nonInteractive,
                    Verbosity = _verbosity
                };
                PluginCredentialResponse response;
                if (Interlocked.CompareExchange(ref _deprecationMessageWarningLogged, 1, 0) == 0)
                {
                    _logger.LogWarning(string.Format(CultureInfo.CurrentCulture, Resources.PluginWarning_PluginIsBeingDeprecated, Path, CrossPlatformPluginLink));
                }

                try
                {
                    response = GetPluginResponse(request, cancellationToken);
                }
                catch (PluginUnexpectedStatusException) when (PassVerbosityFlag(request))
                {
                    // older providers may throw if the verbosity flag is sent,
                    // so retry without it
                    request.Verbosity = null;
                    response = GetPluginResponse(request, cancellationToken);
                }

                if (response.IsValid)
                {
                    var result = new AuthTypeFilteredCredentials(
                        new NetworkCredential(response.Username, response.Password),
                        response.AuthTypes ?? Enumerable.Empty<string>());

                    taskResponse = new CredentialResponse(result);
                }
                else
                {
                    taskResponse = new CredentialResponse(CredentialStatus.ProviderNotApplicable);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (PluginException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw PluginException.Create(Path, e);
            }

            return Task.FromResult(taskResponse);
        }

        /// <summary>
        /// Path to plugin credential provider executable.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Seconds to wait for plugin credential service to respond.
        /// </summary>
        public int TimeoutSeconds { get; }

        private PluginCredentialResponse GetPluginResponse(PluginCredentialRequest request,
            CancellationToken cancellationToken)
        {
            var argumentString =
                $"-uri {request.Uri}"
                + (request.IsRetry ? " -isRetry" : string.Empty)
                + (request.NonInteractive ? " -nonInteractive" : string.Empty);

            // only apply -verbosity flag if set and != Normal
            // since normal is default
            if (PassVerbosityFlag(request))
            {
                argumentString += $" -verbosity {request.Verbosity.ToLower(CultureInfo.InvariantCulture)}";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = Path,
                Arguments = argumentString,
#if IS_DESKTOP                
                WindowStyle = ProcessWindowStyle.Hidden,
                ErrorDialog = false,
#endif
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            string stdOut = null;
            var exitCode = Execute(startInfo, cancellationToken, out stdOut);

            var status = (PluginCredentialResponseExitCode)exitCode;

            PluginCredentialResponse credentialResponse;
            try
            {
                // Mono will add utf-16 byte order mark to the start of stdOut, remove it here.
                credentialResponse =
                    JsonConvert.DeserializeObject<PluginCredentialResponse>(stdOut.Trim(new char[] { '\uFEFF' }))
                    ?? new PluginCredentialResponse();
            }
            catch (Exception)
            {
                // Do not expose stdout message, since it may contain credentials
                throw PluginException.CreateUnreadableResponseExceptionMessage(Path, status);
            }

            switch (status)
            {
                case PluginCredentialResponseExitCode.Success:
                    if (!credentialResponse.IsValid)
                    {
                        throw PluginException.CreateInvalidResponseExceptionMessage(
                            Path,
                            status,
                            credentialResponse);
                    }

                    return credentialResponse;

                case PluginCredentialResponseExitCode.ProviderNotApplicable:
                    credentialResponse.Username = null;
                    credentialResponse.Password = null;

                    return credentialResponse;

                case PluginCredentialResponseExitCode.Failure:
                    throw PluginException.CreateAbortMessage(Path, credentialResponse.Message);

                default:
                    throw PluginUnexpectedStatusException.CreateUnexpectedStatusMessage(Path, status);
            }
        }

        public virtual int Execute(ProcessStartInfo startInfo, CancellationToken cancellationToken, out string stdOut)
        {
            var outBuffer = new StringBuilder();

            cancellationToken.ThrowIfCancellationRequested();

            var process = Process.Start(startInfo);
            if (process == null)
            {
                throw PluginException.CreateNotStartedMessage(Path);
            }

            process.OutputDataReceived += (object o, DataReceivedEventArgs e) => { outBuffer.AppendLine(e.Data); };

            // Trace and error information may be written to standard error by the provider.
            // It should be logged at the Information level so it will appear if Verbosity >= Normal.
            process.ErrorDataReceived += (object o, DataReceivedEventArgs e) =>
            {
                if (!string.IsNullOrWhiteSpace(e?.Data))
                {
                    // This is a workaround for mono issue: https://github.com/NuGet/Home/issues/4004
                    if (!process.HasExited)
                    {
                        _logger.LogInformation($"{process.ProcessName}: {e.Data}");
                    }
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using (cancellationToken.Register(() => Kill(process)))
            {
                if (!process.WaitForExit(TimeoutSeconds * 1000))
                {
                    Kill(process);
                    throw PluginException.CreateTimeoutMessage(Path, TimeoutSeconds);
                }
                // Give time for the Async event handlers to finish by calling WaitForExit again.
                // if the first one succeeded
                // Note: Read remarks from https://msdn.microsoft.com/en-us/library/ty0d8k56(v=vs.110).aspx
                // for reason.
                process.WaitForExit();
            }

            process.CancelErrorRead();
            process.CancelOutputRead();
            cancellationToken.ThrowIfCancellationRequested();

            stdOut = outBuffer.ToString();
            return process.ExitCode;
        }

        private bool PassVerbosityFlag(PluginCredentialRequest request)
        {
            return request.Verbosity != null
                && !string.Equals(request.Verbosity, NormalVerbosity, StringComparison.OrdinalIgnoreCase);
        }

        private static void Kill(Process p)
        {
            if (p.HasExited)
            {
                return;
            }

            try
            {
                p.Kill();
            }
            catch (InvalidOperationException)
            {
                // the process may have exited, 
                // in this case ignore the exception
            }
        }
    }
}
