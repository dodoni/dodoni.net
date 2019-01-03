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
using System.Xml;
using System.Globalization;

using Dodoni.BasicComponents.Containers;

namespace Dodoni.BasicComponents.Utilities
{
    public partial class ConfigurationFile : IDisposable
    {
        /// <summary>Represents a collection of properties in a configuration file, i.e. a specified section in the xml file representation of the configuration file.
        /// </summary>
        public class PropertyCollection : IInfoOutputQueriable, IIdentifierNameable
        {
            #region private members

            /// <summary>The configuration file in its <see cref="ConfigurationFile"/> representation.
            /// </summary>
            private ConfigurationFile m_ConfigurationFile;

            /// <summary>The name of the collection of properties configuration file.
            /// </summary>
            private readonly IdentifierString m_PropertyCollectionName;

            /// <summary>The parent node of the section in the xml file that contains the collection of properties.
            /// </summary>
            private XmlNode m_PropertyCollectionParentNode;

            /// <summary>A value indicating whether the configuration file already contains settings with respect to the current <see cref="ConfigurationFile.PropertyCollection"/> object.
            /// </summary>
            private readonly bool m_ContainsSettings;
            #endregion

            #region internal protected constructors

            /// <summary>Initializes a new instance of the <see cref="PropertyCollection" /> class.
            /// </summary>
            /// <param name="configurationFile">The configuration file.</param>
            /// <param name="propertyCollectionName">The name of the property collection.</param>
            internal protected PropertyCollection(ConfigurationFile configurationFile, IdentifierString propertyCollectionName)
            {
                m_ConfigurationFile = configurationFile;
                m_PropertyCollectionName = propertyCollectionName;

                m_PropertyCollectionParentNode = configurationFile.GetSectionNode(propertyCollectionName.IDString, out m_ContainsSettings);
            }

            /// <summary>Initializes a new instance of the <see cref="PropertyCollection" /> class.
            /// </summary>
            /// <param name="configurationFile">The configuration file.</param>
            /// <param name="propertyCollectionName">The name of the property collection.</param>
            internal protected PropertyCollection(ConfigurationFile configurationFile, string propertyCollectionName)
                : this(configurationFile, IdentifierString.Create(propertyCollectionName))
            {
            }
            #endregion

            #region public properties

            #region IIdentifierNameable Members

            /// <summary>Gets the name of the current instance.
            /// </summary>
            /// <value>The language independent name of the current instance.</value>
            public IdentifierString Name
            {
                get { return m_PropertyCollectionName; }
            }

            /// <summary>Gets the long name of the current instance.
            /// </summary>
            /// <value>The (perhaps) language dependent long name of the current instance.</value>
            public IdentifierString LongName
            {
                get { return m_PropertyCollectionName; }
            }
            #endregion

            #region IInfoOutputQueriable Members

            /// <summary>Gets the info-output level of detail.
            /// </summary>
            /// <value>The info-output level of detail.</value>
            public InfoOutputDetailLevel InfoOutputDetailLevel
            {
                get { return InfoOutputDetailLevel.Full; }
            }
            #endregion

            /// <summary>Gets a value indicating whether the configuration file already contains settings with respect to the current <see cref="ConfigurationFile"/> object; will be set to <c>true</c>
            /// after calling <see cref="Save()"/>.
            /// </summary>
            /// <value><c>true</c> if the configuration file already contains some settings; otherwise, <c>false</c>.</value>
            public bool ContainsSettings
            {
                get { return m_ContainsSettings; }
            }
            #endregion

            #region public methods

            #region IInfoOutputQueriable Members

            /// <summary>Sets the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> property.
            /// </summary>
            /// <param name="infoOutputDetailLevel">The info-output level of detail.</param>
            /// <returns>A value indicating whether the <see cref="IInfoOutputQueriable.InfoOutputDetailLevel" /> has been set to <paramref name="infoOutputDetailLevel" />.
            /// </returns>
            public bool TrySetInfoOutputDetailLevel(InfoOutputDetailLevel infoOutputDetailLevel)
            {
                return (infoOutputDetailLevel == Containers.InfoOutputDetailLevel.Full);
            }

            /// <summary>Gets informations of the current object as a specific <see cref="InfoOutput" /> instance.
            /// </summary>
            /// <param name="infoOutput">The <see cref="InfoOutput" /> object which is to be filled with informations concering the current instance.</param>
            /// <param name="categoryName">The name of the category, i.e. all informations will be added to these category.</param>
            public void FillInfoOutput(InfoOutput infoOutput, string categoryName = InfoOutput.GeneralCategoryName)
            {
                var infoPackage = infoOutput.AcquirePackage(categoryName);

                if (m_PropertyCollectionParentNode.HasChildNodes == true)
                {
                    for (int j = 0; j < m_PropertyCollectionParentNode.ChildNodes.Count; j++)
                    {
                        var node = m_PropertyCollectionParentNode.ChildNodes[j];

                        var InfoProperty = new InfoOutputProperty(node.Name, node.InnerText);
                        infoPackage.Add(InfoProperty);
                    }
                }
            }
            #endregion

            #region setValue methods

