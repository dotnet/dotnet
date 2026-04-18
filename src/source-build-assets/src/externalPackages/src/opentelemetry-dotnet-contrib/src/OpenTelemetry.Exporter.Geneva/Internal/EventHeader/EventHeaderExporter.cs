// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.LinuxTracepoints;
using Microsoft.LinuxTracepoints.Provider;

namespace OpenTelemetry.Exporter.Geneva.EventHeader;

internal static class EventHeaderExporter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(EventHeaderDynamicBuilder eb, string key, object value)
    {
        Debug.Assert(value != null, "value was null");

        switch (value)
        {
            // TODO: what are all types that needs to be supported?
            case bool vb:
                eb.AddUInt8(key, (byte)(vb ? 1 : 0), EventHeaderFieldFormat.Boolean);
                break;
            case byte vui8:
                eb.AddUInt8(key, vui8);
                break;
            case sbyte vi8:
                eb.AddInt8(key, vi8);
                break;
            case short vi16:
                eb.AddInt16(key, vi16);
                break;
            case ushort vui16:
                eb.AddUInt16(key, vui16);
                break;
            case int vi32:
                eb.AddInt32(key, vi32);
                break;
            case uint vui32:
                eb.AddUInt32(key, vui32);
                break;
            case long vi64:
                eb.AddInt64(key, vi64);
                break;
            case ulong vui64:
                eb.AddUInt64(key, vui64);
                break;
            case float vf:
                eb.AddFloat32(key, vf);
                break;
            case double vd:
                eb.AddFloat64(key, vd);
                break;
            case string vs: // TODO: which type to use? does StringLimit also apply like TldExporter.StringLengthLimit?
                eb.AddString16(key, vs);
                break;
            case DateTime vdt:
                // TODO: what format to use? an integer or a string?
                string rfc3339String = vdt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFZ", CultureInfo.InvariantCulture);
                eb.AddString16(key, rfc3339String);
                break;

            case byte[] vui8array:
                eb.AddUInt8Array(key, vui8array);
                break;
            case sbyte[] vi8array:
                eb.AddInt8Array(key, vi8array);
                break;
            case short[] vi16array:
                eb.AddInt16Array(key, vi16array);
                break;
            case ushort[] vui16array:
                eb.AddUInt16Array(key, vui16array);
                break;
            case int[] vi32array:
                eb.AddInt32Array(key, vi32array);
                break;
            case uint[] vui32array:
                eb.AddUInt32Array(key, vui32array);
                break;
            case long[] vi64array:
                eb.AddInt64Array(key, vi64array);
                break;
            case ulong[] vui64array:
                eb.AddUInt64Array(key, vui64array);
                break;
            case float[] vfarray:
                eb.AddFloat32Array(key, vfarray);
                break;
            case double[] vdarray:
                eb.AddFloat64Array(key, vdarray);
                break;
            case string[] vsarray:
                eb.AddString16Array(key, vsarray);
                break;
            case DateTime[] vdtarray:
                // TODO: is this ever called?
                string[] rfc3339Strings = new string[vdtarray.Length];
                for (var i = 0; i < vdtarray.Length; ++i)
                {
                    rfc3339Strings[i] = vdtarray[i].ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFZ", CultureInfo.InvariantCulture);
                }

                eb.AddString16Array(key, rfc3339Strings);
                break;

            default:
                string repr;
                try
                {
                    repr = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
                }
                catch
                {
                    repr = $"ERROR: type {value!.GetType().FullName} is not supported";
                }

                eb.AddString16(key, repr);
                break;
        }
    }
}

#endif
