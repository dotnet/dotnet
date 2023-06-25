using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.CommandLine
{
    public class Console : LoggerBase, IConsole
    {
        /// <summary>
        /// All operations writing to Out should be wrapped in a lock to 
        /// avoid color mismatches during parallel operations.
        /// </summary>
        private readonly static object _writerLock = new object();

        [System.Runtime.InteropServices.DllImport("libc", EntryPoint = "isatty")]
        extern static int _isatty(int fd);

        public Console()
        {
            // setup CancelKeyPress handler so that the console colors are
            // restored to their original values when nuget.exe is interrupted
            // by Ctrl-C.
            var originalForegroundColor = System.Console.ForegroundColor;
            var originalBackgroundColor = System.Console.BackgroundColor;
            System.Console.CancelKeyPress += (sender, e) =>
            {
                System.Console.ForegroundColor = originalForegroundColor;
                System.Console.BackgroundColor = originalBackgroundColor;
            };
        }

        public int CursorLeft
        {
            get
            {
                if (MockCursorLeft != null)
                {
                    return (int)MockCursorLeft;
                }

                try
                {
                    return System.Console.CursorLeft;
                }
                catch (IOException)
                {
                    return 0;
                }
            }
            set
            {
                System.Console.CursorLeft = value;
            }
        }

        public int WindowWidth
        {
            get
            {
                if (MockWindowWidth != null)
                {
                    return (int)MockWindowWidth;
                }

                try
                {
                    var width = System.Console.WindowWidth;
                    if (width > 0)
                    {
                        return width;
                    }
                    else
                    {
                        // This happens when redirecting output to a file, on
                        // Linux and OS X (running with Mono).
                        return int.MaxValue;
                    }
                }
                catch (IOException)
                {
                    // probably means redirected to file
                    return int.MaxValue;
                }
            }
            set
            {
                System.Console.WindowWidth = value;
            }
        }


        internal int? MockWindowWidth { get; set; }

        internal int? MockCursorLeft { get; set; }

        private Verbosity _verbosity;

        public Verbosity Verbosity
        {
            get
            {
                return _verbosity;
            }

            set
            {
                _verbosity = value;
                VerbosityLevel = GetVerbosityLevel(_verbosity);
            }
        }

        public bool IsNonInteractive
        {
            get; set;
        }

        private TextWriter Out
        {
            get { return Verbosity == Verbosity.Quiet ? TextWriter.Null : System.Console.Out; }
        }

        public void Write(object value)
        {
            lock (_writerLock)
            {
                Out.Write(value);
            }
        }

        public void Write(string value)
        {
            lock (_writerLock)
            {
                Out.Write(value);
            }
        }

        public void Write(string format, params object[] args)
        {
            lock (_writerLock)
            {
                if (args == null || !args.Any())
                {
                    // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                    // and contained braces (for instance html)
                    Out.Write(format);
                }
                else
                {
                    Out.Write(format, args);
                }
            }
        }

        public void WriteLine()
        {
            lock (_writerLock)
            {
                Out.WriteLine();
            }
        }

        public void WriteLine(object value)
        {
            lock (_writerLock)
            {
                Out.WriteLine(value);
            }
        }

        public void WriteLine(string value)
        {
            lock (_writerLock)
            {
                Out.WriteLine(value);
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            lock (_writerLock)
            {
                Out.WriteLine(format, args);
            }
        }

        public void WriteError(object value)
        {
            WriteError(value.ToString());
        }

        public void WriteError(string value)
        {
            WriteError(value, Array.Empty<object>());
        }

        public void WriteError(string format, params object[] args)
        {
            WriteColor(System.Console.Error, ConsoleColor.Red, format, args);
        }

        public void WriteWarning(string value)
        {
            WriteWarning(prependWarningText: true, value: value, args: Array.Empty<object>());
        }

        public void WriteWarning(bool prependWarningText, string value)
        {
            WriteWarning(prependWarningText, value, Array.Empty<object>());
        }

        public void WriteWarning(string value, params object[] args)
        {
            WriteWarning(prependWarningText: true, value: value, args: args);
        }

        public void WriteWarning(bool prependWarningText, string value, params object[] args)
        {
            string message = prependWarningText
                                 ? String.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("CommandLine_Warning"), value)
                                 : value;

            WriteColor(System.Console.Out, ConsoleColor.Yellow, message, args);
        }

        public void WriteLine(ConsoleColor color, string value, params object[] args)
        {
            WriteColor(Out, color, value, args);
        }

        private static void WriteColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            lock (_writerLock)
            {
                var currentColor = System.Console.ForegroundColor;
                try
                {
                    currentColor = System.Console.ForegroundColor;
                    System.Console.ForegroundColor = color;
                    if (args == null || !args.Any())
                    {
                        // If it doesn't look like something that needs to be formatted, don't format it.
                        writer.WriteLine(value);
                    }
                    else
                    {
                        writer.WriteLine(value, args);
                    }
                }
                finally
                {
                    System.Console.ForegroundColor = currentColor;
                }
            }
        }

        public void PrintJustified(int startIndex, string text)
        {
            PrintJustified(startIndex, text, WindowWidth);
        }

        public void PrintJustified(int startIndex, string text, int maxWidth)
        {
            if (maxWidth > startIndex)
            {
                maxWidth = maxWidth - startIndex;
            }

            lock (_writerLock)
            {
                using (StringReader stringReader = new StringReader(text))
                {
                    string line;
                    while ((line = stringReader.ReadLine()) != null)
                    {
                        // Trim extra whitespace at the beginning and end of the line
                        line = line.Trim();

                        if (line == string.Empty)
                        {
                            Out.WriteLine();
                            continue;
                        }

                        while (true)
                        {
                            // Calculate the number of chars to print based on the width of the System.Console
                            int length = Math.Min(line.Length, maxWidth);

                            string content = line.Substring(0, length);
                            content = content.TrimEnd();

                            int leftPadding = startIndex + content.Length - CursorLeft;

                            // Print it with the correct padding
                            Out.WriteLine((leftPadding > 0) ? content.PadLeft(leftPadding) : content);

                            line = line.Substring(content.Length).Trim();

                            if (line.Trim() == string.Empty)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public bool Confirm(string description)
        {
            if (IsNonInteractive)
            {
                return true;
            }

            var currentColor = ConsoleColor.Gray;
            try
            {
                currentColor = System.Console.ForegroundColor;
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.Write(String.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("ConsoleConfirmMessage"), description));
                var result = System.Console.ReadLine();
                return result.StartsWith(LocalizedResourceManager.GetString("ConsoleConfirmMessageAccept"), StringComparison.OrdinalIgnoreCase);
            }
            finally
            {
                System.Console.ForegroundColor = currentColor;
            }
        }

        public ConsoleKeyInfo ReadKey()
        {
            EnsureInteractive();
            return System.Console.ReadKey(intercept: true);
        }

        public string ReadLine()
        {
            EnsureInteractive();
            return System.Console.ReadLine();
        }

        public void ReadSecureString(SecureString secureString)
        {
            EnsureInteractive();
            try
            {
                ReadSecureStringFromConsole(secureString);
            }
            catch (InvalidOperationException)
            {
                // This can happen when you redirect nuget.exe input, either from the shell with "<" or 
                // from code with ProcessStartInfo. 
                // In this case, just read data from Console.ReadLine()
                foreach (var c in ReadLine())
                {
                    secureString.AppendChar(c);
                }
            }
            secureString.MakeReadOnly();
        }

        private static void ReadSecureStringFromConsole(SecureString secureString)
        {
            // When you redirect nuget.exe input, either from the shell with "<" or
            // from code with ProcessStartInfo, throw exception on mono.
            if (!RuntimeEnvironmentHelper.IsWindows && RuntimeEnvironmentHelper.IsMono && _isatty(1) != 1)
            {
                throw new InvalidOperationException();
            }
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = System.Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (secureString.Length < 1)
                    {
                        continue;
                    }
                    System.Console.SetCursorPosition(System.Console.CursorLeft - 1, System.Console.CursorTop);
                    System.Console.Write(' ');
                    System.Console.SetCursorPosition(System.Console.CursorLeft - 1, System.Console.CursorTop);
                    secureString.RemoveAt(secureString.Length - 1);
                }
                else
                {
                    secureString.AppendChar(keyInfo.KeyChar);
                    System.Console.Write('*');
                }
            }
            System.Console.WriteLine();
        }

        private void EnsureInteractive()
        {
            if (IsNonInteractive)
            {
                throw new InvalidOperationException(LocalizedResourceManager.GetString("Error_CannotPromptForInput"));
            }
        }

        public override void Log(ILogMessage message)
        {
            if (DisplayMessage(message.Level))
            {
                if (message.Level == LogLevel.Debug)
                {
                    WriteColor(Out, ConsoleColor.Gray, message.Message);
                }
                else if (message.Level == LogLevel.Warning)
                {
                    WriteWarning(message.FormatWithCode());
                }
                else if (message.Level == LogLevel.Error)
                {
                    // Use standard error format for Packaging Errors
                    if (message.Code >= NuGetLogCode.NU5000 && message.Code <= NuGetLogCode.NU5500)
                    {
                        WriteError(string.Concat(LocalizedResourceManager.GetString("Error"), " ", message.FormatWithCode()));
                    }
                    else
                    {
                        WriteError(message.FormatWithCode());
                    }
                }
                else
                {
                    // Verbose, Information
                    WriteLine(message.Message);
                }
            }
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);

            return Task.CompletedTask;
        }

        private static LogLevel GetVerbosityLevel(Verbosity level)
        {
            switch (level)
            {
                case Verbosity.Detailed:
                    return LogLevel.Debug;
                case Verbosity.Normal:
                    return LogLevel.Information;
                case Verbosity.Quiet:
                    return LogLevel.Warning;
            }

            return LogLevel.Information;
        }
    }
}
