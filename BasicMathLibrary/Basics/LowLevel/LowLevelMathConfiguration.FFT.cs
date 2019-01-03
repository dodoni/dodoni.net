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
using System.Reflection;
using System.Collections.Generic;

using Dodoni.BasicComponents.Utilities;
using Dodoni.MathLibrary.Basics.LowLevel.BuildIn;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    public static partial class LowLevelMathConfiguration
    {
        /// <summary>The configuration for Fast Fourier Transformations.
        /// </summary>
        public static class FFT
        {
            /// <summary>Provides methods to determine which implementation to apply.
            /// </summary>
            public static class Libraries
            {
                /// <summary>A naive build-in, i.e. managed code implementation.
                /// </summary>
                public static readonly Basics.FFT.ILibrary NaiveBuildIn = new BuildInNaiveFourierTransformationLibrary();

                /// <summary>Initializes the <see cref="Libraries" /> class.
                /// </summary>
                static Libraries()
                {
                }
            }

            /// <summary>Stores the specified FFT device in the configuration file.
            /// </summary>
            /// <param name="device">The FFT device.</param>
            /// <remarks>The config file will not change. Use <see cref="LowLevelMathConfiguration.WriteConfigFile()"/> to write the changes into the file.</remarks>
            public static void Setup(Basics.FFT.ILibrary device)
            {
                LowLevelMathConfiguration.StoreLibraryConfiguration("FFT", device.GetType());
            }

            /// <summary>Stores the specified FFT device in the configuration file.
            /// </summary>
            /// <param name="assemblyFilePath">The file path of the assembly that contains the Library (the Library will be loaded later via Managed Extensibility Framework).</param>
            /// <remarks>The config file will not change. Use <see cref="LowLevelMathConfiguration.WriteConfigFile()"/> to write the changes into the file.</remarks>
            public static void Setup(string assemblyFilePath)
            {
                LowLevelMathConfiguration.StoreLibraryConfiguration("FFT", assemblyFilePath);
            }

            /// <summary>Gets the reference to the FFT library with respect to the configuration file.
            /// </summary>
            /// <returns>The reference to the FFT library with respect to the configuration file, perhaps <c>null</c>.</returns>
            public static Basics.FFT.ILibrary CreateFromConfigurationFile()
            {
                var libraryLoader = new LibraryLoader<Basics.FFT.ILibrary>("FFT");

                if (libraryLoader.Value != null)
                {
                    return libraryLoader.Value;
                }
                return null;
            }
        }
    }
}