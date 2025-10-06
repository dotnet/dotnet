// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0161 // Convert to file-scoped namespace

internal class NamespacelessClass
{
    public void Method0() { }
    public void Method1(int i) { }
    public void Method2(List<string> ls) { }
    public void Method3(string p, int l) { }
    internal class Inner
    {
        public void Method0() { }
        public void Method1(int i) { }
        public void Method2<U>(int i) { }
        public void Method3<U, T>(int i) { }
    }
}

internal record NamespacelessRecord
{
    public void Method0() { }
    public void Method1(int i) { }
    public void Method2(List<string> ls) { }
    public void Method3(string p, int l) { }
    internal record Inner
    {
        public void Method0() { }
        public void Method1(int i) { }
        public void Method2<U>(int i) { }
        public void Method3<U, T>(int i) { }
    }
}

namespace TestClasses
{
    internal class Outer
    {
        public void Method0() { }
        public void Method1(int i) { }
        public void Method2(List<string> ls) { }
        public void Method3(string p, int l) { }
        internal class Inner
        {
            public void Method0() { }
            public void Method1(int i) { }
            public void Method2<U>(int i) { }
            public void Method3<U, T>(int i) { }
        }
    }

    internal class OuterPrime : Outer { }

    internal class Outer<T>
    {
        public void Method0() { }
        public void Method1(T t) { }
        public void Method2<U>(U[] u) { }
        public void Method3<U>(T t, U u) { }

        internal class Inner<V>
        {
            public void Method0() { }
            public void Method1(T t) { }
            public void Method2(V v) { }
            public void Method3<U>(T t, U u, V v) { }
            public void Method4<U, X>(X x, U u) { }
            public void Method5<U, X>(List<X> x, U u) { }

            internal class MoreInner<I>
            {
                public void Method0<U>(T t, V v, I i, U u) { }
            }
        }
    }

    internal class OuterPrime<Z> : Outer<Z> { }

    internal class OuterPrime<Y, Z> : Outer<Z> { }

    internal class OuterString : Outer<string> { }

    internal interface IImplementation
    {
        void ImplMethod0();
        void ImplMethod1(int i);
    }

    internal class Impl : IImplementation
    {
        void IImplementation.ImplMethod0() { }
        void IImplementation.ImplMethod1(int i) { }
    }

    internal interface IImplementation<T>
    {
        void ImplMethod0();
        void ImplMethod1(T t);
        void ImplMethod2<U>(T t, U u, string s);
    }

    internal class Impl<T> : IImplementation<T>
    {
        void IImplementation<T>.ImplMethod0() { }
        void IImplementation<T>.ImplMethod1(T t) { }
        void IImplementation<T>.ImplMethod2<U>(T t, U u, string s) { }
    }

    internal class Overloads
    {
        public void Overload0() { }
        public void Overload0(int i) { }
        public void Overload0(int i, Overloads c) { }
        public unsafe void Overload0(int* p) { }
        public void Overload0(dynamic d) { }
        public void Overload0<U>(U u) { }
        public void Overload0<U>() { }
        public void Overload0<U, T>() { }
        public void Overload0<U>(U[] u) { }
        public void Overload0<U>(U[][] u) { }
        public void Overload0<U>(U[,] u) { }
        public void Overload0<U>(U[,,] u) { }
        public void Overload0<U>(List<int> l) { }
        public void Overload0<U>(List<U> l) { }
        public void Overload0<U, V>(Tuple<U, V> t0, Tuple<V, U> t1) { }
        public void Overload0(Tuple<Tuple<string[,], int>> t0) { }
        public void Overload0(Tuple<Tuple<string>, Tuple<int>> t) { }
        public void Overload0<U>(Tuple<Tuple<Outer<U>.Inner<U>>> t) { }
    }

    public class Class狧麱狵錋狾龍龪啊阿埃挨哎唉0u㐀㐁㐂㐃㐄㐅㐆㐇6ⅶ０ǒoU1U2U38丂丄丅丆丏丒丟
    {
        public void Method0()
        {

        }
    }
}


#pragma warning restore IDE0161 // Convert to file-scoped namespace
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822 // Mark members as static
