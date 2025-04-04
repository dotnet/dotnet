﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
//
//  Contents:  Implements a converter to an instance descriptor for 
//             DynamicResourceExtension
//
//

using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace System.Windows
{
    /// <summary>
    /// Type converter to inform the serialization system how to construct a DynamicResourceExtension from
    /// an instance. It reports that ResourceKey should be used as the first parameter to the constructor.
    /// </summary>
    public class DynamicResourceExtensionConverter: TypeConverter
    {
        /// <summary>
        /// True if converting to an instance descriptor
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts to an instance descriptor
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                ArgumentNullException.ThrowIfNull(value);

                DynamicResourceExtension dynamicResource = value as DynamicResourceExtension;

                if (dynamicResource == null)
                    throw new ArgumentException(SR.Format(SR.MustBeOfType, "value", "DynamicResourceExtension"), nameof(value)); 

                return new InstanceDescriptor(typeof(DynamicResourceExtension).GetConstructor(new Type[] { typeof(object) }), 
                    new object[] { dynamicResource.ResourceKey } );
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
