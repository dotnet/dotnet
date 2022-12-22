// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Windows.Input;

namespace NuGet.VisualStudio
{
    [Serializable]
    public class VsKeyInfo
    {
        private static readonly Lazy<VsKeyInfo> VsKeyInfoReturn = new Lazy<VsKeyInfo>(
            () => Create(Key.Return, '\r', 13));

        private VsKeyInfo()
        {
        }

        public static VsKeyInfo Create(Key key,
            char keyChar,
            byte virtualKey,
            KeyStates keyStates = default(KeyStates),
            bool shiftPressed = false,
            bool controlPressed = false,
            bool altPressed = false,
            bool capsLockToggled = false,
            bool numLockToggled = false)
        {
            return new VsKeyInfo
            {
                Key = key,
                KeyChar = keyChar,
                VirtualKey = virtualKey,
                KeyStates = keyStates,
                ShiftPressed = shiftPressed,
                ControlPressed = controlPressed,
                AltPressed = altPressed,
                CapsLockToggled = capsLockToggled,
                NumLockToggled = numLockToggled
            };
        }

        public static VsKeyInfo Enter
        {
            get { return VsKeyInfoReturn.Value; }
        }

        public Key Key { get; private set; }
        public char KeyChar { get; private set; }
        public byte VirtualKey { get; private set; }
        public KeyStates KeyStates { get; private set; }
        public bool ShiftPressed { get; private set; }
        public bool ControlPressed { get; private set; }
        public bool AltPressed { get; private set; }
        public bool CapsLockToggled { get; private set; }
        public bool NumLockToggled { get; private set; }
    }
}
