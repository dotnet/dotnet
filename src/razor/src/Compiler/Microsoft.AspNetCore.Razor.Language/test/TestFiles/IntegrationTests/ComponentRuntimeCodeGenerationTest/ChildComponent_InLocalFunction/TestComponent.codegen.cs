﻿// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line default
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Components;
#nullable restore
#line (1,2)-(1,51) "x:\dir\subdir\Test\TestComponent.cshtml"
using Microsoft.AspNetCore.Components.RenderTree;

#nullable disable
    #line default
    #line hidden
    #nullable restore
    public partial class TestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    #nullable disable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line (2,3)-(5,1) "x:\dir\subdir\Test\TestComponent.cshtml"

    void RenderChildComponent()
    {

#line default
#line hidden
#nullable disable

            __builder.OpenComponent<global::Test.MyComponent>(0);
            __builder.CloseComponent();
#nullable restore
#line (6,1)-(7,1) "x:\dir\subdir\Test\TestComponent.cshtml"
    }

#line default
#line hidden
#nullable disable

#nullable restore
#line (9,3)-(9,28) "x:\dir\subdir\Test\TestComponent.cshtml"
 RenderChildComponent(); 

#line default
#line hidden
#nullable disable

        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
