// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Windows.Markup;
using System.Xaml;

namespace MS.Internal.Xaml.Context
{
    internal static class ContextServices
    {
        // Used to implement IProvideValueTarget Service provider.
        public static object GetTargetProperty(ObjectWriterContext xamlContext)
        {
            // If the XamlMember implements IProvideValueTarget, ask it for the TargetProperty first
            Debug.Assert(xamlContext.ParentProperty is not null);

            if (xamlContext.ParentProperty is IProvideValueTarget ipvt)
            {
                return ipvt.TargetProperty;
            }

            XamlMember parentProperty = xamlContext.ParentProperty;
            //
            // We should never have a null ParentProperty here but
            // protect against null refs since we are going to dereference it
            if (parentProperty is null)
            {
                return null;
            }

            if (parentProperty.IsAttachable)
            {
                //
                // IPVT returns the static Set method for attached properties in 3.0
                return parentProperty.Setter;
            }
            else
            {
                //
                // This branch cover regular property (will return non null)
                // and items in a collection/diction (will return null - IPVT returns null in 3.0 for collections/dictionaries).
                return parentProperty.UnderlyingMember;
            }
        }
    }
}
