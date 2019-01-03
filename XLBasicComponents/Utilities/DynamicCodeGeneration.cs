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
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Utilities;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Serves as helper class for dynamic code generation.
    /// </summary>
    public static class DynamicCodeGeneration
    {
        #region nested enumerations

        /// <summary>Represents the type of a specific parameter.
        /// </summary>
        public enum eParameterType
        {
            /// <summary>A boolean.
            /// </summary>
            [String("bool")]
            Bool,

            /// <summary>A array of boolean.
            /// </summary>
            [String("bool[]")]
            BoolArray,

            /// <summary>A double precision floating-point number.
            /// </summary>
            [String("double")]
            Double,

            /// <summary>A array of double precision floating-point numbers.
            /// </summary>
            [String("double[]")]
            DoubleArray,

            /// <summary>A integer.
            /// </summary>
            [String("int")]
            Int,

            /// <summary>A array of integer.
            /// </summary>
            [String("int[]")]
            IntArray,

            /// <summary>A long integer.
            /// </summary>
            [String("long")]
            Long,

            /// <summary>A array of long integer.
            /// </summary>
            [String("long[]")]
            LongArray,

            /// <summary>A object, i.e. a generic (unknown) parameter type.
            /// </summary>
            [String("object")]
            Object
        }
        #endregion

        #region private (static) members

        /// <summary>The mapping between the internal (function) parameter and a representation which is more usefull for the GUI. 
        /// </summary>
        private static Dictionary<eParameterType, Type> sm_ParameterTypeMapping = new Dictionary<eParameterType, Type>();
        #endregion

        #region public (static) constructors

        /// <summary>Initializes the <see cref="DynamicCodeGeneration"/> class.
        /// </summary>
        static DynamicCodeGeneration()
        {
            sm_ParameterTypeMapping.Add(eParameterType.Bool, typeof(bool));
            sm_ParameterTypeMapping.Add(eParameterType.BoolArray, typeof(bool[]));
            sm_ParameterTypeMapping.Add(eParameterType.Double, typeof(double));
            sm_ParameterTypeMapping.Add(eParameterType.DoubleArray, typeof(double[]));
            sm_ParameterTypeMapping.Add(eParameterType.Int, typeof(int));
            sm_ParameterTypeMapping.Add(eParameterType.IntArray, typeof(int[]));
            sm_ParameterTypeMapping.Add(eParameterType.Long, typeof(long));
            sm_ParameterTypeMapping.Add(eParameterType.LongArray, typeof(long[]));
            sm_ParameterTypeMapping.Add(eParameterType.Object, typeof(object));
        }
        #endregion

        #region public static methods

        /// <summary>Gets the <see cref="Type"/> instance which represents the <paramref name="parameterType"/>,
        /// i.e. converts a <see cref="eParameterType"/> into a .NET <see cref="Type"/> representation.
        /// </summary>
        /// <param name="parameterType">The type of the parameter in its <see cref="eParameterType"/> representation.</param>
        /// <returns>The <see cref="Type"/> representation of the <paramref name="parameterType"/>.</returns>
        public static Type GetParameterType(eParameterType parameterType)
        {
            return sm_ParameterTypeMapping[parameterType];
        }

        /// <summary>Gets the <see cref="eParameterType"/> representation which represents a the <paramref name="parameterType"/>,
        /// i.e. converts a <see cref="Type"/> object into a <see cref="eParameterType"/> representation. 
        /// </summary>
        /// <param name="parameterType">The type of the parameter in its <see cref="Type"/> representation.</param>
        /// <returns>The <see cref="eParameterType"/> representation of the <paramref name="parameterType"/>,
        /// perhaps <see cref="eParameterType.Object"/>.</returns>
        public static eParameterType GetParameterType(Type parameterType)
        {
            /* here, we do a linear search */
            foreach (var type in sm_ParameterTypeMapping)
            {
                if (type.Value.Equals(parameterType))
                {
                    return type.Key;
                }
            }
            return eParameterType.Object;
        }

        /// <summary>Gets a specific generic function argument.
        /// </summary>
        /// <param name="functionSignature">The signature of the function, i.e. the names of the function parameters and the corresponding
        /// type or <c>typeof(Object)</c> if the type is unknown.</param>
        /// <param name="parameterValues">The names and values of several function parameters (can be <c>null</c>).</param>
        /// <param name="argument">A array which represents the function argument (output).</param>
        /// <param name="argumentIndex">The null-based index of <paramref name="argument"/> which represents the actually function argument (output).</param>
        public static void GetGenericArgument(IEnumerable<Tuple<IdentifierString, Type>> functionSignature, IEnumerable<Tuple<IdentifierString, dynamic>> parameterValues, out object[] argument, out int argumentIndex)
        {
            if (functionSignature == null)
            {
                throw new ArgumentNullException("functionSignature");
            }
            argument = new object[functionSignature.Count()];

            if (parameterValues == null)
            {
                if (functionSignature.Count() != 1)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsInvalid, "null [parameterValue]"), "parameterValues");
                }
                argumentIndex = 0;
            }
            else
            {
                argumentIndex = -1;

                int j = 0;
                foreach (var bodyArgument in functionSignature)
                {
                    IdentifierString functionParameterName = bodyArgument.Item1;

                    bool foundName = false;
                    foreach (var parameterValue in parameterValues)
                    {
                        if (parameterValue.Item1.Equals(functionParameterName) == true)
                        {
                            argument[j] = parameterValue.Item2;
                            foundName = true;
                            break;
                        }
                    }

                    if (foundName == false)
                    {
                        if (argumentIndex == -1)
                        {
                            argumentIndex = j;
                        }
                        else
                        {
                            throw new Exception("One and only one parameter must be the effective argument.");
                        }
                    }
                    j++;
                }
            }
        }

        /// <summary>Gets a specific generic function argument.
        /// </summary>
        /// <param name="functionSignature">The function body, i.e. the names of the function parameters and the corresponding
        /// type or <c>typeof(Object)</c> if the type is unknown.</param>
        /// <param name="parameterValues">The names and values of several function parameters.</param>
        /// <param name="argument">A array which represents the function argument (output).</param>
        /// <param name="argumentIndexes">A list of null-based indexes of <paramref name="argument"/> which represents the actually function argument (output).</param>
        public static void GetGenericArgument(IEnumerable<Tuple<IdentifierString, Type>> functionSignature, IEnumerable<Tuple<IdentifierString, dynamic>> parameterValues, out object[] argument, out int[] argumentIndexes)
        {
            if (functionSignature == null)
            {
                throw new ArgumentNullException("functionBody");
            }

            argument = new object[functionSignature.Count()];
            List<int> indexes = new List<int>();

            if (parameterValues == null)
            {
                for (int k = 0; k < argument.Length; k++)
                {
                    indexes.Add(k);
                }
            }
            else
            {
                int j = 0;
                foreach (var bodyArgument in functionSignature)
                {
                    IdentifierString functionParameterName = bodyArgument.Item1;

                    bool foundName = false;
                    foreach (var parameterValue in parameterValues)
                    {
                        if (parameterValue.Item1.Equals(functionParameterName) == true)
                        {
                            argument[j] = parameterValue.Item2;
                            foundName = true;
                            break;
                        }
                    }

                    if (foundName == false)
                    {
                        indexes.Add(j);
                    }
                    j++;
                }
            }

            if (indexes.Count == 0)
            {
                throw new ArgumentException("No remaining parameter found.");
            }
            argumentIndexes = indexes.ToArray();
        }
        #endregion
    }
}