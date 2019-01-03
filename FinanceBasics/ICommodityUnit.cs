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

using Dodoni.BasicComponents;

namespace Dodoni.Finance
{
    /// <summary>Serves as interface for a commodity unit, i.e. gallon, barrel, Metric ton etc.
    /// </summary>
    public interface ICommodityUnit : IIdentifierNameable, IAnnotatable
    {
        /// <summary>Gets a converting factor from the unit of the current object into a specified commodity unit.
        /// </summary>
        /// <param name="destinationUnit">The destination commodity unit.</param>
        /// <param name="destinationUnitQuantity">The quantity of the destination unit to convert to units of the current commodity unit.</param>
        /// <param name="convertingFactor">The converting factor, i.e. the number of units of the current commodity unit needed to represents <paramref name="destinationUnit"/>.</param>
        /// <returns>A value indicating whether <paramref name="convertingFactor"/> contains valid data.</returns>
        bool TryGetConvertingFactor(ICommodityUnit destinationUnit, double destinationUnitQuantity, out double convertingFactor);
    }

    /// <summary>Serves as interface for a commodity unit, i.e. gallon, barrel, Metric ton etc.
    /// </summary>
    /// <typeparam name="TDestinationUnit">The type of the destination commodity unit.</typeparam>
    public interface ICommodityUnit<TDestinationUnit> : ICommodityUnit
    {
        /// <summary>Gets a converting factor from the unit of the current object into a specified commodity unit.
        /// </summary>
        /// <param name="destinationUnit">The destination commodity unit.</param>
        /// <param name="destinationUnitQuantity">The quantity of the destination unit to convert to units of the current commodity unit.</param>
        /// <param name="convertingFactor">The converting factor, i.e. the number of units of the current commodity unit needed to represents <paramref name="destinationUnit"/>.</param>
        /// <returns>A value indicating whether <paramref name="convertingFactor"/> contains valid data.</returns>
        bool TryGetConvertingFactor(TDestinationUnit destinationUnit, double destinationUnitQuantity, out double convertingFactor);
    }
}