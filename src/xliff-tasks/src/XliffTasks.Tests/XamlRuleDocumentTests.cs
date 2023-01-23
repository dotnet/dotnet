// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;
using Xunit;

namespace XliffTasks.Tests
{
    public class XamlRuleTranslationTests
    {
      [Fact]
      public void Test()
      {
        var source = """
<?xml version="1.0" encoding="utf-8" ?>
<!-- Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information. -->
<Rule Name="Build"
      Description="Specifies properties that control how the project builds."
      DisplayName="Build"
      PageTemplate="generic"
      Order="200"
      xmlns="http://schemas.microsoft.com/build/2009/properties" 
      xmlns:xliff="https://github.com/dotnet/xliff-tasks" 
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
      mc:Ignorable="xliff">

  <Rule.Categories>
    <Category Name="General"
              DisplayName="General" />

    <Category Name="ErrorsAndWarnings"
              Description="Configures the error and warning options for the build process."
              DisplayName="Errors and warnings" />

    <Category Name="Output"
              Description="Configures the output options for the build process."
              DisplayName="Output" />

    <Category Name="Events"
              Description="Configures custom events that run before and after build."
              DisplayName="Events" />

    <Category Name="StrongNaming"
              Description="Configures strong name signing of build outputs."
              DisplayName="Strong naming" />

    <Category Name="Advanced"
              DisplayName="Advanced"
              Description="Advanced settings for the application." />
  </Rule.Categories>

  <Rule.DataSource>
    <DataSource Persistence="ProjectFile"
                SourceOfDefaultValue="AfterContext"
                HasConfigurationCondition="True" />
  </Rule.DataSource>
  
  <StringProperty Name="DefineConstants" 
                       DisplayName="Conditional compilation symbols"
                       Description="Specifies symbols on which to perform conditional compilation."
                       HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147079"
                       Category="General">
    <StringProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception"
                  HasConfigurationCondition="True" />
    </StringProperty.DataSource>
    <StringProperty.ValueEditors>
       <NameValuePair Name="TypeDescriptorText" Value="Custom symbols" xliff:LocalizedProperties="Value" />
       <NameValuePair Name="SearchTerms" Value="MySearchTerms" />
      <ValueEditor EditorType="MultiStringSelector">
        <ValueEditor.Metadata>
          <NameValuePair Name="AllowsCustomStrings" Value="True" />
          <NameValuePair Name="ShouldDisplayEvaluatedPreview" Value="True" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>

  <DynamicEnumProperty Name="PlatformTarget"
                DisplayName="Platform target"
                Description="Specifies the processor to be targeted by the output file. Choose 'Any CPU' to specify that any processor is acceptable, allowing the application to run on the broadest range of hardware."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147129"
                Category="General"
                EnumProvider="PlatformTargetEnumProvider"
                MultipleValuesAllowed="False">
    <DynamicEnumProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception"
                  HasConfigurationCondition="False" />
    </DynamicEnumProperty.DataSource>
  </DynamicEnumProperty>

  <EnumProperty Name="Nullable"
                DisplayName="Nullable"
                Description="Specifies the project-wide C# nullable context. Only available for projects that use C# 8.0 or later."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2146796"
                Category="General">
    <EnumProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </EnumProperty.DataSource>
    <EnumProperty.Metadata>
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>(has-csharp-lang-version-or-greater "8.0")</NameValuePair.Value>
      </NameValuePair>
    </EnumProperty.Metadata>
    <EnumValue Name="disable"
               DisplayName="Disable" />
    <EnumValue Name="enable"
               DisplayName="Enable" />
    <EnumValue Name="warnings"
               DisplayName="Warnings" />
    <EnumValue Name="annotations"
               DisplayName="Annotations" />
  </EnumProperty>

  <BoolProperty Name="GenerateAssemblyInfo"
                DisplayName="Generate assembly info"
                Description="Transform project properties into assembly attributes during build."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2220622"
                Category="General" />

  <BoolProperty Name="Prefer32Bit"
                DisplayName="Prefer 32-bit"
                Description="Run in 32-bit mode on systems that support both 32-bit and 64-bit applications."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2166468"
                Category="General">
    <BoolProperty.Metadata>
      <NameValuePair Name="DependsOn" Value="Build::PlatformTarget" />
      <NameValuePair Name="DependsOn" Value="Application::OutputType" />
      <NameValuePair Name="VisibilityCondition">
        <!-- Visibility based on: https://github.com/dotnet/msbuild/blob/9bcc06cbe19ae2482ab18eab90a82fd079b26897/src/Tasks/Microsoft.NETFramework.CurrentVersion.props#L87 -->
        <NameValuePair.Value>
          (and
            (has-net-framework)
            (has-evaluated-value "Build" "PlatformTarget" "Any CPU")
            (or
              (has-evaluated-value "Application" "OutputType" "Exe")
              (has-evaluated-value "Application" "OutputType" "WinExe")
              (has-evaluated-value "Application" "OutputType" "AppContainerExe")
            )
          )
        </NameValuePair.Value>
      </NameValuePair>
    </BoolProperty.Metadata>
  </BoolProperty>

  <!-- Localization Notice: 'unsafe' is used as a keyword in the description and should not be translated -->
  <BoolProperty Name="AllowUnsafeBlocks"
                DisplayName="Unsafe code"
                Description="Allow code that uses the 'unsafe' keyword to compile."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2146797"
                Category="General">
    <BoolProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </BoolProperty.DataSource>
    <BoolProperty.Metadata>
      <NameValuePair Name="SearchTerms" Value="pointers" />
    </BoolProperty.Metadata>
  </BoolProperty>

  <BoolProperty Name="Optimize"
                DisplayName="Optimize code"
                Description="Enable compiler optimizations for smaller, faster, and more efficient output."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147080"
                Category="General">
    <BoolProperty.Metadata>
      <NameValuePair Name="SearchTerms" Value="optimise;optimisation" />
    </BoolProperty.Metadata>
  </BoolProperty>

  <EnumProperty Name="DebugType"
                DisplayName="Debug symbols"
                Description="Specifies the kind of debug symbols produced during build."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2173089"
                Category="General">
    <EnumProperty.DataSource>
      <DataSource HasConfigurationCondition="True"
                  Persistence="ProjectFileWithInterception" />
    </EnumProperty.DataSource>
    <EnumProperty.Metadata>
      <NameValuePair Name="SearchTerms" Value="debug type" />
    </EnumProperty.Metadata>
    <EnumValue Name="none"
               DisplayName="No symbols are emitted" />
    <!--
    Note that 'pdbonly' is the same as 'full'.
    <EnumValue Name="pdbonly"
               DisplayName="PDB Only" />
    -->
    <EnumValue Name="full"
               DisplayName="PDB file, current platform" />
    <EnumValue Name="portable"
               DisplayName="PDB file, portable across platforms" />
    <EnumValue Name="embedded"
               DisplayName="Embedded in DLL/EXE, portable across platforms" />
  </EnumProperty>

  <BoolProperty Name="WarningLevelOverridden"
                ReadOnly="True"
                Visible="False">
    <BoolProperty.DataSource>
      <DataSource HasConfigurationCondition="True"
                  Persistence="ProjectFileWithInterception" />
    </BoolProperty.DataSource>
  </BoolProperty>

  <EnumProperty Name="WarningLevel"
                DisplayName="Warning level"
                Description="Specifies the level to display for compiler warnings. Higher levels produce more warnings, and include all warnings from lower levels."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2146798"
                Category="ErrorsAndWarnings">
    <EnumProperty.Metadata>
      <NameValuePair Name="EditabilityCondition">
        <NameValuePair.Value>
          (has-evaluated-value "Build" "WarningLevelOverridden" false)
        </NameValuePair.Value>
      </NameValuePair>
    </EnumProperty.Metadata>
    <EnumValue Name="0"
               DisplayName="0 - Disable all warnings" />
    <EnumValue Name="1"
               DisplayName="1 - Severe warning messages" />
    <EnumValue Name="2"
               DisplayName="2 - Less severe warnings, such as warnings about hiding class members" />
    <EnumValue Name="3"
               DisplayName="3 - Less severe warnings, such as warnings about expressions that always evaluate to true or false" />
    <EnumValue Name="4"
               DisplayName="4 - Informational warnings" />
    <EnumValue Name="5"
               DisplayName="5 - Warnings from C# 9" />
    <EnumValue Name="6"
               DisplayName="6 - Warnings from C# 10" />
    <EnumValue Name="7"
               DisplayName="7 - Warnings from C# 11" />
    <EnumValue Name="9999"
               DisplayName="9999 - All warnings" />
  </EnumProperty>
  
  <StringProperty Name="NoWarn"
                  DisplayName="Suppress specific warnings"
                  Description="Blocks the compiler from generating the specified warnings. Separate multiple warning numbers with a comma (',') or semicolon (';')."
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147300"
                  Category="ErrorsAndWarnings" />

  <BoolProperty Name="TreatWarningsAsErrors"
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147301"
                DisplayName="Treat warnings as errors"
                Description="Instruct the compiler to treat warnings as errors."
                Category="ErrorsAndWarnings">
    <BoolProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception" />
    </BoolProperty.DataSource>
  </BoolProperty>

  <StringProperty Name="WarningsAsErrors"
                  DisplayName="Treat specific warnings as errors"
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147301"
                  Description="Specifies which warnings are treated as errors. Separate multiple warning numbers with a comma (',') or semicolon (';')."
                  Category="ErrorsAndWarnings">
    <StringProperty.Metadata>
      <NameValuePair Name="DependsOn" Value="Build::TreatWarningsAsErrors" />
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>
          (has-evaluated-value "Build" "TreatWarningsAsErrors" false)
        </NameValuePair.Value>
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>

  <StringProperty Name="WarningsNotAsErrors"
                  DisplayName="Exclude specific warnings as errors"
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147301"
                  Description="Specifies which warnings are excluded from being treated as errors. Separate multiple warning numbers with a comma (',') or semicolon (';')."
                  Category="ErrorsAndWarnings">
    <StringProperty.Metadata>
      <NameValuePair Name="DependsOn" Value="Build::TreatWarningsAsErrors" />
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>
          (has-evaluated-value "Build" "TreatWarningsAsErrors" true)
        </NameValuePair.Value>
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>

  <StringProperty Name="BaseOutputPath"
                  DisplayName="Base output path"
                  Description="Specifies the base location for the project's output during build. Subfolders will be appended to this path to differentiate project configuration."
                  Category="Output"
                  Subtype="directory">
    <StringProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </StringProperty.DataSource>
  </StringProperty>

  <StringProperty Name="BaseIntermediateOutputPath"
                  DisplayName="Base intermediate output path"
                  Description="Specifies the base location for the project's intermediate output during build. Subfolders will be appended to this path to differentiate project configuration."
                  Category="Output"
                  Subtype="directory">
    <StringProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </StringProperty.DataSource>
  </StringProperty>

  <BoolProperty Name="ProduceReferenceAssembly"
                DisplayName="Reference assembly"
                Description="Produce a reference assembly containing the public API of the project."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2166115"
                Category="Output">
    <BoolProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </BoolProperty.DataSource>
  </BoolProperty>

  <BoolProperty Name="GenerateDocumentationFile"
                DisplayName="Documentation file"
                Description="Generate a file containing API documentation."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165772"
                Category="Output">
    <BoolProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </BoolProperty.DataSource>
    <BoolProperty.Metadata>
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>(has-project-capability "GenerateDocumentationFile")</NameValuePair.Value>
      </NameValuePair>
    </BoolProperty.Metadata>
  </BoolProperty>

  <!-- TODO consider removing this property from the UI altogether -->
  <StringProperty Name="DocumentationFile"
                  DisplayName="XML documentation file path"
                  Description="Optional path for the API documentation file. Leave blank to use the default location."
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147081"
                  Category="Output"
                  Subtype="file">
    <StringProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </StringProperty.DataSource>
    <StringProperty.Metadata>
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>(has-evaluated-value "Build" "GenerateDocumentationFile" true)</NameValuePair.Value>
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>

  <StringProperty Name="PreBuildEvent"
                  DisplayName="Pre-build event"
                  Description="Specifies commands that run before the build starts. Does not run if the project is up-to-date. A non-zero exit code will fail the build before it runs."
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165773"
                  Category="Events">
    <StringProperty.DataSource>
      <DataSource HasConfigurationCondition="False"
                  Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="AfterContext" />
    </StringProperty.DataSource>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType="MultiLineString">
        <ValueEditor.Metadata>
          <NameValuePair Name="UseMonospaceFont" Value="True" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>

  <StringProperty Name="PostBuildEvent"
                  DisplayName="Post-build event"
                  Description="Specifies commands that run after the build completes. Does not run if the build failed. Use 'call' to invoke .bat files. A non-zero exit code will fail the build."
                  HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165773"
                  Category="Events">
    <StringProperty.DataSource>
      <DataSource HasConfigurationCondition="False"
                  Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="AfterContext" />
    </StringProperty.DataSource>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType="MultiLineString">
        <ValueEditor.Metadata>
          <NameValuePair Name="UseMonospaceFont" Value="True" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>

  <EnumProperty Name="RunPostBuildEvent"
                DisplayName="When to run the post-build event"
                Description="Specifies under which condition the post-build event will be executed."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165773"
                Category="Events">
    <EnumProperty.DataSource>
      <DataSource HasConfigurationCondition="False"
                  PersistedName="RunPostBuildEvent"
                  Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="AfterContext" />
    </EnumProperty.DataSource>
    <EnumValue Name="Always"
               DisplayName="Always" />
    <EnumValue Name="OnBuildSuccess"
               DisplayName="When the build succeeds"
               IsDefault="True" />
    <EnumValue Name="OnOutputUpdated"
               DisplayName="When the output is updated" />
  </EnumProperty>

  <BoolProperty Name="SignAssembly"
                Description="Sign the output assembly to give it a strong name."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147136"
                DisplayName="Sign the assembly"
                Category="StrongNaming">
    <BoolProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="BeforeContext"
                  HasConfigurationCondition="False" />
    </BoolProperty.DataSource>
  </BoolProperty>

  <StringProperty Name="AssemblyOriginatorKeyFile"
                  DisplayName="Strong name key file"
                  Category="StrongNaming"
                  Subtype="file">
    <StringProperty.Metadata>
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>(has-evaluated-value "Build" "SignAssembly" true)</NameValuePair.Value>
      </NameValuePair>
    </StringProperty.Metadata>
    <StringProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="BeforeContext"
                  HasConfigurationCondition="False" />
    </StringProperty.DataSource>
  </StringProperty>

  <BoolProperty Name="DelaySign"
                Description="Use delayed signing when access to the private key is restricted. The public key will be used during the build, and addition of the private key information deferred until the assembly is handed off."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2147243"
                DisplayName="Delay sign only"
                Category="StrongNaming">
    <BoolProperty.Metadata>
      <NameValuePair Name="VisibilityCondition">
        <NameValuePair.Value>(has-evaluated-value "Build" "SignAssembly" true)</NameValuePair.Value>
      </NameValuePair>
    </BoolProperty.Metadata>
    <BoolProperty.DataSource>
      <DataSource Persistence="ProjectFileWithInterception"
                  SourceOfDefaultValue="BeforeContext"
                  HasConfigurationCondition="False" />
    </BoolProperty.DataSource>
  </BoolProperty>

  <StringProperty Name="LangVersion"
                  DisplayName="Language version"
                  Description="The version of the language available to code in this project."
                  HelpUrl="https://aka.ms/csharp-versions"
                  ReadOnly="true"
                  Category="Advanced">
    <StringProperty.ValueEditors>
      <ValueEditor EditorType="String">
        <ValueEditor.Metadata>
          <NameValuePair Name="ShowEvaluatedPreviewOnly" Value="True" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>

  <BoolProperty Name="CheckForOverflowUnderflow"
                DisplayName="Check for arithmetic overflow"
                Description="Throw exceptions when integer arithmetic produces out of range values."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2166113"
                Category="Advanced">
    <BoolProperty.Metadata>
      <NameValuePair Name="SearchTerms" Value="checked;unchecked" />
    </BoolProperty.Metadata>
  </BoolProperty>

  <BoolProperty Name="Deterministic"
                DisplayName="Deterministic"
                Description="Produce identical compilation output for identical inputs."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165770"
                Category="Advanced" />

  <EnumProperty Name="ErrorReport"
                DisplayName="Internal compiler error reporting"
                Description="Send internal compiler error (ICE) reports to Microsoft."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2165771"
                Category="Advanced">
    <EnumProperty.DataSource>
      <DataSource HasConfigurationCondition="False" />
    </EnumProperty.DataSource>
    <EnumValue Name="none"
               DisplayName="Never send" />
    <EnumValue Name="prompt"
               DisplayName="Prompt before sending" />
    <EnumValue Name="queue"
               DisplayName="Queue" />
    <EnumValue Name="send"
               DisplayName="Send" />
  </EnumProperty>

  <EnumProperty Name="FileAlignment"
                DisplayName="File alignment"
                Description="Specifies, in bytes, where to align the sections of the output file."
                HelpUrl="https://go.microsoft.com/fwlink/?linkid=2166114"
                Category="Advanced">
    <EnumValue Name="512"
               DisplayName="512" />
    <EnumValue Name="1024"
               DisplayName="1024" />
    <EnumValue Name="2048"
               DisplayName="2048" />
    <EnumValue Name="4096"
               DisplayName="4096" />
    <EnumValue Name="8192"
               DisplayName="8192" />
  </EnumProperty>

</Rule>
""";
        
        XamlRuleDocument document = new();
        StringWriter writer = new();
        document.Load(new StringReader(source));

        XlfDocument xliffDocument = new();
        xliffDocument.LoadNew("fr");
        xliffDocument.Update(document, "test.xaml");

      }
      
      
        [Fact]
        public void BasicLoadAndTranslate()
        {
            string source =
@"<Rule Name=""MyRule""
        DisplayName=""My rule display name""
        PageTemplate=""generic""
        Description=""My rule description""
        xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" DisplayName=""My category display name"">
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <EnumProperty Name=""MyEnumProperty"" DisplayName=""My enum property display name"" Category=""MyCategory"" Description=""Specifies the source file will be copied to the output directory."">
    <!-- DisplayName: My enum property display name comment -->
    <!-- Description: My enum property description comment -->
    <EnumValue Name=""First"" DisplayName=""Do the first thing"">
      <!-- DisplayName: My first item comment -->
    </EnumValue>
    <EnumValue Name=""Second"" DisplayName=""Do the second thing"" />
    <EnumValue Name=""Third"" DisplayName=""Do the third thing"" />
  </EnumProperty>
  <BoolProperty Name=""MyBoolProperty"" Description=""My bool property description."" />
  <StringProperty Name=""MyStringProperty"">
    <StringProperty.Metadata>
      <NameValuePair Name=""SearchTerms"" Value=""My;Search;Terms"" TranslatableProp1=""tr1"" TranslatableProp2=""tr2"" NonTranslatableProp3=""same"" xliff:LocalizedProperties=""TranslatableProp1;TranslatableProp2"">
        <!-- Value: My search terms comment -->
      </NameValuePair>
      <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"">
        <!-- Value: My type descriptor text comment -->
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>
</Rule>";

            string expectedXlf =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.xaml"">
    <body>
      <trans-unit id=""BoolProperty|MyBoolProperty|Description"">
        <source>My bool property description.</source>
        <target state=""new"">My bool property description.</target>
        <note />
      </trans-unit>
      <trans-unit id=""Category|MyCategory|DisplayName"">
        <source>My category display name</source>
        <target state=""new"">My category display name</target>
        <note>My category display name comment</note>
      </trans-unit>
      <trans-unit id=""EnumProperty|MyEnumProperty|Description"">
        <source>Specifies the source file will be copied to the output directory.</source>
        <target state=""new"">Specifies the source file will be copied to the output directory.</target>
        <note>My enum property description comment</note>
      </trans-unit>
      <trans-unit id=""EnumProperty|MyEnumProperty|DisplayName"">
        <source>My enum property display name</source>
        <target state=""new"">My enum property display name</target>
        <note>My enum property display name comment</note>
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.First|DisplayName"">
        <source>Do the first thing</source>
        <target state=""new"">Do the first thing</target>
        <note>My first item comment</note>
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.Second|DisplayName"">
        <source>Do the second thing</source>
        <target state=""new"">Do the second thing</target>
        <note />
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.Third|DisplayName"">
        <source>Do the third thing</source>
        <target state=""new"">Do the third thing</target>
        <note />
      </trans-unit>
      <trans-unit id=""Rule|MyRule|Description"">
        <source>My rule description</source>
        <target state=""new"">My rule description</target>
        <note>My rule description comment</note>
      </trans-unit>
      <trans-unit id=""Rule|MyRule|DisplayName"">
        <source>My rule display name</source>
        <target state=""new"">My rule display name</target>
        <note>My rule display name comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms"">
        <source>My;Search;Terms</source>
        <target state=""new"">My;Search;Terms</target>
        <note>My search terms comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"">
        <source>tr1</source>
        <target state=""new"">tr1</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"">
        <source>tr2</source>
        <target state=""new"">tr2</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"">
        <source>Custom symbols</source>
        <target state=""new"">Custom symbols</target>
        <note>My type descriptor text comment</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            Dictionary<string, string> translations = new()
            {
                ["Rule|MyRule|DisplayName"] = "AAA",
                ["Rule|MyRule|Description"] = "BBB",
                ["Category|MyCategory|DisplayName"] = "CCC",
                ["EnumProperty|MyEnumProperty|DisplayName"] = "DDD",
                ["EnumProperty|MyEnumProperty|Description"] = "EEE",
                ["EnumValue|MyEnumProperty.First|DisplayName"] = "FFF",
                ["EnumValue|MyEnumProperty.Second|DisplayName"] = "GGG",
                ["EnumValue|MyEnumProperty.Third|DisplayName"] = "HHH",
                ["BoolProperty|MyBoolProperty|Description"] = "III",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms"] = "JJJ",
                ["StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"] = "NNN",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"] = "LLL",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"] = "MMM",
            };

