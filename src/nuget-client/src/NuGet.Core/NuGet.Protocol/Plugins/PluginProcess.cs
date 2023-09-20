// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Represents a plugin process.
    /// </summary>
    public sealed class PluginProcess : IPluginProcess
    {
        private int? _exitCode;
        private bool _hasStarted;
        private int? _id;
        private bool _isDisposed;
        private readonly Process _process;
        private readonly ProcessStartInfo _startInfo;

        /// <summary>
        /// Occurs when a process exits.
        /// </summary>
        public event EventHandler<IPluginProcess> Exited;

        /// <summary>
        /// Occurs when a line of output has been received.
        /// </summary>
        public event EventHandler<LineReadEventArgs> LineRead;

        public int? ExitCode
        {
            get
            {
                UpdateExitCodeIfNecessary();

                return _exitCode;
            }
        }

        internal string FilePath => _process.MainModule.FileName;

        /// <summary>
        /// Gets the process ID if the process was started; otherwise, <see langword="null" />.
        /// </summary>
        public int? Id
        {
            get
            {
                UpdateIdIfNecessary();

                return _id;
            }
        }

        internal StreamWriter StandardInput => _process.StandardInput;

        /// <summary>
        /// Instantiates a new <see cref="PluginProcess" /> class from the current process.
        /// </summary>
        public PluginProcess()
        {
            _process = Process.GetCurrentProcess();
            _hasStarted = true;
        }

        /// <summary>
        /// Instantiates a new <see cref="PluginProcess" /> class.
        /// </summary>
        /// <param name="startInfo">A plugin process.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="startInfo" /> is <see langword="null" />.</exception>
        public PluginProcess(ProcessStartInfo startInfo)
        {
            if (startInfo == null)
            {
                throw new ArgumentNullException(nameof(startInfo));
            }

            _startInfo = startInfo;
            _process = new Process();
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

            _process.OutputDataReceived -= OnOutputDataReceived;
            _process.Dispose();

            GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        /// <summary>
        /// Asynchronously starts reading the standard output stream.
        /// </summary>
        public void BeginReadLine()
        {
            _process.BeginOutputReadLine();
        }

        /// <summary>
        /// Cancels asynchronous reading of the standard output stream.
        /// </summary>
        public void CancelRead()
        {
            _process.CancelOutputRead();
        }

        /// <summary>
        /// Stops the associated process.
        /// </summary>
        public void Kill()
        {
            try
            {
                // Give some time for the plugin process to shutdown cleanly, flush write buffers, etc.
                if (!_process.HasExited && !_process.WaitForExit(milliseconds: 1000))
                {
                    _process.Kill();
                }
            }
            catch (Exception)
            {
            }

            UpdateExitCodeIfNecessary();
            UpdateIdIfNecessary();
        }

        public void Start()
        {
            if (_hasStarted)
            {
                throw new InvalidOperationException();
            }

            _process.OutputDataReceived += OnOutputDataReceived;
            _process.Exited += OnProcessExited;

            _process.EnableRaisingEvents = true;
            _process.StartInfo = _startInfo;

            _hasStarted = true;

            _process.Start();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LineRead?.Invoke(sender, new LineReadEventArgs(e.Data));
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            if (sender is Process process)
            {
                process.Exited -= OnProcessExited;

                UpdateExitCodeIfNecessary();
                UpdateIdIfNecessary();
            }

            Exited?.Invoke(this, this);
        }

        private void UpdateExitCodeIfNecessary()
        {
            if (!_exitCode.HasValue)
            {
                try
                {
                    _exitCode = _process.ExitCode;
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private void UpdateIdIfNecessary()
        {
            if (!_id.HasValue)
            {
                try
                {
                    _id = _process.Id;
                }
                catch (InvalidOperationException)
                {
                }
            }
        }
    }
}
