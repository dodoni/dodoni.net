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
using System.Reflection;
using System.Collections.Generic;

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public static partial class LowLevelMathConfiguration
    {
        /// <summary>The configuration for Linear Algebra PACKage, see http://www.netlib.org/lapack/index.html for further information.
        /// </summary>
        public static class LAPACK
        {
            /// <summary>Provides methods to determine which implementation to apply.
            /// </summary>
            public static class Libraries
            {
                /// <summary>The standard implementation.
                /// </summary>
                public static readonly Basics.LAPACK.ILibrary Standard = new BuildInDummyLAPACK();

                /// <summary>Initializes the <see cref="Libraries" /> class.
                /// </summary>
                static Libraries()
                {
                }
            }

            /// <summary>Stores the specified LAPACK device in the configuration file.
            /// </summary>
            /// <param name="device">The LAPACK device.</param>
            /// <remarks>The config file will not change. Use <see cref="LowLevelMathConfiguration.WriteConfigFile()"/> to write the changes into the file.</remarks>
            public static void Setup(Dodoni.MathLibrary.Basics.LAPACK.ILibrary device)
            {
                LowLevelMathConfiguration.StoreLibraryConfiguration("LAPACK", device.GetType());
            }

            /// <summary>Stores the specified LAPACK device in the configuration file.
            /// </summary>
            /// <param name="assemblyFilePath">The file path of the assembly that contains the Library (the Library will be loaded later via Managed Extensibility Framework).</param>
            /// <remarks>The config file will not change. Use <see cref="LowLevelMathConfiguration.WriteConfigFile()"/> to write the changes into the file.</remarks>
            public static void Setup(string assemblyFilePath)
            {
                LowLevelMathConfiguration.StoreLibraryConfiguration("LAPACK", assemblyFilePath);
            }

            /// <summary>Gets the reference to the LAPACK library with respect to the configuration file.
            /// </summary>
            /// <returns>The reference to the LAPACK library with respect to the configuration file, perhaps <c>null</c>.</returns>
            public static Basics.LAPACK.ILibrary CreateFromConfigurationFile()
            {
                var libraryLoader = new LibraryLoader<Basics.LAPACK.ILibrary>("LAPACK");

                if (libraryLoader.Value != null)
                {
                    return libraryLoader.Value;
                }
                return null;
            }
        }
    }
}