﻿//
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

namespace Nanomsg2.Sharp
{
    internal static class Imports
    {
        /// <summary>
        /// "nng.dll" but without ".dll" (see https://www.mono-project.com/docs/advanced/pinvoke/)
        /// </summary>
        public const string NanomsgDll = "nng";

        public const CallingConvention Cdecl = CallingConvention.Cdecl;
    }
}
