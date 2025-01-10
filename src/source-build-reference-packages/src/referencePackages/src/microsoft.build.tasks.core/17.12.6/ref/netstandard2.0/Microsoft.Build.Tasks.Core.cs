// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Build.Tasks.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.SafeDirectories)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Build.Tasks.Core.dll")]
[assembly: System.Reflection.AssemblyFileVersion("17.12.6.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("17.12.6+db5f6012cb7f6e2dd7066c50c573c0d352713407")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® Build Tools®")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Build.Tasks.Core.dll")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/msbuild")]
[assembly: System.Reflection.AssemblyVersionAttribute("15.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Build.Tasks
{
    public sealed partial class AddToWin32Manifest : TaskExtension
    {
        public AddToWin32Manifest() { }

        public Framework.ITaskItem? ApplicationManifest { get { throw null; } set { } }

        [Framework.Output]
        public string ManifestPath { get { throw null; } }

        [Framework.Required]
        public string OutputDirectory { get { throw null; } set { } }

        [Framework.Required]
        public string SupportedArchitectures { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class AL : TaskRequiresFramework
    {
        public AL() { }

        public string AlgorithmId { get { throw null; } set { } }

        public string BaseAddress { get { throw null; } set { } }

        public string CompanyName { get { throw null; } set { } }

        public string Configuration { get { throw null; } set { } }

        public string Copyright { get { throw null; } set { } }

        public string Culture { get { throw null; } set { } }

        public bool DelaySign { get { throw null; } set { } }

        public string Description { get { throw null; } set { } }

        public Framework.ITaskItem[] EmbedResources { get { throw null; } set { } }

        public string EvidenceFile { get { throw null; } set { } }

        public string FileVersion { get { throw null; } set { } }

        public string Flags { get { throw null; } set { } }

        public bool GenerateFullPaths { get { throw null; } set { } }

        public string KeyContainer { get { throw null; } set { } }

        public string KeyFile { get { throw null; } set { } }

        public Framework.ITaskItem[] LinkResources { get { throw null; } set { } }

        public string MainEntryPoint { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputAssembly { get { throw null; } set { } }

        public string Platform { get { throw null; } set { } }

        public bool Prefer32Bit { get { throw null; } set { } }

        public string ProductName { get { throw null; } set { } }

        public string ProductVersion { get { throw null; } set { } }

        public string[] ResponseFiles { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        public Framework.ITaskItem[] SourceModules { get { throw null; } set { } }

        public string TargetType { get { throw null; } set { } }

        public string TemplateFile { get { throw null; } set { } }

        public string Title { get { throw null; } set { } }

        public string Trademark { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        public string Win32Icon { get { throw null; } set { } }

        public string Win32Resource { get { throw null; } set { } }
    }

    public sealed partial class AspNetCompiler : TaskRequiresFramework
    {
        public AspNetCompiler() { }

        public bool AllowPartiallyTrustedCallers { get { throw null; } set { } }

        public bool Clean { get { throw null; } set { } }

        public bool Debug { get { throw null; } set { } }

        public bool DelaySign { get { throw null; } set { } }

        public bool FixedNames { get { throw null; } set { } }

        public bool Force { get { throw null; } set { } }

        public string KeyContainer { get { throw null; } set { } }

        public string KeyFile { get { throw null; } set { } }

        public string MetabasePath { get { throw null; } set { } }

        public string PhysicalPath { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        public string TargetPath { get { throw null; } set { } }

        public bool Updateable { get { throw null; } set { } }

        public string VirtualPath { get { throw null; } set { } }
    }

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

        public bool RespectAlreadyAssignedItemCulture { get { throw null; } set { } }

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

    public partial class CombineTargetFrameworkInfoProperties : TaskExtension
    {
        public CombineTargetFrameworkInfoProperties() { }

        public Framework.ITaskItem[] PropertiesAndValues { get { throw null; } set { } }

        [Framework.Output]
        public string Result { get { throw null; } set { } }

        public string RootElementName { get { throw null; } set { } }

        public bool UseAttributeForTargetFrameworkInfoPropertyNames { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class CombineXmlElements : TaskExtension
    {
        public CombineXmlElements() { }

        [Framework.Output]
        public string Result { get { throw null; } set { } }

        public string RootElementName { get { throw null; } set { } }

        public Framework.ITaskItem[] XmlElements { get { throw null; } set { } }

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

    public partial class Copy : TaskExtension, Framework.IIncrementalTask, Framework.ICancelableTask, Framework.ITask
    {
        public Copy() { }

        [Framework.Output]
        public Framework.ITaskItem[] CopiedFiles { get { throw null; } }

        [Framework.Output]
        public Framework.ITaskItem[] DestinationFiles { get { throw null; } set { } }

        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        public bool ErrorIfLinkFails { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        public bool OverwriteReadOnlyFiles { get { throw null; } set { } }

        public int Retries { get { throw null; } set { } }

        public int RetryDelayMilliseconds { get { throw null; } set { } }

        public bool SkipUnchangedFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] SourceFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] SourceFolders { get { throw null; } set { } }

        public bool UseHardlinksIfPossible { get { throw null; } set { } }

        public bool UseSymboliclinksIfPossible { get { throw null; } set { } }

        [Framework.Output]
        public bool WroteAtLeastOneFile { get { throw null; } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public partial class CreateCSharpManifestResourceName : CreateManifestResourceName
    {
        protected override string SourceFileExtension { get { throw null; } }

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

        protected abstract string SourceFileExtension { get; }

        public bool UseDependentUponConvention { get { throw null; } set { } }

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
        protected override string SourceFileExtension { get { throw null; } }

        protected override string CreateManifestName(string fileName, string linkFileName, string rootNamespace, string dependentUponFileName, System.IO.Stream binaryStream) { throw null; }

        protected override bool IsSourceFile(string fileName) { throw null; }
    }

    public partial class Delete : TaskExtension, Framework.ICancelableTask, Framework.ITask, Framework.IIncrementalTask
    {
        public Delete() { }

        [Framework.Output]
        public Framework.ITaskItem[] DeletedFiles { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public int Retries { get { throw null; } set { } }

        public int RetryDelayMilliseconds { get { throw null; } set { } }

        public bool TreatErrorsAsWarnings { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public sealed partial class DownloadFile : TaskExtension, Framework.ICancelableTask, Framework.ITask, Framework.IIncrementalTask
    {
        public DownloadFile() { }

        public Framework.ITaskItem DestinationFileName { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem DownloadedFile { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        public int Retries { get { throw null; } set { } }

        public int RetryDelayMilliseconds { get { throw null; } set { } }

        public bool SkipUnchangedFiles { get { throw null; } set { } }

        [Framework.Required]
        public string SourceUrl { get { throw null; } set { } }

        public int Timeout { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public sealed partial class Error : TaskExtension
    {
        public Error() { }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        public string HelpLink { get { throw null; } set { } }

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

    public partial class FindInvalidProjectReferences : TaskExtension
    {
        public FindInvalidProjectReferences() { }

        [Framework.Output]
        public Framework.ITaskItem[] InvalidReferences { get { throw null; } }

        public Framework.ITaskItem[] ProjectReferences { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformIdentifier { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformVersion { get { throw null; } set { } }

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

    public sealed partial class FormatUrl : TaskExtension
    {
        public FormatUrl() { }

        public string InputUrl { get { throw null; } set { } }

        [Framework.Output]
        public string OutputUrl { get { throw null; } set { } }

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

    public sealed partial class GenerateApplicationManifest : GenerateManifestBase
    {
        public string ClrVersion { get { throw null; } set { } }

        public Framework.ITaskItem ConfigFile { get { throw null; } set { } }

        public Framework.ITaskItem[] Dependencies { get { throw null; } set { } }

        public string ErrorReportUrl { get { throw null; } set { } }

        public Framework.ITaskItem[] FileAssociations { get { throw null; } set { } }

        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public bool HostInBrowser { get { throw null; } set { } }

        public Framework.ITaskItem IconFile { get { throw null; } set { } }

        public Framework.ITaskItem[] IsolatedComReferences { get { throw null; } set { } }

        public string ManifestType { get { throw null; } set { } }

        public string OSVersion { get { throw null; } set { } }

        public string Product { get { throw null; } set { } }

        public string Publisher { get { throw null; } set { } }

        public bool RequiresMinimumFramework35SP1 { get { throw null; } set { } }

        public string SuiteName { get { throw null; } set { } }

        public string SupportUrl { get { throw null; } set { } }

        public string TargetFrameworkProfile { get { throw null; } set { } }

        public string TargetFrameworkSubset { get { throw null; } set { } }

        public Framework.ITaskItem TrustInfoFile { get { throw null; } set { } }

        public bool UseApplicationTrust { get { throw null; } set { } }

        public override bool Execute() { throw null; }

        protected override System.Type GetObjectType() { throw null; }

        protected override bool OnManifestLoaded(Deployment.ManifestUtilities.Manifest manifest) { throw null; }

        protected override bool OnManifestResolved(Deployment.ManifestUtilities.Manifest manifest) { throw null; }

        protected internal override bool ValidateInputs() { throw null; }
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

    public sealed partial class GenerateBootstrapper : TaskRequiresFramework
    {
        public GenerateBootstrapper() { }

        public string ApplicationFile { get { throw null; } set { } }

        public string ApplicationName { get { throw null; } set { } }

        public bool ApplicationRequiresElevation { get { throw null; } set { } }

        public string ApplicationUrl { get { throw null; } set { } }

        [Framework.Output]
        public string[] BootstrapperComponentFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] BootstrapperItems { get { throw null; } set { } }

        [Framework.Output]
        public string BootstrapperKeyFile { get { throw null; } set { } }

        public string ComponentsLocation { get { throw null; } set { } }

        public string ComponentsUrl { get { throw null; } set { } }

        public bool CopyComponents { get { throw null; } set { } }

        public string Culture { get { throw null; } set { } }

        public string FallbackCulture { get { throw null; } set { } }

        public string OutputPath { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }

        public string SupportUrl { get { throw null; } set { } }

        public bool Validate { get { throw null; } set { } }

        public string VisualStudioVersion { get { throw null; } set { } }
    }

    public sealed partial class GenerateDeploymentManifest : GenerateManifestBase
    {
        public bool CreateDesktopShortcut { get { throw null; } set { } }

        public string DeploymentUrl { get { throw null; } set { } }

        public bool DisallowUrlActivation { get { throw null; } set { } }

        public string ErrorReportUrl { get { throw null; } set { } }

        public bool Install { get { throw null; } set { } }

        public bool MapFileExtensions { get { throw null; } set { } }

        public string MinimumRequiredVersion { get { throw null; } set { } }

        public string Product { get { throw null; } set { } }

        public string Publisher { get { throw null; } set { } }

        public string SuiteName { get { throw null; } set { } }

        public string SupportUrl { get { throw null; } set { } }

        public bool TrustUrlParameters { get { throw null; } set { } }

        public bool UpdateEnabled { get { throw null; } set { } }

        public int UpdateInterval { get { throw null; } set { } }

        public string UpdateMode { get { throw null; } set { } }

        public string UpdateUnit { get { throw null; } set { } }

        public override bool Execute() { throw null; }

        protected override System.Type GetObjectType() { throw null; }

        protected override bool OnManifestLoaded(Deployment.ManifestUtilities.Manifest manifest) { throw null; }

        protected override bool OnManifestResolved(Deployment.ManifestUtilities.Manifest manifest) { throw null; }

        protected internal override bool ValidateInputs() { throw null; }
    }

    public sealed partial class GenerateLauncher : TaskExtension
    {
        public GenerateLauncher() { }

        public string AssemblyName { get { throw null; } set { } }

        public Framework.ITaskItem EntryPoint { get { throw null; } set { } }

        public string LauncherPath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputEntryPoint { get { throw null; } set { } }

        public string OutputPath { get { throw null; } set { } }

        public string VisualStudioVersion { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public abstract partial class GenerateManifestBase : Utilities.Task
    {
        public string AssemblyName { get { throw null; } set { } }

        public string AssemblyVersion { get { throw null; } set { } }

        public string Description { get { throw null; } set { } }

        public Framework.ITaskItem EntryPoint { get { throw null; } set { } }

        public Framework.ITaskItem InputManifest { get { throw null; } set { } }

        public bool LauncherBasedDeployment { get { throw null; } set { } }

        public int MaxTargetPath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputManifest { get { throw null; } set { } }

        public string Platform { get { throw null; } set { } }

        public string TargetCulture { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        protected internal Deployment.ManifestUtilities.AssemblyReference AddAssemblyFromItem(Framework.ITaskItem item) { throw null; }

        protected internal Deployment.ManifestUtilities.AssemblyReference AddAssemblyNameFromItem(Framework.ITaskItem item, Deployment.ManifestUtilities.AssemblyReferenceType referenceType) { throw null; }

        protected internal Deployment.ManifestUtilities.AssemblyReference AddEntryPointFromItem(Framework.ITaskItem item, Deployment.ManifestUtilities.AssemblyReferenceType referenceType) { throw null; }

        protected internal Deployment.ManifestUtilities.FileReference AddFileFromItem(Framework.ITaskItem item) { throw null; }

        public override bool Execute() { throw null; }

        protected internal Deployment.ManifestUtilities.FileReference FindFileFromItem(Framework.ITaskItem item) { throw null; }

        protected abstract System.Type GetObjectType();
        protected abstract bool OnManifestLoaded(Deployment.ManifestUtilities.Manifest manifest);
        protected abstract bool OnManifestResolved(Deployment.ManifestUtilities.Manifest manifest);
        protected internal virtual bool ValidateInputs() { throw null; }

        protected internal virtual bool ValidateOutput() { throw null; }
    }

    [Framework.RequiredRuntime("v2.0")]
    public sealed partial class GenerateResource : TaskExtension, Framework.IIncrementalTask
    {
        public GenerateResource() { }

        public Framework.ITaskItem[] AdditionalInputs { get { throw null; } set { } }

        public string[] EnvironmentVariables { get { throw null; } set { } }

        public Framework.ITaskItem[] ExcludedInputPaths { get { throw null; } set { } }

        public bool ExecuteAsTool { get { throw null; } set { } }

        public bool ExtractResWFiles { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

        public bool UsePreserializedResources { get { throw null; } set { } }

        public bool UseSourcePath { get { throw null; } set { } }

        public bool WarnOnBinaryFormatterUse { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class GenerateTrustInfo : TaskRequiresFramework
    {
        public GenerateTrustInfo() { }

        public Framework.ITaskItem[] ApplicationDependencies { get { throw null; } set { } }

        public Framework.ITaskItem BaseManifest { get { throw null; } set { } }

        public string ExcludedPermissions { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        public string TargetZone { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem TrustInfoFile { get { throw null; } set { } }
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

    public partial class GetCompatiblePlatform : TaskExtension
    {
        public GetCompatiblePlatform() { }

        [Framework.Required]
        public Framework.ITaskItem[] AnnotatedProjects { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[]? AssignedProjectsWithPlatform { get { throw null; } set { } }

        [Framework.Required]
        public string CurrentProjectPlatform { get { throw null; } set { } }

        public string PlatformLookupTable { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class GetFileHash : TaskExtension, Framework.ICancelableTask, Framework.ITask
    {
        public GetFileHash() { }

        public string Algorithm { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        [Framework.Output]
        public string Hash { get { throw null; } set { } }

        public string HashEncoding { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] Items { get { throw null; } set { } }

        public string MetadataName { get { throw null; } set { } }

        public void Cancel() { }

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
        public string FrameworkVersion472Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion47Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkVersion48Path { get { throw null; } }

        [Framework.Output]
        public string Path { get { throw null; } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class GetFrameworkSdkPath : TaskRequiresFramework
    {
        public GetFrameworkSdkPath() { }

        [Framework.Output]
        public string FrameworkSdkVersion20Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion35Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion40Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion451Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion45Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion461Path { get { throw null; } }

        [Framework.Output]
        public string FrameworkSdkVersion46Path { get { throw null; } }

        [Framework.Output]
        public string Path { get { throw null; } set { } }
    }

    public partial class GetInstalledSDKLocations : TaskExtension
    {
        public GetInstalledSDKLocations() { }

        [Framework.Output]
        public Framework.ITaskItem[] InstalledSDKs { get { throw null; } set { } }

        public string[] SDKDirectoryRoots { get { throw null; } set { } }

        public string[] SDKExtensionDirectoryRoots { get { throw null; } set { } }

        public string SDKRegistryRoot { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformIdentifier { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformVersion { get { throw null; } set { } }

        public bool WarnWhenNoSDKsFound { get { throw null; } set { } }

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

        public string TargetFrameworkFallbackSearchPaths { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        [Framework.Output]
        public string TargetFrameworkMonikerDisplayName { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class GetSDKReferenceFiles : TaskExtension
    {
        public GetSDKReferenceFiles() { }

        public string CacheFileFolderPath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] CopyLocalFiles { get { throw null; } }

        public bool LogCacheFileExceptions { get { throw null; } set { } }

        public bool LogRedistConflictBetweenSDKsAsWarning { get { throw null; } set { } }

        public bool LogRedistConflictWithinSDKAsWarning { get { throw null; } set { } }

        public bool LogRedistFilesList { get { throw null; } set { } }

        public bool LogReferenceConflictBetweenSDKsAsWarning { get { throw null; } set { } }

        public bool LogReferenceConflictWithinSDKAsWarning { get { throw null; } set { } }

        public bool LogReferencesList { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] RedistFiles { get { throw null; } }

        public string[] ReferenceExtensions { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] References { get { throw null; } }

        public Framework.ITaskItem[] ResolvedSDKReferences { get { throw null; } set { } }

        public string TargetPlatformIdentifier { get { throw null; } set { } }

        public string TargetPlatformVersion { get { throw null; } set { } }

        public string TargetSDKIdentifier { get { throw null; } set { } }

        public string TargetSDKVersion { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class Hash : TaskExtension
    {
        public Hash() { }

        [Framework.Output]
        public string HashResult { get { throw null; } set { } }

        public bool IgnoreCase { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] ItemsToHash { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial interface IFixedTypeInfo
    {
        void AddressOfMember(int memid, System.Runtime.InteropServices.ComTypes.INVOKEKIND invKind, out System.IntPtr ppv);
        void CreateInstance(object pUnkOuter, ref System.Guid riid, out object ppvObj);
        void GetContainingTypeLib(out System.Runtime.InteropServices.ComTypes.ITypeLib ppTLB, out int pIndex);
        void GetDllEntry(int memid, System.Runtime.InteropServices.ComTypes.INVOKEKIND invKind, System.IntPtr pBstrDllName, System.IntPtr pBstrName, System.IntPtr pwOrdinal);
        void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);
        void GetFuncDesc(int index, out System.IntPtr ppFuncDesc);
        void GetIDsOfNames(string[] rgszNames, int cNames, int[] pMemId);
        void GetImplTypeFlags(int index, out System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS pImplTypeFlags);
        void GetMops(int memid, out string pBstrMops);
        void GetNames(int memid, string[] rgBstrNames, int cMaxNames, out int pcNames);
        void GetRefTypeInfo(System.IntPtr hRef, out IFixedTypeInfo ppTI);
        void GetRefTypeOfImplType(int index, out System.IntPtr href);
        void GetTypeAttr(out System.IntPtr ppTypeAttr);
        void GetTypeComp(out System.Runtime.InteropServices.ComTypes.ITypeComp ppTComp);
        void GetVarDesc(int index, out System.IntPtr ppVarDesc);
        void Invoke(object pvInstance, int memid, short wFlags, ref System.Runtime.InteropServices.ComTypes.DISPPARAMS pDispParams, System.IntPtr pVarResult, System.IntPtr pExcepInfo, out int puArgErr);
        void ReleaseFuncDesc(System.IntPtr pFuncDesc);
        void ReleaseTypeAttr(System.IntPtr pTypeAttr);
        void ReleaseVarDesc(System.IntPtr pVarDesc);
    }

    public partial interface IUnregisterAssemblyTaskContract
    {
        Framework.ITaskItem[] Assemblies { get; set; }

        Framework.ITaskItem AssemblyListFile { get; set; }

        Framework.ITaskItem[] TypeLibFiles { get; set; }
    }

    public partial class LC : ToolTaskExtension
    {
        public LC() { }

        [Framework.Required]
        public Framework.ITaskItem LicenseTarget { get { throw null; } set { } }

        public bool NoLogo { get { throw null; } set { } }

        public string OutputDirectory { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputLicense { get { throw null; } set { } }

        public Framework.ITaskItem[] ReferencedAssemblies { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Sources { get { throw null; } set { } }

        [Framework.Required]
        public string TargetFrameworkVersion { get { throw null; } set { } }

        protected override string ToolName { get { throw null; } }

        protected internal override void AddCommandLineCommands(CommandLineBuilderExtension commandLine) { }

        protected internal override void AddResponseFileCommands(CommandLineBuilderExtension commandLine) { }

        public override bool Execute() { throw null; }

        protected override string GenerateFullPathToTool() { throw null; }

        protected override bool ValidateParameters() { throw null; }
    }

    public partial class MakeDir : TaskExtension, Framework.IIncrementalTask
    {
        public MakeDir() { }

        [Framework.Required]
        public Framework.ITaskItem[] Directories { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] DirectoriesCreated { get { throw null; } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

    public partial class Move : TaskExtension, Framework.ICancelableTask, Framework.ITask, Framework.IIncrementalTask
    {
        public Move() { }

        [Framework.Output]
        public Framework.ITaskItem[] DestinationFiles { get { throw null; } set { } }

        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

    public sealed partial class MSBuildInternalMessage : TaskExtension
    {
        public MSBuildInternalMessage() { }

        public string[] FormatArguments { get { throw null; } set { } }

        public string MessageImportance { get { throw null; } set { } }

        [Framework.Required]
        public string ResourceName { get { throw null; } set { } }

        [Framework.Required]
        public string Severity { get { throw null; } set { } }

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

    public sealed partial class RegisterAssembly : TaskRequiresFramework
    {
        public RegisterAssembly() { }

        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        public Framework.ITaskItem AssemblyListFile { get { throw null; } set { } }

        public bool CreateCodeBase { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] TypeLibFiles { get { throw null; } set { } }
    }

    public partial class RemoveDir : TaskExtension, Framework.IIncrementalTask
    {
        public RemoveDir() { }

        [Framework.Required]
        public Framework.ITaskItem[] Directories { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

    public sealed partial class RequiresFramework35SP1Assembly : TaskExtension
    {
        public RequiresFramework35SP1Assembly() { }

        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        public bool CreateDesktopShortcut { get { throw null; } set { } }

        public Framework.ITaskItem DeploymentManifestEntryPoint { get { throw null; } set { } }

        public Framework.ITaskItem EntryPoint { get { throw null; } set { } }

        public string ErrorReportUrl { get { throw null; } set { } }

        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public Framework.ITaskItem[] ReferencedAssemblies { get { throw null; } set { } }

        [Framework.Output]
        public bool RequiresMinimumFramework35SP1 { get { throw null; } set { } }

        public bool SigningManifests { get { throw null; } set { } }

        public string SuiteName { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class ResolveAssemblyReference : TaskExtension, Framework.IIncrementalTask
    {
        public ResolveAssemblyReference() { }

        public string[] AllowedAssemblyExtensions { get { throw null; } set { } }

        public string[] AllowedRelatedFileExtensions { get { throw null; } set { } }

        public string AppConfigFile { get { throw null; } set { } }

        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        public Framework.ITaskItem[] AssemblyFiles { get { throw null; } set { } }

        public string AssemblyInformationCacheOutputPath { get { throw null; } set { } }

        public Framework.ITaskItem[] AssemblyInformationCachePaths { get { throw null; } set { } }

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

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

        public bool OutputUnresolvedAssemblyConflicts { get { throw null; } set { } }

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

        [Framework.Output]
        public Framework.ITaskItem[] UnresolvedAssemblyConflicts { get { throw null; } }

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

    public sealed partial class ResolveComReference : TaskRequiresFramework
    {
        public ResolveComReference() { }

        public bool DelaySign { get { throw null; } set { } }

        public string[] EnvironmentVariables { get { throw null; } set { } }

        public bool ExecuteAsTool { get { throw null; } set { } }

        public bool IncludeVersionInInteropName { get { throw null; } set { } }

        public string KeyContainer { get { throw null; } set { } }

        public string KeyFile { get { throw null; } set { } }

        public bool NoClassMembers { get { throw null; } set { } }

        public Framework.ITaskItem[] ResolvedAssemblyReferences { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedFiles { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedModules { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        public bool Silent { get { throw null; } set { } }

        public string StateFile { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public string TargetProcessorArchitecture { get { throw null; } set { } }

        public Framework.ITaskItem[] TypeLibFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] TypeLibNames { get { throw null; } set { } }

        public string WrapperOutputDirectory { get { throw null; } set { } }
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

    public sealed partial class ResolveManifestFiles : TaskExtension
    {
        public ResolveManifestFiles() { }

        public string AssemblyName { get { throw null; } set { } }

        public Framework.ITaskItem DeploymentManifestEntryPoint { get { throw null; } set { } }

        public Framework.ITaskItem EntryPoint { get { throw null; } set { } }

        public Framework.ITaskItem[] ExtraFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public bool IsSelfContainedPublish { get { throw null; } set { } }

        public bool IsSingleFilePublish { get { throw null; } set { } }

        public bool LauncherBasedDeployment { get { throw null; } set { } }

        public Framework.ITaskItem[] ManagedAssemblies { get { throw null; } set { } }

        public Framework.ITaskItem[] NativeAssemblies { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] OutputAssemblies { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputDeploymentManifestEntryPoint { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputEntryPoint { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] OutputFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] PublishFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] RuntimePackAssets { get { throw null; } set { } }

        public Framework.ITaskItem[] SatelliteAssemblies { get { throw null; } set { } }

        public bool SigningManifests { get { throw null; } set { } }

        public string TargetCulture { get { throw null; } set { } }

        public string TargetFrameworkIdentifier { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class ResolveNativeReference : TaskRequiresFramework
    {
        public ResolveNativeReference() { }

        public string[] AdditionalSearchPaths { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainedComComponents { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainedLooseEtcFiles { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainedLooseTlbFiles { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainedPrerequisiteAssemblies { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainedTypeLibraries { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ContainingReferenceFiles { get { throw null; } set { } }

        public Framework.ITaskItem[] NativeReferences { get { throw null; } set { } }
    }

    public partial class ResolveNonMSBuildProjectOutput : ResolveProjectBase
    {
        public string PreresolvedProjectOutputs { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedOutputPaths { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] UnresolvedProjectReferences { get { throw null; } set { } }

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

    public partial class ResolveSDKReference : TaskExtension
    {
        public ResolveSDKReference() { }

        public Framework.ITaskItem[] DisallowedSDKDependencies { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] InstalledSDKs { get { throw null; } set { } }

        public bool LogResolutionErrorsAsWarnings { get { throw null; } set { } }

        public bool Prefer32Bit { get { throw null; } set { } }

        [Framework.Required]
        public string ProjectName { get { throw null; } set { } }

        public Framework.ITaskItem[] References { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] ResolvedSDKReferences { get { throw null; } }

        public Framework.ITaskItem[] RuntimeReferenceOnlySDKDependencies { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] SDKReferences { get { throw null; } set { } }

        public string TargetedSDKArchitecture { get { throw null; } set { } }

        public string TargetedSDKConfiguration { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformIdentifier { get { throw null; } set { } }

        [Framework.Required]
        public string TargetPlatformVersion { get { throw null; } set { } }

        public bool WarnOnMissingPlatformVersion { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class RoslynCodeTaskFactory : Framework.ITaskFactory
    {
        public string FactoryName { get { throw null; } }

        public System.Type TaskType { get { throw null; } }

        public void CleanupTask(Framework.ITask task) { }

        public Framework.ITask CreateTask(Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }

        public Framework.TaskPropertyInfo[] GetTaskParameters() { throw null; }

        public bool Initialize(string taskName, System.Collections.Generic.IDictionary<string, Framework.TaskPropertyInfo> parameterGroup, string taskBody, Framework.IBuildEngine taskFactoryLoggingHost) { throw null; }
    }

    public sealed partial class SetRidAgnosticValueForProjects : TaskExtension
    {
        public SetRidAgnosticValueForProjects() { }

        public Framework.ITaskItem[] Projects { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] UpdatedProjects { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class SGen : ToolTaskExtension
    {
        public SGen() { }

        [Framework.Required]
        public string BuildAssemblyName { get { throw null; } set { } }

        [Framework.Required]
        public string BuildAssemblyPath { get { throw null; } set { } }

        public bool DelaySign { get { throw null; } set { } }

        public string KeyContainer { get { throw null; } set { } }

        public string KeyFile { get { throw null; } set { } }

        public string Platform { get { throw null; } set { } }

        public string[] References { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] SerializationAssembly { get { throw null; } set { } }

        public string SerializationAssemblyName { get { throw null; } }

        [Framework.Required]
        public bool ShouldGenerateSerializer { get { throw null; } set { } }

        protected override string ToolName { get { throw null; } }

        public string[] Types { get { throw null; } set { } }

        public bool UseKeep { get { throw null; } set { } }

        [Framework.Required]
        public bool UseProxyTypes { get { throw null; } set { } }

        public override bool Execute() { throw null; }

        protected override string GenerateFullPathToTool() { throw null; }
    }

    public sealed partial class SignFile : Utilities.Task
    {
        [Framework.Required]
        public string CertificateThumbprint { get { throw null; } set { } }

        public bool DisallowMansignTimestampFallback { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem SigningTarget { get { throw null; } set { } }

        public string TargetFrameworkIdentifier { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public string TimestampUrl { get { throw null; } set { } }

        public override bool Execute() { throw null; }
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

    public abstract partial class TaskRequiresFramework : TaskExtension
    {
        internal TaskRequiresFramework() { }

        public override bool Execute() { throw null; }
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

    public partial class Touch : TaskExtension, Framework.IIncrementalTask
    {
        public Touch() { }

        public bool AlwaysCreate { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] Files { get { throw null; } set { } }

        public bool ForceTouch { get { throw null; } set { } }

        public string Importance { get { throw null; } set { } }

        public string Time { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem[] TouchedFiles { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class UnregisterAssembly : TaskRequiresFramework, IUnregisterAssemblyTaskContract
    {
        public UnregisterAssembly() { }

        public Framework.ITaskItem[] Assemblies { get { throw null; } set { } }

        public Framework.ITaskItem AssemblyListFile { get { throw null; } set { } }

        public Framework.ITaskItem[] TypeLibFiles { get { throw null; } set { } }
    }

    public sealed partial class Unzip : TaskExtension, Framework.ICancelableTask, Framework.ITask, Framework.IIncrementalTask
    {
        public Unzip() { }

        [Framework.Required]
        public Framework.ITaskItem DestinationFolder { get { throw null; } set { } }

        public string Exclude { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        public string Include { get { throw null; } set { } }

        public bool OverwriteReadOnlyFiles { get { throw null; } set { } }

        public bool SkipUnchangedFiles { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem[] SourceFiles { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public sealed partial class UpdateManifest : TaskRequiresFramework
    {
        public UpdateManifest() { }

        public Framework.ITaskItem ApplicationManifest { get { throw null; } set { } }

        public string ApplicationPath { get { throw null; } set { } }

        public Framework.ITaskItem InputManifest { get { throw null; } set { } }

        [Framework.Output]
        public Framework.ITaskItem OutputManifest { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }
    }

    public sealed partial class VerifyFileHash : TaskExtension, Framework.ICancelableTask, Framework.ITask
    {
        public VerifyFileHash() { }

        public string Algorithm { get { throw null; } set { } }

        [Framework.Required]
        public string File { get { throw null; } set { } }

        [Framework.Required]
        public string Hash { get { throw null; } set { } }

        public string HashEncoding { get { throw null; } set { } }

        public void Cancel() { }

        public override bool Execute() { throw null; }
    }

    public sealed partial class Warning : TaskExtension
    {
        public Warning() { }

        public string Code { get { throw null; } set { } }

        public string File { get { throw null; } set { } }

        public string HelpKeyword { get { throw null; } set { } }

        public string HelpLink { get { throw null; } set { } }

        public string Text { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class WinMDExp : TaskRequiresFramework
    {
        public WinMDExp() { }

        public string AssemblyUnificationPolicy { get { throw null; } set { } }

        public string DisabledWarnings { get { throw null; } set { } }

        public string InputDocumentationFile { get { throw null; } set { } }

        public string InputPDBFile { get { throw null; } set { } }

        public string OutputDocumentationFile { get { throw null; } set { } }

        public string OutputPDBFile { get { throw null; } set { } }

        [Framework.Output]
        public string OutputWindowsMetadataFile { get { throw null; } set { } }

        public Framework.ITaskItem[] References { get { throw null; } set { } }

        public string SdkToolsPath { get { throw null; } set { } }

        public bool TreatWarningsAsErrors { get { throw null; } set { } }

        public bool UTF8Output { get { throw null; } set { } }

        public string WinMDModule { get { throw null; } set { } }
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

    public partial class WriteLinesToFile : TaskExtension, Framework.IIncrementalTask
    {
        public WriteLinesToFile() { }

        [System.Obsolete]
        public bool CanBeIncremental { get { throw null; } }

        public string Encoding { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

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

        [Framework.Required]
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

        [Framework.Required]
        public string Query { get { throw null; } set { } }

        public Framework.ITaskItem Value { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem XmlInputPath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public partial class XslTransformation : TaskExtension
    {
        public XslTransformation() { }

        [Framework.Required]
        public Framework.ITaskItem[] OutputPaths { get { throw null; } set { } }

        public string Parameters { get { throw null; } set { } }

        public bool PreserveWhitespace { get { throw null; } set { } }

        public bool UseTrustedSettings { get { throw null; } set { } }

        public string XmlContent { get { throw null; } set { } }

        public Framework.ITaskItem[] XmlInputPaths { get { throw null; } set { } }

        public Framework.ITaskItem XslCompiledDllPath { get { throw null; } set { } }

        public string XslContent { get { throw null; } set { } }

        public Framework.ITaskItem XslInputPath { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }

    public sealed partial class ZipDirectory : TaskExtension, Framework.IIncrementalTask
    {
        public ZipDirectory() { }

        [Framework.Required]
        public Framework.ITaskItem DestinationFile { get { throw null; } set { } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        public bool Overwrite { get { throw null; } set { } }

        [Framework.Required]
        public Framework.ITaskItem SourceDirectory { get { throw null; } set { } }

        public override bool Execute() { throw null; }
    }
}

namespace Microsoft.Build.Tasks.Deployment.Bootstrapper
{
    public partial class BootstrapperBuilder : IBootstrapperBuilder
    {
        public BootstrapperBuilder() { }

        public BootstrapperBuilder(string visualStudioVersion) { }

        public string Path { get { throw null; } set { } }

        public ProductCollection Products { get { throw null; } }

        public BuildResults Build(BuildSettings settings) { throw null; }

        public string[] GetOutputFolders(string[] productCodes, string culture, string fallbackCulture, ComponentsLocation componentsLocation) { throw null; }

        public static string XmlToConfigurationFile(System.Xml.XmlNode input) { throw null; }
    }

    public partial class BuildMessage : IBuildMessage
    {
        internal BuildMessage() { }

        public int HelpId { get { throw null; } }

        public string HelpKeyword { get { throw null; } }

        public string Message { get { throw null; } }

        public BuildMessageSeverity Severity { get { throw null; } }
    }

    public enum BuildMessageSeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    public partial class BuildResults : IBuildResults
    {
        internal BuildResults() { }

        public string[] ComponentFiles { get { throw null; } }

        public string KeyFile { get { throw null; } }

        public BuildMessage[] Messages { get { throw null; } }

        public bool Succeeded { get { throw null; } }
    }

    public partial class BuildSettings : IBuildSettings
    {
        public string ApplicationFile { get { throw null; } set { } }

        public string ApplicationName { get { throw null; } set { } }

        public bool ApplicationRequiresElevation { get { throw null; } set { } }

        public string ApplicationUrl { get { throw null; } set { } }

        public ComponentsLocation ComponentsLocation { get { throw null; } set { } }

        public string ComponentsUrl { get { throw null; } set { } }

        public bool CopyComponents { get { throw null; } set { } }

        public int FallbackLCID { get { throw null; } set { } }

        public int LCID { get { throw null; } set { } }

        public string OutputPath { get { throw null; } set { } }

        public ProductBuilderCollection ProductBuilders { get { throw null; } }

        public string SupportUrl { get { throw null; } set { } }

        public bool Validate { get { throw null; } set { } }
    }

    public enum ComponentsLocation
    {
        HomeSite = 0,
        Relative = 1,
        Absolute = 2
    }

    public partial interface IBootstrapperBuilder
    {
        [System.Runtime.InteropServices.DispId(1)]
        string Path { get; set; }

        [System.Runtime.InteropServices.DispId(4)]
        ProductCollection Products { get; }

        [System.Runtime.InteropServices.DispId(5)]
        BuildResults Build(BuildSettings settings);
    }

    public partial interface IBuildMessage
    {
        [System.Runtime.InteropServices.DispId(4)]
        int HelpId { get; }

        [System.Runtime.InteropServices.DispId(3)]
        string HelpKeyword { get; }

        [System.Runtime.InteropServices.DispId(2)]
        string Message { get; }

        [System.Runtime.InteropServices.DispId(1)]
        BuildMessageSeverity Severity { get; }
    }

    public partial interface IBuildResults
    {
        [System.Runtime.InteropServices.DispId(3)]
        string[] ComponentFiles { get; }

        [System.Runtime.InteropServices.DispId(2)]
        string KeyFile { get; }

        [System.Runtime.InteropServices.DispId(4)]
        BuildMessage[] Messages { get; }

        [System.Runtime.InteropServices.DispId(1)]
        bool Succeeded { get; }
    }

    public partial interface IBuildSettings
    {
        [System.Runtime.InteropServices.DispId(2)]
        string ApplicationFile { get; set; }

        [System.Runtime.InteropServices.DispId(1)]
        string ApplicationName { get; set; }

        [System.Runtime.InteropServices.DispId(13)]
        bool ApplicationRequiresElevation { get; set; }

        [System.Runtime.InteropServices.DispId(3)]
        string ApplicationUrl { get; set; }

        [System.Runtime.InteropServices.DispId(11)]
        ComponentsLocation ComponentsLocation { get; set; }

        [System.Runtime.InteropServices.DispId(4)]
        string ComponentsUrl { get; set; }

        [System.Runtime.InteropServices.DispId(5)]
        bool CopyComponents { get; set; }

        [System.Runtime.InteropServices.DispId(7)]
        int FallbackLCID { get; set; }

        [System.Runtime.InteropServices.DispId(6)]
        int LCID { get; set; }

        [System.Runtime.InteropServices.DispId(8)]
        string OutputPath { get; set; }

        [System.Runtime.InteropServices.DispId(9)]
        ProductBuilderCollection ProductBuilders { get; }

        [System.Runtime.InteropServices.DispId(12)]
        string SupportUrl { get; set; }

        [System.Runtime.InteropServices.DispId(10)]
        bool Validate { get; set; }
    }

    public partial interface IProduct
    {
        [System.Runtime.InteropServices.DispId(4)]
        ProductCollection Includes { get; }

        [System.Runtime.InteropServices.DispId(2)]
        string Name { get; }

        [System.Runtime.InteropServices.DispId(1)]
        ProductBuilder ProductBuilder { get; }

        [System.Runtime.InteropServices.DispId(3)]
        string ProductCode { get; }
    }

    public partial interface IProductBuilder
    {
        [System.Runtime.InteropServices.DispId(1)]
        Product Product { get; }
    }

    public partial interface IProductBuilderCollection
    {
        [System.Runtime.InteropServices.DispId(2)]
        void Add(ProductBuilder builder);
    }

    public partial interface IProductCollection
    {
        [System.Runtime.InteropServices.DispId(1)]
        int Count { get; }

        [System.Runtime.InteropServices.DispId(2)]
        Product Item(int index);
        [System.Runtime.InteropServices.DispId(3)]
        Product Product(string productCode);
    }

    public partial class Product : IProduct
    {
        public ProductCollection Includes { get { throw null; } }

        public string Name { get { throw null; } }

        public ProductBuilder ProductBuilder { get { throw null; } }

        public string ProductCode { get { throw null; } }
    }

    public partial class ProductBuilder : IProductBuilder
    {
        internal ProductBuilder() { }

        public Product Product { get { throw null; } }
    }

    public partial class ProductBuilderCollection : IProductBuilderCollection, System.Collections.IEnumerable
    {
        internal ProductBuilderCollection() { }

        public void Add(ProductBuilder builder) { }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public partial class ProductCollection : IProductCollection, System.Collections.IEnumerable
    {
        internal ProductCollection() { }

        public int Count { get { throw null; } }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }

        public Product Item(int index) { throw null; }

        public Product Product(string productCode) { throw null; }
    }
}

namespace Microsoft.Build.Tasks.Deployment.ManifestUtilities
{
    public sealed partial class ApplicationIdentity
    {
        public ApplicationIdentity(string url, AssemblyIdentity deployManifestIdentity, AssemblyIdentity applicationManifestIdentity) { }

        public ApplicationIdentity(string url, string deployManifestPath, string applicationManifestPath) { }

        public override string ToString() { throw null; }
    }

    public sealed partial class ApplicationManifest : AssemblyManifest
    {
        public ApplicationManifest() { }

        public ApplicationManifest(string targetFrameworkVersion) { }

        public string ConfigFile { get { throw null; } set { } }

        public override AssemblyReference EntryPoint { get { throw null; } set { } }

        public string ErrorReportUrl { get { throw null; } set { } }

        public FileAssociationCollection FileAssociations { get { throw null; } }

        public bool HostInBrowser { get { throw null; } set { } }

        public string IconFile { get { throw null; } set { } }

        public bool IsClickOnceManifest { get { throw null; } set { } }

        public int MaxTargetPath { get { throw null; } set { } }

        public string OSDescription { get { throw null; } set { } }

        public string OSSupportUrl { get { throw null; } set { } }

        public string OSVersion { get { throw null; } set { } }

        public string Product { get { throw null; } set { } }

        public string Publisher { get { throw null; } set { } }

        public string SuiteName { get { throw null; } set { } }

        public string SupportUrl { get { throw null; } set { } }

        public string TargetFrameworkVersion { get { throw null; } set { } }

        public TrustInfo TrustInfo { get { throw null; } set { } }

        public bool UseApplicationTrust { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlConfigFile { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlElement("EntryPointIdentity")]
        public AssemblyIdentity XmlEntryPointIdentity { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlEntryPointParameters { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlEntryPointPath { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlErrorReportUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("FileAssociations")]
        public FileAssociation[] XmlFileAssociations { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlHostInBrowser { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIconFile { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIsClickOnceManifest { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSBuild { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSDescription { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSMajor { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSMinor { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSRevision { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlOSSupportUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProduct { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlPublisher { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSuiteName { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSupportUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlUseApplicationTrust { get { throw null; } set { } }

        public override void Validate() { }
    }

    public sealed partial class AssemblyIdentity
    {
        public AssemblyIdentity() { }

        public AssemblyIdentity(AssemblyIdentity identity) { }

        public AssemblyIdentity(string name, string version, string publicKeyToken, string culture, string processorArchitecture, string type) { }

        public AssemblyIdentity(string name, string version, string publicKeyToken, string culture, string processorArchitecture) { }

        public AssemblyIdentity(string name, string version, string publicKeyToken, string culture) { }

        public AssemblyIdentity(string name, string version) { }

        public AssemblyIdentity(string name) { }

        public string Culture { get { throw null; } set { } }

        public bool IsFrameworkAssembly { get { throw null; } }

        public bool IsNeutralPlatform { get { throw null; } }

        public bool IsStrongName { get { throw null; } }

        public string Name { get { throw null; } set { } }

        public string ProcessorArchitecture { get { throw null; } set { } }

        public string PublicKeyToken { get { throw null; } set { } }

        public string Type { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlCulture { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlName { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProcessorArchitecture { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlPublicKeyToken { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlType { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlVersion { get { throw null; } set { } }

        public static AssemblyIdentity FromAssemblyName(string assemblyName) { throw null; }

        public static AssemblyIdentity FromFile(string path) { throw null; }

        public static AssemblyIdentity FromManagedAssembly(string path) { throw null; }

        public static AssemblyIdentity FromManifest(string path) { throw null; }

        public static AssemblyIdentity FromNativeAssembly(string path) { throw null; }

        public string GetFullName(FullNameFlags flags) { throw null; }

        public bool IsInFramework(string frameworkIdentifier, string frameworkVersion) { throw null; }

        public override string ToString() { throw null; }

        [System.Flags]
        public enum FullNameFlags
        {
            Default = 0,
            ProcessorArchitecture = 1,
            Type = 2,
            All = 3
        }
    }

    public partial class AssemblyManifest : Manifest
    {
        public ProxyStub[] ExternalProxyStubs { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("ExternalProxyStubs")]
        public ProxyStub[] XmlExternalProxyStubs { get { throw null; } set { } }
    }

    public sealed partial class AssemblyReference : BaseReference
    {
        public AssemblyReference() { }

        public AssemblyReference(string path) { }

        public AssemblyIdentity AssemblyIdentity { get { throw null; } set { } }

        public bool IsPrerequisite { get { throw null; } set { } }

        public AssemblyReferenceType ReferenceType { get { throw null; } set { } }

        protected internal override string SortName { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlElement("AssemblyIdentity")]
        public AssemblyIdentity XmlAssemblyIdentity { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIsNative { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIsPrerequisite { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public sealed partial class AssemblyReferenceCollection : System.Collections.IEnumerable
    {
        internal AssemblyReferenceCollection() { }

        public int Count { get { throw null; } }

        public AssemblyReference this[int index] { get { throw null; } }

        public AssemblyReference Add(AssemblyReference assembly) { throw null; }

        public AssemblyReference Add(string path) { throw null; }

        public void Clear() { }

        public AssemblyReference Find(AssemblyIdentity identity) { throw null; }

        public AssemblyReference Find(string name) { throw null; }

        public AssemblyReference FindTargetPath(string targetPath) { throw null; }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }

        public void Remove(AssemblyReference assemblyReference) { }
    }

    public enum AssemblyReferenceType
    {
        Unspecified = 0,
        ClickOnceManifest = 1,
        ManagedAssembly = 2,
        NativeAssembly = 3
    }

    public abstract partial class BaseReference
    {
        protected internal BaseReference() { }

        protected internal BaseReference(string path) { }

        public string Group { get { throw null; } set { } }

        public string Hash { get { throw null; } set { } }

        public bool IsOptional { get { throw null; } set { } }

        public string ResolvedPath { get { throw null; } set { } }

        public long Size { get { throw null; } set { } }

        protected internal abstract string SortName { get; }

        public string SourcePath { get { throw null; } set { } }

        public string TargetPath { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlGroup { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlHash { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlHashAlgorithm { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIsOptional { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlPath { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSize { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public partial class ComClass
    {
        public string ClsId { get { throw null; } }

        public string Description { get { throw null; } }

        public string ProgId { get { throw null; } }

        public string ThreadingModel { get { throw null; } }

        public string TlbId { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        public string XmlClsId { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDescription { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProgId { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlThreadingModel { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlTlbId { get { throw null; } set { } }
    }

    public sealed partial class CompatibleFramework
    {
        public string Profile { get { throw null; } set { } }

        public string SupportedRuntime { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProfile { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSupportedRuntime { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlVersion { get { throw null; } set { } }
    }

    public sealed partial class CompatibleFrameworkCollection : System.Collections.IEnumerable
    {
        internal CompatibleFrameworkCollection() { }

        public int Count { get { throw null; } }

        public CompatibleFramework this[int index] { get { throw null; } }

        public void Add(CompatibleFramework compatibleFramework) { }

        public void Clear() { }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public sealed partial class DeployManifest : Manifest
    {
        public DeployManifest() { }

        public DeployManifest(string targetFrameworkMoniker) { }

        public CompatibleFrameworkCollection CompatibleFrameworks { get { throw null; } }

        public bool CreateDesktopShortcut { get { throw null; } set { } }

        public string DeploymentUrl { get { throw null; } set { } }

        public bool DisallowUrlActivation { get { throw null; } set { } }

        public override AssemblyReference EntryPoint { get { throw null; } set { } }

        public string ErrorReportUrl { get { throw null; } set { } }

        public bool Install { get { throw null; } set { } }

        public bool MapFileExtensions { get { throw null; } set { } }

        public string MinimumRequiredVersion { get { throw null; } set { } }

        public string Product { get { throw null; } set { } }

        public string Publisher { get { throw null; } set { } }

        public string SuiteName { get { throw null; } set { } }

        public string SupportUrl { get { throw null; } set { } }

        public string TargetFrameworkMoniker { get { throw null; } set { } }

        public bool TrustUrlParameters { get { throw null; } set { } }

        public bool UpdateEnabled { get { throw null; } set { } }

        public int UpdateInterval { get { throw null; } set { } }

        public UpdateMode UpdateMode { get { throw null; } set { } }

        public UpdateUnit UpdateUnit { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("CompatibleFrameworks")]
        public CompatibleFramework[] XmlCompatibleFrameworks { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlCreateDesktopShortcut { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDeploymentUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDisallowUrlActivation { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlErrorReportUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlInstall { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlMapFileExtensions { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlMinimumRequiredVersion { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProduct { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlPublisher { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSuiteName { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSupportUrl { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlTrustUrlParameters { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlUpdateEnabled { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlUpdateInterval { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlUpdateMode { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlUpdateUnit { get { throw null; } set { } }

        public override void Validate() { }
    }

    public sealed partial class FileAssociation
    {
        public string DefaultIcon { get { throw null; } set { } }

        public string Description { get { throw null; } set { } }

        public string Extension { get { throw null; } set { } }

        public string ProgId { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDefaultIcon { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDescription { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlExtension { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlProgId { get { throw null; } set { } }
    }

    public sealed partial class FileAssociationCollection : System.Collections.IEnumerable
    {
        internal FileAssociationCollection() { }

        public int Count { get { throw null; } }

        public FileAssociation this[int index] { get { throw null; } }

        public void Add(FileAssociation fileAssociation) { }

        public void Clear() { }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public sealed partial class FileReference : BaseReference
    {
        public FileReference() { }

        public FileReference(string path) { }

        public ComClass[] ComClasses { get { throw null; } }

        public bool IsDataFile { get { throw null; } set { } }

        public ProxyStub[] ProxyStubs { get { throw null; } }

        protected internal override string SortName { get { throw null; } }

        public TypeLib[] TypeLibs { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("ComClasses")]
        public ComClass[] XmlComClasses { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("ProxyStubs")]
        public ProxyStub[] XmlProxyStubs { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("TypeLibs")]
        public TypeLib[] XmlTypeLibs { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlWriteableType { get { throw null; } set { } }
    }

    public sealed partial class FileReferenceCollection : System.Collections.IEnumerable
    {
        internal FileReferenceCollection() { }

        public int Count { get { throw null; } }

        public FileReference this[int index] { get { throw null; } }

        public FileReference Add(FileReference file) { throw null; }

        public FileReference Add(string path) { throw null; }

        public void Clear() { }

        public FileReference FindTargetPath(string targetPath) { throw null; }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }

        public void Remove(FileReference file) { }
    }

    public partial class LauncherBuilder
    {
        public LauncherBuilder(string launcherPath) { }

        public string LauncherPath { get { throw null; } set { } }

        public Bootstrapper.BuildResults Build(string filename, string outputPath) { throw null; }
    }

    public abstract partial class Manifest
    {
        protected internal Manifest() { }

        public AssemblyIdentity AssemblyIdentity { get { throw null; } set { } }

        public string AssemblyName { get { throw null; } set { } }

        public AssemblyReferenceCollection AssemblyReferences { get { throw null; } }

        public string Description { get { throw null; } set { } }

        public virtual AssemblyReference EntryPoint { get { throw null; } set { } }

        public FileReferenceCollection FileReferences { get { throw null; } }

        public System.IO.Stream InputStream { get { throw null; } set { } }

        public bool LauncherBasedDeployment { get { throw null; } set { } }

        public OutputMessageCollection OutputMessages { get { throw null; } }

        public bool ReadOnly { get { throw null; } set { } }

        public string SourcePath { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlElement("AssemblyIdentity")]
        public AssemblyIdentity XmlAssemblyIdentity { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("AssemblyReferences")]
        public AssemblyReference[] XmlAssemblyReferences { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlDescription { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        [System.Xml.Serialization.XmlArray("FileReferences")]
        public FileReference[] XmlFileReferences { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlSchema { get { throw null; } set { } }

        public void ResolveFiles() { }

        public void ResolveFiles(string[] searchPaths) { }

        public override string ToString() { throw null; }

        public void UpdateFileInfo() { }

        public void UpdateFileInfo(string targetFrameworkVersion) { }

        public virtual void Validate() { }

        protected void ValidatePlatform() { }
    }

    public static partial class ManifestReader
    {
        public static Manifest ReadManifest(System.IO.Stream input, bool preserveStream) { throw null; }

        public static Manifest ReadManifest(string path, bool preserveStream) { throw null; }

        public static Manifest ReadManifest(string manifestType, System.IO.Stream input, bool preserveStream) { throw null; }

        public static Manifest ReadManifest(string manifestType, string path, bool preserveStream) { throw null; }
    }

    public static partial class ManifestWriter
    {
        public static void WriteManifest(Manifest manifest, System.IO.Stream output) { }

        public static void WriteManifest(Manifest manifest, string path, string targetframeWorkVersion) { }

        public static void WriteManifest(Manifest manifest, string path) { }

        public static void WriteManifest(Manifest manifest) { }
    }

    public sealed partial class OutputMessage
    {
        internal OutputMessage() { }

        public string Name { get { throw null; } }

        public string Text { get { throw null; } }

        public OutputMessageType Type { get { throw null; } }

        public string[] GetArguments() { throw null; }
    }

    public sealed partial class OutputMessageCollection : System.Collections.IEnumerable
    {
        internal OutputMessageCollection() { }

        public int ErrorCount { get { throw null; } }

        public OutputMessage this[int index] { get { throw null; } }

        public int WarningCount { get { throw null; } }

        public void Clear() { }

        public System.Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public enum OutputMessageType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    public partial class ProxyStub
    {
        public string BaseInterface { get { throw null; } }

        public string IID { get { throw null; } }

        public string Name { get { throw null; } }

        public string NumMethods { get { throw null; } }

        public string TlbId { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        public string XmlBaseInterface { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlIID { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlName { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlNumMethods { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlTlbId { get { throw null; } set { } }
    }

    public static partial class SecurityUtilities
    {
        public static void SignFile(System.Security.Cryptography.X509Certificates.X509Certificate2 cert, System.Uri timestampUrl, string path) { }

        public static void SignFile(string certPath, System.Security.SecureString certPassword, System.Uri timestampUrl, string path) { }

        public static void SignFile(string certThumbprint, System.Uri timestampUrl, string path, string targetFrameworkVersion, string targetFrameworkIdentifier, bool disallowMansignTimestampFallback) { }

        public static void SignFile(string certThumbprint, System.Uri timestampUrl, string path, string targetFrameworkVersion, string targetFrameworkIdentifier) { }

        public static void SignFile(string certThumbprint, System.Uri timestampUrl, string path, string targetFrameworkVersion) { }

        public static void SignFile(string certThumbprint, System.Uri timestampUrl, string path) { }
    }

    public sealed partial class TrustInfo
    {
        public bool HasUnmanagedCodePermission { get { throw null; } }

        public bool IsFullTrust { get { throw null; } }

        public bool PreserveFullTrustPermissionSet { get { throw null; } set { } }

        public string SameSiteAccess { get { throw null; } set { } }

        public void Clear() { }

        public void Read(System.IO.Stream input) { }

        public void Read(string path) { }

        public void ReadManifest(System.IO.Stream input) { }

        public void ReadManifest(string path) { }

        public override string ToString() { throw null; }

        public void Write(System.IO.Stream output) { }

        public void Write(string path) { }

        public void WriteManifest(System.IO.Stream input, System.IO.Stream output) { }

        public void WriteManifest(System.IO.Stream output) { }

        public void WriteManifest(string path) { }
    }

    public partial class TypeLib
    {
        public string Flags { get { throw null; } }

        public string HelpDirectory { get { throw null; } }

        public string ResourceId { get { throw null; } }

        public string TlbId { get { throw null; } }

        public string Version { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        public string XmlFlags { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlHelpDirectory { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlResourceId { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlTlbId { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlVersion { get { throw null; } set { } }
    }

    public enum UpdateMode
    {
        Background = 0,
        Foreground = 1
    }

    public enum UpdateUnit
    {
        Hours = 0,
        Days = 1,
        Weeks = 2
    }

    public partial class WindowClass
    {
        public WindowClass() { }

        public WindowClass(string name, bool versioned) { }

        public string Name { get { throw null; } }

        public bool Versioned { get { throw null; } }

        [System.ComponentModel.Browsable(false)]
        public string XmlName { get { throw null; } set { } }

        [System.ComponentModel.Browsable(false)]
        public string XmlVersioned { get { throw null; } set { } }
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

namespace System.Deployment.Internal.CodeSigning
{
    public sealed partial class RSAPKCS1SHA256SignatureDescription : Security.Cryptography.SignatureDescription
    {
        public override Security.Cryptography.AsymmetricSignatureDeformatter CreateDeformatter(Security.Cryptography.AsymmetricAlgorithm key) { throw null; }

        public override Security.Cryptography.AsymmetricSignatureFormatter CreateFormatter(Security.Cryptography.AsymmetricAlgorithm key) { throw null; }
    }
}