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
    /// <summary>Represents the level of detail for information output, i.e. taken into account for creation of 
    /// <see cref="InfoOutput"/> instances via <see cref="IInfoOutputQueriable.FillInfoOutput(InfoOutput,string)"/>.
    /// </summary>
    /// <remarks>The level of info-output detail is used for example in the case of calibration, pricing etc. and indicates how 
    /// many informations will be exported via <see cref="IInfoOutputQueriable.FillInfoOutput(InfoOutput,string)"/> after
    /// such a calculation. 
    /// <para>Each implementation may interpret the level of detail in a different way.</para></remarks>
    [LanguageResource("Dodoni.BasicComponents.Containers.Resources")]
    public enum InfoOutputDetailLevel
    {
        /// <summary>No info-output, i.e. <see cref="IInfoOutputQueriable.FillInfoOutput(InfoOutput,string)"/>  returns <c>false</c> in any case.
        /// </summary>
        [LanguageString("InfoOutputDetailLevelNone")]
        NoInfoOutput = 0,

        /// <summary>Generate info-output with a low level of detail only.
        /// </summary>
        /// <remarks>In this case property values will be returned, but inhomogeneous data given
        /// as <see cref="DataTable"/> objects will not be part of the info-output.</remarks>
        [LanguageString("InfoOutputDetailLevelLow")]
        Low = 1,

        /// <summary>Generate info-output with a middle level of detail only.
        /// </summary>
        [LanguageString("InfoOutputDetailLevelMiddle")]
        Middle = 2,

        /// <summary>Generate info-output with a high level of detail only.
        /// </summary>
        [LanguageString("InfoOutputDetailLevelHigh")]
        High = 3,

        /// <summary>Generate info-output with all available informations.
        /// </summary>
        [LanguageString("InfoOutputDetailLevelFull")]
        Full = 4
    }
}