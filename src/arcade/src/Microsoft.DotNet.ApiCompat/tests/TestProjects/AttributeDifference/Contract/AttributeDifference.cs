// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace AttributeDifference
{
    [DisplayName("Attribute difference class1")]
    public class AttributeDifferenceClass1
    {
        public string MethodWithAttribute(string myParameter, [DefaultValue("myObject")] object myObject) => throw null;
        public T GenericMethodWithAttribute<T>() => throw null;
        public void MethodWithAttribute() { }
        public string PropertyWithAttribute { get; set; }
        public event System.EventHandler EventWithAttribute { add { } remove { } }
    }
    public class AttributeDifferenceGenericCLass<TOne, [DefaultValue("TTwo")] TTwo>
    {
    }
}
