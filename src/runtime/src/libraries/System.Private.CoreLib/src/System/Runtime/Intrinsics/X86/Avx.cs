// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics.X86
{
    /// <summary>
    /// This class provides access to Intel AVX hardware instructions via intrinsics
    /// </summary>
    [Intrinsic]
    [CLSCompliant(false)]
    public abstract class Avx : Sse42
    {
        internal Avx() { }

        public static new bool IsSupported { get => IsSupported; }

        [Intrinsic]
        public new abstract class X64 : Sse42.X64
        {
            internal X64() { }

            public static new bool IsSupported { get => IsSupported; }
        }


        /// <summary>
        /// __m256 _mm256_add_ps (__m256 a, __m256 b)
        ///   VADDPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Add(Vector256<float> left, Vector256<float> right) => Add(left, right);
        /// <summary>
        /// __m256d _mm256_add_pd (__m256d a, __m256d b)
        ///   VADDPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Add(Vector256<double> left, Vector256<double> right) => Add(left, right);

        /// <summary>
        /// __m256 _mm256_addsub_ps (__m256 a, __m256 b)
        ///   VADDSUBPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> AddSubtract(Vector256<float> left, Vector256<float> right) => AddSubtract(left, right);
        /// <summary>
        /// __m256d _mm256_addsub_pd (__m256d a, __m256d b)
        ///   VADDSUBPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> AddSubtract(Vector256<double> left, Vector256<double> right) => AddSubtract(left, right);

        /// <summary>
        /// __m256 _mm256_and_ps (__m256 a, __m256 b)
        ///   VANDPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> And(Vector256<float> left, Vector256<float> right) => And(left, right);
        /// <summary>
        /// __m256d _mm256_and_pd (__m256d a, __m256d b)
        ///   VANDPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> And(Vector256<double> left, Vector256<double> right) => And(left, right);

        /// <summary>
        /// __m256 _mm256_andnot_ps (__m256 a, __m256 b)
        ///   VANDNPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> AndNot(Vector256<float> left, Vector256<float> right) => AndNot(left, right);
        /// <summary>
        /// __m256d _mm256_andnot_pd (__m256d a, __m256d b)
        ///   VANDNPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> AndNot(Vector256<double> left, Vector256<double> right) => AndNot(left, right);

        /// <summary>
        /// __m256 _mm256_blend_ps (__m256 a, __m256 b, const int imm8)
        ///   VBLENDPS ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<float> Blend(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control) => Blend(left, right, control);
        /// <summary>
        /// __m256d _mm256_blend_pd (__m256d a, __m256d b, const int imm8)
        ///   VBLENDPD ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<double> Blend(Vector256<double> left, Vector256<double> right, [ConstantExpected] byte control) => Blend(left, right, control);

        /// <summary>
        /// __m256 _mm256_blendv_ps (__m256 a, __m256 b, __m256 mask)
        ///   VBLENDVPS ymm, ymm, ymm/m256, ymm
        /// </summary>
        public static Vector256<float> BlendVariable(Vector256<float> left, Vector256<float> right, Vector256<float> mask) => BlendVariable(left, right, mask);
        /// <summary>
        /// __m256d _mm256_blendv_pd (__m256d a, __m256d b, __m256d mask)
        ///   VBLENDVPD ymm, ymm, ymm/m256, ymm
        /// </summary>
        public static Vector256<double> BlendVariable(Vector256<double> left, Vector256<double> right, Vector256<double> mask) => BlendVariable(left, right, mask);

        /// <summary>
        /// __m128 _mm_broadcast_ss (float const * mem_addr)
        ///   VBROADCASTSS xmm, m32
        /// </summary>
        public static unsafe Vector128<float> BroadcastScalarToVector128(float* source) => BroadcastScalarToVector128(source);

        /// <summary>
        /// __m256 _mm256_broadcast_ss (float const * mem_addr)
        ///   VBROADCASTSS ymm, m32
        /// </summary>
        public static unsafe Vector256<float> BroadcastScalarToVector256(float* source) => BroadcastScalarToVector256(source);
        /// <summary>
        /// __m256d _mm256_broadcast_sd (double const * mem_addr)
        ///   VBROADCASTSD ymm, m64
        /// </summary>
        public static unsafe Vector256<double> BroadcastScalarToVector256(double* source) => BroadcastScalarToVector256(source);

        /// <summary>
        /// __m256 _mm256_broadcast_ps (__m128 const * mem_addr)
        ///   VBROADCASTF128, ymm, m128
        /// </summary>
        public static unsafe Vector256<float> BroadcastVector128ToVector256(float* address) => BroadcastVector128ToVector256(address);
        /// <summary>
        /// __m256d _mm256_broadcast_pd (__m128d const * mem_addr)
        ///   VBROADCASTF128, ymm, m128
        /// </summary>
        public static unsafe Vector256<double> BroadcastVector128ToVector256(double* address) => BroadcastVector128ToVector256(address);

        /// <summary>
        /// __m256 _mm256_ceil_ps (__m256 a)
        ///   VROUNDPS ymm, ymm/m256, imm8(10)
        /// </summary>
        public static Vector256<float> Ceiling(Vector256<float> value) => Ceiling(value);
        /// <summary>
        /// __m256d _mm256_ceil_pd (__m256d a)
        ///   VROUNDPD ymm, ymm/m256, imm8(10)
        /// </summary>
        public static Vector256<double> Ceiling(Vector256<double> value) => Ceiling(value);

        /// <summary>
        /// __m128 _mm_cmp_ps (__m128 a, __m128 b, const int imm8)
        ///   VCMPPS xmm, xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<float> Compare(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => Compare(left, right, mode);
        /// <summary>
        /// __m128d _mm_cmp_pd (__m128d a, __m128d b, const int imm8)
        ///   VCMPPD xmm, xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<double> Compare(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => Compare(left, right, mode);
        /// <summary>
        /// __m256 _mm256_cmp_ps (__m256 a, __m256 b, const int imm8)
        ///   VCMPPS ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<float> Compare(Vector256<float> left, Vector256<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => Compare(left, right, mode);
        /// <summary>
        /// __m256d _mm256_cmp_pd (__m256d a, __m256d b, const int imm8)
        ///   VCMPPD ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<double> Compare(Vector256<double> left, Vector256<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => Compare(left, right, mode);

        /// <summary>
        /// __m256 _mm256_cmpeq_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(0)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedEqualNonSignaling);
        /// <summary>
        /// __m256d _mm256_cmpeq_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(0)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedEqualNonSignaling);

        /// <summary>
        /// __m256 _mm256_cmpgt_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(14)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareGreaterThan(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedGreaterThanSignaling);
        /// <summary>
        /// __m256d _mm256_cmpgt_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(14)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareGreaterThan(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedGreaterThanSignaling);

        /// <summary>
        /// __m256 _mm256_cmpge_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(13)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareGreaterThanOrEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedGreaterThanOrEqualSignaling);
        /// <summary>
        /// __m256d _mm256_cmpge_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(13)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareGreaterThanOrEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedGreaterThanOrEqualSignaling);

        /// <summary>
        /// __m256 _mm256_cmplt_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(1)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareLessThan(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedLessThanSignaling);
        /// <summary>
        /// __m256d _mm256_cmplt_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(1)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareLessThan(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedLessThanSignaling);

        /// <summary>
        /// __m256 _mm256_cmple_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(2)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareLessThanOrEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedLessThanOrEqualSignaling);
        /// <summary>
        /// __m256d _mm256_cmple_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(2)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareLessThanOrEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedLessThanOrEqualSignaling);

        /// <summary>
        /// __m256 _mm256_cmpneq_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(4)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareNotEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNotEqualNonSignaling);
        /// <summary>
        /// __m256d _mm256_cmpneq_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(4)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareNotEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNotEqualNonSignaling);

        /// <summary>
        /// __m256 _mm256_cmpngt_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(10)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareNotGreaterThan(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNotGreaterThanSignaling);
        /// <summary>
        /// __m256d _mm256_cmpngt_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(10)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareNotGreaterThan(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNotGreaterThanSignaling);

        /// <summary>
        /// __m256 _mm256_cmpnge_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(9)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareNotGreaterThanOrEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNotGreaterThanOrEqualSignaling);
        /// <summary>
        /// __m256d _mm256_cmpnge_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(9)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareNotGreaterThanOrEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNotGreaterThanOrEqualSignaling);

        /// <summary>
        /// __m256 _mm256_cmpnlt_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(5)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareNotLessThan(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNotLessThanSignaling);
        /// <summary>
        /// __m256d _mm256_cmpnlt_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(5)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareNotLessThan(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNotLessThanSignaling);

        /// <summary>
        /// __m256 _mm256_cmpnle_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(6)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareNotLessThanOrEqual(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNotLessThanOrEqualSignaling);
        /// <summary>
        /// __m256d _mm256_cmpnle_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(6)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareNotLessThanOrEqual(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNotLessThanOrEqualSignaling);

        /// <summary>
        /// __m256 _mm256_cmpord_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(7)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareOrdered(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.OrderedNonSignaling);
        /// <summary>
        /// __m256d _mm256_cmpord_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(7)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareOrdered(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.OrderedNonSignaling);

        /// <summary>
        /// __m128d _mm_cmp_sd (__m128d a, __m128d b, const int imm8)
        ///   VCMPSS xmm, xmm, xmm/m32, imm8
        /// </summary>
        public static Vector128<double> CompareScalar(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => CompareScalar(left, right, mode);
        /// <summary>
        /// __m128 _mm_cmp_ss (__m128 a, __m128 b, const int imm8)
        ///   VCMPSD xmm, xmm, xmm/m64, imm8
        /// </summary>
        public static Vector128<float> CompareScalar(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode) => CompareScalar(left, right, mode);

        /// <summary>
        /// __m256 _mm256_cmpunord_ps (__m256 a,  __m256 b)
        ///   CMPPS ymm, ymm/m256, imm8(3)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<float> CompareUnordered(Vector256<float> left, Vector256<float> right) => Compare(left, right, FloatComparisonMode.UnorderedNonSignaling);
        /// <summary>
        /// __m256d _mm256_cmpunord_pd (__m256d a,  __m256d b)
        ///   CMPPD ymm, ymm/m256, imm8(3)
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static Vector256<double> CompareUnordered(Vector256<double> left, Vector256<double> right) => Compare(left, right, FloatComparisonMode.UnorderedNonSignaling);

        /// <summary>
        /// __m128i _mm256_cvtpd_epi32 (__m256d a)
        ///   VCVTPD2DQ xmm, ymm/m256
        /// </summary>
        public static Vector128<int> ConvertToVector128Int32(Vector256<double> value) => ConvertToVector128Int32(value);
        /// <summary>
        /// __m128 _mm256_cvtpd_ps (__m256d a)
        ///   VCVTPD2PS xmm, ymm/m256
        /// </summary>
        public static Vector128<float> ConvertToVector128Single(Vector256<double> value) => ConvertToVector128Single(value);
        /// <summary>
        /// __m256i _mm256_cvtps_epi32 (__m256 a)
        ///   VCVTPS2DQ ymm, ymm/m256
        /// </summary>
        public static Vector256<int> ConvertToVector256Int32(Vector256<float> value) => ConvertToVector256Int32(value);
        /// <summary>
        /// __m256 _mm256_cvtepi32_ps (__m256i a)
        ///   VCVTDQ2PS ymm, ymm/m256
        /// </summary>
        public static Vector256<float> ConvertToVector256Single(Vector256<int> value) => ConvertToVector256Single(value);
        /// <summary>
        /// __m256d _mm256_cvtps_pd (__m128 a)
        ///   VCVTPS2PD ymm, xmm/m128
        /// </summary>
        public static Vector256<double> ConvertToVector256Double(Vector128<float> value) => ConvertToVector256Double(value);
        /// <summary>
        /// __m256d _mm256_cvtepi32_pd (__m128i a)
        ///   VCVTDQ2PD ymm, xmm/m128
        /// </summary>
        public static Vector256<double> ConvertToVector256Double(Vector128<int> value) => ConvertToVector256Double(value);

        /// <summary>
        /// __m128i _mm256_cvttpd_epi32 (__m256d a)
        ///   VCVTTPD2DQ xmm, ymm/m256
        /// </summary>
        public static Vector128<int> ConvertToVector128Int32WithTruncation(Vector256<double> value) => ConvertToVector128Int32WithTruncation(value);
        /// <summary>
        /// __m256i _mm256_cvttps_epi32 (__m256 a)
        ///   VCVTTPS2DQ ymm, ymm/m256
        /// </summary>
        public static Vector256<int> ConvertToVector256Int32WithTruncation(Vector256<float> value) => ConvertToVector256Int32WithTruncation(value);

        /// <summary>
        /// __m256 _mm256_div_ps (__m256 a, __m256 b)
        ///   VDIVPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Divide(Vector256<float> left, Vector256<float> right) => Divide(left, right);
        /// <summary>
        /// __m256d _mm256_div_pd (__m256d a, __m256d b)
        ///   VDIVPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Divide(Vector256<double> left, Vector256<double> right) => Divide(left, right);

        /// <summary>
        /// __m256 _mm256_dp_ps (__m256 a, __m256 b, const int imm8)
        ///   VDPPS ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<float> DotProduct(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control) => DotProduct(left, right, control);

        /// <summary>
        /// __m256 _mm256_moveldup_ps (__m256 a)
        ///   VMOVSLDUP ymm, ymm/m256
        /// </summary>
        public static Vector256<float> DuplicateEvenIndexed(Vector256<float> value) => DuplicateEvenIndexed(value);
        /// <summary>
        /// __m256d _mm256_movedup_pd (__m256d a)
        ///   VMOVDDUP ymm, ymm/m256
        /// </summary>
        public static Vector256<double> DuplicateEvenIndexed(Vector256<double> value) => DuplicateEvenIndexed(value);

        /// <summary>
        /// __m256 _mm256_movehdup_ps (__m256 a)
        ///   VMOVSHDUP ymm, ymm/m256
        /// </summary>
        public static Vector256<float> DuplicateOddIndexed(Vector256<float> value) => DuplicateOddIndexed(value);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<byte> ExtractVector128(Vector256<byte> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<sbyte> ExtractVector128(Vector256<sbyte> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<short> ExtractVector128(Vector256<short> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<ushort> ExtractVector128(Vector256<ushort> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<int> ExtractVector128(Vector256<int> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<uint> ExtractVector128(Vector256<uint> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<long> ExtractVector128(Vector256<long> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128i _mm256_extractf128_si256 (__m256i a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<ulong> ExtractVector128(Vector256<ulong> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128 _mm256_extractf128_ps (__m256 a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<float> ExtractVector128(Vector256<float> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m128d _mm256_extractf128_pd (__m256d a, const int imm8)
        ///   VEXTRACTF128 xmm/m128, ymm, imm8
        /// </summary>
        public static Vector128<double> ExtractVector128(Vector256<double> value, [ConstantExpected] byte index) => ExtractVector128(value, index);

        /// <summary>
        /// __m256 _mm256_floor_ps (__m256 a)
        ///   VROUNDPS ymm, ymm/m256, imm8(9)
        /// </summary>
        public static Vector256<float> Floor(Vector256<float> value) => Floor(value);
        /// <summary>
        /// __m256d _mm256_floor_pd (__m256d a)
        ///   VROUNDPS ymm, ymm/m256, imm8(9)
        /// </summary>
        public static Vector256<double> Floor(Vector256<double> value) => Floor(value);

        /// <summary>
        /// __m256 _mm256_hadd_ps (__m256 a, __m256 b)
        ///   VHADDPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> HorizontalAdd(Vector256<float> left, Vector256<float> right) => HorizontalAdd(left, right);
        /// <summary>
        /// __m256d _mm256_hadd_pd (__m256d a, __m256d b)
        ///   VHADDPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> HorizontalAdd(Vector256<double> left, Vector256<double> right) => HorizontalAdd(left, right);

        /// <summary>
        /// __m256 _mm256_hsub_ps (__m256 a, __m256 b)
        ///   VHSUBPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> HorizontalSubtract(Vector256<float> left, Vector256<float> right) => HorizontalSubtract(left, right);
        /// <summary>
        /// __m256d _mm256_hsub_pd (__m256d a, __m256d b)
        ///   VHSUBPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> HorizontalSubtract(Vector256<double> left, Vector256<double> right) => HorizontalSubtract(left, right);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<byte> InsertVector128(Vector256<byte> value, Vector128<byte> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<sbyte> InsertVector128(Vector256<sbyte> value, Vector128<sbyte> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<short> InsertVector128(Vector256<short> value, Vector128<short> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<ushort> InsertVector128(Vector256<ushort> value, Vector128<ushort> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<int> InsertVector128(Vector256<int> value, Vector128<int> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<uint> InsertVector128(Vector256<uint> value, Vector128<uint> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<long> InsertVector128(Vector256<long> value, Vector128<long> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_insertf128_si256 (__m256i a, __m128i b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<ulong> InsertVector128(Vector256<ulong> value, Vector128<ulong> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256 _mm256_insertf128_ps (__m256 a, __m128 b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<float> InsertVector128(Vector256<float> value, Vector128<float> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256d _mm256_insertf128_pd (__m256d a, __m128d b, int imm8)
        ///   VINSERTF128 ymm, ymm, xmm/m128, imm8
        /// </summary>
        public static Vector256<double> InsertVector128(Vector256<double> value, Vector128<double> data, [ConstantExpected] byte index) => InsertVector128(value, data, index);

        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<sbyte> LoadVector256(sbyte* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<byte> LoadVector256(byte* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<short> LoadVector256(short* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<ushort> LoadVector256(ushort* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<int> LoadVector256(int* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<uint> LoadVector256(uint* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<long> LoadVector256(long* address) => LoadVector256(address);
        /// <summary>
        /// __m256i _mm256_loadu_si256 (__m256i const * mem_addr)
        ///   VMOVDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<ulong> LoadVector256(ulong* address) => LoadVector256(address);
        /// <summary>
        /// __m256 _mm256_loadu_ps (float const * mem_addr)
        ///   VMOVUPS ymm, ymm/m256
        /// </summary>
        public static unsafe Vector256<float> LoadVector256(float* address) => LoadVector256(address);
        /// <summary>
        /// __m256d _mm256_loadu_pd (double const * mem_addr)
        ///   VMOVUPD ymm, ymm/m256
        /// </summary>
        public static unsafe Vector256<double> LoadVector256(double* address) => LoadVector256(address);

        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<sbyte> LoadAlignedVector256(sbyte* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<byte> LoadAlignedVector256(byte* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<short> LoadAlignedVector256(short* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<ushort> LoadAlignedVector256(ushort* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<int> LoadAlignedVector256(int* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<uint> LoadAlignedVector256(uint* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<long> LoadAlignedVector256(long* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256i _mm256_load_si256 (__m256i const * mem_addr)
        ///   VMOVDQA ymm, m256
        /// </summary>
        public static unsafe Vector256<ulong> LoadAlignedVector256(ulong* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256 _mm256_load_ps (float const * mem_addr)
        ///   VMOVAPS ymm, ymm/m256
        /// </summary>
        public static unsafe Vector256<float> LoadAlignedVector256(float* address) => LoadAlignedVector256(address);
        /// <summary>
        /// __m256d _mm256_load_pd (double const * mem_addr)
        ///   VMOVAPD ymm, ymm/m256
        /// </summary>
        public static unsafe Vector256<double> LoadAlignedVector256(double* address) => LoadAlignedVector256(address);

        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<sbyte> LoadDquVector256(sbyte* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<byte> LoadDquVector256(byte* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<short> LoadDquVector256(short* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<ushort> LoadDquVector256(ushort* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<int> LoadDquVector256(int* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<uint> LoadDquVector256(uint* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<long> LoadDquVector256(long* address) => LoadDquVector256(address);
        /// <summary>
        /// __m256i _mm256_lddqu_si256 (__m256i const * mem_addr)
        ///   VLDDQU ymm, m256
        /// </summary>
        public static unsafe Vector256<ulong> LoadDquVector256(ulong* address) => LoadDquVector256(address);

        /// <summary>
        /// __m128 _mm_maskload_ps (float const * mem_addr, __m128i mask)
        ///   VMASKMOVPS xmm, xmm, m128
        /// </summary>
        public static unsafe Vector128<float> MaskLoad(float* address, Vector128<float> mask) => MaskLoad(address, mask);
        /// <summary>
        /// __m128d _mm_maskload_pd (double const * mem_addr, __m128i mask)
        ///   VMASKMOVPD xmm, xmm, m128
        /// </summary>
        public static unsafe Vector128<double> MaskLoad(double* address, Vector128<double> mask) => MaskLoad(address, mask);

        /// <summary>
        /// __m256 _mm256_maskload_ps (float const * mem_addr, __m256i mask)
        ///   VMASKMOVPS ymm, ymm, m256
        /// </summary>
        public static unsafe Vector256<float> MaskLoad(float* address, Vector256<float> mask) => MaskLoad(address, mask);
        /// <summary>
        /// __m256d _mm256_maskload_pd (double const * mem_addr, __m256i mask)
        ///   VMASKMOVPD ymm, ymm, m256
        /// </summary>
        public static unsafe Vector256<double> MaskLoad(double* address, Vector256<double> mask) => MaskLoad(address, mask);

        /// <summary>
        /// void _mm_maskstore_ps (float * mem_addr, __m128i mask, __m128 a)
        ///   VMASKMOVPS m128, xmm, xmm
        /// </summary>
        public static unsafe void MaskStore(float* address, Vector128<float> mask, Vector128<float> source) => MaskStore(address, mask, source);
        /// <summary>
        /// void _mm_maskstore_pd (double * mem_addr, __m128i mask, __m128d a)
        ///   VMASKMOVPD m128, xmm, xmm
        /// </summary>
        public static unsafe void MaskStore(double* address, Vector128<double> mask, Vector128<double> source) => MaskStore(address, mask, source);

        /// <summary>
        /// void _mm256_maskstore_ps (float * mem_addr, __m256i mask, __m256 a)
        ///   VMASKMOVPS m256, ymm, ymm
        /// </summary>
        public static unsafe void MaskStore(float* address, Vector256<float> mask, Vector256<float> source) => MaskStore(address, mask, source);
        /// <summary>
        /// void _mm256_maskstore_pd (double * mem_addr, __m256i mask, __m256d a)
        ///   VMASKMOVPD m256, ymm, ymm
        /// </summary>
        public static unsafe void MaskStore(double* address, Vector256<double> mask, Vector256<double> source) => MaskStore(address, mask, source);

        /// <summary>
        /// __m256 _mm256_max_ps (__m256 a, __m256 b)
        ///   VMAXPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Max(Vector256<float> left, Vector256<float> right) => Max(left, right);
        /// <summary>
        /// __m256d _mm256_max_pd (__m256d a, __m256d b)
        ///   VMAXPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Max(Vector256<double> left, Vector256<double> right) => Max(left, right);

        /// <summary>
        /// __m256 _mm256_min_ps (__m256 a, __m256 b)
        ///   VMINPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Min(Vector256<float> left, Vector256<float> right) => Min(left, right);
        /// <summary>
        /// __m256d _mm256_min_pd (__m256d a, __m256d b)
        ///   VMINPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Min(Vector256<double> left, Vector256<double> right) => Min(left, right);

        /// <summary>
        /// int _mm256_movemask_ps (__m256 a)
        ///   VMOVMSKPS reg, ymm
        /// </summary>
        public static int MoveMask(Vector256<float> value) => MoveMask(value);
        /// <summary>
        /// int _mm256_movemask_pd (__m256d a)
        ///   VMOVMSKPD reg, ymm
        /// </summary>
        public static int MoveMask(Vector256<double> value) => MoveMask(value);

        /// <summary>
        /// __m256 _mm256_mul_ps (__m256 a, __m256 b)
        ///   VMULPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Multiply(Vector256<float> left, Vector256<float> right) => Multiply(left, right);
        /// <summary>
        /// __m256d _mm256_mul_pd (__m256d a, __m256d b)
        ///   VMULPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Multiply(Vector256<double> left, Vector256<double> right) => Multiply(left, right);

        /// <summary>
        /// __m256 _mm256_or_ps (__m256 a, __m256 b)
        ///   VORPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Or(Vector256<float> left, Vector256<float> right) => Or(left, right);
        /// <summary>
        /// __m256d _mm256_or_pd (__m256d a, __m256d b)
        ///   VORPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Or(Vector256<double> left, Vector256<double> right) => Or(left, right);

        /// <summary>
        /// __m128 _mm_permute_ps (__m128 a, int imm8)
        ///   VPERMILPS xmm, xmm, imm8
        /// </summary>
        public static Vector128<float> Permute(Vector128<float> value, [ConstantExpected] byte control) => Permute(value, control);
        /// <summary>
        /// __m128d _mm_permute_pd (__m128d a, int imm8)
        ///   VPERMILPD xmm, xmm, imm8
        /// </summary>
        public static Vector128<double> Permute(Vector128<double> value, [ConstantExpected] byte control) => Permute(value, control);

        /// <summary>
        /// __m256 _mm256_permute_ps (__m256 a, int imm8)
        ///   VPERMILPS ymm, ymm, imm8
        /// </summary>
        public static Vector256<float> Permute(Vector256<float> value, [ConstantExpected] byte control) => Permute(value, control);
        /// <summary>
        /// __m256d _mm256_permute_pd (__m256d a, int imm8)
        ///   VPERMILPD ymm, ymm, imm8
        /// </summary>
        public static Vector256<double> Permute(Vector256<double> value, [ConstantExpected] byte control) => Permute(value, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<byte> Permute2x128(Vector256<byte> left, Vector256<byte> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<sbyte> Permute2x128(Vector256<sbyte> left, Vector256<sbyte> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<short> Permute2x128(Vector256<short> left, Vector256<short> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<ushort> Permute2x128(Vector256<ushort> left, Vector256<ushort> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<int> Permute2x128(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<uint> Permute2x128(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<long> Permute2x128(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256i _mm256_permute2f128_si256 (__m256i a, __m256i b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<ulong> Permute2x128(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);
        /// <summary>
        /// __m256 _mm256_permute2f128_ps (__m256 a, __m256 b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<float> Permute2x128(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m256d _mm256_permute2f128_pd (__m256d a, __m256d b, int imm8)
        ///   VPERM2F128 ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<double> Permute2x128(Vector256<double> left, Vector256<double> right, [ConstantExpected] byte control) => Permute2x128(left, right, control);

        /// <summary>
        /// __m128 _mm_permutevar_ps (__m128 a, __m128i b)
        ///   VPERMILPS xmm, xmm, xmm/m128
        /// </summary>
        public static Vector128<float> PermuteVar(Vector128<float> left, Vector128<int> control) => PermuteVar(left, control);
        /// <summary>
        /// __m128d _mm_permutevar_pd (__m128d a, __m128i b)
        ///   VPERMILPD xmm, xmm, xmm/m128
        /// </summary>
        public static Vector128<double> PermuteVar(Vector128<double> left, Vector128<long> control) => PermuteVar(left, control);
        /// <summary>
        /// __m256 _mm256_permutevar_ps (__m256 a, __m256i b)
        ///   VPERMILPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> PermuteVar(Vector256<float> left, Vector256<int> control) => PermuteVar(left, control);
        /// <summary>
        /// __m256d _mm256_permutevar_pd (__m256d a, __m256i b)
        ///   VPERMILPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> PermuteVar(Vector256<double> left, Vector256<long> control) => PermuteVar(left, control);

        /// <summary>
        /// __m256 _mm256_rcp_ps (__m256 a)
        ///   VRCPPS ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Reciprocal(Vector256<float> value) => Reciprocal(value);

        /// <summary>
        /// __m256 _mm256_rsqrt_ps (__m256 a)
        ///   VRSQRTPS ymm, ymm/m256
        /// </summary>
        public static Vector256<float> ReciprocalSqrt(Vector256<float> value) => ReciprocalSqrt(value);

        /// <summary>
        /// __m256 _mm256_round_ps (__m256 a, _MM_FROUND_TO_NEAREST_INT | _MM_FROUND_NO_EXC)
        ///   VROUNDPS ymm, ymm/m256, imm8(8)
        /// </summary>
        public static Vector256<float> RoundToNearestInteger(Vector256<float> value) => RoundToNearestInteger(value);
        /// <summary>
        /// __m256 _mm256_round_ps (__m256 a, _MM_FROUND_TO_NEG_INF | _MM_FROUND_NO_EXC)
        ///   VROUNDPS ymm, ymm/m256, imm8(9)
        /// </summary>
        public static Vector256<float> RoundToNegativeInfinity(Vector256<float> value) => RoundToNegativeInfinity(value);
        /// <summary>
        /// __m256 _mm256_round_ps (__m256 a, _MM_FROUND_TO_POS_INF | _MM_FROUND_NO_EXC)
        ///   VROUNDPS ymm, ymm/m256, imm8(10)
        /// </summary>
        public static Vector256<float> RoundToPositiveInfinity(Vector256<float> value) => RoundToPositiveInfinity(value);
        /// <summary>
        /// __m256 _mm256_round_ps (__m256 a, _MM_FROUND_TO_ZERO | _MM_FROUND_NO_EXC)
        ///   VROUNDPS ymm, ymm/m256, imm8(11)
        /// </summary>
        public static Vector256<float> RoundToZero(Vector256<float> value) => RoundToZero(value);
        /// <summary>
        /// __m256 _mm256_round_ps (__m256 a, _MM_FROUND_CUR_DIRECTION)
        ///   VROUNDPS ymm, ymm/m256, imm8(4)
        /// </summary>
        public static Vector256<float> RoundCurrentDirection(Vector256<float> value) => RoundCurrentDirection(value);

        /// <summary>
        /// __m256d _mm256_round_pd (__m256d a, _MM_FROUND_TO_NEAREST_INT | _MM_FROUND_NO_EXC)
        ///   VROUNDPD ymm, ymm/m256, imm8(8)
        /// </summary>
        public static Vector256<double> RoundToNearestInteger(Vector256<double> value) => RoundToNearestInteger(value);
        /// <summary>
        /// __m256d _mm256_round_pd (__m256d a, _MM_FROUND_TO_NEG_INF | _MM_FROUND_NO_EXC)
        ///   VROUNDPD ymm, ymm/m256, imm8(9)
        /// </summary>
        public static Vector256<double> RoundToNegativeInfinity(Vector256<double> value) => RoundToNegativeInfinity(value);
        /// <summary>
        /// __m256d _mm256_round_pd (__m256d a, _MM_FROUND_TO_POS_INF | _MM_FROUND_NO_EXC)
        ///   VROUNDPD ymm, ymm/m256, imm8(10)
        /// </summary>
        public static Vector256<double> RoundToPositiveInfinity(Vector256<double> value) => RoundToPositiveInfinity(value);
        /// <summary>
        /// __m256d _mm256_round_pd (__m256d a, _MM_FROUND_TO_ZERO | _MM_FROUND_NO_EXC)
        ///   VROUNDPD ymm, ymm/m256, imm8(11)
        /// </summary>
        public static Vector256<double> RoundToZero(Vector256<double> value) => RoundToZero(value);
        /// <summary>
        /// __m256d _mm256_round_pd (__m256d a, _MM_FROUND_CUR_DIRECTION)
        ///   VROUNDPD ymm, ymm/m256, imm8(4)
        /// </summary>
        public static Vector256<double> RoundCurrentDirection(Vector256<double> value) => RoundCurrentDirection(value);

        /// <summary>
        /// __m256 _mm256_shuffle_ps (__m256 a, __m256 b, const int imm8)
        ///   VSHUFPS ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<float> Shuffle(Vector256<float> value, Vector256<float> right, [ConstantExpected] byte control) => Shuffle(value, right, control);
        /// <summary>
        /// __m256d _mm256_shuffle_pd (__m256d a, __m256d b, const int imm8)
        ///   VSHUFPD ymm, ymm, ymm/m256, imm8
        /// </summary>
        public static Vector256<double> Shuffle(Vector256<double> value, Vector256<double> right, [ConstantExpected] byte control) => Shuffle(value, right, control);

        /// <summary>
        /// __m256 _mm256_sqrt_ps (__m256 a)
        ///   VSQRTPS ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Sqrt(Vector256<float> value) => Sqrt(value);
        /// <summary>
        /// __m256d _mm256_sqrt_pd (__m256d a)
        ///   VSQRTPD ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Sqrt(Vector256<double> value) => Sqrt(value);

        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(sbyte* address, Vector256<sbyte> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(byte* address, Vector256<byte> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(short* address, Vector256<short> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(ushort* address, Vector256<ushort> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(int* address, Vector256<int> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(uint* address, Vector256<uint> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(long* address, Vector256<long> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQA m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(ulong* address, Vector256<ulong> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_ps (float * mem_addr, __m256 a)
        ///   VMOVAPS m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(float* address, Vector256<float> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm256_store_pd (double * mem_addr, __m256d a)
        ///   VMOVAPD m256, ymm
        /// </summary>
        public static unsafe void StoreAligned(double* address, Vector256<double> source) => StoreAligned(address, source);

        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(sbyte* address, Vector256<sbyte> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(byte* address, Vector256<byte> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(short* address, Vector256<short> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(ushort* address, Vector256<ushort> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(int* address, Vector256<int> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(uint* address, Vector256<uint> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(long* address, Vector256<long> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_si256 (__m256i * mem_addr, __m256i a)
        ///   VMOVNTDQ m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(ulong* address, Vector256<ulong> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_ps (float * mem_addr, __m256 a)
        ///   MOVNTPS m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(float* address, Vector256<float> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm256_stream_pd (double * mem_addr, __m256d a)
        ///   MOVNTPD m256, ymm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(double* address, Vector256<double> source) => StoreAlignedNonTemporal(address, source);

        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(sbyte* address, Vector256<sbyte> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(byte* address, Vector256<byte> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(short* address, Vector256<short> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(ushort* address, Vector256<ushort> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(int* address, Vector256<int> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(uint* address, Vector256<uint> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(long* address, Vector256<long> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_si256 (__m256i * mem_addr, __m256i a)
        ///   MOVDQU m256, ymm
        /// </summary>
        public static unsafe void Store(ulong* address, Vector256<ulong> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_ps (float * mem_addr, __m256 a)
        ///   MOVUPS m256, ymm
        /// </summary>
        public static unsafe void Store(float* address, Vector256<float> source) => Store(address, source);
        /// <summary>
        /// void _mm256_storeu_pd (double * mem_addr, __m256d a)
        ///   MOVUPD m256, ymm
        /// </summary>
        public static unsafe void Store(double* address, Vector256<double> source) => Store(address, source);

        /// <summary>
        /// __m256 _mm256_sub_ps (__m256 a, __m256 b)
        ///   VSUBPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Subtract(Vector256<float> left, Vector256<float> right) => Subtract(left, right);
        /// <summary>
        /// __m256d _mm256_sub_pd (__m256d a, __m256d b)
        ///   VSUBPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Subtract(Vector256<double> left, Vector256<double> right) => Subtract(left, right);

        /// <summary>
        /// int _mm_testc_ps (__m128 a, __m128 b)
        ///   VTESTPS xmm, xmm/m128
        /// </summary>
        public static bool TestC(Vector128<float> left, Vector128<float> right) => TestC(left, right);
        /// <summary>
        /// int _mm_testc_pd (__m128d a, __m128d b)
        ///   VTESTPD xmm, xmm/m128
        /// </summary>
        public static bool TestC(Vector128<double> left, Vector128<double> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<byte> left, Vector256<byte> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<sbyte> left, Vector256<sbyte> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<short> left, Vector256<short> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<ushort> left, Vector256<ushort> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<int> left, Vector256<int> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<uint> left, Vector256<uint> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<long> left, Vector256<long> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<ulong> left, Vector256<ulong> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_ps (__m256 a, __m256 b)
        ///   VTESTPS ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<float> left, Vector256<float> right) => TestC(left, right);

        /// <summary>
        /// int _mm256_testc_pd (__m256d a, __m256d b)
        ///   VTESTPS ymm, ymm/m256
        /// </summary>
        public static bool TestC(Vector256<double> left, Vector256<double> right) => TestC(left, right);

        /// <summary>
        /// int _mm_testnzc_ps (__m128 a, __m128 b)
        ///   VTESTPS xmm, xmm/m128
        /// </summary>
        public static bool TestNotZAndNotC(Vector128<float> left, Vector128<float> right) => TestNotZAndNotC(left, right);
        /// <summary>
        /// int _mm_testnzc_pd (__m128d a, __m128d b)
        ///   VTESTPD xmm, xmm/m128
        /// </summary>
        public static bool TestNotZAndNotC(Vector128<double> left, Vector128<double> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<byte> left, Vector256<byte> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<sbyte> left, Vector256<sbyte> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<short> left, Vector256<short> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<ushort> left, Vector256<ushort> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<int> left, Vector256<int> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<uint> left, Vector256<uint> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<long> left, Vector256<long> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<ulong> left, Vector256<ulong> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_ps (__m256 a, __m256 b)
        ///   VTESTPS ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<float> left, Vector256<float> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm256_testnzc_pd (__m256d a, __m256d b)
        ///   VTESTPD ymm, ymm/m256
        /// </summary>
        public static bool TestNotZAndNotC(Vector256<double> left, Vector256<double> right) => TestNotZAndNotC(left, right);

        /// <summary>
        /// int _mm_testz_ps (__m128 a, __m128 b)
        ///   VTESTPS xmm, xmm/m128
        /// </summary>
        public static bool TestZ(Vector128<float> left, Vector128<float> right) => TestZ(left, right);
        /// <summary>
        /// int _mm_testz_pd (__m128d a, __m128d b)
        ///   VTESTPD xmm, xmm/m128
        /// </summary>
        public static bool TestZ(Vector128<double> left, Vector128<double> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<byte> left, Vector256<byte> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<sbyte> left, Vector256<sbyte> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<short> left, Vector256<short> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<ushort> left, Vector256<ushort> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<int> left, Vector256<int> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<uint> left, Vector256<uint> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<long> left, Vector256<long> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_si256 (__m256i a, __m256i b)
        ///   VPTEST ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<ulong> left, Vector256<ulong> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_ps (__m256 a, __m256 b)
        ///   VTESTPS ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<float> left, Vector256<float> right) => TestZ(left, right);

        /// <summary>
        /// int _mm256_testz_pd (__m256d a, __m256d b)
        ///   VTESTPD ymm, ymm/m256
        /// </summary>
        public static bool TestZ(Vector256<double> left, Vector256<double> right) => TestZ(left, right);

        /// <summary>
        /// __m256 _mm256_unpackhi_ps (__m256 a, __m256 b)
        ///   VUNPCKHPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> UnpackHigh(Vector256<float> left, Vector256<float> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m256d _mm256_unpackhi_pd (__m256d a, __m256d b)
        ///   VUNPCKHPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> UnpackHigh(Vector256<double> left, Vector256<double> right) => UnpackHigh(left, right);

        /// <summary>
        /// __m256 _mm256_unpacklo_ps (__m256 a, __m256 b)
        ///   VUNPCKLPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> UnpackLow(Vector256<float> left, Vector256<float> right) => UnpackLow(left, right);
        /// <summary>
        /// __m256d _mm256_unpacklo_pd (__m256d a, __m256d b)
        ///   VUNPCKLPD ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> UnpackLow(Vector256<double> left, Vector256<double> right) => UnpackLow(left, right);

        /// <summary>
        /// __m256 _mm256_xor_ps (__m256 a, __m256 b)
        ///   VXORPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<float> Xor(Vector256<float> left, Vector256<float> right) => Xor(left, right);
        /// <summary>
        /// __m256d _mm256_xor_pd (__m256d a, __m256d b)
        ///   VXORPS ymm, ymm, ymm/m256
        /// </summary>
        public static Vector256<double> Xor(Vector256<double> left, Vector256<double> right) => Xor(left, right);
    }
}