            string expectedTranslation =
@"<Rule Name=""MyRule"" DisplayName=""AAA"" PageTemplate=""generic"" Description=""BBB"" xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" DisplayName=""CCC"">
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <EnumProperty Name=""MyEnumProperty"" DisplayName=""DDD"" Category=""MyCategory"" Description=""EEE"">
    <!-- DisplayName: My enum property display name comment -->
    <!-- Description: My enum property description comment -->
    <EnumValue Name=""First"" DisplayName=""FFF"">
      <!-- DisplayName: My first item comment -->
    </EnumValue>
    <EnumValue Name=""Second"" DisplayName=""GGG"" />
    <EnumValue Name=""Third"" DisplayName=""HHH"" />
  </EnumProperty>
  <BoolProperty Name=""MyBoolProperty"" Description=""III"" />
  <StringProperty Name=""MyStringProperty"">
    <StringProperty.Metadata>
      <NameValuePair Name=""SearchTerms"" Value=""JJJ"" TranslatableProp1=""LLL"" TranslatableProp2=""MMM"" NonTranslatableProp3=""same"" xliff:LocalizedProperties=""TranslatableProp1;TranslatableProp2"">
        <!-- Value: My search terms comment -->
      </NameValuePair>
      <NameValuePair Name=""TypeDescriptorText"" Value=""NNN"" xliff:LocalizedProperties=""Value"">
        <!-- Value: My type descriptor text comment -->
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>
</Rule>";

