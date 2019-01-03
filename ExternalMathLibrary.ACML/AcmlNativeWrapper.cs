/* MIT License
Copyright (c) 2011-2019 Markus Wendt (http://www.dodoni-project.net)

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
Please see http://www.dodoni-project.net/ for more information concerning the Dodoni.net project. 
*/
using System;
using System.Runtime.InteropServices;

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Basics.LowLevel.Native
{
    /// <summary>Provides properties of the wrapper for AMD's ACML Library.
    /// </summary>
    /// <remarks>AMD Core Math Library (ACML) is an end-of-life software development library released by AMD, see https://en.wikipedia.org/wiki/AMD_Core_Math_Library 
    /// and https://web.archive.org/web/20141015020134/http://developer.amd.com/tools-and-sdks/cpu-development/amd-core-math-library-acml/acml-downloads-resources/#download </remarks>
    public static class AcmlNativeWrapper
    {
        #region public (const) members

        /// <summary>The name of the external dll.
        /// </summary>
        public const string dllName = "libACML.dll";

        /// <summary>The calling convention of the functions in the external dll.
        /// </summary>
        public const CallingConvention callingConvention = CallingConvention.Cdecl;
        #endregion

        #region private function import

        [DllImport(dllName, CallingConvention = callingConvention, EntryPoint = "ACMLVERSION", ExactSpelling = true)]
        private static extern void acml_acmlVersion(out int major, out int minor, out int patch);
        #endregion

        #region public (static) properties

        /// <summary>Gets the version of the ACML Library in its <see cref="System.String"/> representation.
        /// </summary>
        /// <value>The version of the ACML Library in its <see cref="System.String"/> representation.</value>
        public static string Version
        {
            get
            {
                acml_acmlVersion(out int major, out int minor, out int patch);
                return String.Format("ACML {0}.{1}.{2}", major, minor, patch);
            }
        }
        #endregion
    }
}