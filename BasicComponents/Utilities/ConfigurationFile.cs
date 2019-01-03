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
using System.IO;
using System.Xml;

namespace Dodoni.BasicComponents.Utilities
{
    /// <summary>Represents a configuration file, i.e. a collection of names and values.
    /// </summary>
    public partial class ConfigurationFile : IDisposable
    {
        #region private static members

        /// <summary>The configuration of the Dodoni.net framework.
        /// </summary>
        public static readonly ConfigurationFile Dodoni = Create("Dodoni.net.config", "Dodoni.net");
        #endregion

        #region private members

        /// <summary>The file path of the configuration file used to store the user settings.
        /// </summary>
        private string m_FilePath;

        /// <summary>The configuration file in its <see cref="XmlDocument"/> representation.
        /// </summary>
        private XmlDocument m_XmlFileRepresenation;

        /// <summary>The name of the main node in the xml representation of the configuration file.
        /// </summary>
        private string m_XmlRootNodeName;
        #endregion

        #region protected constructors

        /// <summary>Initializes a new instance of the <see cref="ConfigurationFile"/> class, the path of the assembly will be used as well as the assembly file name. 
        /// </summary>
        /// <param name="configurationFileName">The name of the configuration file.</param>
        /// <param name="configurationFilePath">The path with respect to <paramref name="configurationFileName"/>.</param>
        /// <param name="xmlRootNodeName">The name of the root node in the XML file representation of the configuration file.</param>
        protected ConfigurationFile(string configurationFileName, string configurationFilePath, string xmlRootNodeName = "Dodoni.net")
        {
            try
            {
                m_XmlRootNodeName = xmlRootNodeName;
                m_FilePath = Path.Combine(configurationFilePath, configurationFileName);
                m_XmlFileRepresenation = new XmlDocument();

                if (File.Exists(m_FilePath) == false) // no config file available, create an internal representation
                {
                    XmlNode mainNode = m_XmlFileRepresenation.CreateElement(xmlRootNodeName);
                    m_XmlFileRepresenation.AppendChild(mainNode);
                }
                else
                {
                    XmlReaderSettings xmlSettings = new XmlReaderSettings
                    {
                        ConformanceLevel = ConformanceLevel.Fragment,
                        IgnoreWhitespace = true,
                        IgnoreComments = true,
                        CloseInput = true
                    };
                    var reader = XmlReader.Create(m_FilePath, xmlSettings);
                    m_XmlFileRepresenation.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationFileErrorException(e.Message, e);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ConfigurationFile"/> class, the path of the assembly will be used as well as the assembly file name. 
        /// </summary>
        /// <param name="configurationFileName">The name of the configuration file (with respect to the base directory of the <see cref="AppDomain"/>).</param>
        /// <param name="xmlRootNodeName">The name of the root node in the XML file representation of the configuration file.</param>
        protected ConfigurationFile(string configurationFileName, string xmlRootNodeName = "Dodoni.net")
            : this(configurationFileName, AppDomain.CurrentDomain.BaseDirectory, xmlRootNodeName)
        {
        }
        #endregion

        #region public properties

        /// <summary>Gets the physical path to the configuration file represented by this <see cref="ConfigurationFile"/> file path.
        /// </summary>
        /// <value>The file path of the configuration.</value>
        public string FilePath
        {
            get { return m_FilePath; }
        }
        #endregion

        #region public methods

        #region IDisposable Members

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

        /// <summary>Gets a specified property collection, i.e. a collection of settings grouped by a specified name.
        /// </summary>
        /// <param name="propertyCollectionName">The name of the property collection.</param>
        /// <returns>A perhaps empty collection of properties (i.e. settings) in the configuration file in its <see cref="ConfigurationFile.PropertyCollection"/> representation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="propertyCollectionName"/> is <c>null</c>.</exception>
        public PropertyCollection GetPropertyCollection(string propertyCollectionName)
        {
            if (propertyCollectionName == null)
            {
                throw new ArgumentNullException(nameof(propertyCollectionName));
            }
            return new PropertyCollection(this, IdentifierString.Create(propertyCollectionName));
        }

        /// <summary>Gets a specified property collection, i.e. a collection of settings grouped by a specified name.
        /// </summary>
        /// <typeparam name="T">The type of the property collection.</typeparam>
        /// <param name="propertyCollectionName">The name of the property collection.</param>
        /// <param name="propertyCollectionFactory">A factory for <typeparamref name="T"/> objects, where the first argument is the current instance and the second argument is the <paramref name="propertyCollectionName"/>.</param>
        /// <returns>A perhaps empty collection of properties (i.e. settings) in the configuration file in its <typeparamref name="T"/> representation.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="propertyCollectionName"/> is <c>null</c>.</exception>
        /// <remarks>The argument <paramref name="propertyCollectionFactory"/> is a workaround because C# does not allow to restrict a generic type to a class that contains a specified constructor, only <code>where T: PropertyCollection, new()</code> is possible.</remarks>
        public T GetPropertyCollection<T>(string propertyCollectionName, Func<ConfigurationFile, string, T> propertyCollectionFactory)
            where T : PropertyCollection
        {
            if (propertyCollectionName == null)
            {
                throw new ArgumentNullException(nameof(propertyCollectionName));
            }
            return propertyCollectionFactory(this, propertyCollectionName);
        }

        /// <summary>Gets a specified data table, i. e. a collection of homogeneous data grouped by a specified name.
        /// </summary>
        /// <param name="tableName">The name of the table, i.e. of the section in the XML representation of the configuration file.</param>
        /// <param name="tableEntryName">The name of each data table entry.</param>
        /// <param name="fieldNames">The name of each field of the data table entries.</param>
        /// <returns>A <see cref="Table"/> object that contains homogeneous data.</returns>
        public Table GetTable(string tableName, string tableEntryName, params string[] fieldNames)
        {
            return new Table(this, IdentifierString.Create(tableName), IdentifierString.Create(tableEntryName), fieldNames);
        }

        /// <summary>Gets a specified data table, i.e. a collection of homogenious data grouped by a specified name.
        /// </summary>
        /// <typeparam name="T">The type of the table.</typeparam>
        /// <param name="tableName">The name of the table, i.e. of the section in the XML representation of the configuration file.</param>
        /// <param name="tableFactory">A factory for <typeparamref name="T"/> objects, where the first argument is the current instance and the second argument is the <paramref name="tableName"/>.</param>
        /// <returns>A typed <see cref="Table"/> object as object of type <typeparamref name="T"/> that contains homogeneous data.</returns>
        /// <remarks>The argument <paramref name="tableFactory"/> is a workaround because in C# does not allow to restrict a generic type to a class that contains a specified constructor, only <code>where T: Table, new()</code> is possible.</remarks>
        public T GetTable<T>(string tableName, Func<ConfigurationFile, string, T> tableFactory)
            where T : Table
        {
            return tableFactory(this, tableName);
        }

        /// <summary>Gets a specified section node in the Xml representation of the configuration file in its <see cref="XmlNode"/> representation.
        /// </summary>
        /// <param name="sectionName">The name of the section.</param>
        /// <param name="sectionExists">A value indicating whether a section of the specified name already exists in the internal <see cref="XmlDocument"/> representation.</param>
        /// <returns>A <see cref="XmlNode"/> object that represents the root node of a specified section in the Xml file representation of the configuration file. If no section with
        /// the specified <paramref name="sectionName"/> exists an empty node will be inserted into the internal <see cref="XmlDocument"/> representation and returned.</returns>
        /// <exception cref="ConfigurationFileErrorException">Thrown if the configuration file exists and is in an invalid format.</exception>
        public XmlNode GetSectionNode(string sectionName, out bool sectionExists)
        {
            var sectionNode = m_XmlFileRepresenation.SelectSingleNode(String.Format(@"/{0}/{1}", m_XmlRootNodeName, sectionName));

            if (sectionNode == null)
            {
                sectionExists = false;

                var rootNode = m_XmlFileRepresenation.SelectSingleNode(@"/" + m_XmlRootNodeName);
                if (rootNode == null)
                {
                    throw new ConfigurationFileErrorException(String.Format("No node {0} in configuration file {1} available.", m_XmlRootNodeName, m_FilePath));
                }
                sectionNode = m_XmlFileRepresenation.CreateElement(sectionName);
                rootNode.AppendChild(sectionNode);
            }
            else
            {
                sectionExists = true;
            }
            return sectionNode;
        }

        /// <summary>Gets a specified section node in the Xml representation of the configuration file in its <see cref="XmlNode"/> representation.
        /// </summary>
        /// <param name="sectionName">The name of the section.</param>
        /// <returns>A <see cref="XmlNode"/> object that represents the root node of a specified section in the Xml file representation of the configuration file. If no section with
        /// the specified <paramref name="sectionName"/> exists an empty node will be inserted into the internal <see cref="XmlDocument"/> representation and returned.</returns>
        /// <exception cref="ConfigurationFileErrorException">Thrown if the configuration file exists and is in an invalid format.</exception>
        public XmlNode GetSectionNode(string sectionName)
        {
            var sectionNode = m_XmlFileRepresenation.SelectSingleNode(String.Format(@"/{0}/{1}", m_XmlRootNodeName, sectionName));

            if (sectionNode == null)
            {
                var rootNode = m_XmlFileRepresenation.SelectSingleNode(@"/" + m_XmlRootNodeName);
                if (rootNode == null)
                {
                    throw new ConfigurationFileErrorException(String.Format("No node {0} in configuration file {1} available.", m_XmlRootNodeName, m_FilePath));
                }
                sectionNode = m_XmlFileRepresenation.CreateElement(sectionName);
                rootNode.AppendChild(sectionNode);
            }
            return sectionNode;
        }

        /// <summary>Writes the configuration settings into the configuration file.
        /// </summary>
        /// <exception cref="ConfigurationFileErrorException">Thrown if some error occured while saving the settings.</exception>
        public void Save()
        {
            try
            {
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = false,
                    Indent = true
                };
                var writer = XmlWriter.Create(m_FilePath, settings);

                m_XmlFileRepresenation.WriteTo(writer);
                writer.Close();
            }
            catch (Exception e)
            {
                throw new ConfigurationFileErrorException(String.Format("An error occured while writing the configuration settings into file {0}.", m_FilePath), e);
            }
        }
        #endregion

        #region public static methods

        /// <summary>Creates a new <see cref="ConfigurationFile"/> object.
        /// </summary>
        /// <param name="configurationFileName">The name of the configuration file (the path will be ignored).</param>
        /// <param name="xmlRootNodeName">The name of the root node in the XML file representation of the configuration file.</param>
        /// <returns>A new <see cref="ConfigurationFile"/> object.</returns>
        public static ConfigurationFile Create(string configurationFileName, string xmlRootNodeName)
        {
            return new ConfigurationFile(configurationFileName, xmlRootNodeName);
        }

        /// <summary>Creates a new <see cref="ConfigurationFile"/> object.
        /// </summary>
        /// <param name="configurationFileName">The name of the configuration file.</param>
        /// <param name="xmlRootNodeName">The name of the root node in the XML file representation of the configuration file.</param>
        /// <param name="configurationFilePath">The path with respect to <paramref name="configurationFileName"/>.</param>
        /// <returns>A new <see cref="ConfigurationFile"/> object.</returns>
        public static ConfigurationFile Create(string configurationFileName, string xmlRootNodeName, string configurationFilePath)
        {
            return new ConfigurationFile(configurationFileName, configurationFilePath, xmlRootNodeName);
        }
        #endregion
    }
}