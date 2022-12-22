// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Writers;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.DotNet.ApiCompat
{
    // This filter is activated when ApiCompat is used to detect changes that validate MDIL servicing rules.
    //
    internal class MdilPublicOnlyCciFilter : PublicOnlyCciFilter
    {
        public MdilPublicOnlyCciFilter(bool excludeAttributes = true)
            : base(excludeAttributes)
        {
        }

        // How MDIL affects type visibility:
        //
        // - A type marked [TreatAsPublicSurface] is treated as public.
        //
        // - If any member of the type is marked [TreatedAsPublicSurface], the type is treated as public by ApiCompat (but its
        //   non-[TreatAs] public members are not.)
        //
        //   Note that there may be corner cases where this causes ApiCompat to enforce rules that aren't really necessary - e.g.
        //   a non-public non-versionable struct that has one [TreatAs] static method will be forbidden from changing its
        //   layout even though this isn't strictly required by Triton. This is a consequence of the fact that ApiCompat
        //   is not designed to support non-public types exposing public members.
        // 
        //   
        public override bool Include(ITypeDefinition type)
        {
            if (type == null || Dummy.Type == type)
                return false;
            //if (type.IsTreatedAsVisibleOutsideAssembly())
            //    return true;
            //if (type.Members.Any(m => m.IsTreatedAsVisibleOutsideAssembly()))
            //    return true;
            return base.Include(type);
        }

        // How MDIL affects member visibility:
        //
        // - A member marked [TreatAsPublicSurface] is treated as public regardless of its own visibility or that of its containing type.
        //
        public override bool Include(ITypeDefinitionMember member)
        {
            if (member == null)
                return false;

            //if (member.IsTreatedAsVisibleOutsideAssembly())
            //    return true;

            return base.Include(member);
        }
    }
}
