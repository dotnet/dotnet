// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Net.Sockets
{
    // Contains information for a socket's linger time, the amount of time it will
    // remain after closing if data remains to be sent.
    public class LingerOption
    {
        private bool _enabled;
        private int _lingerTime;

        public LingerOption(bool enable, int seconds)
        {
            Enabled = enable;
            LingerTime = seconds;
        }

        // Enables or disables lingering after close.
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        // The amount of time, in seconds, to remain connected after a close.
        public int LingerTime
        {
            get
            {
                return _lingerTime;
            }
            set
            {
                _lingerTime = value;
            }
        }

        public override bool Equals(object? comparand)
        {
            return comparand is LingerOption option && option.Enabled == _enabled && option.LingerTime == _lingerTime;
        }

        public override int GetHashCode() => HashCode.Combine(_enabled, _lingerTime);
    }
}
