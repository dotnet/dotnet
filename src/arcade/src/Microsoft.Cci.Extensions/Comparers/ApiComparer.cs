// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Comparers
{
    public sealed class ApiComparer<T> : IComparer<T>
    {
        private readonly Func<T, ApiKind> _kindProvider;
        private readonly Func<T, string> _nameProvider;

        public ApiComparer(Func<T, ApiKind> kindProvider, Func<T, string> nameProvider)
        {
            _kindProvider = kindProvider;
            _nameProvider = nameProvider;
        }

        public int Compare(T x, T y)
        {
            var kindX = _kindProvider(x);
            var kindY = _kindProvider(y);
            var kindComparison = CompareKind(kindX, kindY);
            if (kindComparison != 0)
                return kindComparison;

            var nameX = _nameProvider(x);
            var nameY = _nameProvider(y);
            if (kindX == ApiKind.Namespace && kindY == ApiKind.Namespace)
                return CompareQualifiedNamespaceNames(nameX, nameY);

            return CompareNames(nameX, nameY);
        }

        private static int CompareKind(ApiKind x, ApiKind y)
        {
            var xKindOrder = GetKindOrder(x);
            var yKindOrder = GetKindOrder(y);
            return xKindOrder.CompareTo(yKindOrder);
        }

        private static int CompareNames(string x, string y)
        {
            var xNonGenericName = GetNonGenericName(x);
            var yNonGenericName = GetNonGenericName(y);

            var nameComparison = StringComparer.OrdinalIgnoreCase.Compare(xNonGenericName, yNonGenericName);
            if (nameComparison != 0)
                return nameComparison;

            return x.Length.CompareTo(y.Length);
        }

        private static int CompareQualifiedNamespaceNames(string nameX, string nameY)
        {
            string beforeFirstDotX;
            string afterFirstDotX;
            SplitAtFirstDot(nameX, out beforeFirstDotX, out afterFirstDotX);

            string beforeFirstDotY;
            string afterFirstDotY;
            SplitAtFirstDot(nameY, out beforeFirstDotY, out afterFirstDotY);

            var firstComparison = CompareNamespaceNames(beforeFirstDotX, beforeFirstDotY);
            if (firstComparison != 0)
                return firstComparison;

            return StringComparer.OrdinalIgnoreCase.Compare(nameX, nameY);
        }

        private static int CompareNamespaceNames(string nameX, string nameY)
        {
            var orderX = GetNamspaceOrder(nameX);
            var orderY = GetNamspaceOrder(nameY);

            var comparison = orderX.CompareTo(orderY);
            if (comparison != 0)
                return comparison;

            return StringComparer.OrdinalIgnoreCase.Compare(nameX, nameY);
        }

        private static int GetKindOrder(ApiKind kind)
        {
            switch (kind)
            {
                // Namespace -- no order
                case ApiKind.Namespace:
                    return 0;

                // Types -- no order between types
                case ApiKind.Interface:
                case ApiKind.Delegate:
                case ApiKind.Enum:
                case ApiKind.Struct:
                case ApiKind.Class:
                    return 1;

                // Members
                case ApiKind.EnumField:
                case ApiKind.Field:
                    return 2;
                case ApiKind.Constructor:
                    return 3;
                case ApiKind.Property:
                    return 4;
                case ApiKind.Method:
                case ApiKind.PropertyAccessor:
                case ApiKind.EventAccessor:
                case ApiKind.DelegateMember:
                    return 5;
                case ApiKind.Event:
                    return 6;
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        private static int GetNamspaceOrder(string name)
        {
            switch (name)
            {
                case "System":
                    return 0;
                case "Microsoft":
                    return 1;
                case "Windows":
                    return 2;
                default:
                    return 3;
            }
        }

        private static string GetNonGenericName(string name)
        {
            var i = name.IndexOf("<", StringComparison.OrdinalIgnoreCase);
            return i > -1 ? name.Substring(0, i) : name;
        }

        private static void SplitAtFirstDot(string qualifiedName, out string beforeFirstDot, out string afterFirstDot)
        {
            var firstDot = qualifiedName.IndexOf('.');
            if (firstDot < 0)
            {
                beforeFirstDot = qualifiedName;
                afterFirstDot = string.Empty;
            }
            else
            {
                beforeFirstDot = qualifiedName.Substring(0, firstDot);
                afterFirstDot = qualifiedName.Substring(firstDot + 1);
            }
        }
    }
}
