// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Extensions
{
    public static class ApiKindExtensions
    {
        public static bool IsInfrastructure(this ApiKind kind)
        {
            switch (kind)
            {
                case ApiKind.EnumField:
                case ApiKind.DelegateMember:
                case ApiKind.PropertyAccessor:
                case ApiKind.EventAccessor:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNamespace(this ApiKind kind)
        {
            return kind == ApiKind.Namespace;
        }

        public static bool IsType(this ApiKind kind)
        {
            switch (kind)
            {
                case ApiKind.Interface:
                case ApiKind.Delegate:
                case ApiKind.Enum:
                case ApiKind.Struct:
                case ApiKind.Class:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsMember(this ApiKind kind)
        {
            switch (kind)
            {
                case ApiKind.EnumField:
                case ApiKind.DelegateMember:
                case ApiKind.Field:
                case ApiKind.Property:
                case ApiKind.Event:
                case ApiKind.Constructor:
                case ApiKind.PropertyAccessor:
                case ApiKind.EventAccessor:
                case ApiKind.Method:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAccessor(this ApiKind kind)
        {
            switch (kind)
            {
                case ApiKind.PropertyAccessor:
                case ApiKind.EventAccessor:
                    return true;
                default:
                    return false;
            }
        }
    }
}
