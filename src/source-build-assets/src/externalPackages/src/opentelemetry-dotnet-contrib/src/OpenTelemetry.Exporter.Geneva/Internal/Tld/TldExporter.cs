// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using OpenTelemetry.Exporter.Geneva.External;

namespace OpenTelemetry.Exporter.Geneva.Tld;

internal static class TldExporter
{
    internal const int StringLengthLimit = (1 << 14) - 1; // 16 * 1024 - 1 = 16383
    internal static readonly IReadOnlyDictionary<string, string> V40_PART_A_TLD_MAPPING = new Dictionary<string, string>
    {
        // Part A
        [Schema.V40.PartA.IKey] = "iKey",
        [Schema.V40.PartA.Name] = "name",
        [Schema.V40.PartA.Time] = "time",

        // Part A Application Extension
        [Schema.V40.PartA.Extensions.App.Id] = "ext_app_id",
        [Schema.V40.PartA.Extensions.App.Ver] = "ext_app_ver",

        // Part A Cloud Extension
        [Schema.V40.PartA.Extensions.Cloud.Role] = "ext_cloud_role",
        [Schema.V40.PartA.Extensions.Cloud.RoleInstance] = "ext_cloud_roleInstance",
        [Schema.V40.PartA.Extensions.Cloud.RoleVer] = "ext_cloud_roleVer",

        // Part A Os extension
        [Schema.V40.PartA.Extensions.Os.Name] = "ext_os_name",
        [Schema.V40.PartA.Extensions.Os.Ver] = "ext_os_ver",
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(EventBuilder eb, string key, object value)
    {
        Debug.Assert(value != null, "value was null");

        switch (value)
        {
            case bool vb:
                eb.AddUInt8(key, (byte)(vb ? 1 : 0), EventOutType.Boolean);
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
            case string vs:
                eb.AddCountedAnsiString(key, vs, Encoding.UTF8, 0, Math.Min(vs.Length, StringLengthLimit));
                break;
            case DateTime vdt:
                eb.AddFileTime(key, vdt.ToUniversalTime());
                break;

            // TODO: case bool[]
            // TODO: case obj[]
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
                eb.AddCountedStringArray(key, vsarray);
                break;
            case DateTime[] vdtarray:
                for (var i = 0; i < vdtarray.Length; i++)
                {
                    vdtarray[i] = vdtarray[i].ToUniversalTime();
                }

                eb.AddFileTimeArray(key, vdtarray);
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

                eb.AddCountedAnsiString(key, repr, Encoding.UTF8, 0, Math.Min(repr.Length, StringLengthLimit));
                break;
        }
    }
}
