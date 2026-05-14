using System;
using System.Security;

namespace NuGet.CommandLine
{
    public interface IConsole : Common.ILogger
    {
        int CursorLeft { get; set; }
        int WindowWidth { get; set; }
        Verbosity Verbosity { get; set; }
        bool IsNonInteractive { get; set; }

        void Write(object value);
        void Write(string value);
        void Write(string format, params object[] args);

        void WriteLine();
        void WriteLine(object value);
        void WriteLine(string value);
        void WriteLine(string format, params object[] args);
        void WriteLine(ConsoleColor color, string value, params object[] args);

        void WriteError(object value);
        void WriteError(string value);
        void WriteError(string format, params object[] args);

        void WriteWarning(string value);
        void WriteWarning(bool prependWarningText, string value);
        void WriteWarning(string value, params object[] args);
        void WriteWarning(bool prependWarningText, string value, params object[] args);

        bool Confirm(string description);
        ConsoleKeyInfo ReadKey();
        string ReadLine();
        void ReadSecureString(SecureString secureString);

        void PrintJustified(int startIndex, string text);
        void PrintJustified(int startIndex, string text, int maxWidth);
    }
}
