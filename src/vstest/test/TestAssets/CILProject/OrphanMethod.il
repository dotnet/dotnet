.assembly OrphanMethod {}
.assembly extern mscorlib {}
.assembly extern System.Core {}

.method static public int32 Add(int32 x, int32 y) cil managed 
{
    .maxstack 2

    ldarg.0
    ldarg.1
    add
    ret
}

.method static public int32 'An awesome method to add 𝓧➕𝓨'(int32 '𝓧', int32 '𝓨') cil managed 
{
    .maxstack 2

    ldarg.0
    ldarg.1
    add
    ret
}

.class public CleanNamespaceName.SecondLevel.'𝐌𝐲 𝗮𝘄𝗲𝘀𝗼𝗺𝗲 𝘤𝘭𝘢𝘴𝘴 𝘢𝘯 𝒊𝒏𝒂𝒄𝒄𝒆𝒔𝒔𝒊𝒃𝒍𝒆 𝙣𝙖𝙢𝙚 😭' 
{
    .method static public int32 '𝑓'(int32 '𝓧', int32 '𝓨') cil managed 
    {
        .maxstack 2

        ldarg.0
        ldarg.1
        add
        ret
    }
    
    .method static public int32 Sum(int32 '𝓧', int32 '𝓨') cil managed 
    {
        .maxstack 2

        ldarg.0
        ldarg.1
        add
        ret
    }
    
    .method static public int32 'Method with . in it'(int32 '𝓧', int32 '𝓨') cil managed 
    {
        .maxstack 2

        ldarg.0
        ldarg.1
        add
        ret
    }
}

.class public CleanNamespaceName.SecondLevel.'Deeply wrong .namespace name'.'My generic class'<'Generic Type'> 
{
    .field private !0 'Generic Item'

    .method public instance void .ctor() 
    {
        .maxstack 1
        ldarg.0
        call instance void System.Object::.ctor()

        ret
    }

    .method static public int32 'Method with . in it'<'Generic Type'>(!'Generic Type' '𝓧', int32 '𝓨') cil managed 
    {
        .maxstack 2

        ldarg.0
        ldarg.1
        add
        ret
    }
}

.class public CleanNamespaceName.SecondLevel.'Deeply wrong .namespace name'.'NamespaceA.NamespaceB.ClassName`1+InnerClass`2'
{
    .method public instance int32 'MethodName`1(System.Int32,System.Int32,System.Int32)'()
    {
        .maxstack 1

        ldc.i4.s 0
        ret
    }
    
    .method public instance int32 'MethodName`2(System.Int32,System.Int32,System.Int32)'(int32 '1st parameter', int32 '2nd parameter')
    {
        .maxstack 1

        ldc.i4.s 0
        ret
    }

    .method public instance int32 'MethodName`3'(int32 '1st parameter', int32 '2nd parameter')
    {
        .maxstack 1

        ldc.i4.s 1
        ret
    }

    .class nested public 'Inner Class'
    {
        .method public instance int32 'MethodName`3'(int32 '1st parameter', int32 '2nd parameter')
        {
            .maxstack 1

            ldc.i4.s 1
            ret
        }
    }
}

.class public 'My generic class'<'Generic Type'> 
{
    .field private !0 'Generic Item'

    .method public instance void .ctor() 
    {
        .maxstack 1
        ldarg.0
        call instance void System.Object::.ctor()

        ret
    }
}

.class public 'NamespaceA.NamespaceB.ClassName`1+InnerClass`2'
{
    .method public instance int32 'MethodName`2(System.Int32,System.Int32,System.Int32)'()
    {
        .maxstack 1

        ldc.i4.s 0
        ret
    }

    .method public instance int32 'MethodName`3'(int32 '1st parameter', int32 '2nd parameter')
    {
        .maxstack 1

        ldc.i4.s 1
        ret
    }

    .method public hidebysig instance void Overload0() cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(int32 i) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(int32 i, class 'NamespaceA.NamespaceB.ClassName`1+InnerClass`2' c) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(int32* p) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(object d) cil managed
    {
        .param [1]
        .custom instance void [System.Core]System.Runtime.CompilerServices.DynamicAttribute::.ctor() = ( 01 00 00 00 ) 
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(!!U u) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>() cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U,T>() cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(!!U[] u) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(!!U[][] u) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(!!U[0...,0...] u) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(!!U[0...,0...,0...] u) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(class [mscorlib]System.Collections.Generic.List`1<int32> l) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(class [mscorlib]System.Collections.Generic.List`1<!!U> l) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U,V>(class [mscorlib]System.Tuple`2<!!U,!!V> t0,
                        class [mscorlib]System.Tuple`2<!!V,!!U> t1) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(class [mscorlib]System.Tuple`1<class [mscorlib]System.Tuple`2<string[0...,0...],int32>> t0) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0(class [mscorlib]System.Tuple`2<class [mscorlib]System.Tuple`1<string>,class [mscorlib]System.Tuple`1<int32>> t) cil managed
    {
        .maxstack 1
        nop
        ret
    }

    .method public hidebysig instance void Overload0<U>(class [mscorlib]System.Tuple`1<class [mscorlib]System.Tuple`1<class CleanNamespaceName.SecondLevel.'Deeply wrong .namespace name'.'NamespaceA.NamespaceB.ClassName`1+InnerClass`2'>> t) cil managed
    {
        .maxstack 1
        nop
        ret
    }
}