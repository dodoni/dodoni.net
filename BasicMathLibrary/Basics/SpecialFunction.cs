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
    /// <summary>Provides mathematical functions with established names and notations separated into several categories.
    /// </summary>
    public static class SpecialFunction
    {
        #region nested interfaces etc.

        /// <summary>Serves as interface for a Vector Unit Library.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Gets a collection of methods for calculation of Gamma and related functions.
            /// </summary>
            /// <value>A collection of methods for calculation of Gamma and related functions.</value>
            GammaSpecialFunctions Gamma
            {
                get;
            }

            /// <summary>Gets a collection of methods for calculation of Hypergeometric and related functions.
            /// </summary>
            /// <value>A collection of methods for calculation of Hypergeometric and related functions.</value>
            IHypergeometricSpecialFunctions Hypergeometric
            {
                get;
            }

            /// <summary>Gets a collection of methods for calculation of Iterated Exponential and related functions.
            /// </summary>
            /// <value>A collection of methods for calculation of Iterated Exponential and related functions.</value>
            IIteratedExponentialSpecialFunctions IteratedExponential
            {
                get;
            }

            /// <summary>Gets a collection of methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions.
            /// </summary>
            /// <value>A collection of methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions.</value>
            PrimitiveIntegralSpecialFunctions PrimitiveIntegral
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

        /// <summary>Provides methods for calculation of Gamma and related functions.
        /// </summary>
        public static readonly GammaSpecialFunctions Gamma;

        /// <summary>Provides methods for calculation of Hypergeometric and related functions
        /// </summary>
        public static readonly IHypergeometricSpecialFunctions Hypergeometric;

        /// <summary>Provides methods for calculation of Iterated Exponential and related functions; for example Lambert's W function etc.
        /// </summary>
        public static readonly IIteratedExponentialSpecialFunctions IteratedExponential;

        /// <summary>Provides methods for calculation of antiderivatives (i.e. Primitive Integral, Indefinite Integral) of elementary functions; for example error function etc.
        /// </summary>
        public static readonly PrimitiveIntegralSpecialFunctions PrimitiveIntegral;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="SpecialFunction"/> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static SpecialFunction()
        {
            ILibrary specialFunctionsLibrary = null;
            try
            {
                specialFunctionsLibrary = LowLevelMathConfiguration.SpecialFunctions.CreateFromConfigurationFile();
                if (specialFunctionsLibrary == null)
                {
                    Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileNoDefaultImplementationAvailable, "SpecialFunctions");
                    specialFunctionsLibrary = new BuildInPartialSpecialFunctionLibrary();
                }
            }
            catch (Exception e)
            {
                /* thrown of Exceptions in static constructors should be avoided: 
                 */
                Logger.Stream.LogError(e,LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);
                Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileNoDefaultImplementationAvailable, "SpecialFunctions");

                specialFunctionsLibrary = new BuildInPartialSpecialFunctionLibrary();
            }
            Gamma = specialFunctionsLibrary.Gamma;
            Hypergeometric = specialFunctionsLibrary.Hypergeometric;
            IteratedExponential = specialFunctionsLibrary.IteratedExponential;
            PrimitiveIntegral = specialFunctionsLibrary.PrimitiveIntegral;
        }
        #endregion
    }
}