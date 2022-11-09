// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.ValueTuple")]
[assembly: AssemblyDescription("System.ValueTuple")]
[assembly: AssemblyDefaultAlias("System.ValueTuple")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.26515.06")]
[assembly: AssemblyInformationalVersion("4.6.26515.06 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.3.0")]




namespace System
{
    public static partial class TupleExtensions
    {
        public static void Deconstruct<T1>(this System.Tuple<T1> value, out T1 item1) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16, out T17 item17) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16, out T17 item17, out T18 item18) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16, out T17 item17, out T18 item18, out T19 item19) { throw null; }
        public static void Deconstruct<T1, T2>(this System.Tuple<T1, T2> value, out T1 item1, out T2 item2) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16, out T17 item17, out T18 item18, out T19 item19, out T20 item20) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20, T21>>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9, out T10 item10, out T11 item11, out T12 item12, out T13 item13, out T14 item14, out T15 item15, out T16 item16, out T17 item17, out T18 item18, out T19 item19, out T20 item20, out T21 item21) { throw null; }
        public static void Deconstruct<T1, T2, T3>(this System.Tuple<T1, T2, T3> value, out T1 item1, out T2 item2, out T3 item3) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4>(this System.Tuple<T1, T2, T3, T4> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5>(this System.Tuple<T1, T2, T3, T4, T5> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6>(this System.Tuple<T1, T2, T3, T4, T5, T6> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8) { throw null; }
        public static void Deconstruct<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9>> value, out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out T8 item8, out T9 item9) { throw null; }
        public static System.Tuple<T1> ToTuple<T1>(this System.ValueTuple<T1> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19>>> value) { throw null; }
        public static System.Tuple<T1, T2> ToTuple<T1, T2>(this System.ValueTuple<T1, T2> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19, T20>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20, T21>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19, T20, T21>>> value) { throw null; }
        public static System.Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this System.ValueTuple<T1, T2, T3> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this System.ValueTuple<T1, T2, T3, T4> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this System.ValueTuple<T1, T2, T3, T4, T5> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6> ToTuple<T1, T2, T3, T4, T5, T6>(this System.ValueTuple<T1, T2, T3, T4, T5, T6> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7> ToTuple<T1, T2, T3, T4, T5, T6, T7>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>> value) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9>> value) { throw null; }
        public static System.ValueTuple<T1> ToValueTuple<T1>(this System.Tuple<T1> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19>>> value) { throw null; }
        public static System.ValueTuple<T1, T2> ToValueTuple<T1, T2>(this System.Tuple<T1, T2> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19, T20>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9, T10, T11, T12, T13, T14, System.ValueTuple<T15, T16, T17, T18, T19, T20, T21>>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9, T10, T11, T12, T13, T14, System.Tuple<T15, T16, T17, T18, T19, T20, T21>>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3> ToValueTuple<T1, T2, T3>(this System.Tuple<T1, T2, T3> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4> ToValueTuple<T1, T2, T3, T4>(this System.Tuple<T1, T2, T3, T4> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5> ToValueTuple<T1, T2, T3, T4, T5>(this System.Tuple<T1, T2, T3, T4, T5> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6> ToValueTuple<T1, T2, T3, T4, T5, T6>(this System.Tuple<T1, T2, T3, T4, T5, T6> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> ToValueTuple<T1, T2, T3, T4, T5, T6, T7>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8>> value) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8, T9>> ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8, T9>> value) { throw null; }
    }
    public partial struct ValueTuple : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple>, System.IEquatable<System.ValueTuple>
    {
        public int CompareTo(System.ValueTuple other) { throw null; }
        public static System.ValueTuple Create() { throw null; }
        public static System.ValueTuple<T1> Create<T1>(T1 item1) { throw null; }
        public static System.ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) { throw null; }
        public static System.ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { throw null; }
        public static System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, System.ValueTuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1>>, System.IEquatable<System.ValueTuple<T1>>
    {
        public T1 Item1;
        public ValueTuple(T1 item1) { throw null; }
        public int CompareTo(System.ValueTuple<T1> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2>>, System.IEquatable<System.ValueTuple<T1, T2>>
    {
        public T1 Item1;
        public T2 Item2;
        public ValueTuple(T1 item1, T2 item2) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3>>, System.IEquatable<System.ValueTuple<T1, T2, T3>>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public ValueTuple(T1 item1, T2 item2, T3 item3) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3, T4> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3, T4>>, System.IEquatable<System.ValueTuple<T1, T2, T3, T4>>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3, T4> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3, T4> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3, T4, T5> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3, T4, T5>>, System.IEquatable<System.ValueTuple<T1, T2, T3, T4, T5>>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3, T4, T5> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3, T4, T5> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3, T4, T5, T6> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3, T4, T5, T6>>, System.IEquatable<System.ValueTuple<T1, T2, T3, T4, T5, T6>>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3, T4, T5, T6> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3, T4, T5, T6> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>>, System.IEquatable<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable, System.IComparable<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, System.IEquatable<System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>> where TRest : struct
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;
        public T5 Item5;
        public T6 Item6;
        public T7 Item7;
        public TRest Rest;
        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest) { throw null; }
        public int CompareTo(System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object other) { throw null; }
        public override string ToString() { throw null; }
    }
}
namespace System.Runtime.CompilerServices
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Event | System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property | System.AttributeTargets.ReturnValue | System.AttributeTargets.Struct)]
    [System.CLSCompliantAttribute(false)]
    public sealed partial class TupleElementNamesAttribute : System.Attribute
    {
        public TupleElementNamesAttribute(string[] transformNames) { }
        public System.Collections.Generic.IList<string> TransformNames { get { throw null; } }
    }
}
