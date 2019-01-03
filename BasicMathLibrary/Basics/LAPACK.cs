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
    /// <summary>Provides the Linear Algebra PACKage, see http://www.netlib.org/lapack/index.html for further information.
    /// </summary>
    public static class LAPACK
    {
        #region nested interfaces, enumerations etc.

        /// <summary>Serves as interface for a LAPACK Library.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Provides routines that are related to Least Square and Eigenvalue calculations.
            /// </summary>
            /// <value>Provides routines that are related to Least Square and Eigenvalue calculations.</value>
            LapackEigenvalues EigenValues
            {
                get;
            }

            /// <summary>Provides routines that are related to Linear Equations.
            /// </summary>
            /// <value>Provides routines that are related to Linear Equations.</value>
            LapackLinearEquations LinearEquations
            {
                get;
            }

            /// <summary>Provides auxiliary and utility routines of the LAPACK library.
            /// </summary>
            /// <value>Provides auxiliary and utility routines.</value>
            LapackAuxiliaryUtilityRoutines AuxiliaryRoutines
            {
                get;
            }
        }

        /// <summary>Represents the side of a specific matrix operation.
        /// </summary>
        public enum Side
        {
            /// <summary>The matrix is on the left side.
            /// </summary>
            Left,

            /// <summary>The matrix is on the right side.
            /// </summary>
            Right
        }

        /// <summary>Represents the type of a bidiagonal matrix.
        /// </summary>
        public enum BidiagonalMatrixType
        {
            /// <summary>A lower bidiagonal matrix.
            /// </summary>
            LowerBidiagonalMatrix,

            /// <summary>A upper bidiagonal matrix.
            /// </summary>
            UpperBidiagonalMatrix
        }
        #endregion

        #region public static (readonly) members

        /// <summary>Provides routines that are related to Least Square and Eigenvalue calculations.
        /// </summary>
        public static readonly LapackEigenvalues EigenValues;

        /// <summary>Provides routines that are related to Linear Equations.
        /// </summary>
        public static readonly LapackLinearEquations LinearEquations;

        /// <summary>Provides auxiliary and utility routines.
        /// </summary>
        public static readonly LapackAuxiliaryUtilityRoutines AuxiliaryRoutines;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="LAPACK"/> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static LAPACK()
        {
            ILibrary lapack = null;
            try
            {
                lapack = LowLevelMathConfiguration.LAPACK.CreateFromConfigurationFile();
                if (lapack == null)
                {
                    lapack = LowLevelMathConfiguration.LAPACK.Libraries.Standard;  // can be null, i.e. fallback solution is perhaps not available
                    Logger.Stream.LogCritical(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "LAPACK");
                }
            }
            catch (Exception e)  // thrown of Exceptions in static constructors should be avoided
            {
                Logger.Stream.LogError(e, LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);

                lapack = LowLevelMathConfiguration.LAPACK.Libraries.Standard;
                Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "LAPACK");
            }
            EigenValues = lapack.EigenValues;
            LinearEquations = lapack.LinearEquations;
            AuxiliaryRoutines = lapack.AuxiliaryRoutines;
        }
        #endregion
    }
}