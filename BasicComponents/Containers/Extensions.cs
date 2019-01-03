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
using System.Data;

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.BasicComponents.Containers
{
    /// <summary>Provides extension methods for classes of the namespace <c>Dodoni.BasicComponents.Containers</c>.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static class Extensions
    {
        /// <summary>Determines whether the current <see cref="InfoOutputDetailLevel"/> object is at least as comprehensive as an other <see cref="InfoOutputDetailLevel"/>.
        /// </summary>
        /// <param name="infoOutputDetailLevel">The info output detail level.</param>
        /// <param name="otherInfoOutputDetailLevel">The other info output detail level.</param>
        /// <returns><c>true</c> if the current <see cref="InfoOutputDetailLevel"/> object is at least as comprehensive as the other specified <see cref="InfoOutputDetailLevel"/>; otherwise, <c>false</c>.</returns>
        public static bool IsAtLeastAsComprehensiveAs(this InfoOutputDetailLevel infoOutputDetailLevel, InfoOutputDetailLevel otherInfoOutputDetailLevel)
        {
            return (infoOutputDetailLevel >= otherInfoOutputDetailLevel);
        }
    }
}