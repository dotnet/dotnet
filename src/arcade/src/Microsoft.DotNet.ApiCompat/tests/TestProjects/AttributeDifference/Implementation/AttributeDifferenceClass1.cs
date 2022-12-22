// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;

namespace AttributeDifference
{
    [Designer("Foo")]
    [DisplayName("Attribute difference class1")]
    [Foo]
    public class AttributeDifferenceClass1
    {
        public string MethodWithAttribute([Foo] string myParameter, [DefaultValue("myObject")] object myObject) => myParameter;
        public T GenericMethodWithAttribute<[DefaultValue("T")] T>() => default(T);
        [Foo]
        public void MethodWithAttribute() { }
        [Foo]
        public string PropertyWithAttribute { get; set; }
        [Foo]
        public event System.EventHandler EventWithAttribute { add { } remove { } }
    }

    public class AttributeDifferenceGenericCLass<[DefaultValue("TOne")] TOne, [DefaultValue("TTwo")] TTwo>
    {
    }

    internal class FooAttribute : Attribute { }
}