            RunXamlTranslationTest(source, translations, expectedTranslation, expectedXlf);
        }

        
        /* the purpose of this test is to ensure that deeply nested translations translate as expected, as well as attributes declared as a nested element:
          such as:
          <A Name="MyName">
            <A.Description>MyName Description</A.Description>
            <B>
              <C>
                <D>
                  <NameValuePair Name="SearchTerms" TranslatableProp1="this-is-translated" xliff:LocalizedProperties="TranslatableProp1">
                    <NameValuePair.Value>My Search Terms</NameValuePair.Value> 
                </D>
                ....
    
        */
        [Fact]
        public void LoadAndTranslateWithDeeplyNestedTranslationAndElementAttributeSyntax()
        {
            string source =
@"<Rule Name=""MyRule""
        DisplayName=""My rule display name""
        PageTemplate=""generic""
        Description=""My rule description""
        xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" Description=""My category description"">
      <Category.DisplayName>My category display name</Category.DisplayName>
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <StringProperty Name=""MyStringProperty"" DisplayName=""MyStringProperty display name"">
    <StringProperty.Description>MyStringProperty description</StringProperty.Description>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType=""MultiStringSelector"">
        <ValueEditor.Metadata>
          <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"" />
          <NameValuePair Name=""SearchTerms"">
            <NameValuePair.Value>My search terms</NameValuePair.Value>
          </NameValuePair>

          <TestElement Name=""TranslatableElement"" LocalizablePropC=""LocalizableC attribute"" xliff:LocalizedProperties=""LocalizablePropA;LocalizablePropB;LocalizablePropC"">
            <TestElement.LocalizablePropA>LocalizableA descendent prop</TestElement.LocalizablePropA>
            <TestElement.LocalizablePropB>LocalizableB descendent prop</TestElement.LocalizablePropB>
          </TestElement>

          <NameValuePair Name=""ShouldDisplayEvaluatedPreview"" Value=""True"" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>
</Rule>";

            string expectedXlf =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.xaml"">
    <body>
      <trans-unit id=""Category|MyCategory|Description"">
        <source>My category description</source>
        <target state=""new"">My category description</target>
        <note />
      </trans-unit>
      <trans-unit id=""Category|MyCategory|DisplayName"">
        <source>My category display name</source>
        <target state=""new"">My category display name</target>
        <note />
      </trans-unit>
      <trans-unit id=""Rule|MyRule|Description"">
        <source>My rule description</source>
        <target state=""new"">My rule description</target>
        <note>My rule description comment</note>
      </trans-unit>
      <trans-unit id=""Rule|MyRule|DisplayName"">
        <source>My rule display name</source>
        <target state=""new"">My rule display name</target>
        <note>My rule display name comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Description"">
        <source>MyStringProperty description</source>
        <target state=""new"">MyStringProperty description</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|DisplayName"">
        <source>MyStringProperty display name</source>
        <target state=""new"">MyStringProperty display name</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|Metadata|LocalizablePropA"">
        <source>LocalizableA descendent prop</source>
        <target state=""new"">LocalizableA descendent prop</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|Metadata|LocalizablePropB"">
        <source>LocalizableB descendent prop</source>
        <target state=""new"">LocalizableB descendent prop</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|SearchTerms"">
        <source>My search terms</source>
        <target state=""new"">My search terms</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|TranslatableElement|LocalizablePropC"">
        <source>LocalizableC attribute</source>
        <target state=""new"">LocalizableC attribute</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|TypeDescriptorText|Value"">
        <source>Custom symbols</source>
        <target state=""new"">Custom symbols</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            Dictionary<string, string> translations = new()
            {
                ["Rule|MyRule|DisplayName"] = "AAA",
                ["Rule|MyRule|Description"] = "BBB",
                ["Category|MyCategory|DisplayName"] = "CCC",
                ["EnumProperty|MyEnumProperty|DisplayName"] = "DDD",
                ["EnumProperty|MyEnumProperty|Description"] = "EEE",
                ["EnumValue|MyEnumProperty.First|DisplayName"] = "FFF",
                ["EnumValue|MyEnumProperty.Second|DisplayName"] = "GGG",
                ["EnumValue|MyEnumProperty.Third|DisplayName"] = "HHH",
                ["BoolProperty|MyBoolProperty|Description"] = "III",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms"] = "JJJ",
                ["StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"] = "NNN",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"] = "LLL",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"] = "MMM",
            };

