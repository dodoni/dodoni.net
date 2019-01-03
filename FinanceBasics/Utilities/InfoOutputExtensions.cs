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

using Dodoni.BasicComponents.Containers;

namespace Dodoni.Finance.Utilities
{
    /// <summary>Extension methods for <see cref="IInfoOutputQueriable"/> objects.
    /// </summary>
    public static class InfoOutputExtensions
    {
        /// <summary>Adds the property 'State'.
        /// </summary>
        /// <param name="infoOutputCollection">The <see cref="InfoOutputPackage"/> object.</param>
        /// <param name="state">The state.</param>
        public static void Add(this InfoOutputPackage infoOutputCollection, InfoOutputState state)
        {
            infoOutputCollection.Add("State", state, Resources.State);
        }

        /// <summary>Adds the property 'Execution time'.
        /// </summary>
        /// <param name="infoOutputCollection">The <see cref="InfoOutputPackage"/> object.</param>
        /// <param name="executionTime">The exection time in milliseconds.</param>
        public static void AddExecutionTime(this InfoOutputPackage infoOutputCollection, long executionTime)
        {
            infoOutputCollection.Add("Execution time", executionTime, Resources.ExecutionTime);
        }
    }
}