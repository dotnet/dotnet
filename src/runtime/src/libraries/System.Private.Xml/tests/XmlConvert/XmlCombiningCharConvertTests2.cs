// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using OLEDB.Test.ModuleCore;

namespace System.Xml.XmlConvertTests
{
    internal class XmlCombiningCharConvertTests2 : XmlCombiningCharConvertTests
    {
        #region Constructors and Destructors

        public XmlCombiningCharConvertTests2()
        {
            for (int i = 0; i < _byte_CombiningChar.Length; i = i + 2)
            {
                AddVariation(new CVariation(this, "EncodeNmToken-EncodeLocalNmToken : " + _Expbyte_CombiningChar[i / 2], XmlEncodeName2));
            }
        }

        #endregion

        #region Public Methods and Operators

        public int XmlEncodeName2()
        {
            int i = ((CurVariation.id) - 1) * 2;
            string strEnVal = string.Empty;

            char c = (char)BinaryPrimitives.ReadUInt16LittleEndian(new Span<byte>(_byte_CombiningChar, i, 2));
            strEnVal = XmlConvert.EncodeNmToken(c.ToString());
            if (_Expbyte_CombiningChar[i / 2] != "_x0A6F_" && _Expbyte_CombiningChar[i / 2] != "_x0E46_")
            {
                CError.Compare(strEnVal, _Expbyte_CombiningChar[i / 2], "Comparison failed at " + i);
            }
            return TEST_PASS;
        }
        #endregion
    }
}
