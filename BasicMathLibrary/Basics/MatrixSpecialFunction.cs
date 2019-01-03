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
using System.Globalization;
using Microsoft.Extensions.Logging;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Logging;
using Dodoni.MathLibrary.Basics.LowLevel;

namespace Dodoni.MathLibrary.Basics
{
    /// <summary>Provides static methods for trigonometric, logarithmic, and other common mathematical functions for matrices.
    /// </summary>
    public static class MatrixSpecialFunction
    {
        #region nested interfaces etc.

        /// <summary>Serves as interface for a library that provides methods for trigonometric, logarithmic, and other common mathematical functions.
        /// </summary>
        public interface ILibrary : IIdentifierNameable, IAnnotatable
        {
            /// <summary>Gets a <see cref="IQuadraticDenseMatrixSpecialFunctionLibrary"/> object that provides methods (exp, ln, sin etc.) for quadratic dense matrices.
            /// </summary>
            /// <value>A <see cref="IQuadraticDenseMatrixSpecialFunctionLibrary"/> object that provides methods (exp, ln, sin etc.) for quadratic dense matrices.</value>
            IQuadraticDenseMatrixSpecialFunctionLibrary QuadraticDenseLibrary
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

        /// <summary>Provide the library with specified matrix functions for quadratic dense matrices.
        /// </summary>
        public static readonly IQuadraticDenseMatrixSpecialFunctionLibrary ForQuadraticDenseArguments;
        #endregion

        #region static constructor

        /// <summary>Initializes the <see cref="MatrixSpecialFunction"/> class.
        /// </summary>
        /// <remarks>This constructor takes into account the Managed Extensibility Framework (MEF) with respect to <see cref="LowLevelMathConfiguration"/>.</remarks>
        static MatrixSpecialFunction()
        {
            ILibrary matrixSpecialFunctionLibrary = null;
            try
            {
                matrixSpecialFunctionLibrary = LowLevelMathConfiguration.MatrixSpecialFunction.CreateFromConfigurationFile();
                if (matrixSpecialFunctionLibrary == null)
                {
                    matrixSpecialFunctionLibrary = LowLevelMathConfiguration.MatrixSpecialFunction.Libraries.BuildIn;
                    Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "MatrixSpecialFunction");
                }
            }
            catch (Exception e)
            {
                /* thrown of Exceptions in static constructors should be avoided: 
                 */
                Logger.Stream.LogError(e, LowLevelMathConfigurationResources.LogFileMessageCorruptConfigFile);

                matrixSpecialFunctionLibrary = LowLevelMathConfiguration.MatrixSpecialFunction.Libraries.BuildIn;
                Logger.Stream.LogError(LowLevelMathConfigurationResources.LogFileMessageConfigFileUseDefaultImplementation, "MatrixSpecialFunction");
            }
            ForQuadraticDenseArguments = matrixSpecialFunctionLibrary.QuadraticDenseLibrary;
            matrixSpecialFunctionLibrary.Initialize();
        }
        #endregion

        #region public (static) methods

        /// <summary>Gets the value of the exponential function for a specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the exponential function in its <see cref="DenseMatrix"/> representation.</returns>
        public static DenseMatrix Exp(DenseMatrix a)
        {
            return ForQuadraticDenseArguments.Exp.GetValue(a);
        }

        /// <summary>Gets the value of the logarithm function for a specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the logarithm function in its <see cref="DenseMatrix"/> representation.</returns>
        public static DenseMatrix Log(DenseMatrix a)
        {
            return ForQuadraticDenseArguments.Log.GetValue(a);
        }

        /// <summary>Gets the value of the sinus function for a specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the sinus function in its <see cref="DenseMatrix"/> representation.</returns>
        public static DenseMatrix Sin(DenseMatrix a)
        {
            return ForQuadraticDenseArguments.Sin.GetValue(a);
        }

        /// <summary>Gets the value of the cosinus function for a specified matrix operation.
        /// </summary>
        /// <param name="a">The specified dense (quadratic) matrix.</param>
        /// <returns>The value of the cosinus function in its <see cref="DenseMatrix"/> representation.</returns>
        public static DenseMatrix Cos(DenseMatrix a)
        {
            return ForQuadraticDenseArguments.Cos.GetValue(a);
        }
        #endregion

        #region internal (static) methods

        /// <summary>Tests the input for some matrix multiplication, i.e. test the input for the operation 'A * B'.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns of <paramref name="a"/> does not coincide with
        /// the number of rows of <paramref name="b"/>.</exception>
        internal static void TestMultiplicationInput(IMatrix a, IMatrix b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'a'"));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b), String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'b'"));
            }
            if (a.ColumnCount != b.RowCount)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "a.ColumnCount, b.RowCount"));
            }
        }

        /// <summary>Tests the input for some matrix addition/subtraction.
        /// </summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="a"/> or <paramref name="b"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown, if the number of columns/rows of <paramref name="a"/> does not coincide with
        /// the number of columns/rows of <paramref name="b"/>.</exception>
        internal static void TestAdditionInput(IMatrix a, IMatrix b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a), string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'a'"));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b), string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentNull, "'b'"));
            }
            if (a.ColumnCount != b.ColumnCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "a.ColumnCount != b.ColumnCount"));
            }
            if (a.RowCount != b.RowCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.ArgumentCombinationInvalid, "a.RowCount != b.RowCount"));
            }
        }
        #endregion
    }
}