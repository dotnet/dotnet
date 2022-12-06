// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Windows.Input;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal class NuGetRawUserInterface : PSHostRawUserInterface
    {
        private readonly NuGetPSHost _host;

        private IConsole Console
        {
            get { return _host.ActiveConsole; }
        }

        public NuGetRawUserInterface(NuGetPSHost host)
        {
            _host = host;
        }

        public override ConsoleColor BackgroundColor
        {
            get
            {
                // default color controlled by Visual Studio
                return NuGetHostUserInterface.NoColor;
            }
            set { }
        }

        public override Size BufferSize
        {
            get { return new Size(Console.ConsoleWidth, 0); }
            set { Debug.Assert(false, "Not Implemented"); }
        }

        public override Coordinates CursorPosition
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return new Coordinates();
            }
            set { Debug.Assert(false, "Not Implemented"); }
        }

        public override int CursorSize
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return 0;
            }
            set { Debug.Assert(false, "Not Implemented"); }
        }

        public override void FlushInputBuffer()
        {
            Debug.Assert(false, "Not Implemented");
        }

        public override ConsoleColor ForegroundColor
        {
            get
            {
                // default color controlled by Visual Studio
                return NuGetHostUserInterface.NoColor;
            }
            set { }
        }

        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", Justification = "property will be not used")]
        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            // no op
            Debug.Assert(false, "Not Implemented");

            return new BufferCell[0, 0];
        }

        public override bool KeyAvailable
        {
            get { return Console.Dispatcher.IsKeyAvailable; }
        }

        public override Size MaxPhysicalWindowSize
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return new Size();
            }
        }

        public override Size MaxWindowSize
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return new Size();
            }
        }

        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            // NOTE: readkey options are ignored as they are not really usable or applicable in PM console.

            VsKeyInfo keyInfo = Console.Dispatcher.WaitKey();

            if (keyInfo == null)
            {
                // abort current pipeline (ESC pressed)
                throw new PipelineStoppedException();
            }

            ControlKeyStates states = default(ControlKeyStates);
            states |= (keyInfo.CapsLockToggled ? ControlKeyStates.CapsLockOn : 0);
            states |= (keyInfo.NumLockToggled ? ControlKeyStates.NumLockOn : 0);
            states |= (keyInfo.ShiftPressed ? ControlKeyStates.ShiftPressed : 0);
            states |= (keyInfo.AltPressed ? ControlKeyStates.LeftAltPressed : 0); // assume LEFT alt
            states |= (keyInfo.ControlPressed ? ControlKeyStates.LeftCtrlPressed : 0); // assume LEFT ctrl

            return new KeyInfo(keyInfo.VirtualKey, keyInfo.KeyChar, states, keyDown: (keyInfo.KeyStates == KeyStates.Down));
        }

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            Debug.Assert(false, "Not Implemented");
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            Debug.Assert(false, "Not Implemented");
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            Debug.Assert(false, "Not Implemented");
        }

        public override Coordinates WindowPosition
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return new Coordinates();
            }
            set { Debug.Assert(false, "Not Implemented"); }
        }

        public override Size WindowSize
        {
            get { return new Size(Console.ConsoleWidth, 0); }
            set { Debug.Assert(false, "Not Implemented"); }
        }

        public override string WindowTitle
        {
            get
            {
                // no op
                Debug.Assert(false, "Not Implemented");
                return null;
            }
            set { Debug.Assert(false, "Not Implemented"); }
        }
    }
}
