// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//

using MS.Internal;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Converters;

namespace System.Windows.Converters
{
    /// <summary>
    /// RectValueSerializer - ValueSerializer class for converting instances of strings to and from Rect instances
    /// This is used by the MarkupWriter class.
    /// </summary>
    public class RectValueSerializer : ValueSerializer 
    {
        /// <summary>
        /// Returns true.
        /// </summary>
        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the given value can be converted into a string
        /// </summary>
        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
            // Validate the input type
            if (!(value is Rect))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts a string into a Rect.
        /// </summary>
        public override object ConvertFromString(string value, IValueSerializerContext context)
        {
            if (value != null)
            {
                return Rect.Parse(value );
            }
            else
            {
                return base.ConvertFromString( value, context );
            }
        }

        /// <summary>
        /// Converts the value into a string.
        /// </summary>
        public override string ConvertToString(object value, IValueSerializerContext context)
        {
            if (value is Rect instance)
            {

                return instance.ConvertToString(null, System.Windows.Markup.TypeConverterHelper.InvariantEnglishUS);
            }

            return base.ConvertToString(value, context);
        }
    }
}
