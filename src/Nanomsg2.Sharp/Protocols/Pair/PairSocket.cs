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

using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Pair
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class PairSocket : Socket, IPairSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_pair0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PairSocket()
                : base(__Open)
            {
            }
        }
    }

    namespace V1
    {
        public class PairSocket : Socket, IPairSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_pair1_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public PairSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestPairSocket : V1.PairSocket
    {
    }
}
