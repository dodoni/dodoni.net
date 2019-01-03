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

using NUnit.Framework;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>A unit test class for <see cref="EnumExtensions"/>.
    /// </summary>
    [TestFixture]
    public class EnumExtensionsTests
    {
        /// <summary>Prepares the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EnumString.FlagsEnumSeparatorChar = ',';
        }

        /// <summary>A test function that compares the result of <see cref="EnumExtensions.ToFormatString(Enum, EnumStringRepresentationUsage)"/> with respect to <see cref="EnumStringRepresentationUsage.ToStringMethod"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.String"/> representation of <paramref name="value"/>.</returns>
        [TestCase(Weekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(Weekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(Weekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(StringAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(StringAttributeWeekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(StringAttributeWeekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(LanguageAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(LanguageAttributeWeekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(LanguageAttributeWeekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(PartialAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(PartialAttributeWeekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(PartialAttributeWeekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(FileAccess.Read, ExpectedResult = "Read")]
        [TestCase(FileAccess.Write, ExpectedResult = "Write")]
        [TestCase(FileAccess.Delete, ExpectedResult = "Delete")]
        [TestCase(FileAccess.Read | FileAccess.Write | FileAccess.Delete, ExpectedResult = "ReadWrite, Delete")]
        [TestCase(FileAccess.ReadWrite, ExpectedResult = "ReadWrite")]
        [TestCase(StringAttributeFileAccess.Read, ExpectedResult = "Read")]
        [TestCase(StringAttributeFileAccess.Write, ExpectedResult = "Write")]
        [TestCase(StringAttributeFileAccess.Delete, ExpectedResult = "Delete")]
        [TestCase(StringAttributeFileAccess.Read | StringAttributeFileAccess.Write | StringAttributeFileAccess.Delete, ExpectedResult = "ReadWrite, Delete")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "ReadWrite")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read, ExpectedResult = "Read")]
        [TestCase(StringAndLanguageAttributeFileAccess.Write, ExpectedResult = "Write")]
        [TestCase(StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "Delete")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read | StringAndLanguageAttributeFileAccess.Write | StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "ReadWrite, Delete")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "ReadWrite")]
        public string ToFormatString_EnumValue_PlainStringRepresentation(Enum value)
        {
            return value.ToFormatString(EnumStringRepresentationUsage.ToStringMethod);
        }

        /// <summary>A test function that compares the result of <see cref="EnumExtensions.ToFormatString(Enum, EnumStringRepresentationUsage)"/> with respect to <see cref="EnumStringRepresentationUsage.StringAttribute"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.String"/> representation of <paramref name="value"/>.</returns>
        [TestCase(Weekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(Weekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(Weekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(StringAttributeWeekdays.Sunday, ExpectedResult = "Sonntag")]
        [TestCase(StringAttributeWeekdays.Monday, ExpectedResult = "Montag")]
        [TestCase(StringAttributeWeekdays.Tuesday, ExpectedResult = "Dienstag")]
        [TestCase(LanguageAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(LanguageAttributeWeekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(LanguageAttributeWeekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Sunday, ExpectedResult = "Sonntag")]
        [TestCase(StringAndLanguageAttributeWeekdays.Monday, ExpectedResult = "Montag")]
        [TestCase(StringAndLanguageAttributeWeekdays.Tuesday, ExpectedResult = "Dienstag")]
        [TestCase(PartialAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(PartialAttributeWeekdays.Monday, ExpectedResult = "Montag")]
        [TestCase(PartialAttributeWeekdays.Tuesday, ExpectedResult = "Dienstag")]
        [TestCase(FileAccess.Read, ExpectedResult = "Read")]
        [TestCase(FileAccess.Write, ExpectedResult = "Write")]
        [TestCase(FileAccess.ReadWrite, ExpectedResult = "ReadWrite")]
        [TestCase(StringAttributeFileAccess.Read, ExpectedResult = "Lesen")]
        [TestCase(StringAttributeFileAccess.Write, ExpectedResult = "Schreiben")]
        [TestCase(StringAttributeFileAccess.Delete, ExpectedResult = "Löschen")]
        [TestCase(StringAttributeFileAccess.Read | StringAttributeFileAccess.Write | StringAttributeFileAccess.Delete, ExpectedResult = "Lesen oder schreiben,Löschen")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "Lesen oder schreiben")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read, ExpectedResult = "Lesen")]
        [TestCase(StringAndLanguageAttributeFileAccess.Write, ExpectedResult = "Schreiben")]
        [TestCase(StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "Löschen")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read | StringAndLanguageAttributeFileAccess.Write | StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "Lesen oder schreiben,Löschen")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "Lesen oder schreiben")]
        public string ToFormatString_EnumValue_StringAttribute(Enum value)
        {
            return value.ToFormatString(EnumStringRepresentationUsage.StringAttribute);
        }

        /// <summary>A test function that compares the result of <see cref="EnumExtensions.ToFormatString(Enum, EnumStringRepresentationUsage)"/> with respect to <see cref="EnumStringRepresentationUsage.LanguageStringAttribute"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.String"/> representation of <paramref name="value"/>.</returns>
        [TestCase(Weekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(Weekdays.Monday, ExpectedResult = "Monday")]
        [TestCase(Weekdays.Tuesday, ExpectedResult = "Tuesday")]
        [TestCase(StringAttributeWeekdays.Sunday, ExpectedResult = "Sonntag")]
        [TestCase(StringAttributeWeekdays.Monday, ExpectedResult = "Montag")]
        [TestCase(StringAttributeWeekdays.Tuesday, ExpectedResult = "Dienstag")]
        [TestCase(LanguageAttributeWeekdays.Sunday, ExpectedResult = "ResourceSunday")]
        [TestCase(LanguageAttributeWeekdays.Monday, ExpectedResult = "ResourceMonday")]
        [TestCase(LanguageAttributeWeekdays.Tuesday, ExpectedResult = "ResourceTuesday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Sunday, ExpectedResult = "ResourceSunday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Monday, ExpectedResult = "ResourceMonday")]
        [TestCase(StringAndLanguageAttributeWeekdays.Tuesday, ExpectedResult = "ResourceTuesday")]
        [TestCase(PartialAttributeWeekdays.Sunday, ExpectedResult = "Sunday")]
        [TestCase(PartialAttributeWeekdays.Monday, ExpectedResult = "ResourceMonday")]
        [TestCase(PartialAttributeWeekdays.Tuesday, ExpectedResult = "Dienstag")]
        [TestCase(FileAccess.Read, ExpectedResult = "Read")]
        [TestCase(FileAccess.Write, ExpectedResult = "Write")]
        [TestCase(FileAccess.ReadWrite, ExpectedResult = "ReadWrite")]
        [TestCase(StringAttributeFileAccess.Read, ExpectedResult = "Lesen")]
        [TestCase(StringAttributeFileAccess.Write, ExpectedResult = "Schreiben")]
        [TestCase(StringAttributeFileAccess.ReadWrite, ExpectedResult = "Lesen oder schreiben")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "ResourceReadWrite")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read, ExpectedResult = "ResourceRead")]
        [TestCase(StringAndLanguageAttributeFileAccess.Write, ExpectedResult = "ResourceWrite")]
        [TestCase(StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "ResourceDelete")]
        [TestCase(StringAndLanguageAttributeFileAccess.Read | StringAndLanguageAttributeFileAccess.Write | StringAndLanguageAttributeFileAccess.Delete, ExpectedResult = "ResourceReadWrite,ResourceDelete")]
        [TestCase(StringAndLanguageAttributeFileAccess.ReadWrite, ExpectedResult = "ResourceReadWrite")]
        public string ToFormatString_EnumValue_LanguageStringAttribute(Enum value)
        {
            return value.ToFormatString(EnumStringRepresentationUsage.LanguageStringAttribute);
        }

        /// <summary>A test function that compares the result of <see cref="EnumExtensions.GetDescription(Enum)"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The descrption of <paramref name="value"/> in its <see cref="System.String"/> representation; perhaps <see cref="String.Empty"/>.</returns>
        [TestCase(PartialAttributeWeekdays.Tuesday, ExpectedResult = "A simple description")]
        [TestCase(PartialAttributeWeekdays.Monday, ExpectedResult = "ResourceDescription")]
        [TestCase(Weekdays.Sunday, ExpectedResult = "")]
        public string GetDescription_EnumValue_Description(Enum value)
        {
            return value.GetDescription();
        }
    }
}