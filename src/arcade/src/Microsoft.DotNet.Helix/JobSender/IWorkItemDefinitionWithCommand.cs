namespace Microsoft.DotNet.Helix.Client
{
    /// <summary>
    /// Work item definition that lacks required information about
    /// what command to run.
    /// </summary>
    public interface IWorkItemDefinitionWithCommand
    {
        /// <summary>
        /// Specify the actual command to be processed as your job
        /// on one of the machines of the queue you selected.
        /// </summary>
        /// <param name="command">
        /// Any command (like <strong>ls</strong> or <strong>dotnet test Solution.sln</strong>)
        /// you want to be processed. The command is placed inside a script file: sh for linux
        /// and bat for windows. This means that if the command is <strong>RunTests.bat</strong>
        /// on windows it must be prefixed by call to avoid cmd.exe problems.
        /// </param>
        IWorkItemDefinitionWithPayload WithCommand(string command);
    }
}
