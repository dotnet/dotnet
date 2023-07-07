// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Build.Tasks.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Build.Tasks.Whidbey.Unittest, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.SafeDirectories)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Build.Tasks.Core.dll")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Build.Tasks.Core.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® Build Tools®")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("15.7.179.6572")]
[assembly: System.Reflection.AssemblyInformationalVersion("15.7.179+gac19036b0d")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyVersionAttribute("15.1.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Build.Tasks
{
    public partial class AssignCulture : TaskExtension
    {
        public AssignCulture() { }

        [Framework.Output]
        public Framework.ITaskItem[] AssignedFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] AssignedFilesWithCulture { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] AssignedFilesWithNoCulture { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] CultureNeutralAssignedFiles { get { throw null; } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class AssignLinkMetadata : TaskExtension
    {
        public AssignLinkMetadata() { }

        public Framework.ITaskItem[] Items { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] OutputItems { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class AssignProjectConfiguration : ResolveProjectBase
    {
        public bool AddSyntheticProjectReferencesForSolutionDependencies { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] AssignedProjects { get { throw null; } set { } }

        public string CurrentProject { get { throw null; } set { } }

        public string CurrentProjectConfiguration { get { throw null; } set { } }

        public string CurrentProjectPlatform { get { throw null; } set { } }

        public string DefaultToVcxPlatformMapping { get { throw null; } set { } }

        public bool OnlyReferenceAndBuildProjectsEnabledInSolutionConfiguration { get { throw null; } set { } }

        public string OutputType { get { throw null; } set { } }

        public bool ResolveConfigurationPlatformUsingMappings { get { throw null; } set { } }

        public bool ShouldUnsetParentConfigurationAndPlatform { get { throw null; } set { } }

        public string SolutionConfigurationContents { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] UnassignedProjects { get { throw null; } set { } }

        public string VcxToDefaultPlatformMapping { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class AssignTargetPath : TaskExtension
    {
        public AssignTargetPath() { }

        [Framework.Output]
        public Framework.ITaskItem[] AssignedFiles { get { throw null; } }

        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        [Framework.Required]
        public string RootFolder { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    [Framework.RunInMTA]
    public partial class CallTarget : TaskExtension
    {
        public CallTarget() { }

        public bool RunEachTargetSeparately { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] TargetOutputs { get { throw null; } }

        public string[] Targets { get { throw null; } set { } }

        public bool UseResultsCache { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    [System.Obsolete("The CodeTaskFactory is not supported on .NET Core.  This class is included so that users receive run-time errors and should not be used for any other purpose.", true)]
    public sealed partial class CodeTaskFactory : Framework.ITaskFactory
    {
        public string FactoryName { get { throw null; } }

        public System.Type TaskType { get { throw null; } }

        public void CleanupTask(Framework.ITask task) { }

        public Framework.ITask CreateTask(Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }

        public Framework.TaskPropertyInfo[] GetTaskParameters() { throw null; }

        public bool Initialize(string taskName, System.Collections.Generic.IDictionary<string, Framework.TaskPropertyInfo> parameterGroup, string taskBody, Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }
    }

    public partial class CombinePath : TaskExtension
    {
        public CombinePath() { }

        public string BasePath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] CombinedPaths { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Paths { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class CommandLineBuilderExtension : Utilities.CommandLineBuilder
    {
        public CommandLineBuilderExtension() { }

        public CommandLineBuilderExtension(bool quoteHyphensOnCommandLine, bool useNewLineSeparator) { }

        protected string GetQuotedText(string unquotedText) { throw null; }
    }

    public partial class ConvertToAbsolutePath : TaskExtension
    {
        public ConvertToAbsolutePath() { }

        [Framework.Output]
        public Framework.ITaskItem[] AbsolutePaths { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Paths { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class Copy : TaskExtension, Framework.ICancelableTask, Framework.ITask
    {
        public Copy() { }

        [Framework.Output]
        public Framework.ITaskItem[] CopiedFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] DestinationFiles { get { throw null; } set { } }

        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        public bool OverwriteReadOnlyFiles { get { throw null; } set { } }

        public int Retries { get { throw null; } set { } }

        public int RetryDelayMilliseconds { get { throw null; } set { } }

        public bool SkipUnchangedFiles { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] SourceFiles { get { throw null; } set { } }

        public bool UseHardlinksIfPossible { get { throw null; } set { } }

        public bool UseSymboliclinksIfPossible { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public partial class CreateCSharpManifestResourceName : CreateManifestResourceName
    {
        protected override string CreateManifestName(string fileName, string linkFileName, string rootNamespace, string dependentUponFileName, System.IO.Stream binaryStream) { throw null; }

        protected override bool IsSourceFile(string fileName) { throw null; }
    }

    public partial class CreateItem : TaskExtension
    {
        public CreateItem() { }

        public string[] AdditionalMetadata { get { throw null; } set { } }

        public Framework.ITaskItem[] Exclude { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] Include { get { throw null; } set { } }

        public bool PreserveExistingMetadata { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public abstract partial class CreateManifestResourceName : TaskExtension
    {
        protected System.Collections.Generic.Dictionary<string, Framework.ITaskItem> itemSpecToTaskitem;
        protected CreateManifestResourceName() { }

        [Framework.Output]
        public Framework.ITaskItem[] ManifestResourceNames { get { throw null; } }

        public bool PrependCultureAsDirectory { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] ResourceFiles { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ResourceFilesWithManifestResourceNames { get { throw null; } set { } }

        public string RootNamespace { get { throw null; } set { } }

        protected abstract string CreateManifestName(string fileName, string linkFileName, string rootNamespaceName, string dependentUponFileName, System.IO.Stream binaryStream);
        public override bool Execute() { throw null; }

        protected abstract bool IsSourceFile(string fileName);
        public static string MakeValidEverettIdentifier(string name) { throw null; }
    }

    public partial class CreateProperty : TaskExtension
    {
        public CreateProperty() { }

        [Framework.Output]
        public string[] Value { get { throw null; } set { } }

        [Framework.Output]
        public string[] ValueSetByTask { get { throw null; } }

        public override bool Execute() { throw null; }
    }

    public partial class CreateVisualBasicManifestResourceName : CreateManifestResourceName
    {
        protected override string CreateManifestName(string fileName, string linkFileName, string rootNamespace, string dependentUponFileName, System.IO.Stream binaryStream) { throw null; }

        protected override bool IsSourceFile(string fileName) { throw null; }
    }

    public partial class Delete : TaskExtension, Framework.ICancelableTask, Framework.ITask
    {
        public Delete() { }

        [Framework.Output]
        public Framework.ITaskItem[] DeletedFiles { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public bool TreatErrorsAsWarnings { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public sealed partial class Error : TaskExtension
    {
        public Error() { }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        public string Text { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class ErrorFromResources : TaskExtension
    {
        public ErrorFromResources() { }

        public string[] Arguments { get { throw null; } set { } }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        [Framework.Required]
        public string Resource { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class Exec : ToolTaskExtension
    {
        public Exec() { }

        [Framework.Required]
        public string Command { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ConsoleOutput { get { throw null; } }

        public bool ConsoleToMSBuild { get { throw null; } set { } }

        public string CustomErrorRegularExpression { get { throw null; } set { } }

        public string CustomWarningRegularExpression { get { throw null; } set { } }

        public bool IgnoreExitCode { get { throw null; } set { } }

        public bool IgnoreStandardErrorWarningFormat { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] Outputs { get { throw null; } set { } }

        protected override System.Text.Encoding StandardErrorEncoding { get { throw null; } }

        protected override Framework.MessageImportance StandardErrorLoggingImportance { get { throw null; } }

        protected override System.Text.Encoding StandardOutputEncoding { get { throw null; } }

        protected override Framework.MessageImportance StandardOutputLoggingImportance { get { throw null; } }

        [Framework.Output]
        public string StdErrEncoding { get { throw null; } set { } }

        [Framework.Output]
        public string StdOutEncoding { get { throw null; } set { } }

        protected override string ToolName { get { throw null; } }

        public string UseUtf8Encoding { get { throw null; } set { } }

        public string WorkingDirectory { get { throw null; } set { } }

        protected internal override void AddCommandLineCommands(CommandLineBuilderExtension commandLine) { }

        protected override int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands) { throw null; }

        protected override string GenerateFullPathToTool() { throw null; }

        protected override string GetWorkingDirectory() { throw null; }

        protected override bool HandleTaskExecutionErrors() { throw null; }

        protected override void LogEventsFromTextOutput(string singleLine, Framework.MessageImportance messageImportance) { }

        protected override void LogPathToTool(string toolName, string pathToTool) { }

        protected override void LogToolCommand(string message) { }

        protected override bool ValidateParameters() { throw null; }
    }

    public partial struct ExtractedClassName
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool IsInsideConditionalBlock { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }
    }

    public partial class FindAppConfigFile : TaskExtension
    {
        public FindAppConfigFile() { }

        [Framework.Output]
        public Framework.ITaskItem AppConfigFile { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] PrimaryList { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] SecondaryList { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class FindInList : TaskExtension
    {
        public FindInList() { }

        public bool CaseSensitive { get { throw null; } set { } }

        public bool FindLastMatch { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem ItemFound { get { throw null; } set { } }

        [Framework.Required]
        public string ItemSpecToFind { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] List { get { throw null; } set { } }

        public bool MatchFileNameOnly { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class FindUnderPath : TaskExtension
    {
        public FindUnderPath() { }

        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] InPath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] OutOfPath { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem Path { get { throw null; } set { } }

        public bool UpdateToAbsolutePaths { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class FormatVersion : TaskExtension
    {
        public FormatVersion() { }

        public string FormatType { get { throw null; } set { } }

        [Framework.Output]
        public string OutputVersion { get { throw null; } set { } }

        public int Revision { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class GenerateBindingRedirects : TaskExtension
    {
        public GenerateBindingRedirects() { }

        public Framework.ITaskItem AppConfigFile { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputAppConfigFile { get { throw null; } set { } }

        public Framework.ITaskItem[] SuggestedRedirects { get { throw null; } set { } }

        public string TargetName { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    [Framework.RequiredRuntime("v2.0")]
    public sealed partial class GenerateResource : TaskExtension
    {
        public GenerateResource() { }

        public Framework.ITaskItem[] AdditionalInputs { get { throw null; } set { } }

        public string[] EnvironmentVariables { get { throw null; } set { } }

        public Framework.ITaskItem[] ExcludedInputPaths { get { throw null; } set { } }

        public bool ExecuteAsTool { get { throw null; } set { } }

        public bool ExtractResWFiles { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] FilesWritten { get { throw null; } }

        public bool MinimalRebuildFromTracking { get { throw null; } set { } }

        public bool NeverLockTypeAssemblies { get { throw null; } set { } }

        public string OutputDirectory { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] OutputResources { get { throw null; } set { } }

        public bool PublicClass { get { throw null; } set { } }

        public Framework.ITaskItem[] References { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        [Framework.Required]
        [Framework.Output]
        public Framework.ITaskItem[] Sources { get { throw null; } set { } }

        public Framework.ITaskItem StateFile { get { throw null; } set { } }

        [Framework.Output]
        public string StronglyTypedClassName { get { throw null; } set { } }

        [Framework.Output]
        public string StronglyTypedFileName { get { throw null; } set { } }

        public string StronglyTypedLanguage { get { throw null; } set { } }

        public string StronglyTypedManifestPrefix { get { throw null; } set { } }

        public string StronglyTypedNamespace { get { throw null; } set { } }

        public Framework.ITaskItem[] TLogReadFiles { get { throw null; } }

        public Framework.ITaskItem[] TLogWriteFiles { get { throw null; } }

        public string ToolArchitecture { get { throw null; } set { } }

        public string TrackerFrameworkPath { get { throw null; } set { } }

        public string TrackerLogDirectory { get { throw null; } set { } }

        public string TrackerSdkPath { get { throw null; } set { } }

        public bool TrackFileAccess { get { throw null; } set { } }

        public bool UseSourcePath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class GetAssemblyIdentity : TaskExtension
    {
        public GetAssemblyIdentity() { }

        [Framework.Output]
        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] AssemblyFiles { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class GetFrameworkPath : TaskExtension
    {
        public GetFrameworkPath() { }

        [Framework.Output]
        public string FrameworkVersion11Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion20Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion30Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion35Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion40Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion451Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion452Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion45Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion461Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion462Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion46Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion471Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion47Path { get { throw null; } }

        [Framework.Output]
        public string Path { get { throw null; } }

        public override bool Execute() { throw null; }
    }

    public partial class GetReferenceAssemblyPaths : TaskExtension
    {
        public GetReferenceAssemblyPaths() { }

        public bool BypassFrameworkInstallChecks { get { throw null; } set { } }

        [Framework.Output]
        public string[] FullFrameworkReferenceAssemblyPaths { get { throw null; } }

        [Framework.Output]
        public string[] ReferenceAssemblyPaths { get { throw null; } }

        public string RootPath { get { throw null; } set { } }

        public bool SuppressNotFoundError { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        [Framework.Output]
        public string TargetFrameworkMonikerDisplayName { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class Hash : TaskExtension
    {
        public Hash() { }

        [Framework.Output]
        public string HashResult { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] ItemsToHash { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class MakeDir : TaskExtension
    {
        public MakeDir() { }

        [Framework.Required]
        public Framework.ITaskItem[] Directories { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] DirectoriesCreated { get { throw null; } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class Message : TaskExtension
    {
        public Message() { }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        public string Importance { get { throw null; } set { } }

        public bool IsCritical { get { throw null; } set { } }

        public string Text { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class Move : TaskExtension, Framework.ICancelableTask, Framework.ITask
    {
        public Move() { }

        [Framework.Output]
        public Framework.ITaskItem[] DestinationFiles { get { throw null; } set { } }

        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] MovedFiles { get { throw null; } }

        public bool OverwriteReadOnlyFiles { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] SourceFiles { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    [Framework.RunInMTA]
    public partial class MSBuild : TaskExtension
    {
        public MSBuild() { }

        public bool BuildInParallel { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Projects { get { throw null; } set { } }

        public string[] Properties { get { throw null; } set { } }

        public bool RebaseOutputs { get { throw null; } set { } }

        public string RemoveProperties { get { throw null; } set { } }

        public bool RunEachTargetSeparately { get { throw null; } set { } }

        public string SkipNonexistentProjects { get { throw null; } set { } }

        public bool StopOnFirstFailure { get { throw null; } set { } }

        public string[] TargetAndPropertyListSeparators { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] TargetOutputs { get { throw null; } }

        public string[] Targets { get { throw null; } set { } }

        public string ToolsVersion { get { throw null; } set { } }

        public bool UnloadProjectsOnCompletion { get { throw null; } set { } }

        public bool UseResultsCache { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class ReadLinesFromFile : TaskExtension
    {
        public ReadLinesFromFile() { }

        [Framework.Required]
        public Framework.ITaskItem File { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] Lines { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class RemoveDir : TaskExtension
    {
        public RemoveDir() { }

        [Framework.Required]
        public Framework.ITaskItem[] Directories { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] RemovedDirectories { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class RemoveDuplicates : TaskExtension
    {
        public RemoveDuplicates() { }

        [Framework.Output]
        public Framework.ITaskItem[] Filtered { get { throw null; } set { } }

        [Framework.Output]
        public bool HadAnyDuplicates { get { throw null; } set { } }

        public Framework.ITaskItem[] Inputs { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class ResolveAssemblyReference : TaskExtension
    {
        public ResolveAssemblyReference() { }

        public string[] AllowedAssemblyExtensions { get { throw null; } set { } }

        public string[] AllowedRelatedFileExtensions { get { throw null; } set { } }

        public string AppConfigFile { get { throw null; } set { } }

        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        public Framework.ITaskItem[] AssemblyFiles { get { throw null; } set { } }

        public bool AutoUnify { get { throw null; } set { } }

        public string[] CandidateAssemblyFiles { get { throw null; } set { } }

        public bool CopyLocalDependenciesWhenParentReferenceInGac { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] CopyLocalFiles { get { throw null; } }

        [Framework.Output]
        public string DependsOnNETStandard { get { throw null; } }

        [Framework.Output]
        public string DependsOnSystemRuntime { get { throw null; } }

        public bool DoNotCopyLocalIfInGac { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] FilesWritten { get { throw null; } set { } }

        public bool FindDependencies { get { throw null; } set { } }

        public bool FindDependenciesOfExternallyResolvedReferences { get { throw null; } set { } }

        public bool FindRelatedFiles { get { throw null; } set { } }

        public bool FindSatellites { get { throw null; } set { } }

        public bool FindSerializationAssemblies { get { throw null; } set { } }

        public Framework.ITaskItem[] FullFrameworkAssemblyTables { get { throw null; } set { } }

        public string[] FullFrameworkFolders { get { throw null; } set { } }

        public string[] FullTargetFrameworkSubsetNames { get { throw null; } set { } }

        public bool IgnoreDefaultInstalledAssemblySubsetTables { get { throw null; } set { } }

        public bool IgnoreDefaultInstalledAssemblyTables { get { throw null; } set { } }

        public bool IgnoreTargetFrameworkAttributeVersionMismatch { get { throw null; } set { } }

        public bool IgnoreVersionForFrameworkReferences { get { throw null; } set { } }

        public Framework.ITaskItem[] InstalledAssemblySubsetTables { get { throw null; } set { } }

        public Framework.ITaskItem[] InstalledAssemblyTables { get { throw null; } set { } }

        public string[] LatestTargetFrameworkDirectories { get { throw null; } set { } }

        public string ProfileName { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] RelatedFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedDependencyFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedFiles { get { throw null; } }

        public Framework.ITaskItem[] ResolvedSDKReferences { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] SatelliteFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] ScatterFiles { get { throw null; } }

        [Framework.Required]
        public string[] SearchPaths { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] SerializationAssemblyFiles { get { throw null; } }

        public bool Silent { get { throw null; } set { } }

        public string StateFile { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] SuggestedRedirects { get { throw null; } }

        public bool SupportsBindingRedirectGeneration { get { throw null; } set { } }

        public string TargetedRuntimeVersion { get { throw null; } set { } }

        public string[] TargetFrameworkDirectories { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        public string TargetFrameworkMonikerDisplayName { get { throw null; } set { } }

        public string[] TargetFrameworkSubsets { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public string TargetProcessorArchitecture { get { throw null; } set { } }

        public bool UnresolveFrameworkAssembliesFromHigherFrameworks { get { throw null; } set { } }

        public string WarnOrErrorOnTargetArchitectureMismatch { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class ResolveCodeAnalysisRuleSet : TaskExtension
    {
        public ResolveCodeAnalysisRuleSet() { }

        public string CodeAnalysisRuleSet { get { throw null; } set { } }

        public string[] CodeAnalysisRuleSetDirectories { get { throw null; } set { } }

        public string MSBuildProjectDirectory { get { throw null; } set { } }

        [Framework.Output]
        public string ResolvedCodeAnalysisRuleSet { get { throw null; } }

        public override bool Execute() { throw null; }
    }

    public partial class ResolveKeySource : TaskExtension
    {
        public ResolveKeySource() { }

        public int AutoClosePasswordPromptShow { get { throw null; } set { } }

        public int AutoClosePasswordPromptTimeout { get { throw null; } set { } }

        public string CertificateFile { get { throw null; } set { } }

        public string CertificateThumbprint { get { throw null; } set { } }

        public string KeyFile { get { throw null; } set { } }

        [Framework.Output]
        public string ResolvedKeyContainer { get { throw null; } set { } }

        [Framework.Output]
        public string ResolvedKeyFile { get { throw null; } set { } }

        [Framework.Output]
        public string ResolvedThumbprint { get { throw null; } set { } }

        public bool ShowImportDialogDespitePreviousFailures { get { throw null; } set { } }

        public bool SuppressAutoClosePasswordPrompt { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public abstract partial class ResolveProjectBase : TaskExtension
    {
        protected ResolveProjectBase() { }

        [Framework.Required]
        public Framework.ITaskItem[] ProjectReferences { get { throw null; } set { } }

        protected void AddSyntheticProjectReferences(string currentProjectAbsolutePath) { }

        protected System.Xml.XmlElement GetProjectElement(Framework.ITaskItem projectRef) { throw null; }

        protected string GetProjectItem(Framework.ITaskItem projectRef) { throw null; }
    }

    public abstract partial class TaskExtension : Utilities.Task
    {
        internal TaskExtension() { }

        public new Utilities.TaskLoggingHelper Log { get { throw null; } }
    }

    public partial class TaskLoggingHelperExtension : Utilities.TaskLoggingHelper
    {
        public TaskLoggingHelperExtension(Framework.ITask taskInstance, System.Resources.ResourceManager primaryResources, System.Resources.ResourceManager sharedResources, string helpKeywordPrefix) : base(default!) { }

        public System.Resources.ResourceManager TaskSharedResources { get { throw null; } set { } }

        public override string FormatResourceString(string resourceName, params object[] args) { throw null; }
    }

    public sealed partial class Telemetry : TaskExtension
    {
        public Telemetry() { }

        public string EventData { get { throw null; } set { } }

        [Framework.Required]
        public string EventName { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public abstract partial class ToolTaskExtension : Utilities.ToolTask
    {
        internal ToolTaskExtension() { }

        protected internal System.Collections.Hashtable Bag { get { throw null; } }

        protected override bool HasLoggedErrors { get { throw null; } }

        public Utilities.TaskLoggingHelper Log { get { throw null; } }

        protected virtual bool UseNewLineSeparatorInResponseFile { get { throw null; } }

        protected internal virtual void AddCommandLineCommands(CommandLineBuilderExtension commandLine) { }

        protected internal virtual void AddResponseFileCommands(CommandLineBuilderExtension commandLine) { }

        protected override string GenerateCommandLineCommands() { throw null; }

        protected override string GenerateResponseFileCommands() { throw null; }

        protected internal bool GetBoolParameterWithDefault(string parameterName, bool defaultValue) { throw null; }

        protected internal int GetIntParameterWithDefault(string parameterName, int defaultValue) { throw null; }
    }

    public partial class Touch : TaskExtension
    {
        public Touch() { }

        public bool AlwaysCreate { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public bool ForceTouch { get { throw null; } set { } }

        public string Time { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] TouchedFiles { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class Warning : TaskExtension
    {
        public Warning() { }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        public string Text { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class WriteCodeFragment : TaskExtension
    {
        public WriteCodeFragment() { }

        public Framework.ITaskItem[] AssemblyAttributes { get { throw null; } set { } }

        [Framework.Required]
        public string Language { get { throw null; } set { } }

        public Framework.ITaskItem OutputDirectory { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputFile { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class WriteLinesToFile : TaskExtension
    {
        public WriteLinesToFile() { }

        public string Encoding { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem File { get { throw null; } set { } }

        public Framework.ITaskItem[] Lines { get { throw null; } set { } }

        public bool Overwrite { get { throw null; } set { } }

        public bool WriteOnlyWhenDifferent { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    [System.Obsolete("The XamlTaskFactory is not supported on .NET Core.  This class is included so that users receive run-time errors and should not be used for any other purpose.", true)]
    public sealed partial class XamlTaskFactory : Framework.ITaskFactory
    {
        public string FactoryName { get { throw null; } }

        public System.Type TaskType { get { throw null; } }

        public void CleanupTask(Framework.ITask task) { }

        public Framework.ITask CreateTask(Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }

        public Framework.TaskPropertyInfo[] GetTaskParameters() { throw null; }

        public bool Initialize(string taskName, System.Collections.Generic.IDictionary<string, Framework.TaskPropertyInfo> parameterGroup, string taskBody, Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }
    }

    public partial class XmlPeek : TaskExtension
    {
        public XmlPeek() { }

        public string Namespaces { get { throw null; } set { } }

        public bool ProhibitDtd { get { throw null; } set { } }

        public string Query { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] Result { get { throw null; } }

        public string XmlContent { get { throw null; } set { } }

        public Framework.ITaskItem XmlInputPath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class XmlPoke : TaskExtension
    {
        public XmlPoke() { }

        public string Namespaces { get { throw null; } set { } }

        public string Query { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem Value { get { throw null; } set { } }

        public Framework.ITaskItem XmlInputPath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }
}

namespace Microsoft.Build.Tasks.Hosting
{
    public partial interface IAnalyzerHostObject
    {
        bool SetAdditionalFiles(Framework.ITaskItem[] additionalFiles);
        bool SetAnalyzers(Framework.ITaskItem[] analyzers);
        bool SetRuleSet(string ruleSetFile);
    }

    public partial interface ICscHostObject : Framework.ITaskHost
    {
        void BeginInitialization();
        bool Compile();
        bool EndInitialization(out string errorMessage, out int errorCode);
        bool IsDesignTime();
        bool IsUpToDate();
        bool SetAdditionalLibPaths(string[] additionalLibPaths);
        bool SetAddModules(string[] addModules);
        bool SetAllowUnsafeBlocks(bool allowUnsafeBlocks);
        bool SetBaseAddress(string baseAddress);
        bool SetCheckForOverflowUnderflow(bool checkForOverflowUnderflow);
        bool SetCodePage(int codePage);
        bool SetDebugType(string debugType);
        bool SetDefineConstants(string defineConstants);
        bool SetDelaySign(bool delaySignExplicitlySet, bool delaySign);
        bool SetDisabledWarnings(string disabledWarnings);
        bool SetDocumentationFile(string documentationFile);
        bool SetEmitDebugInformation(bool emitDebugInformation);
        bool SetErrorReport(string errorReport);
        bool SetFileAlignment(int fileAlignment);
        bool SetGenerateFullPaths(bool generateFullPaths);
        bool SetKeyContainer(string keyContainer);
        bool SetKeyFile(string keyFile);
        bool SetLangVersion(string langVersion);
        bool SetLinkResources(Framework.ITaskItem[] linkResources);
        bool SetMainEntryPoint(string targetType, string mainEntryPoint);
        bool SetModuleAssemblyName(string moduleAssemblyName);
        bool SetNoConfig(bool noConfig);
        bool SetNoStandardLib(bool noStandardLib);
        bool SetOptimize(bool optimize);
        bool SetOutputAssembly(string outputAssembly);
        bool SetPdbFile(string pdbFile);
        bool SetPlatform(string platform);
        bool SetReferences(Framework.ITaskItem[] references);
        bool SetResources(Framework.ITaskItem[] resources);
        bool SetResponseFiles(Framework.ITaskItem[] responseFiles);
        bool SetSources(Framework.ITaskItem[] sources);
        bool SetTargetType(string targetType);
        bool SetTreatWarningsAsErrors(bool treatWarningsAsErrors);
        bool SetWarningLevel(int warningLevel);
        bool SetWarningsAsErrors(string warningsAsErrors);
        bool SetWarningsNotAsErrors(string warningsNotAsErrors);
        bool SetWin32Icon(string win32Icon);
        bool SetWin32Resource(string win32Resource);
    }

    public partial interface ICscHostObject2 : ICscHostObject, Framework.ITaskHost
    {
        bool SetWin32Manifest(string win32Manifest);
    }

    public partial interface ICscHostObject3 : ICscHostObject2, ICscHostObject, Framework.ITaskHost
    {
        bool SetApplicationConfiguration(string applicationConfiguration);
    }

    public partial interface ICscHostObject4 : ICscHostObject3, ICscHostObject2, ICscHostObject, Framework.ITaskHost
    {
        bool SetHighEntropyVA(bool highEntropyVA);
        bool SetPlatformWith32BitPreference(string platformWith32BitPreference);
        bool SetSubsystemVersion(string subsystemVersion);
    }

    public partial interface IVbcHostObject : Framework.ITaskHost
    {
        void BeginInitialization();
        bool Compile();
        void EndInitialization();
        bool IsDesignTime();
        bool IsUpToDate();
        bool SetAdditionalLibPaths(string[] additionalLibPaths);
        bool SetAddModules(string[] addModules);
        bool SetBaseAddress(string targetType, string baseAddress);
        bool SetCodePage(int codePage);
        bool SetDebugType(bool emitDebugInformation, string debugType);
        bool SetDefineConstants(string defineConstants);
        bool SetDelaySign(bool delaySign);
        bool SetDisabledWarnings(string disabledWarnings);
        bool SetDocumentationFile(string documentationFile);
        bool SetErrorReport(string errorReport);
        bool SetFileAlignment(int fileAlignment);
        bool SetGenerateDocumentation(bool generateDocumentation);
        bool SetImports(Framework.ITaskItem[] importsList);
        bool SetKeyContainer(string keyContainer);
        bool SetKeyFile(string keyFile);
        bool SetLinkResources(Framework.ITaskItem[] linkResources);
        bool SetMainEntryPoint(string mainEntryPoint);
        bool SetNoConfig(bool noConfig);
        bool SetNoStandardLib(bool noStandardLib);
        bool SetNoWarnings(bool noWarnings);
        bool SetOptimize(bool optimize);
        bool SetOptionCompare(string optionCompare);
        bool SetOptionExplicit(bool optionExplicit);
        bool SetOptionStrict(bool optionStrict);
        bool SetOptionStrictType(string optionStrictType);
        bool SetOutputAssembly(string outputAssembly);
        bool SetPlatform(string platform);
        bool SetReferences(Framework.ITaskItem[] references);
        bool SetRemoveIntegerChecks(bool removeIntegerChecks);
        bool SetResources(Framework.ITaskItem[] resources);
        bool SetResponseFiles(Framework.ITaskItem[] responseFiles);
        bool SetRootNamespace(string rootNamespace);
        bool SetSdkPath(string sdkPath);
        bool SetSources(Framework.ITaskItem[] sources);
        bool SetTargetCompactFramework(bool targetCompactFramework);
        bool SetTargetType(string targetType);
        bool SetTreatWarningsAsErrors(bool treatWarningsAsErrors);
        bool SetWarningsAsErrors(string warningsAsErrors);
        bool SetWarningsNotAsErrors(string warningsNotAsErrors);
        bool SetWin32Icon(string win32Icon);
        bool SetWin32Resource(string win32Resource);
    }

    public partial interface IVbcHostObject2 : IVbcHostObject, Framework.ITaskHost
    {
        bool SetModuleAssemblyName(string moduleAssemblyName);
        bool SetOptionInfer(bool optionInfer);
        bool SetWin32Manifest(string win32Manifest);
    }

    public partial interface IVbcHostObject3 : IVbcHostObject2, IVbcHostObject, Framework.ITaskHost
    {
        bool SetLanguageVersion(string languageVersion);
    }

    public partial interface IVbcHostObject4 : IVbcHostObject3, IVbcHostObject2, IVbcHostObject, Framework.ITaskHost
    {
        bool SetVBRuntime(string VBRuntime);
    }

    public partial interface IVbcHostObject5 : IVbcHostObject4, IVbcHostObject3, IVbcHostObject2, IVbcHostObject, Framework.ITaskHost
    {
        int CompileAsync(out System.IntPtr buildSucceededEvent, out System.IntPtr buildFailedEvent);
        int EndCompile(bool buildSuccess);
        IVbcHostObjectFreeThreaded GetFreeThreadedHostObject();
        bool SetHighEntropyVA(bool highEntropyVA);
        bool SetPlatformWith32BitPreference(string platformWith32BitPreference);
        bool SetSubsystemVersion(string subsystemVersion);
    }

    public partial interface IVbcHostObjectFreeThreaded
    {
        bool Compile();
    }
}