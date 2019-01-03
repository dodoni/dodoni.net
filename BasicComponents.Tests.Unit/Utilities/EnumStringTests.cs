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
    /// <summary>A unit test class for <see cref="EnumString"/>.
    /// </summary>
    [TestFixture]
    public class EnumStringTests
    {
        /// <summary>Prepares the unit tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EnumString.FlagsEnumSeparatorChar = ',';
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.Parse(Type, string, EnumStringRepresentationUsage)"/> with respect
        /// to the example enumeration <see cref="FileAccess"/>.
        /// </summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the enumeration.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        [TestCase("Read", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = FileAccess.ReadWrite | FileAccess.Delete)]
        [TestCase("Read", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = FileAccess.ReadWrite | FileAccess.Delete)]
        [TestCase("Read", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = FileAccess.ReadWrite | FileAccess.Delete)]
        public Enum Parse_FileAccessEnumName_ValidEnumValue(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage)
        {
            return EnumString.Parse(typeof(FileAccess), stringRepresentation, enumStringRepresentationUsage);
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.Parse(Type, string, EnumStringRepresentationUsage)"/> with respect
        /// to the example enumeration <see cref="StringAttributeFileAccess"/>.
        /// </summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the enumeration.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        [TestCase("Read", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAttributeFileAccess.ReadWrite | StringAttributeFileAccess.Delete)]
        [TestCase("Lesen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.Read)]
        [TestCase("Schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.Write)]
        [TestCase("Löschen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.Delete)]
        [TestCase("Lesen oder schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Lesen, Schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Schreiben,Löschen,Lesen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite | StringAttributeFileAccess.Delete)]
        [TestCase("Lesen", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.Read)]
        [TestCase("Schreiben", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.Write)]
        [TestCase("Löschen", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.Delete)]
        [TestCase("Lesen oder schreiben", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Lesen, schreiben", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite)]
        [TestCase("Lesen,löschen,schreiben", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAttributeFileAccess.ReadWrite | StringAttributeFileAccess.Delete)]
        public Enum Parse_StringAttributeFileAccessEnumName_ValidEnumValue(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage)
        {
            return EnumString.Parse(typeof(StringAttributeFileAccess), stringRepresentation, enumStringRepresentationUsage);
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.Parse(Type, string, EnumStringRepresentationUsage)"/> with respect
        /// to the example enumeration <see cref="LanguageAttributeFileAccess"/>.
        /// </summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the enumeration.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        [TestCase("Read", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = LanguageAttributeFileAccess.ReadWrite | LanguageAttributeFileAccess.Delete)]
        [TestCase("Read", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Write,delete,read", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite | LanguageAttributeFileAccess.Delete)]
        [TestCase("ResourceRead", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.Read)]
        [TestCase("ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.Write)]
        [TestCase("ResourceDelete", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.Delete)]
        [TestCase("ResourceReadwrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("ResourceRead, ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite)]
        [TestCase("ResourceRead, ResourceDelete, ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = LanguageAttributeFileAccess.ReadWrite | LanguageAttributeFileAccess.Delete)]
        public Enum Parse_LanguageAttributeFileAccessEnumName_ValidEnumValue(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage)
        {
            return EnumString.Parse(typeof(LanguageAttributeFileAccess), stringRepresentation, enumStringRepresentationUsage);
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.Parse(Type, string, EnumStringRepresentationUsage)"/> with respect
        /// to the example enumeration <see cref="StringAndLanguageAttributeFileAccess"/>.
        /// </summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the enumeration.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        [TestCase("Read", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.Read)]
        [TestCase("Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.Write)]
        [TestCase("Delete", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.Delete)]
        [TestCase("ReadWrite", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Read, Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Read,Delete,Write", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite | StringAndLanguageAttributeFileAccess.Delete)]
        [TestCase("Lesen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Read)]
        [TestCase("Schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Write)]
        [TestCase("Löschen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Delete)]
        [TestCase("Lesen oder schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Lesen, schreiben", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("Schreiben, löschen, lesen", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite | StringAndLanguageAttributeFileAccess.Delete)]
        [TestCase("ResourceRead", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Read)]
        [TestCase("ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Write)]
        [TestCase("ResourceDelete", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.Delete)]
        [TestCase("ResourceReadwrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("ResourceRead, ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite)]
        [TestCase("ResourceRead, ResourceDelete, ResourceWrite", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = StringAndLanguageAttributeFileAccess.ReadWrite | StringAndLanguageAttributeFileAccess.Delete)]
        public Enum Parse_StringAndLanguageAttributeFileAccessEnumName_ValidEnumValue(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage)
        {
            return EnumString.Parse(typeof(StringAndLanguageAttributeFileAccess), stringRepresentation, enumStringRepresentationUsage);
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.Parse(Type, string, EnumStringRepresentationUsage)"/> with respect
        /// to the example enumeration <see cref="PartialAttributeWeekdays"/>.
        /// </summary>
        /// <param name="stringRepresentation">The <see cref="System.String"/> representation to search for.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of the items of the enumeration.</param>
        /// <returns>The item of the enumeration corresponds to <paramref name="stringRepresentation"/>.</returns>
        [TestCase("Sunday", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = PartialAttributeWeekdays.Sunday)]
        [TestCase("Monday", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = PartialAttributeWeekdays.Monday)]
        [TestCase("Tuesday", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = PartialAttributeWeekdays.Tuesday)]
        [TestCase("Sunday, Monday", EnumStringRepresentationUsage.ToStringMethod, ExpectedResult = PartialAttributeWeekdays.Sunday | PartialAttributeWeekdays.Monday)]
        [TestCase("Sunday", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = PartialAttributeWeekdays.Sunday)]
        [TestCase("Montag", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = PartialAttributeWeekdays.Monday)]
        [TestCase("Dienstag", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = PartialAttributeWeekdays.Tuesday)]
        [TestCase("Sunday, Montag", EnumStringRepresentationUsage.StringAttribute, ExpectedResult = PartialAttributeWeekdays.Sunday | PartialAttributeWeekdays.Monday)]
        [TestCase("Sunday", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = PartialAttributeWeekdays.Sunday)]
        [TestCase("ResourceMonday", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = PartialAttributeWeekdays.Monday)]
        [TestCase("Dienstag", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = PartialAttributeWeekdays.Tuesday)]
        [TestCase("Sunday, ResourceMonday", EnumStringRepresentationUsage.LanguageStringAttribute, ExpectedResult = PartialAttributeWeekdays.Sunday | PartialAttributeWeekdays.Monday)]
        public Enum Parse_PartialAttributeWeekdaysEnumName_ValidEnumValue(string stringRepresentation, EnumStringRepresentationUsage enumStringRepresentationUsage)
        {
            return EnumString.Parse(typeof(PartialAttributeWeekdays), stringRepresentation, enumStringRepresentationUsage);
        }

        /// <summary>A test function that compares the result of <see cref="EnumString.CompareTo(EnumString)"/> with respect to 
        /// the example enumeration <see cref="StringAndLanguageAttributeFileAccess"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        /// <param name="enumStringRepresentationUsage">The method how to compute the string representation of <paramref name="value"/>.</param>
        /// <param name="enumStringRepresentationUsageOfValueToCompare">The method how to compute the string representation of <paramref name="valueToCompare"/>.</param>
        [Test]
        public void CompareTo_StringAndLanguageAttributeFileAccessEnum_RelationOfUnderlyingEnumValues(
            [Values(StringAndLanguageAttributeFileAccess.Delete, StringAndLanguageAttributeFileAccess.Read, StringAndLanguageAttributeFileAccess.ReadWrite, StringAndLanguageAttributeFileAccess.Write)]
            Enum value,

            [Values(StringAndLanguageAttributeFileAccess.Delete, StringAndLanguageAttributeFileAccess.Read, StringAndLanguageAttributeFileAccess.ReadWrite, StringAndLanguageAttributeFileAccess.Write)]
            Enum valueToCompare,

            [Values(EnumStringRepresentationUsage.ToStringMethod, EnumStringRepresentationUsage.StringAttribute, EnumStringRepresentationUsage.LanguageStringAttribute)]
            EnumStringRepresentationUsage enumStringRepresentationUsage,

            [Values(EnumStringRepresentationUsage.ToStringMethod, EnumStringRepresentationUsage.StringAttribute, EnumStringRepresentationUsage.LanguageStringAttribute)]
            EnumStringRepresentationUsage enumStringRepresentationUsageOfValueToCompare)
        {
            EnumString enumString = EnumString.Create(value, enumStringRepresentationUsage);
            EnumString other = EnumString.Create(valueToCompare, enumStringRepresentationUsageOfValueToCompare);

            Assert.AreEqual(value.CompareTo(valueToCompare), enumString.CompareTo(other));
        }
    }
}