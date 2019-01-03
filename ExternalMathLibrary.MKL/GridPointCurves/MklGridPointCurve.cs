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
using System.Text;
using System.Collections.Generic;

using Dodoni.MathLibrary.GridPointCurves;

namespace Dodoni.MathLibrary.Native.GridPointCurves
{
    /// <summary>Serves as factory for <see cref="IMklCurveDataFitting"/> objects, i.e. a grid point fitting with respect to Intel's MKL Library.
    /// </summary>
    /// <remarks>See Intel Math Kernel Library Reference Manual For further details.</remarks>
    public static partial class MklGridPointCurve
    {
        /// <summary>Describing the structure of partition x, i.e. of the grid point arguments.
        /// </summary>
        public enum xHintValue
        {
            /// <summary>Partition is non-uniform.
            /// </summary>
            DF_NON_UNIFORM_PARTITION = 0x00000001,

            /// <summary>Partition is quasi-uniform.
            /// </summary>
            DF_QUASI_UNIFORM_PARTITION = 0x00000002,

            /// <summary>Partition is uniform.
            /// </summary>
            DF_UNIFORM_PARTITION = 0x00000004,

            /// <summary>No hint is provided. By default, partition is interpreted as non-uniform
            /// </summary>
            DF_NO_HINT = 0x00000000,
        }

        /// <summary>Describing the structure of array y, i.e. of the grid point values.
        /// </summary>
        public enum yHintValue
        {
            /// <summary>Data is stored in row-major format according to C conventions.
            /// </summary>
            DF_MATRIX_STORAGE_ROWS = 0x00000010,

            /// <summary>Data is stored in column-major format according to Fortran conventions.
            /// </summary>
            DF_MATRIX_STORAGE_COLS = 0x00000020,

            /// <summary>The first coordinate of vector-valued data is provided.
            /// </summary>
            DF_1ST_COORDINATE = 0x00000080,

            /// <summary>No hint is provided. By default, the coordinates of vector-valued function y are provided and stored in row-major format.
            /// </summary>
            DF_NO_HINT = 0x00000000
        }

        /// <summary>Describing what the data fitting routine should estimate as final result.
        /// </summary>
        [Flags]
        public enum EstimationType
        {
            /// <summary>Compute derivatives of predefined order. The derivative of the zero order is the spline value.
            /// </summary>
            DF_INTERP = 0x00000001,

            /// <summary>Compute indices of cells in partition x that contain the sites.
            /// </summary>
            DF_CELL = 0x00000002
        }

        /// <summary>The computation method for the calculation of estimated values and integrals.
        /// </summary>
        public enum ComputationMethod
        {
            /// <summary>Method to be used for evaluation of spline related estimates.
            /// </summary>
            DF_METHOD_PP = 1
        }

        /// <summary>The description of the values to apply the curve interpolation.
        /// </summary>
        public enum SiteHint
        {
            /// <summary>Partition is non-uniform.
            /// </summary>
            DF_NON_UNIFORM_PARTITION = 0x00000001,

            /// <summary>Partition is uniform.
            /// </summary>
            DF_UNIFORM_PARTITION = 0x00000004,

            /// <summary>Interpolation sites are sorted in the ascending order and define a non-uniform partition.
            /// </summary>
            DF_SORTED_DATA = 0x00000040,

            /// <summary>No hint is provided. By default, the partition defined by interpolation sites is interpreted as non-uniform.
            /// </summary>
            DF_NO_HINT = 0x00000000
        }

        /// <summary>Describing the result of the interpolation.
        /// </summary>
        public enum ResultHint
        {
            /// <summary>Data is stored in row-major format according to C conventions.
            /// </summary>
            DF_MATRIX_STORAGE_ROWS = 0x00000010,

            /// <summary>Data is stored in column-major format according to Fortran conventions.
            /// </summary>
            DF_MATRIX_STORAGE_COLS = 0x00000020,

            /// <summary>No hint is provided. By default, the results are stored in row-major format.
            /// </summary>
            DF_NO_HINT = 0x00000000
        }

        /// <summary>Describing the integration limits.
        /// </summary>
        public enum IntegrationLimitHint
        {
            /// <summary>Partition is non-uniform.
            /// </summary>
            DF_NON_UNIFORM_PARTITION = 0x00000001,

            /// <summary>Partition is uniform.
            /// </summary>
            DF_UNIFORM_PARTITION = 0x00000004,

            /// <summary>Interpolation sites are sorted in the ascending order and define a non-uniform partition.
            /// </summary>
            DF_SORTED_DATA = 0x00000040,

            /// <summary>No hint is provided. By default, the partition defined by interpolation sites is interpreted as non-uniform.
            /// </summary>
            DF_NO_HINT = 0x00000000
        }

        /// <summary>The description of the result of the integral estimation calculation.
        /// </summary>
        public enum IntegralResultHint
        {
            /// <summary>Data is stored in row-major format according to C conventions.
            /// </summary>
            DF_MATRIX_STORAGE_ROWS = 0x00000010,

            /// <summary>Data is stored in column-major format according to Fortran conventions.
            /// </summary>
            DF_MATRIX_STORAGE_COLS = 0x00000020,

            /// <summary>No hint is provided. By default, the results are stored in row-major format.
            /// </summary>
            DF_NO_HINT = 0x00000000
        }
    }
}