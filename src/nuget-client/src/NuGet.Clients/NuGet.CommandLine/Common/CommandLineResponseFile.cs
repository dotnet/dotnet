#if IS_DESKTOP
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NuGet.CommandLine;

namespace NuGet.Common
{
    public static class CommandLineResponseFile
    {
        /// <summary>
        /// Takes command line args and parses response files prefixed with the @ symbol.  
        /// Response files are text files that contain command line switches. 
        /// Each switch can be on a separate line or all switches can be on one line.  
        /// Multiple response files can be defined in the args. 
        /// Nested response files are supported, but limited to a depth of 3.  
        /// A response file cannot be larger than 2mb. 
        /// </summary>
        /// <param name="args">The args to parse which can contain response files</param>
        /// <returns></returns>
        public static string[] ParseArgsResponseFiles(string[] args)
        {
            return ParseArgsResponseFiles(args, parseArgsResponseFileRecursionDepth: 0);
        }

        private static string[] ParseArgsResponseFiles(string[] args, int parseArgsResponseFileRecursionDepth)
        {
            if (args.Length == 0)
            {
                return args;
            }

            // Response files are not supported on other platforms yet
            if (!RuntimeEnvironmentHelper.IsWindows && !RuntimeEnvironmentHelper.IsMono)
            {
                return args;
            }

            const int MaxRecursionDepth = 3;
            var parsedArgs = new List<string>();
            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg) || !arg.StartsWith("@", StringComparison.InvariantCultureIgnoreCase))
                {
                    parsedArgs.Add(arg);
                    continue;
                }

                // Response file
                if (arg.Length < 2)
                {
                    throw new ArgumentException(LocalizedResourceManager.GetString("Error_ResponseFileInvalid"));
                }

                // Remove '@' symbol
                var responseFileArg = arg.Substring(1, arg.Length - 1);

                // File could be full path or relative path to working directory
                var responseFilePath = responseFileArg.Contains(Path.DirectorySeparatorChar) ?
                    responseFileArg :
                    Path.Combine(Environment.CurrentDirectory, responseFileArg);

                if (!File.Exists(responseFilePath))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_ResponseFileDoesNotExist")), arg);
                }

                var fileInfo = new FileInfo(responseFilePath);
                if (fileInfo.Length == 0)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_ResponseFileNullOrEmpty"), arg));
                }

                const int TwoMegaBytesLength = 2048000;
                if (fileInfo.Length > TwoMegaBytesLength)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_ResponseFileTooLarge"), arg, TwoMegaBytesLength / 1000000));
                }

                var responseFileContents = File.ReadAllText(responseFilePath);
                var responseFileArgs = SplitArgs(responseFileContents);
                foreach (var a in responseFileArgs)
                {
                    if (string.IsNullOrWhiteSpace(a))
                    {
                        continue;
                    }

                    if (a.StartsWith("@", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (parseArgsResponseFileRecursionDepth > MaxRecursionDepth)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_ResponseFileMaxRecursionDepth"), MaxRecursionDepth));
                        }

                        parseArgsResponseFileRecursionDepth++;
                        var nestedResponseFileArgs = ParseArgsResponseFiles(new string[] { a }, parseArgsResponseFileRecursionDepth);
                        if (nestedResponseFileArgs.Length != 0)
                        {
                            parsedArgs.AddRange(nestedResponseFileArgs);
                        }

                        continue;
                    }

                    var parsedResponseFileArg = a.Trim();
                    if (!string.IsNullOrWhiteSpace(parsedResponseFileArg))
                    {
                        parsedArgs.Add(parsedResponseFileArg);
                    }
                }
            }

            return parsedArgs.ToArray();
        }

        /// <summary>
        /// Splits up a string of command line arguments into an array
        /// </summary>
        /// <param name="unsplitArguments">The string to split</param>
        /// <returns>An array with split command line arguments</returns>
        private static string[] SplitArgs(string unsplitArguments)
        {
            if (string.IsNullOrWhiteSpace(unsplitArguments))
            {
                return Array.Empty<string>();
            }

            var ptrToSplitArgs = CommandLineToArgvW(unsplitArguments, out int numberOfArgs);
            if (ptrToSplitArgs == IntPtr.Zero)
            {
                return Array.Empty<string>();
            }

            try
            {
                var splitArgs = new string[numberOfArgs];
                for (var i = 0; i < numberOfArgs; i++)
                {
                    splitArgs[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
                }

                return splitArgs;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrToSplitArgs);
            }
        }

        /// <summary>
        /// Parses a Unicode command line string and returns an array of pointers to the command line arguments, 
        /// along with a count of such arguments, in a way that is similar to the standard C run-time argv and argc values.
        /// </summary>
        /// <param name="commandLine">Unicode string that contains the full command line.</param>
        /// <param name="argsLength">Pointer to an int that receives the number of array elements returned</param>
        /// <returns></returns>
        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string commandLine, out int argsLength);

    }
}
#endif
