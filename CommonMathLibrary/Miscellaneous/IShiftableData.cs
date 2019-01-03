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

using Dodoni.BasicComponents;

namespace Dodoni.MathLibrary.Miscellaneous
{
    /// <summary>Serves as interface for shiftable single data.
    /// </summary>
    public interface IShiftableData : IIdentifierNameable
    {
        /// <summary>Occurs when the <see cref="IShiftableData.Value"/> property has a new value.
        /// </summary>
        event DataChangeEventHandler DataChanged;

        /// <summary>Gets a value indicating whether the data is currently shifted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is currently shifted; otherwise, <c>false</c>.
        /// </value>
        bool IsShifted
        {
            get;
        }

        /// <summary>Gets the perhaps shifted value of the current instance.
        /// </summary>
        /// <value>The perhaps shifted value of the current instance.</value>
        double Value
        {
            get;
        }

        /// <summary>Gets the original (i.e. not shifted) value of the current instance.
        /// </summary>
        /// <returns>The original value of the current instance.</returns>
        double GetOriginalValue();

        /// <summary>Resets the current instance to the original value and resets the <see cref="IShiftableData.IsShifted"/> flag as well.
        /// </summary>
        void ResetToOriginalValue();

        /// <summary>Adds some absolute and some relative shift to the current value.
        /// </summary>
        /// <param name="relativeShift">The relative shift.</param>
        /// <param name="absoluteShift">The absolute shift.</param>
        /// <remarks>The <see cref="IShiftableData.Value"/> of the current instance after calling is equal to 
        /// <para>
        /// 'previous value' * ( 1.0 + relative shift) + absolute shift.
        /// </para></remarks>
        void AddShift(double relativeShift, double absoluteShift);

        /// <summary>Adds some relative shift to the current value.
        /// </summary>
        /// <param name="relativeShift">The relative shift.</param>
        /// <remarks>The <see cref="IShiftableData.Value"/> of the current instance after calling is equal to 
        /// <para>
        /// 'previous value' * ( 1.0 + relative shift).
        /// </para></remarks>
        void AddShift(double relativeShift);

        /// <summary>Sets <see cref="IShiftableData.Value"/>.
        /// </summary>
        /// <param name="value">The new value.</param>
        void SetValue(double value);
    }
}