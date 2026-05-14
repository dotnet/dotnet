using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace NuGet.CommandLine
{
    [InheritedExport]
    public interface ICommand
    {
        string CurrentDirectory { get; set; }

        CommandAttribute CommandAttribute { get; }

        DeprecatedCommandAttribute DeprecatedCommandAttribute { get; }

        IList<string> Arguments { get; }

        void Execute();

        /// <summary>
        /// Returns a value indicating whether the specified option should be included in 
        /// the output of the help command.
        /// </summary>
        /// <param name="optionName">The name of the option.</param>
        /// <returns>True if the option should be included in the output of the help command;
        /// otherwise, false.</returns>
        bool IncludedInHelp(string optionName);
    }
}
