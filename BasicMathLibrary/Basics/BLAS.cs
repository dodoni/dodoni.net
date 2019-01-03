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
    /// <summary>Provides the Basic Linear Algebra Subprograms, see http://www.netlib.org/blas for further information.
    /// </summary>
    public static class BLAS
    {
        #region public nested interfaces, enumerations etc.

        /// <summary>Serves as interface for a BLAS Library.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Gets Level 1 operations, i.e. vector operations.
            /// </summary>
            /// <value>Level 1 operations.</value>
            ILevel1BLAS Level1
            {
                get;
            }

            /// <summary>Gets Level 2 operations, i.e. matrix-vector operations.
            /// </summary>
            /// <value>Level 2 operations.</value>
            ILevel2BLAS Level2
            {
                get;
            }

            /// <summary>Gets Level 3 operations, i.e. matrix-matrix operations.
            /// </summary>
            /// <value>Level 3 operations.</value>
            ILevel3BLAS Level3
            {
                get;
            }
        }

        /// <summary>Represents the states of the property 'transpose' for a specific matrix.
        /// </summary>
        public enum MatrixTransposeState
        {
            /// <summary>The matrix will not be transposed or is not transpose, respectively.
            /// </summary>
            NoTranspose,

            /// <summary>The matrix will be transposed or is transpose, respectively.
            /// </summary>
            Transpose,

            /// <summary>The conjugate matrix will be tranposed or is transpose, respecitively.
            /// </summary>
            Hermite
        }

        /// <summary>Represents the type of a triangular matrix.
        /// </summary>
        public enum TriangularMatrixType
        {
            /// <summary>A lower triangular matrix.
            /// </summary>
            LowerTriangularMatrix,

            /// <summary>A upper triangular matrix.
            /// </summary>
            UpperTriangularMatrix
        }

        /// <summary>Represents the side of a specific matrix in the calculation of a specified BLAS level 3 method.
        /// </summary>
        public enum Side
        {
            /// <summary>The matrix A or op(A) is on the left side.
            /// </summary>
            Left,

            /// <summary>The matrix A or op(A) is on the right side.
            /// </summary>
            Right
        }

        /// <summary>Represents the kind of operation for the BLAS method ?SYR2K.
        /// </summary>
        public enum Xsyr2kOperation
        {
            /// <summary>Calculate C := alpha*A*B^t + alpha*B*A^t + beta*C.
            /// </summary>
            ATimesBTransPlusBTimesATrans,

            /// <summary>Calculate C := alpha*A^t*B + alpha*B^t*A + beta*C.
            /// </summary>
            ATransTimesBPlusBTransTimesA
        }

        /// <summary>Represents the kind of operation for the BLAS method ZHER2K.
        /// </summary>
        public enum Zher2kOperation
        {
            /// <summary>Calculate C := \alpha*A*B^h + conjg(\alpha)*B*A^h + \beta * C.
            /// </summary>
            ATimesBHermitePlusBTimesAHermite,

            /// <summary>Calculate C := \alpha*B^h*A + conjg(\alpha)*A^h*B + beta*C.
            /// </summary>
            BHermiteTimesAPlusAHermiteTimesB
        }

        /// <summary>Represents the kind of operation for the BLAS method ZHERK.
        /// </summary>
        public enum ZherkOperation
        {
            /// <summary>Calculate C := \alpha * A * A^h + \beta*C.
            /// </summary>
            ATimesAHermite,

            /// <summary>Calculate C := alpha*A^h * A + beta*C.
            /// </summary>
            AHermiteTimesA
        }

        /// <summary>Represents the kind of operation for the BLAS method ?SYRK.
        /// </summary>
        public enum XsyrkOperation
        {
            /// <summary>Calculate C := alpha*A*A^t + beta*C.
            /// </summary>
            ATimesATranspose,

            /// <summary>Calculate C := alpha*A^t*A + beta*C.
            /// </summary>
            AtransposeTimesA
        }
        #endregion

        #region public (readonly) members

        /// <summary>Level 1 operations, i.e. vector operations.
        /// </summary>
        public static readonly ILevel1BLAS Level1;

        /// <summary>Level 2 operations, i.e. matrix-vector operations.
        /// </summary>
        public static readonly ILevel2BLAS Level2;

        /// <summary>Level 3 operations, i.e. matrix-matrix operations.
        /// </summary>
        public static readonly ILevel3BLAS Level3;

        /// <summary>Provides Matrix storage conversion methods.
        /// </summary>
        public static readonly MatrixStorageConversion MatrixStorageConversion = new MatrixStorageConversion();
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="BLAS"/> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static BLAS()
        {
            ILibrary blas = null;
            try
            {
                blas = LowLevelMathConfiguration.BLAS.CreateFromConfigurationFile();
                if (blas == null)
                {
                    blas = LowLevelMathConfiguration.BLAS.Libraries.BuildIn;
                    Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "BLAS");
                }
            }
            catch (Exception e)
            {
                /* thrown of Exceptions in static constructors should be avoided: 
                 */
                Logger.Stream.LogError(e, LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);

                blas = LowLevelMathConfiguration.BLAS.Libraries.BuildIn;
                Logger.Stream.LogError(String.Format(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "BLAS"));
            }
            Level1 = blas.Level1;
            Level2 = blas.Level2;
            Level3 = blas.Level3;
        }
        #endregion
    }
}