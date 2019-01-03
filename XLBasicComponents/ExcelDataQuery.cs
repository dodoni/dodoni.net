/*
   Copyright (C) 2011-2014 Markus Wendt
   All rights reserved.
 
   Redistribution and use in source and binary forms, including commercial applications, with or without modification, 
   are permitted provided that the following conditions are met: 
 
   1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
      Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.

   2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
      disclaimer in the documentation and/or other materials provided with the distribution. 

   3. If you use this software in a product, an acknowledgment (see the following) in the product documentation is required. 
      The end-user documentation included with the redistribution, if any, must include the following acknowledgment: 

      "Dodoni.net (http://www.dodoni-project.net/) Copyright (C) 2011-2012 Markus Wendt" 

      Alternately, this acknowledgment may appear in the software itself, if and wherever such third-party acknowledgments normally appear. 

   4. Neither the name 'Dodoni.net' nor the names of its contributors may be used to endorse or promote products 
      derived from this software without specific prior written permission. For written permission, please contact info<at>dodoni-project.net. 

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
   BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
   SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
   DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
   INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
   OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
   For more information, please see http://www.dodoni-project.net/.
 */
using System;

using ExcelDna.Integration;

namespace Dodoni.XLBasicComponents
{
    /// <summary>Serves as factory for <see cref="IExcelDataQuery"/> objects which represents a specific Excel range or a collection of property names/values etc.
    /// </summary>
    public static partial class ExcelDataQuery
    {
        #region public const members

        /// <summary>The name of the <see cref="IExcelDataQuery"/> which contains in 'general properties'.
        /// </summary>
        public const string GeneralPropertyDataQueryName = "General Properties";

        /// <summary>The name of the property which contains in general the name of the object ('object name').
        /// </summary>
        public const string PropertyObjectName = "Object name";
        #endregion

        #region nested declarations

        /// <summary>Represents the type of the user input Excel-Range.
        /// </summary>
        public enum RangeType
        {
            /// <summary>Convert the specified argument of a specific User Defined Function (UDF) etc. into its <see cref="IExcelDataQuery"/> representation.
            /// </summary>
            Regular,

            /// <summary>The specified argument of a specific User Defined Function (UDF) etc. is some 'header Range' only, but data below the header should be taken into 
            /// account as well. Therefore the <see cref="IExcelDataQuery"/> representation is the provided argument plus the range below this 'header Range'.
            /// </summary>
            /// <remarks>One may set the IsVolatile to <c>true</c> in the <see cref="ExcelFunctionAttribute"/>.</remarks>
            HeaderOnly
        }

        /// <summary>Gets a specific object with respect to a specific object name.
        /// </summary>
        /// <param name="objectName">The name of the object.</param>
        /// <param name="value">The value (output).</param>
        /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
        /// <typeparam name="T">The type of the pool element.</typeparam>
        public delegate bool tTryGetPoolElement<T>(string objectName, out T value);
        #endregion

        #region public (static) methods

        /// <summary>Creates a specific <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <param name="excelRange">The Excel Range.</param>
        /// <returns>A <see cref="IExcelDataQuery"/> object which represents the specific user input; the name is set to <see cref="String.Empty"/>.</returns>
        /// <remarks>Use this method if you do not store the return value but you need to convert Excel input.</remarks>
        public static IExcelDataQuery Create(object excelRange)
        {
            return Create(excelRange, String.Empty);
        }

        /// <summary>Creates a specific <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <param name="excelRange">The Excel Range.</param>
        /// <param name="excelRangeName">The name of the Excel Range.</param>
        /// <returns>A <see cref="IExcelDataQuery"/> object which represents the specific user input.</returns>
        public static IExcelDataQuery Create(object excelRange, string excelRangeName)
        {
            if (IsEmpty(excelRange) == true)
            {
                return new ExcelNullQuery(excelRangeName);
            }
            else if (excelRange is ExcelReference)
            {
                ExcelReference excelReference = excelRange as ExcelReference;
                object[,] valueArray = excelReference.GetValue() as object[,];
                if (valueArray != null)
                {
                    if (valueArray.GetLength(0) == 1)  // just one row
                    {
                        return new ExcelTableQueryRowVector(excelReference, excelRangeName);
                    }
                    return new ExcelTableQueryRegular(excelReference, excelRangeName);
                }
                return new ExcelCellQuery(excelReference, excelRangeName);
            }
            else if (excelRange is IExcelDataQuery)
            {
                return (IExcelDataQuery)excelRange;
            }
            else if (excelRange is object[,])
            {
                return new ExcelTableQueryPlain(excelRange as object[,], excelRangeName);
            }
            return new ExcelCellQueryPlain(excelRange, excelRangeName);
        }

