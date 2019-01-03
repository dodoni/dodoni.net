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

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>A sample enumeration containing several weekdays, no attributes are used.
    /// </summary>
    internal enum Weekdays
    {
        Sunday,
        Monday,
        Tuesday
    }

    /// <summary>A sample enumeration containing several weekdays, <see cref="StringAttribute"/> annotations are used.
    /// </summary>
    internal enum StringAttributeWeekdays
    {
        [String("Sonntag")]
        Sunday,

        [String("Montag")]
        Monday,

        [String("Dienstag")]
        Tuesday
    }

    /// <summary>A sample enumeration containing several weekdays, <see cref="LanguageStringAttribute"/> annotations are used.
    /// </summary>
    [LanguageResource("Dodoni.BasicComponents.Utilities.EnumTestResources")]
    internal enum LanguageAttributeWeekdays
    {
        [LanguageString("Sunday")]
        Sunday,

        [LanguageString("Monday")]
        Monday,

        [LanguageString("Tuesday")]
        Tuesday
    }


    /// <summary>A sample enumertion containing several weekdays, <see cref="StringAttribute"/> and <see cref="LanguageStringAttribute"/> annotations are used.
    /// </summary>
    [LanguageResource("Dodoni.BasicComponents.Utilities.EnumTestResources")]
    internal enum StringAndLanguageAttributeWeekdays
    {
        [LanguageString("Sunday")]
        [String("Sonntag")]
        Sunday,

        [String("Montag")]
        [LanguageString("Monday")]
        Monday,

        [LanguageString("Tuesday")]
        [String("Dienstag")]
        Tuesday
    }

    /// <summary>A sample enumertion containing several weekdays, <see cref="StringAttribute"/> and <see cref="LanguageStringAttribute"/> annotations are used.
    /// </summary>
    [LanguageResource("Dodoni.BasicComponents.Utilities.EnumTestResources")]
    internal enum PartialAttributeWeekdays
    {
        /// <summary>The 'Sunday', no annotation.
        /// </summary>
        Sunday,

        /// <summary>The 'Monday', with <see cref="StringAttribute"/> and <see cref="LanguageStringAttribute"/> annotations.
        /// </summary>
        [String("Montag")]
        [LanguageString("Monday")]
        [LanguageDescription("MondayDescription")]
        [Description("This attribute should be ignored, 'LanguageDescription' will be prefered.")]
        Monday,

        /// <summary>The 'Tuesday', with <see cref="StringAttribute"/> and <see cref="DescriptionAttribute"/> annotation.
        /// </summary>
        [String("Dienstag")]
        [Description("A simple description")]
        Tuesday
    }

    /// <summary>A sample enumeration (bit-field) representing a specific file access, no further attributes are used.
    /// </summary>
    [Flags]
    internal enum FileAccess
    {
        Read = 1,
        Write = 2,
        Delete = 4,
        ReadWrite = Read | Write
    }

    /// <summary>A sample enumeration (bit-field) representing a specific file access, <see cref="StringAttribute"/> annotations are used.
    /// </summary>
    [Flags]
    internal enum StringAttributeFileAccess
    {
        [String("Lesen")]
        Read = 1,

        [String("Schreiben")]
        Write = 2,

        [String("Löschen")]
        Delete = 4,

        [String("Lesen oder schreiben")]
        ReadWrite = Read | Write
    }

    /// <summary>A sample enumeration (bit-field) representing a specific file access, <see cref="LanguageStringAttribute"/> annotations are used.
    /// </summary>
    [LanguageResource("Dodoni.BasicComponents.Utilities.EnumTestResources")]
    [Flags]
    internal enum LanguageAttributeFileAccess
    {
        [LanguageString("Read")]
        Read = 1,

        [LanguageString("Write")]
        Write = 2,

        [LanguageString("Delete")]
        Delete = 4,

        [LanguageString("ReadWrite")]
        ReadWrite = Read | Write
    }


    /// <summary>A sample enumeration (bit-field) representing a specific file access,  <see cref="StringAttribute"/> and <see cref="LanguageStringAttribute"/> annotations are used.
    /// </summary>
    [LanguageResource("Dodoni.BasicComponents.Utilities.EnumTestResources")]
    [Flags]
    internal enum StringAndLanguageAttributeFileAccess
    {
        [String("Lesen")]
        [LanguageString("Read")]
        Read = 1,

        [LanguageString("Write")]
        [String("Schreiben")]
        Write = 2,

        [LanguageString("Delete")]
        [String("Löschen")]
        Delete = 4,

        [LanguageString("ReadWrite")]
        [String("Lesen oder schreiben")]
        ReadWrite = Read | Write
    }
}