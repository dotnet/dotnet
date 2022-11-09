// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.Deployment.Launcher
{
    internal class Logger
    {
        private static bool AppendToExistingLog = true;
        private static Logger Instance = new Logger();

        internal StreamWriter Writer { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        private Logger()
        {
            Writer = null;
        }

        /// <summary>
        /// Constructor that instantiates a new object with provided StreamWriter.
        /// </summary>
        /// <param name="sw">StreaWriter object</param>
        private Logger(StreamWriter sw)
        {
            Writer = sw;
        }

        /// <summary>
        /// Writes text message to log file.
        /// </summary>
        /// <param name="message">Text message</param>
        private void WriteLine(string message)
        {
            if (Writer != null)
            {
                Writer.WriteLine(message);
            }
        }

        /// <summary>
        /// Logs header text of new Launcher instance.
        /// </summary>
        internal void LogInstanceHeader()
        {
            LogInfo(DateTime.Now.ToString());
        }

        /// <summary>
        /// Logs footer text.
        /// </summary>
        private static void LogInstanceFooter()
        {
            LogInfo("");
        }

        /// <summary>
        /// Closes logger, which closes StreamWriter if it exists.
        /// </summary>
        private void Close()
        {
            if (Writer != null)
            {
                LogInstanceFooter();
                Writer.Close();
                Writer = null;
            }
        }

        /// <summary>
        /// Starts logging, using new StreamReader, and updates the Instance object.
        /// </summary>
        /// <param name="path"></param>
        internal static void StartLogging(string path)
        {
            if (Instance.Writer != null || string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                StreamWriter sw = new StreamWriter(path, AppendToExistingLog);
                Instance = new Logger(sw);
                Instance.LogInstanceHeader();
            }
            catch (Exception)
            {
                // Failed to open the log file.
                // Logging is optional, do not fail the Launcher.
            }
        }

        /// <summary>
        /// Stops logging.
        /// </summary>
        internal static void StopLogging()
        {
            Instance.Close();
        }

        /// <summary>
        /// Logs an info message with specified string format and optional arguments.
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="args">Arguments</param>
        internal static void LogInfo(string format, params object[] args)
        {
            LogInfo(string.Format(format, args));
        }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="info"></param>
        internal static void LogInfo(string info)
        {
            string[] ss = info.Split('\n');

            foreach (string line in ss)
            {
                Instance.WriteLine(line);
            }
        }

        /// <summary>
        /// Logs an error message with specified string format and optional arguments.
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="args">Arguments</param>
        internal static void LogError(string format, params object[] args)
        {
            LogError(string.Format(format, args));
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="error">Error message</param>
        internal static void LogError(string error)
        {
            string[] ss = error.Split('\n');

            foreach (string line in ss)
            {
                Instance.WriteLine("*** Error: " + line);
            }
        }
    }
}
