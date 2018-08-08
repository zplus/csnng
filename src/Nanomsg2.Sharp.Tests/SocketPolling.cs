//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

using System;
using System.Runtime.InteropServices;

// TODO: TBD: may refactor parts of this into the core API
namespace Nanomsg2.Sharp
{
    using static LayoutKind;

    [StructLayout(Sequential, Pack = 1)]
    internal struct POLLFD
    {
        public IntPtr Fd;

        public short Events;

        public short Revents;
        
        internal POLLFD(int fd, short e, short re)
        {
            // Not sure about this.  SOCKET should be IntPtr, but NNG_OPT_RECVFD is an int.
            Fd = (IntPtr)fd;
            Events = e;
            Revents = re;
        }
    }

    [Flags]
    internal enum PollEvent : short
    {
        ReaddNormal = 0x0100,
        ReadBand = 0x0200,
        In = ReaddNormal | ReadBand,
        HighPriority = 0x0400,
        WriteNormal = 0x0010,
        Out = WriteNormal,
        WriteBand = 0x0020,
        Error = 0x0001,
        Hangup = 0x0002,
        Invalid = 0x0004,
    }

    internal static class PollExtensionMethods
    {
        public static PollEvent ToPollEvent(this short value) => (PollEvent) value;

        public static short ToShort(this PollEvent value) => (short) value;
    }
}