        /// <summary>Creates a specific <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <param name="excelRange">The Excel Range.</param>
        /// <param name="excelRangeType">A value indicating how to interpret <paramref name="excelRange"/> as user input.</param>
        /// <param name="excelRangeName">The name of the Excel Range.</param>
        /// <returns>A <see cref="IExcelDataQuery"/> object which represents the specific user input.</returns>
        public static IExcelDataQuery Create(object excelRange, RangeType excelRangeType, string excelRangeName = "")
        {
            return Create(excelRange, excelRangeType, ExcelDnaUtil.ExcelLimits.MaxRows, excelRangeName);
        }

        /// <summary>Creates a specific <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <param name="excelRange">The Excel Range.</param>
        /// <param name="excelRangeType">A value indicating how to interpret <paramref name="excelRange"/> as user input.</param>
        /// <param name="excelRangeName">The name of the Excel Range.</param>
        /// <param name="maxRowCount">If <paramref name="excelRangeType"/> indicates that <paramref name="excelRange"/> contains the 'header Range' of the data only then 
        /// the maximal number of rows to take into account below the 'header Range' are specified; otherwise the parameter will be ignored.</param>
        /// <returns>A <see cref="IExcelDataQuery"/> object which represents the specific user input.</returns>
        public static IExcelDataQuery Create(object excelRange, RangeType excelRangeType, int maxRowCount, string excelRangeName = "")
        {
            switch (excelRangeType)
            {
                case RangeType.Regular:
                    return Create(excelRange, excelRangeName);

                case RangeType.HeaderOnly:
                    if (excelRange is ExcelReference)
                    {
                        return new ExcelTableQueryHeader(excelRange as ExcelReference, excelRangeName, maxRowCount);
                    }
                    throw new ArgumentException("For the Excel range parameter 'AllowReference' should be set to 'true'.");

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>Creates a specific <see cref="IExcelDataQuery"/> object.
        /// </summary>
        /// <param name="propertyNames">The Excel Range (exactly one row or one column) which contains a list of property names.</param>
        /// <param name="propertyValues">The Excel range (exactly one row or one column) which contains the values of the properties.</param>
        /// <param name="excelDataQueryName">The name of the <see cref="IExcelDataQuery"/> object.</param>
        /// <returns>A <see cref="IExcelDataQuery"/> object which represents the specific user input.</returns>
        public static IExcelDataQuery Create(object propertyNames, object propertyValues, string excelDataQueryName = "General Properties")
        {
            if ((IsEmpty(propertyNames) == true) || (IsEmpty(propertyValues) == true))
            {
                return new ExcelNullQuery(excelDataQueryName);
            }
            else if ((propertyNames is object[,]) && (propertyValues is object[,]))
            {
                throw new NotImplementedException("Use 'ExcelArgument' attribute and set 'AllowReference=true'.");
            }
            else if ((propertyNames is ExcelReference) && (propertyValues is ExcelReference))
            {
                ExcelReference xlPropertyNames = (ExcelReference)propertyNames;
                ExcelReference xlPropertyValues = (ExcelReference)propertyValues;

                if (xlPropertyNames.ColumnLast - xlPropertyNames.ColumnFirst == 0) // the properties (name/value) are given row-wise 
                {
                    if (xlPropertyNames.RowLast - xlPropertyNames.RowFirst == 0)  // exact one property given
                    {
                        return new ExcelPropertyQuerySingle(xlPropertyNames, xlPropertyValues, excelDataQueryName);
                    }
                    return new ExcelPropertyQueryRowWise(xlPropertyNames, xlPropertyValues, excelDataQueryName);
                }
                else if (xlPropertyNames.RowLast - xlPropertyNames.RowFirst == 0)  // the properties (name/values) are given column-wise
                {
                    return new ExcelPropertyQueryColumnWise(xlPropertyNames, xlPropertyValues, excelDataQueryName);
                }
                else
                {
                    throw new ArgumentException("The Excel Range which represents properties must have exactly one row or one column.");
                }
            }
            else
            {
                return new ExcelPropertyQuerySingle(propertyNames, propertyValues, excelDataQueryName);
            }
        }

        /// <summary>Determines whether a specific excel range is empty.
        /// </summary>
        /// <param name="excelRange">The excel range.</param>
        /// <returns><c>true</c> if the specified excel range is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(object excelRange)
        {
            return ((excelRange == null) || (excelRange is ExcelMissing) || (excelRange is ExcelEmpty));
        }
        #endregion
    }
}