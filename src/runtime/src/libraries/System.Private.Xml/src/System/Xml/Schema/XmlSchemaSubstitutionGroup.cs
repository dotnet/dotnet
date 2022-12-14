// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
    internal class XmlSchemaSubstitutionGroup : XmlSchemaObject
    {
        private readonly ArrayList _membersList = new ArrayList();
        private XmlQualifiedName _examplar = XmlQualifiedName.Empty;

        [XmlIgnore]
        internal ArrayList Members
        {
            get { return _membersList; }
        }

        [XmlIgnore]
        internal XmlQualifiedName Examplar
        {
            get { return _examplar; }
            set { _examplar = value; }
        }
    }

    internal sealed class XmlSchemaSubstitutionGroupV1Compat : XmlSchemaSubstitutionGroup
    {
        private readonly XmlSchemaChoice _choice = new XmlSchemaChoice();

        [XmlIgnore]
        internal XmlSchemaChoice Choice
        {
            get { return _choice; }
        }
    }
}
