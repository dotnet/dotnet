// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.IO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

// Used to abstract away platform-specific file/directory path information.
//
// The System.IO.Path methods don't processes Windows paths in a Windows way
// on *nix (rightly so), so we need to use platform-specific paths.
//
// Target paths are always Windows style.
internal static class TestProjectData
{
    static TestProjectData()
    {
        var baseDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "c:\\users\\example\\src" : "/home/example";
        var someProjectPath = Path.Combine(baseDirectory, "SomeProject");
        var someProjectObjPath = Path.Combine(someProjectPath, "obj");

        SomeProject = new HostProject(Path.Combine(someProjectPath, "SomeProject.csproj"), someProjectObjPath, RazorConfiguration.Default, "SomeProject");
        SomeProjectFile1 = new HostDocument(Path.Combine(someProjectPath, "File1.cshtml"), "File1.cshtml", FileKinds.Legacy);
        SomeProjectFile2 = new HostDocument(Path.Combine(someProjectPath, "File2.cshtml"), "File2.cshtml", FileKinds.Legacy);
        SomeProjectImportFile = new HostDocument(Path.Combine(someProjectPath, "_Imports.cshtml"), "_Imports.cshtml", FileKinds.Legacy);
        SomeProjectNestedFile3 = new HostDocument(Path.Combine(someProjectPath, "Nested", "File3.cshtml"), "Nested\\File3.cshtml", FileKinds.Legacy);
        SomeProjectNestedFile4 = new HostDocument(Path.Combine(someProjectPath, "Nested", "File4.cshtml"), "Nested\\File4.cshtml", FileKinds.Legacy);
        SomeProjectNestedImportFile = new HostDocument(Path.Combine(someProjectPath, "Nested", "_Imports.cshtml"), "Nested\\_Imports.cshtml", FileKinds.Legacy);
        SomeProjectComponentFile1 = new HostDocument(Path.Combine(someProjectPath, "File1.razor"), "File1.razor", FileKinds.Component);
        SomeProjectComponentFile2 = new HostDocument(Path.Combine(someProjectPath, "File2.razor"), "File2.razor", FileKinds.Component);
        SomeProjectComponentImportFile1 = new HostDocument(Path.Combine(someProjectPath, "_Imports.razor"), "_Imports.razor", FileKinds.Component);
        SomeProjectNestedComponentFile3 = new HostDocument(Path.Combine(someProjectPath, "Nested", "File3.razor"), "Nested\\File3.razor", FileKinds.Component);
        SomeProjectNestedComponentFile4 = new HostDocument(Path.Combine(someProjectPath, "Nested", "File4.razor"), "Nested\\File4.razor", FileKinds.Component);
        SomeProjectCshtmlComponentFile5 = new HostDocument(Path.Combine(someProjectPath, "File5.cshtml"), "File5.cshtml", FileKinds.Component);

        var anotherProjectPath = Path.Combine(baseDirectory, "AnotherProject");
        var anotherProjectObjPath = Path.Combine(anotherProjectPath, "obj");

        AnotherProject = new HostProject(Path.Combine(anotherProjectPath, "AnotherProject.csproj"), anotherProjectObjPath, RazorConfiguration.Default, "AnotherProject");
        AnotherProjectFile1 = new HostDocument(Path.Combine(anotherProjectPath, "File1.cshtml"), "File1.cshtml", FileKinds.Legacy);
        AnotherProjectFile2 = new HostDocument(Path.Combine(anotherProjectPath, "File2.cshtml"), "File2.cshtml", FileKinds.Legacy);
        AnotherProjectImportFile = new HostDocument(Path.Combine(anotherProjectPath, "_Imports.cshtml"), "_Imports.cshtml", FileKinds.Legacy);
        AnotherProjectNestedFile3 = new HostDocument(Path.Combine(anotherProjectPath, "Nested", "File3.cshtml"), "Nested\\File1.cshtml", FileKinds.Legacy);
        AnotherProjectNestedFile4 = new HostDocument(Path.Combine(anotherProjectPath, "Nested", "File4.cshtml"), "Nested\\File2.cshtml", FileKinds.Legacy);
        AnotherProjectNestedImportFile = new HostDocument(Path.Combine(anotherProjectPath, "Nested", "_Imports.cshtml"), "Nested\\_Imports.cshtml", FileKinds.Legacy);
        AnotherProjectComponentFile1 = new HostDocument(Path.Combine(anotherProjectPath, "File1.razor"), "File1.razor", FileKinds.Component);
        AnotherProjectComponentFile2 = new HostDocument(Path.Combine(anotherProjectPath, "File2.razor"), "File2.razor", FileKinds.Component);
        AnotherProjectNestedComponentFile3 = new HostDocument(Path.Combine(anotherProjectPath, "Nested", "File3.razor"), "Nested\\File1.razor", FileKinds.Component);
        AnotherProjectNestedComponentFile4 = new HostDocument(Path.Combine(anotherProjectPath, "Nested", "File4.razor"), "Nested\\File2.razor", FileKinds.Component);
    }

    public static readonly HostProject SomeProject;
    public static readonly HostDocument SomeProjectFile1;
    public static readonly HostDocument SomeProjectFile2;
    public static readonly HostDocument SomeProjectImportFile;
    public static readonly HostDocument SomeProjectNestedFile3;
    public static readonly HostDocument SomeProjectNestedFile4;
    public static readonly HostDocument SomeProjectNestedImportFile;
    public static readonly HostDocument SomeProjectComponentFile1;
    public static readonly HostDocument SomeProjectComponentFile2;
    public static readonly HostDocument SomeProjectComponentImportFile1;
    public static readonly HostDocument SomeProjectNestedComponentFile3;
    public static readonly HostDocument SomeProjectNestedComponentFile4;
    public static readonly HostDocument SomeProjectCshtmlComponentFile5;

    public static readonly HostProject AnotherProject;
    public static readonly HostDocument AnotherProjectFile1;
    public static readonly HostDocument AnotherProjectFile2;
    public static readonly HostDocument AnotherProjectImportFile;
    public static readonly HostDocument AnotherProjectNestedFile3;
    public static readonly HostDocument AnotherProjectNestedFile4;
    public static readonly HostDocument AnotherProjectNestedImportFile;
    public static readonly HostDocument AnotherProjectComponentFile1;
    public static readonly HostDocument AnotherProjectComponentFile2;
    public static readonly HostDocument AnotherProjectNestedComponentFile3;
    public static readonly HostDocument AnotherProjectNestedComponentFile4;
}