            /// <summary>Sets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the XML file.</param>
            /// <param name="value">The value in its <see cref="System.String"/> representation.</param>
            public void SetValue(string key, string value)
            {
                var keyValuePairs = m_PropertyCollectionParentNode.SelectNodes(key);
                if (keyValuePairs.Count == 0)
                {
                    var newConfigNode = m_ConfigurationFile.m_XmlFileRepresenation.CreateElement(key);
                    newConfigNode.AppendChild(m_ConfigurationFile.m_XmlFileRepresenation.CreateTextNode(value));
                    m_PropertyCollectionParentNode.AppendChild(newConfigNode);
                }
                else if (keyValuePairs.Count == 1)
                {
                    keyValuePairs[0].InnerText = value;
                }
                else
                {
                    throw new ConfigurationFileErrorException(String.Format("Configuration file {0} is invalid for key {1}.", m_ConfigurationFile.m_FilePath, key));
                }
            }

            /// <summary>Sets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the XML file.</param>
            /// <param name="value">The value in its <see cref="System.Boolean"/> representation.</param>
            public void SetValue(string key, bool value)
            {
                string boolValueString = (value == true) ? Boolean.TrueString : Boolean.FalseString;
                SetValue(key, boolValueString);
            }

            /// <summary>Sets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the XML file.</param>
            /// <param name="value">The value in its <see cref="System.Enum"/> representation.</param>
            public void SetValue(string key, Enum value)
            {
                SetValue(key, value.ToString());
            }

            /// <summary>Sets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the XML file.</param>
            /// <param name="value">The value in its <see cref="System.Enum"/> representation.</param>
            public void SetValue(string key, double value)
            {
                SetValue(key, value.ToString(CultureInfo.InvariantCulture));
            }

            /// <summary>Sets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the XML file.</param>
            /// <param name="value">The value in its <see cref="System.Enum"/> representation.</param>
            public void SetValue(string key, int value)
            {
                SetValue(key, value.ToString(CultureInfo.InvariantCulture));
            }
            #endregion

            #region getValue methods

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.String"/> representation (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            public bool TryGetValue(string key, out string value)
            {
                var keyValuePairs = m_PropertyCollectionParentNode.SelectNodes(key);
                if (keyValuePairs.Count == 0)
                {
                    value = String.Empty;
                    return false;
                }
                else if (keyValuePairs.Count == 1)
                {
                    value = keyValuePairs[0].InnerText;
                    return true;
                }
                else
                {
                    throw new ConfigurationFileErrorException(String.Format("Configuration file {0} is invalid for key {1}.", m_ConfigurationFile.m_FilePath, key));
                }
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.String"/> representation (output).</param>
            /// <param name="defaultValue">The default value to take into account if the configuration file does not contain an entry for <paramref name="key"/>.</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains some data from the configuration file.</returns>
            public bool TryGetValue(string key, out string value, string defaultValue)
            {
                var keyValuePairs = m_PropertyCollectionParentNode.SelectNodes(key);
                if (keyValuePairs.Count == 0)
                {
                    value = defaultValue;
                    return false;
                }
                else if (keyValuePairs.Count == 1)
                {
                    value = keyValuePairs[0].InnerText;
                    return true;
                }
                else
                {
                    throw new ConfigurationFileErrorException(String.Format("Configuration file {0} is invalid for key {1}.", m_ConfigurationFile.m_FilePath, key));
                }
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.Boolean"/> representation (output).</param>
            /// <param name="defaultValue">The default value to take into account if the configuration file does not contain an entry for <paramref name="key"/>.</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains some data from the configuration file.</returns>
            public bool TryGetValue(string key, out bool value, bool defaultValue = false)
            {
                if (TryGetValue(key, out string tempValue) && Boolean.TryParse(tempValue, out value))
                {
                    return true;
                }
                value = defaultValue;
                return false;
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.Double"/> representation (output).</param>
            /// <param name="defaultValue">The default value to take into account if the configuration file does not contain an entry for <paramref name="key"/>.</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains some data from the configuration file.</returns>
            public bool TryGetValue(string key, out double value, double defaultValue = Double.NaN)
            {
                if (TryGetValue(key, out string tempValue) && Double.TryParse(tempValue, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    return true;
                }
                value = defaultValue;
                return false;
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.Int32"/> representation (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            public bool TryGetValue(string key, out int value)
            {
                if (TryGetValue(key, out string tempValue) && Int32.TryParse(tempValue, out value))
                {
                    return true;
                }
                value = default;
                return false;
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <see cref="System.Int32"/> representation (output).</param>
            /// <param name="defaultValue">The default value to take into account if the configuration file does not contain an entry for <paramref name="key"/>.</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains some data from the configuration file.</returns>
            public bool TryGetValue(string key, out int value, int defaultValue)
            {
                if (TryGetValue(key, out string tempValue) && Int32.TryParse(tempValue, out value))
                {
                    return true;
                }
                value = defaultValue;
                return false;
            }

            /// <summary>Gets a specific entry in the configuration file.
            /// </summary>
            /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
            /// <param name="key">The key, i.e. the key in the xml file.</param>
            /// <param name="value">The value in its <typeparamref name="TEnum"/> representation (output).</param>
            /// <returns>A value indicating whether <paramref name="value"/> contains valid data.</returns>
            public bool TryGetEnumValue<TEnum>(string key, out TEnum value)
                where TEnum : struct
            {
                if (TryGetValue(key, out string tempValue) && Enum.TryParse<TEnum>(tempValue, true, out value))
                {
                    return true;
                }
                value = default;
                return false;
            }
            #endregion

            #endregion
        }
    }
}