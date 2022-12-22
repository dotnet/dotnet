// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class CodeActionFormattingTest : FormattingTestBase
{
    public CodeActionFormattingTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task AddDebuggerDisplay()
    {
        await RunCodeActionFormattingTestAsync(
input: @"
@functions {
    class Goo$$
    {
        
    }
}
",
codeActionEdits: new[]
{
Edit(7, 6, 7, 6, "System.Diagnostics;\r\nusing "),
Edit(67, 0, 67, 8, ""),
Edit(69, 34, 70, 7, "\r\n\r\n        [DebuggerDisplay($\"{{{nameof(GetDebuggerDisplay)}(),nq}}\")]"),
Edit(71, 0, 71, 4, "        "),
Edit(72, 5, 72, 5, "\r\n            private string GetDebuggerDisplay()\r\n            {"),
Edit(73, 0, 73, 0, "                return ToString();\r\n            }\r\n"),
Edit(73, 8, 74, 4, "")
},
expected: @"@using System.Diagnostics;

@functions {
    [DebuggerDisplay($""{{{nameof(GetDebuggerDisplay)}(),nq}}"")]  
    class Goo
    {
        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
");
    }

    [Fact]
    public async Task GenerateConstructor()
    {
        await RunCodeActionFormattingTestAsync(
input: @"
@functions {
    class Goo$$
    {
        
    }
}
",
codeActionEdits: new[]
{
Edit(67, 0, 67, 8, ""),
Edit(69, 33, 69, 33, "\r\n\r\n        class Goo"),
Edit(70, 0, 70, 12, "        {"),
Edit(71, 0, 71, 9, "            public"),
Edit(71, 13, 71, 13, "()"),
Edit(72, 0, 72, 4, "            "),
Edit(73, 0, 73, 4, "            }"),
Edit(74, 0, 74, 4, "        "),
},
expected: @"
@functions {
    class Goo
    {
        public Goo()
        {
        }    
    }
}
");
    }

    [Fact]
    public async Task OverrideCompletion()
    {
        await RunCodeActionFormattingTestAsync(
input: @"
@functions {
    override $$
}
",
codeActionEdits: new[]
{
Edit(65, 0, 72, 0, "        {\r\n        }\r\n#pragma warning restore 1998\r\n#nullable restore\r\n#line 2 \"e:/Scratch/BlazorApp13/BlazorApp13/Client/Pages/Test.razor\"\r\n\r\n        protected override void OnAfterRender(bool firstRender)\r\n        {\r\n            base.OnAfterRender(firstRender);/*$0*/\r\n        }\r\n"),
},
expected: @"
@functions {
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);/*$0*/
    }
}
");
    }
}
