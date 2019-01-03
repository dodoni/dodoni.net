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
using Microsoft.Extensions.Logging;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Provides functions for Vector units, i.e. methods applied to arrays of floating point numbers, complex numbers etc.
    /// </summary>
    public static class VectorUnit
    {
        #region nested interfaces etc.

        /// <summary>Serves as interface for a Vector Unit Library.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Gets basic mathematical functions.
            /// </summary>
            /// <value>Provides basic mathematical functions.</value>
            IVectorUnitBasics Basics
            {
                get;
            }

            /// <summary>Gets special mathematical functions.
            /// </summary>
            /// <value>Provides special mathematical functions.</value>
            IVectorUnitSpecial Special
            {
                get;
            }

            /// <summary>Initializes the Library.
            /// </summary>
            /// <remarks>Call this method before using the Library the first time.</remarks>
            void Initialize();
        }
        #endregion

        #region public (readonly) members

        /// <summary>Provide basic mathematical functions.
        /// </summary>
        public static readonly IVectorUnitBasics Basics;

        /// <summary>Provides special mathematical functions.
        /// </summary>
        public static readonly IVectorUnitSpecial Special;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="VectorUnit"/> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static VectorUnit()
        {
            ILibrary vectorUnit = null;
            try
            {
                vectorUnit = LowLevelMathConfiguration.VectorUnit.CreateFromConfigurationFile();
                if (vectorUnit == null)
                {
                    vectorUnit = LowLevelMathConfiguration.VectorUnit.Libraries.BuildIn;
                    Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "VectorUnit");
                }
            }
            catch (Exception e)
            {
                /* thrown of Exceptions in static constructors should be avoided: 
                 */
                Logger.Stream.LogError(e,LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);

                vectorUnit = LowLevelMathConfiguration.VectorUnit.Libraries.BuildIn;
                Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "VectorUnit");
            }
            Basics = vectorUnit.Basics;
            Special = vectorUnit.Special;
            vectorUnit.Initialize();
        }
        #endregion
    }
}