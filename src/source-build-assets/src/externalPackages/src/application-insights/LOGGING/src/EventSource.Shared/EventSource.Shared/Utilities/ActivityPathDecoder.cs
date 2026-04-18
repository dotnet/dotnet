//-----------------------------------------------------------------------
// <copyright file="ActivityPathDecoder.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.TraceEvent.Shared.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// A class to decode ETW Activity ID GUIDs into activity paths.
    /// </summary>
    /// <remarks>
    /// TODO: currently uses unsafe code. Will have to be refactored to safe code for partially-trusted environments like SharePoint. 
    /// </remarks>
    internal static class ActivityPathDecoder
    {
        /// <summary>
        /// The encoding for a list of numbers used to make Activity  Guids.   Basically
        /// we operate on nibbles (which are nice because they show up as hex digits).  The
        /// list is ended with a end nibble (0) and depending on the nibble value (Below)
        /// the value is either encoded into nibble itself or it can spill over into the
        /// bytes that follow.   
        /// </summary>
        private enum NumberListCodes : byte
        {
            End = 0x0,             // ends the list.   No valid value has this prefix.   
            LastImmediateValue = 0xA,
            PrefixCode = 0xB,
            MultiByte1 = 0xC,   // 1 byte follows.  If this Nibble is in the high bits, it the high bits of the number are stored in the low nibble.   
                                // commented out because the code does not explicitly reference the names (but they are logically defined).  
                                // MultiByte2 = 0xD,   // 2 bytes follow (we don't bother with the nibble optimzation
                                // MultiByte3 = 0xE,   // 3 bytes follow (we don't bother with the nibble optimzation
                                // MultiByte4 = 0xF,   // 4 bytes follow (we don't bother with the nibble optimzation
        }

        /// <summary>
        /// Checks whether the passed activity GUID represents an activity path.
        /// </summary>
        /// /// <param name="guid">Activity GUID to check.</param>
        /// <param name="processID">ID of the process.</param>        
        /// <remarks>
        /// You can pass a process ID of 0 to this routine and it will do the best it can, but the possibility
        /// of error is significantly higher (but still under .1%).
        /// </remarks>
        /// <returns>True if 'guid' follow the EventSource style activity ID for the process with ID processID, otherwise false.</returns>
        public static unsafe bool IsActivityPath(Guid guid, int processID)
        {
            uint* uintPtr = (uint*)&guid;

            uint sum = uintPtr[0] + uintPtr[1] + uintPtr[2] + 0x599D99AD;
            if (processID == 0)
            {
                // We guess that the process ID is < 16 bits and because it was xored
                // with the lower bits, the upper 16 bits should be independent of the
                // particular process, so we can at least confirm that the upper bits
                // match. 
                return (sum & 0xFFFF0000) == (uintPtr[3] & 0xFFFF0000);
            }

            if ((sum ^ (uint)processID) == uintPtr[3])
            {
                // This is the new style 
                return true;
            }

            return sum == uintPtr[3];         // This is old style where we don't make the ID unique machine wide.  
        }

        /// <summary>
        /// Returns a string representation for the activity path. 
        /// </summary>
        /// <param name="guid">Activity path to convert to string representation.</param>
        /// <remarks>
        /// If the GUID is not an activity path then the method returns the normal string representation for a GUID.
        /// </remarks>
        /// <returns>String representation for the activity path.</returns>
        public static unsafe string GetActivityPathString(Guid guid)
        {
            if (!IsActivityPath(guid, Process.GetCurrentProcess().Id))
            {
                return guid.ToString();
            }

            var processID = ActivityPathProcessID(guid);
            StringBuilder sb = StringBuilderCache.Acquire();
            if (processID != 0)
            {
                sb.Append("/#");    // Use /# to mark the fact that the first number is a process ID.   
                sb.Append(processID);
            }
            else
            {
                sb.Append('/'); // Use // to start to make it easy to anchor
            }

            byte* bytePtr = (byte*)&guid;
            byte* endPtr = bytePtr + 12;
            char separator = '/';
            while (bytePtr < endPtr)
            {
                uint nibble = (uint)(*bytePtr >> 4);
                bool secondNibble = false;              // are we reading the second nibble (low order bits) of the byte.
                NextNibble:
                if (nibble == (uint)NumberListCodes.End)
                {
                    break;
                }

                if (nibble <= (uint)NumberListCodes.LastImmediateValue)
                {
                    sb.Append('/').Append(nibble);
                    if (!secondNibble)
                    {
                        nibble = (uint)(*bytePtr & 0xF);
                        secondNibble = true;
                        goto NextNibble;
                    }

                    // We read the second nibble so we move on to the next byte. 
                    bytePtr++;
                    continue;
                }
                else if (nibble == (uint)NumberListCodes.PrefixCode)
                {
                    // This are the prefix codes. If the next nibble is MultiByte, then this is an overflow ID.
                    // we we denote with a $ instead of a / separator.

                    // Read the next nibble.  
                    if (!secondNibble)
                    {
                        nibble = (uint)(*bytePtr & 0xF);
                    }
                    else
                    {
                        bytePtr++;
                        if (endPtr <= bytePtr)
                        {
                            break;
                        }

                        nibble = (uint)(*bytePtr >> 4);
                    }

                    if (nibble < (uint)NumberListCodes.MultiByte1)
                    {
                        // If the nibble is less than MultiByte we have not defined what that means 
                        // For now we simply give up, and stop parsing.  We could add more cases here...
                        return guid.ToString();
                    }

                    // If we get here we have a overflow ID, which is just like a normal ID but the separator is $
                    separator = '$';

                    // Fall into the Multi-byte decode case.  
                }

                Debug.Assert(nibble >= (uint)NumberListCodes.MultiByte1, "Single-byte number should be fully handled by the code above");

                // At this point we are decoding a multi-byte number.
                // We are fetching the number as a stream of bytes. 
                uint numBytes = nibble - (uint)NumberListCodes.MultiByte1;

                uint value = 0;
                if (!secondNibble)
                {
                    value = (uint)(*bytePtr & 0xF);
                }

                bytePtr++;       // Adance to the value bytes

                numBytes++;     // Now numBytes is 1-4 and reprsents the number of bytes to read.  
                if (endPtr < bytePtr + numBytes)
                {
                    break;
                }

                // Compute the number (little endian) (thus backwards).  
                for (int i = (int)numBytes - 1; i >= 0; --i)
                {
                    value = (value << 8) + bytePtr[i];
                }

                // Print the value
                sb.Append(separator).Append(value);

                bytePtr += numBytes;        // Advance past the bytes.
            }

            sb.Append('/');
            return StringBuilderCache.GetStringAndRelease(sb);
        }

        /// <summary>
        /// Extracts process ID from an activity path.
        /// </summary>
        /// <param name="guid">Activity GUID.</param>
        /// <returns>Process ID part of the activity path.</returns>
        /// <remarks>The method assumes the passed GUID is in fact and activity path.</remarks>
        private static unsafe int ActivityPathProcessID(Guid guid)
        {
            uint* uintPtr = (uint*)&guid;
            uint sum = uintPtr[0] + uintPtr[1] + uintPtr[2] + 0x599D99AD;
            return (int)(sum ^ uintPtr[3]);
        }
    }
}
