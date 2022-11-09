// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.Contracts;

namespace Microsoft.Cci.Differs
{
    public class Difference
    {
        public Difference(object id, string message)
        {
            Contract.Requires(id != null);
            Contract.Requires(message != null);

            Id = GetIdFromObject(id);
            Message = message;
        }

        public string Id { get; private set; }

        public string Message { get; private set; }

        private static string GetIdFromObject(object obj)
        {
            Contract.Requires(obj != null);

            string id = obj as string;
            if (id != null)
                return id;

            Enum en = obj as Enum;
            if (en != null)
                return en.ToString();

            return obj.GetType().Name;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Id, Message);
        }
    }

    public class IncompatibleDifference : Difference
    {
        public IncompatibleDifference(object id, string message)
            : base(id, message)
        {
        }
    }

    public class TypeMismatchInCompatibleDifference : IncompatibleDifference
    {
        public TypeMismatchInCompatibleDifference(object id, string message, ITypeReference type1, ITypeReference type2)
            : base(id, message)
        {
            this.Type1 = type1;
            this.Type2 = type2;
        }

        public ITypeReference Type1 { get; private set; }

        public ITypeReference Type2 { get; private set; }
    }

    public class DifferenceWithContext<T> : Difference
    {
        public DifferenceWithContext(object id, string message, T context)
            : base(id, message)
        {
            Context = context;
        }

        public T Context { get; private set; }
    }
}
