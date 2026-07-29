// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net9.0 --filter Microsoft.IdentityModel.Benchmarks.Base64UrlEncoderTests*

    [MemoryDiagnoser]
    public class Base64UrlEncoderTests
    {
        private const string base64UrlEncodedString = "V1aOrL0rTJZYRxPHmMZdTWR0-ilwg9V9iEoKSn3CYl5vmBNqsM0x4VtvRbK8nCmnCIc2__QE92D4vQDR8AQ2j_BljJUkNY51VrbZ1wBar_7X2NktF_AQLkqDmwuagjhONR9_MIVysq36EAqxoHAwHHJx87XrrOkPDD8kiQ2uZEgPgK-4o02hhjsETU7KWiOKg4nKlLUU2YwuW2ZQxVubPfEv5SrW8BDgvNwPseyXfKrznhNAQHgwUX6sh1lTBm-cQdujkNsG62DeJSA2o9A_IhpKOuyQpaNda6U8jbBh3FGZhmFAm6yxNag3b6jAVlxphRNDvlm6UprgoFbvzcuH8W5ZH60LjNxsSKLH8W3gHIc7jhDA0vH2T8Nf2HEqFmqcsGr6aNm86ilWg1tchS_DlFPWqu8Wm3EEHTSJcd7BxMTvr9syRLICmhVsfHwdgMy1WfKklnyGJ_RT3kvbfCPQ2sSRMiOqCkdwCUECu-CcxS4CiIanlWnIpllmBov6vawcR6o6gmcFuqxhw2rp3815glnF7jNkmr7hsd0DPQ7qRUOHlGkF8_Sgretbgpb61y8a8DlVLlb7nBBQbTFif-lBAH4gfWWeNF9A3RFPQ8e8UKghJ7u_4ua9W_Lk_xpDkyGDXrkAzTYLxOGujRaWexOpwWSOKsXgIqXa94px0HAUIAVwP2Gy_gWcVz47ayedXh1Tcqb3K1hDlzZt4XK6O9eu-lAgy6gBltSrkntumDB-XEkxRabh8FNMln_LeEh_TgwWX4iVBR1-VD-VJw1e_aypVWj_E178TjCeb6Lc9pKD_r2VAieZpVp0c15g3vxznBWPD5mviHnK_NbSiccodSfpzGJbUsBuvKvhK4EFSw4_YlWJFlEXj3XYtiqO60crVynlEEqegLncI6RrjWe8WEfXEm_yeiglH5I-asU5sl0pBdLRdeg1xo1SZfR-CtgJ0dliwGkPDE6HcyGqhddMbIze_5I8ZazQ31PQaShhXtdH3K_cWXe4WhpR-_qYTrwib89ux2zZxePCkb_RXyvd09hv1J1kkmTf9f7q1xXfiBw49Iun90tJaOMru6PeL3Ayixj4d2C-rnwS43jcRJJ_SBiRgpBQo3Gg893UkxY2l2prQa-zU9GdbwlfDF9Htijxm75SuoxOldhTFDcpw6QqKjt1116gfkmgg16hXjvNhV8sCqxmHdKoIM6EOKVy5MAIJcg_-wbAVhbJQ205udIPb49GY1yDePieu2eQa6TU8Pn66YK5Kl4K6kCmOY6NpDdhDk6BwyJ6Z9wz2nF8OwF2mDKpMdP2nkFnq8iq2z9o7s7HwIP8pbr99kvMlw";
        // Add padding
        private static readonly string base64EncodedString = base64UrlEncodedString.Replace('-', '+').Replace('_', '/') + "==";
        // Add "padding" without adding special characters
        private static readonly string base64NoSpecialCharsEncodedString = base64UrlEncodedString.Replace('-', 'A').Replace('_', 'B') + "ab";
        // Add padding as only special characters (Base64-encoded but could be decoded with Base64Url API)
        private static readonly string base64NoSpecialCharsExceptPaddingEncodedString = base64UrlEncodedString.Replace('-', 'A').Replace('_', 'B') + "==";
        private static readonly string decodedString = Base64UrlEncoder.Decode(base64UrlEncodedString);
        private static readonly byte[] decodedBytes = Base64UrlEncoder.DecodeBytes(base64UrlEncodedString);

        [Benchmark]
        public void Decode_String_Base64Url() => Base64UrlEncoder.Decode(base64UrlEncodedString);

        [Benchmark]
        public void Decode_Span_Base64Url() => Base64UrlEncoder.Decode(base64UrlEncodedString.AsSpan());

        [Benchmark]
        public void DecodeBytes_Base64Url() => Base64UrlEncoder.DecodeBytes(base64UrlEncodedString);

        [Benchmark]
        public void Decode_Span_Output_Base64Url() => Base64UrlEncoder.Decode(base64UrlEncodedString.AsSpan(), new byte[Base64.GetMaxDecodedFromUtf8Length(base64UrlEncodedString.Length + 2)]);

        [Benchmark]
        public void Decode_String_Base64() => Base64UrlEncoder.Decode(base64EncodedString);

        [Benchmark]
        public void Decode_Span_Base64() => Base64UrlEncoder.Decode(base64EncodedString.AsSpan());

        [Benchmark]
        public void DecodeBytes_Base64() => Base64UrlEncoder.DecodeBytes(base64EncodedString);

        [Benchmark]
        public void Decode_Span_Output_Base64() => Base64UrlEncoder.Decode(base64EncodedString.AsSpan(), new byte[Base64.GetMaxDecodedFromUtf8Length(base64EncodedString.Length + 2)]);

        [Benchmark]
        public void Decode_String_Base64NoSpecialChars() => Base64UrlEncoder.Decode(base64NoSpecialCharsEncodedString);

        [Benchmark]
        public void Decode_Span_Base64NoSpecialChars() => Base64UrlEncoder.Decode(base64NoSpecialCharsEncodedString.AsSpan());

        [Benchmark]
        public void DecodeBytes_Base64NoSpecialChars() => Base64UrlEncoder.DecodeBytes(base64NoSpecialCharsEncodedString);

        [Benchmark]
        public void Decode_Span_Output_Base64NoSpecialChars() => Base64UrlEncoder.Decode(base64NoSpecialCharsEncodedString.AsSpan(), new byte[Base64.GetMaxDecodedFromUtf8Length(base64NoSpecialCharsEncodedString.Length + 2)]);

        [Benchmark]
        public void Decode_String_Base64NoSpecialCharsExceptPadding() => Base64UrlEncoder.Decode(base64NoSpecialCharsExceptPaddingEncodedString);

        [Benchmark]
        public void Decode_Span_Base64NoSpecialCharsExceptPadding() => Base64UrlEncoder.Decode(base64NoSpecialCharsExceptPaddingEncodedString.AsSpan());

        [Benchmark]
        public void DecodeBytes_Base64NoSpecialCharsExceptPadding() => Base64UrlEncoder.DecodeBytes(base64NoSpecialCharsExceptPaddingEncodedString);

        [Benchmark]
        public void Decode_Span_Output_Base64NoSpecialCharsExceptPadding() => Base64UrlEncoder.Decode(base64NoSpecialCharsExceptPaddingEncodedString.AsSpan(), new byte[Base64.GetMaxDecodedFromUtf8Length(base64NoSpecialCharsExceptPaddingEncodedString.Length + 2)]);

        [Benchmark]
        public void Encode_String_Base64Url() => Base64UrlEncoder.Encode(decodedString);

        [Benchmark]
        public void Encode_Bytes_Base64Url() => Base64UrlEncoder.Encode(decodedBytes);

        [Benchmark]
        public void Encode_Span_Base64Url() => Base64UrlEncoder.Encode(decodedBytes, new char[Base64.GetMaxEncodedToUtf8Length(decodedBytes.Length)]);

        [Benchmark]
        public void Encode_Bytes_Offset_Length_Base64Url() => Base64UrlEncoder.Encode(decodedBytes, decodedBytes.Length / 2, decodedBytes.Length / 2 - 10);
    }
}
