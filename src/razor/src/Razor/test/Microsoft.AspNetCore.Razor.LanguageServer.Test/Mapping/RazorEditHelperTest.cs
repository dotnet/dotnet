// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Mapping;

public class RazorEditHelperTest : LanguageServerTestBase
{
    private readonly IDocumentMappingService _documentMappingService;

    public RazorEditHelperTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _documentMappingService = new LspDocumentMappingService(
            FilePathService,
            new TestDocumentContextFactory(),
            LoggerFactory);
    }

    [Fact]
    public Task SimpleEdits_Apply()
        => TestAsync(
            csharpSource: """
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task MappedAndUnmappedEdits_Apply()
        => TestAsync(
            csharpSource: """
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}

                    void UnmappedMethod()
                    {
                    }
                }
                """,
            razorSource: """
                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }

                    void Method()
                    {
                    }
                }
                """,
            expectedRazorSource: """
                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task NewUsing_TopOfFile()
        => TestAsync(
            csharpSource: """
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @using System
                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task RemovedUsing_IsRemoved()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}

                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                {|mapUsing:@using System|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                class MyComponent : ComponentBase
                {
                    public int Counter { get; set; }
                }
                """,
            expectedRazorSource: """

                @code {
                    public int Counter { get; set; } 
                }
                """);

    [Fact]
    public Task NewUsing_AfterExisting()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}

                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                {|mapUsing:@using System|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @using System
                @using System.Collections.Generic

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task NewUsing_AfterPage()
        => TestAsync(
            csharpSource: """
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task NewUsing_AfterPage2()
        => TestAsync(
            csharpSource: """
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"

                <h3>Counter</h3>
                <p>Current count: @Counter</p>

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic

                <h3>Counter</h3>
                <p>Current count: @Counter</p>

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task RenamedUsing_Applies()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using Renamed;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using Renamed

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task AddUsing_AfterSystem()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using OtherNamespace;
                using System;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using OtherNamespace

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task AddUsing_OrdersSystemCorrectly()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using MyNamespace;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using MyNamespace|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using OtherNamespace;
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic
                @using OtherNamespace

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task UsingIndentation_DoesNotApply()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                    using System;
                    using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task UsingAliasRemoved_HandledCorrectly()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                {|mapUsing3:using Goo = Bar;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}
                {|mapUsing3:@using Goo = Bar|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task UsingAliasAdded_HandledCorrectly()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;
                using Goo = Bar;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic
                @using Goo = Bar

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task UsingStaticRemoved_HandledCorrectly()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using System.Collections.Generic;|}
                {|mapUsing3:using static Test.Bar;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}
                {|mapUsing3:@using static Test.Bar|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task UsingStaticAdded_HandledCorrectly()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using static System.Collections.Generic;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using System.Collections.Generic|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using System;
                using System.Collections.Generic;
                using static Test.Bar;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic
                @using static Test.Bar

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task AddUsingMultipleUsingGroups_AppliesCurrently()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using MyNamespace;|}
                {|mapUsing3:using MyNamespace2;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}
                {|mapUsing2:@using MyNamespace|}

                <p></p>

                {|mapUsing3:@using MyNamespace2|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                using OtherNamespace;
                using System;
                using System.Collections.Generic;
                using MyNamespace;
                using MyNamespace2;

                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"
                @using System
                @using System.Collections.Generic
                @using MyNamespace
                @using OtherNamespace

                <p></p>

                @using MyNamespace2

                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    [Fact]
    public Task RemovingMultipleUsingGroups_AppliesCurrently()
        => TestAsync(
            csharpSource: """
                {|mapUsing:using System;|}
                {|mapUsing2:using MyNamespace;|}
                {|mapUsing3:using MyNamespace2;|}
                class MyComponent : ComponentBase
                {
                    {|map1:public int Counter { get; set; }|}
                }
                """,
            razorSource: """
                @page "/counter"
                {|mapUsing:@using System|}

                <p></p>

                {|mapUsing2:@using MyNamespace|}

                <p></p>

                {|mapUsing3:@using MyNamespace2|}

                @code {
                    {|map1:public int Counter { get; set; }|} 
                }
                """,
            newCSharpSource: """
                class MyComponent : ComponentBase
                {
                    public int NewCounter { get; set; }
                }
                """,
            expectedRazorSource: """
                @page "/counter"

                <p></p>


                <p></p>


                @code {
                    public int NewCounter { get; set; } 
                }
                """);

    private async Task TestAsync(
        TestCode csharpSource,
        TestCode razorSource,
        string newCSharpSource,
        string expectedRazorSource)
    {
        var csharpSpans = csharpSource.NamedSpans;
        var razorSpans = razorSource.NamedSpans;

        AssertEx.SetEqual(csharpSpans.Keys.OrderAsArray(), razorSpans.Keys.OrderAsArray());

        var csharpPath = @"C:\path\to\document.razor.g.cs";
        var razorPath = @"C:\path\to\document.razor";
        var csharpSourceText = SourceText.From(csharpSource.Text);
        var razorSourceText = SourceText.From(razorSource.Text);

        var sourceMappings = new List<SourceMapping>();
        foreach (var key in csharpSpans.Keys)
        {
            var csharpSpan = csharpSpans[key].Single();
            var razorSpan = razorSpans[key].Single();

            var csharpLinePosition = csharpSourceText.GetLinePositionSpan(csharpSpan);
            var razorLinePosition = razorSourceText.GetLinePositionSpan(razorSpan);

            var csharpSourceSpan = new SourceSpan(
                csharpPath,
                csharpSpan.Start,
                csharpLinePosition.Start.Line,
                csharpLinePosition.Start.Character,
                csharpSpan.Length);

            var razorSourceSpan = new SourceSpan(
                razorPath,
                razorSpan.Start,
                razorLinePosition.Start.Line,
                razorLinePosition.Start.Character,
                razorSpan.Length);

            sourceMappings.Add(new SourceMapping(razorSourceSpan, csharpSourceSpan));
        }

        var newCSharpSourceText = SourceText.From(newCSharpSource);
        var changes = GetChanges(csharpSource.Text, newCSharpSource);

        var codeDocument = CreateCodeDocument(razorSource.Text, tagHelpers: [], filePath: razorPath);
        var csharpDocument = TestRazorCSharpDocument.Create(
            codeDocument,
            csharpSource.Text,
            sourceMappings.OrderByAsArray(s => s.GeneratedSpan.AbsoluteIndex));

        codeDocument.SetCSharpDocument(csharpDocument);
        var snapshot = TestDocumentSnapshot.Create(razorPath, codeDocument);

        var mappedChanges = await RazorEditHelper.MapCSharpEditsAsync(
            changes.SelectAsArray(c => c.ToRazorTextChange()),
            snapshot,
            _documentMappingService,
            FailingTelemetryReporter.Instance,
            CancellationToken.None);

        Assert.NotEmpty(mappedChanges);

        var responseTextChanges = mappedChanges.Select(e => e.ToTextChange()).ToArray();
        var newRazorSourceText = razorSourceText.WithChanges(responseTextChanges);
        AssertEx.EqualOrDiff(expectedRazorSource, newRazorSourceText.ToString());
    }

    private ImmutableArray<TextChange> GetChanges(string csharpSource, string newCSharpSource)
    {
        var tree = CSharpSyntaxTree.ParseText(csharpSource);
        var newTree = CSharpSyntaxTree.ParseText(newCSharpSource);

        return newTree.GetChanges(tree).ToImmutableArray();
    }

    private class FailingTelemetryReporter : ITelemetryReporter
    {
        public static readonly FailingTelemetryReporter Instance = new FailingTelemetryReporter();

        public TelemetryScope BeginBlock(string name, Severity severity, TimeSpan minTimeToReport)
            => TelemetryScope.Null;

        public TelemetryScope BeginBlock(string name, Severity severity, TimeSpan minTimeToReport, Property property)
            => TelemetryScope.Null;

        public TelemetryScope BeginBlock(string name, Severity severity, TimeSpan minTimeToReport, Property property1, Property property2)
            => TelemetryScope.Null;

        public TelemetryScope BeginBlock(string name, Severity severity, TimeSpan minTimeToReport, Property property1, Property property2, Property property3)
            => TelemetryScope.Null;

        public TelemetryScope BeginBlock(string name, Severity severity, TimeSpan minTimeToReport, params ReadOnlySpan<Property> properties)
            => TelemetryScope.Null;

        public void ReportEvent(string name, Severity severity)
        {
        }

        public void ReportEvent(string name, Severity severity, Property property)
        {
        }

        public void ReportEvent(string name, Severity severity, Property property1, Property property2)
        {
        }

        public void ReportEvent(string name, Severity severity, Property property1, Property property2, Property property3)
        {
        }

        public void ReportEvent(string name, Severity severity, params ReadOnlySpan<Property> properties)
        {
        }

        public void ReportFault(Exception _, string? message, params object?[] @params)
        {
            Assert.Fail($"Did not expect to report a fault. :: {message} :: {string.Join(",", @params ?? [])}");
        }

        public TelemetryScope TrackLspRequest(string lspMethodName, string lspServerName, TimeSpan minTimeToReport, Guid correlationId)
            => TelemetryScope.Null;

        public void ReportRequestTiming(string name, string? language, TimeSpan queuedDuration, TimeSpan requestDuration, TelemetryResult result)
        {
        }
    }
}