            string expectedTranslation =
@"<Rule Name=""MyRule"" DisplayName=""AAA"" PageTemplate=""generic"" Description=""BBB"" xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" Description=""My category description"">
      <Category.DisplayName>CCC</Category.DisplayName>
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <StringProperty Name=""MyStringProperty"" DisplayName=""MyStringProperty display name"">
    <StringProperty.Description>MyStringProperty description</StringProperty.Description>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType=""MultiStringSelector"">
        <ValueEditor.Metadata>
          <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"" />
          <NameValuePair Name=""SearchTerms"">
            <NameValuePair.Value>My search terms</NameValuePair.Value>
          </NameValuePair>
          <TestElement Name=""TranslatableElement"" LocalizablePropC=""LocalizableC attribute"" xliff:LocalizedProperties=""LocalizablePropA;LocalizablePropB;LocalizablePropC"">
            <TestElement.LocalizablePropA>LocalizableA descendent prop</TestElement.LocalizablePropA>
            <TestElement.LocalizablePropB>LocalizableB descendent prop</TestElement.LocalizablePropB>
          </TestElement>
          <NameValuePair Name=""ShouldDisplayEvaluatedPreview"" Value=""True"" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>
</Rule>";

            RunXamlTranslationTest(source, translations, expectedTranslation, expectedXlf);
        }
        
        private static void RunXamlTranslationTest(string source, Dictionary<string, string> translations, string expectedTranslation, string expectedXlf)
        {
          XamlRuleDocument document = new();
          StringWriter writer = new();
          document.Load(new StringReader(source));

          XlfDocument xliffDocument = new();
          xliffDocument.LoadNew("fr");
          xliffDocument.Update(document, "test.xaml");

          document.Translate(translations);
          document.Save(writer);

          AssertEx.EqualIgnoringLineEndings(expectedTranslation, writer.ToString());

          StringWriter xliffWriter = new();
          xliffDocument.Save(xliffWriter);

          AssertEx.EqualIgnoringLineEndings(expectedXlf, xliffWriter.ToString());
        }
    }
}
