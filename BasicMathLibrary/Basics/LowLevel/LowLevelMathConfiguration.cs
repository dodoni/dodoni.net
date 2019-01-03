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
using System.Linq;
using System.Reflection;
using System.Composition;
using System.Composition.Hosting;
using System.Collections.Generic;

using System.Composition.Convention;

using Dodoni.BasicComponents.Utilities;

namespace Dodoni.MathLibrary.Basics.LowLevel
{
    /// <summary>Represents the configuration for the dll-Wrapper of the low level mathematical functions, i.e. the kind of interfaces for BLAS, FFT etc.
    /// </summary>
    /// <remarks>A restart is necessary if some config file changed.</remarks>
    public static partial class LowLevelMathConfiguration
    {
        #region nested classes

        /// <summary>Serves as loader for a specific library with respect to Managed Extensibility Framework and the configuration file.
        /// </summary>
        /// <typeparam name="T">The type of the library to load.</typeparam>
        internal class LibraryLoader<T>
        {
            /// <summary>Gets the reference to the Library in its <typeparamref name="T"/> representation.
            /// </summary>
            /// <value>The Library.</value>
            [Import]
            internal T Value { get; private set; }

            /// <summary>Initializes a new instance of the <see cref="LibraryLoader{T}" /> class.
            /// </summary>
            /// <param name="xmlLibraryName">The name of the library to load in the configuration file.</param>
            internal LibraryLoader(string xmlLibraryName)
            {
                var libraryConfig = ConfigurationFile.Dodoni.GetSectionNode("MathLibraries", out bool configExists);

                if (configExists == true)
                {
                    var libraryXmlNode = libraryConfig.SelectNodes(xmlLibraryName);
                    if (libraryXmlNode.Count == 1)
                    {
                        var filePathXmlNodeList = libraryXmlNode[0].SelectNodes("FilePath");

                        if ((filePathXmlNodeList.Count == 1) && (filePathXmlNodeList[0].InnerText != String.Empty))
                        {
                            string filePath = filePathXmlNodeList[0].InnerText;

                            if (File.Exists(filePath) == false)
                            {
                                throw new ConfigurationFileErrorException(String.Format("Corrupt configuration file detected. Wrong file path for {0}.", xmlLibraryName));
                            }

                            var assembly = Assembly.LoadFile(filePath);
                            var configuration = new ContainerConfiguration().WithAssembly(assembly);
                            var container = configuration.CreateContainer();
                            Value = container.GetExport<T>();
                        }
                        else
                        {
                            var fullyQualifiedClassName = libraryXmlNode[0].SelectNodes("TypeName")[0].InnerText;
                            var fullyQualifiedAssemblyName = libraryXmlNode[0].SelectNodes("AssemblyName")[0].InnerText;

                            var assembly = Assembly.LoadFile(Path.GetFullPath(String.Format(@".\{0}.dll", fullyQualifiedAssemblyName.Split(',')[1].Trim())));

                            var configuration = new ContainerConfiguration();  // todo: check whether MEF(2) provides simplifications in .net 3.0?
                            configuration.WithAssembly(assembly);
                            var container = configuration.CreateContainer();

                            Value = container.GetExports<T>().Where(x => x.GetType().FullName == fullyQualifiedClassName).SingleOrDefault();
                        }
                    }
                }
                else
                {
                    Value = default(T);
                }
            }
        }
        #endregion

        #region public static methods

        /// <summary>Writes configuration parameters into the config file.
        /// </summary>
        /// <exception cref="ConfigurationFileErrorException">Thrown, if the configuration is invalid and it is not allowed to write the configuration into some file.</exception>
        public static void WriteConfigFile()
        {
            ConfigurationFile.Dodoni.Save();
        }
        #endregion

        #region internal static methods

        /// <summary>Stores the configuration of a specified Library in the configuration file.
        /// </summary>
        /// <param name="libraryName">The name of the library (i.e. in the Xml representation of the configuration file).</param>
        /// <param name="filePath">The file path of the assembly that contains the Library.</param>
        internal static void StoreLibraryConfiguration(string libraryName, string filePath)
        {
            var libraryConfig = ConfigurationFile.Dodoni.GetSectionNode("MathLibraries");

            var specificLibraryXmlNodes = libraryConfig.SelectNodes(libraryName);
            XmlNode specificLibraryXmlNode;

            if (specificLibraryXmlNodes.Count == 0)
            {
                specificLibraryXmlNode = libraryConfig.OwnerDocument.CreateElement(libraryName);
                libraryConfig.AppendChild(specificLibraryXmlNode);
            }
            else if (specificLibraryXmlNodes.Count == 1)
            {
                specificLibraryXmlNode = specificLibraryXmlNodes[0];
                specificLibraryXmlNode.RemoveAll();
            }
            else
            {
                throw new ConfigurationFileErrorException(String.Format("Corrupt configuration file detected"));
            }

            var filePathXmlNode = specificLibraryXmlNode.OwnerDocument.CreateElement("FilePath");
            filePathXmlNode.InnerText = filePath;

            specificLibraryXmlNode.AppendChild(filePathXmlNode);

            ConfigurationFile.Dodoni.Save();
        }

        /// <summary>Stores the configuration of a specified Library in the configuration file.
        /// </summary>
        /// <param name="libraryName">The name of the library (i.e. in the Xml representation of the configuration file).</param>
        /// <param name="libraryType">The type of the library.</param>
        internal static void StoreLibraryConfiguration(string libraryName, Type libraryType)
        {
            var libraryConfig = ConfigurationFile.Dodoni.GetSectionNode("MathLibraries");

            var specificLibraryXmlNodes = libraryConfig.SelectNodes(libraryName);
            XmlNode specificLibraryXmlNode;

            if (specificLibraryXmlNodes.Count == 0)
            {
                specificLibraryXmlNode = libraryConfig.OwnerDocument.CreateElement(libraryName);
                libraryConfig.AppendChild(specificLibraryXmlNode);
            }
            else if (specificLibraryXmlNodes.Count == 1)
            {
                specificLibraryXmlNode = specificLibraryXmlNodes[0];
                specificLibraryXmlNode.RemoveAll();
            }
            else
            {
                throw new ConfigurationFileErrorException(String.Format("Corrupt configuration file detected"));
            }
            var libraryTypeNameXmlNode = specificLibraryXmlNode.OwnerDocument.CreateElement("TypeName");
            var libraryAssemblyNameXmlNode = specificLibraryXmlNode.OwnerDocument.CreateElement("AssemblyName");

            libraryTypeNameXmlNode.InnerText = libraryType.FullName;
            libraryAssemblyNameXmlNode.InnerText = libraryType.AssemblyQualifiedName;

            specificLibraryXmlNode.AppendChild(libraryAssemblyNameXmlNode);
            specificLibraryXmlNode.AppendChild(libraryTypeNameXmlNode);
        }
        #endregion
    }
}