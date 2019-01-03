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
using System.Text;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Dodoni.BasicComponents;
using Dodoni.BasicComponents.Containers;

using Dodoni.XLBasicComponents.IO;

namespace Dodoni.XLBasicComponents.Utilities
{
    /// <summary>Represents a Windows form for a tool to manage the <see cref="ExcelPool"/>.
    /// </summary>
    internal partial class ExcelPoolInspectorForm : Form
    {
        #region private members

        /// <summary>The 'general' user control that contains the name, type, logfile etc. of a specific <see cref="ExcelPoolItem"/>.
        /// </summary>
        private ExcelPoolInspectorGeneralUserControl m_GeneralUserControl = new ExcelPoolInspectorGeneralUserControl();

        /// <summary>A user control that contains input data, i.e. data of a specific <see cref="GuidedExcelDataQuery"/> object.
        /// </summary>
        private ExcelPoolInspectorInputUserControl m_InputUserControl = new ExcelPoolInspectorInputUserControl();

        /// <summary>A user control that contains the output data which is represented by some <see cref="DataTable"/> object.
        /// </summary>
        private ExcelPoolInspectorOutputTableUserControl m_OutputTableUserControl = new ExcelPoolInspectorOutputTableUserControl();

        /// <summary>A user control that contains the output data which is a list of properties.
        /// </summary>
        private ExcelPoolInspectorOutputPropertiesUserControl m_OutputPropertyUserControl = new ExcelPoolInspectorOutputPropertiesUserControl();

        /// <summary>A user control that contains the output data which is represented by a parent and a child <see cref="DataTable"/> object.
        /// </summary>
        private ExcelPoolInspectorOutputParentChildTableUserControl m_OutputParentChildTableUserControl = new ExcelPoolInspectorOutputParentChildTableUserControl();

        /// <summary>For each <see cref="ExcelPoolItemType"/> the root node where to add the <see cref="ExcelPoolItem"/> objects.
        /// </summary>
        private Dictionary<Guid, TreeNode> m_ExcelPoolItemTypeRootNode = new Dictionary<Guid, TreeNode>();
        #endregion

        #region public constructors

        /// <summary>Initializes a new instance of the <see cref="ExcelPoolInspectorForm"/> class.
        /// </summary>
        public ExcelPoolInspectorForm()
        {
            InitializeComponent();

            ExcelPool.ItemAdded += new Action<ExcelPool.ItemAddedEventArgs>(ExcelPool_ItemAdded);
        }
        #endregion

        #region protected methods

        /// <summary>Adds for each <see cref="ExcelPoolItemType"/> a node into the <see cref="TreeView"/> control and store the index the roots by the GUID.
        /// </summary>
        protected void BuildExcelPoolItemTypeTreeOutline()
        {
            foreach (var poolItemCreator in ExcelPoolItemCreator.Values)
            {
                TreeNodeCollection rootNodeCollection = poolItemTree.Nodes;

                if (poolItemCreator.ObjectType.SubCategories != null)
                {
                    foreach (IdentifierString subCategory in poolItemCreator.ObjectType.SubCategories)
                    {
                        /* search for the (sub-)category name and add it into the tree: */
                        TreeNode subCategoryNode;
                        int subCategoryInsertNodeIndex;
                        if (TryGetCategoryNode(subCategory, rootNodeCollection, out subCategoryNode, out subCategoryInsertNodeIndex) == false)
                        {
                            subCategoryNode = new TreeNode(subCategory.String);
                            subCategoryNode.Tag = subCategory;

                            rootNodeCollection.Insert(subCategoryInsertNodeIndex, subCategoryNode);
                        }
                        rootNodeCollection = subCategoryNode.Nodes;
                    }
                }
                /* add a node which represents the ExcelPoolItemType */
                TreeNode poolItemTypeNode = new TreeNode(poolItemCreator.ObjectType.Name.String);
                poolItemTypeNode.Tag = poolItemCreator;

                /* find the correct position where to add- 'rootNodeCollection' contains nodes with a specitic (sub-)category name or with other PoolItemTypes: */
                int indexToAdd = 0;
                for (int j = 0; j < rootNodeCollection.Count; j++)
                {
                    if (rootNodeCollection[j].Text.CompareTo(poolItemCreator.ObjectType.Name.String) <= 0)  // if 'rootNodeCollection[j].Name' < 'poolItemCreatorObjectTypeName' 
                    {
                        indexToAdd = j + 1;
                    }
                    else
                    {
                        break;  // linear search
                    }
                }
                rootNodeCollection.Insert(indexToAdd, poolItemTypeNode);

                m_ExcelPoolItemTypeRootNode.Add(poolItemCreator.ObjectType.Identifier, poolItemTypeNode);
            }
        }

        /// <summary>Adds the <see cref="TreeNode"/> representation of all <see cref="ExcelPoolItem"/> objects into the <see cref="TreeView"/> control.
        /// </summary>
        protected void AddAllExcelPoolItems()
        {
            foreach (ExcelPoolItem excelPoolItem in ExcelPool.Items)
            {
                TreeNode excelPoolItemTypeTreeNode;
                if (m_ExcelPoolItemTypeRootNode.TryGetValue(excelPoolItem.ObjectType.Identifier, out excelPoolItemTypeTreeNode) == true)
                {
                    excelPoolItemTypeTreeNode.Nodes.Add(GetExcelPoolItemNode(excelPoolItem));
                }
            }
        }

        /// <summary>Removes all <see cref="TreeNode"/> representation of <see cref="ExcelPoolItem"/> objects in the <see cref="TreeView"/> control.
        /// </summary>
        protected void ResetTree()
        {
            foreach (var treeNode in m_ExcelPoolItemTypeRootNode.Values)
            {
                treeNode.Nodes.Clear();
            }
        }

        /// <summary>Gets the <see cref="TreeNode"/> representation of an <see cref="ExcelPoolItem"/> object.
        /// </summary>
        /// <param name="excelPoolItem">The <see cref="ExcelPoolItem"/> object to convert.</param>
        protected TreeNode GetExcelPoolItemNode(ExcelPoolItem excelPoolItem)
        {
            TreeNode excelPoolItemNode = new TreeNode(excelPoolItem.LongName.String);
            excelPoolItemNode.Tag = excelPoolItem;

            /* add a 'General' node which represents the logfile, time of creation etc. */
            TreeNode generalNode = new TreeNode("General");
            generalNode.Tag = excelPoolItem;
            excelPoolItemNode.Nodes.Add(generalNode);

            /* add the 'input' nodes, where the sub-nodes contains the GuidedExcelDataQuery objects needed as input:  */
            TreeNode inputNode = new TreeNode("Input");
            excelPoolItemNode.Nodes.Add(inputNode);

            foreach (var inputDataQuery in excelPoolItem.GetDataQueryRepresentation())
            {
                TreeNode inputDataQueryNode = new TreeNode(inputDataQuery.LongName.String);

                inputDataQueryNode.Tag = Tuple.Create(excelPoolItem, inputDataQuery);
                if (inputNode.Tag == null)
                {
                    inputNode.Tag = inputDataQueryNode.Tag;
                }
                inputNode.Nodes.Add(inputDataQueryNode);
            }

            /* add the 'output' nodes, where the successor nodes contains the properties and data table of the InfoOutput object */
            TreeNode outputNode = new TreeNode("Output");
            excelPoolItemNode.Nodes.Add(outputNode);

            InfoOutput infoOutput = new InfoOutput();
            excelPoolItem.Value.FillInfoOutput(infoOutput);

            foreach (var infoOutputCollection in infoOutput)
            {
                TreeNode outputCategoryNode;
                if (infoOutput.Count > 1)
                {
                    outputCategoryNode = new TreeNode(infoOutputCollection.CategoryName.String);
                    outputNode.Nodes.Add(outputCategoryNode);
                }
                else
                {
                    outputCategoryNode = outputNode;
                }

                foreach (var propertyGroup in infoOutputCollection.Properties)
                {
                    TreeNode propertyGroupNode = new TreeNode(propertyGroup.Item1.String);
                    propertyGroupNode.Tag = Tuple.Create(excelPoolItem, propertyGroup.Item1, propertyGroup.Item2);

                    outputCategoryNode.Nodes.Add(propertyGroupNode);
                    if (outputNode.Tag == null)
                    {
                        outputNode.Tag = propertyGroupNode.Tag;
                    }
                }

                foreach (var table in infoOutputCollection.GetDataTables(InfoOutputPackage.DataTableType.Single))
                {
                    TreeNode tableNode = new TreeNode(table.Item1.String);
                    tableNode.Tag = Tuple.Create(excelPoolItem, table.Item1, table.Item2);

                    outputCategoryNode.Nodes.Add(tableNode);

                    if (outputNode.Tag == null)
                    {
                        outputNode.Tag = tableNode.Tag;
                    }
                }

                foreach (var parentChildTable in infoOutputCollection.ParentChildDataTables)
                {
                    TreeNode tableNode = new TreeNode(parentChildTable.Item1.String);
                    tableNode.Tag = Tuple.Create(excelPoolItem, parentChildTable.Item1, parentChildTable.Item2);

                    outputCategoryNode.Nodes.Add(tableNode);

                    if (outputNode.Tag == null)
                    {
                        outputNode.Tag = tableNode.Tag;
                    }
                }
            }
            return excelPoolItemNode;
        }
        #endregion

        #region private methods

        #region node methods

        /// <summary>Gets a <see cref="TreeNode"/> which represents a specific (sub-)category a of <see cref="ExcelPoolItemType"/>.
        /// </summary>
        /// <param name="subCategoryName">The name of the (sub-) category.</param>
        /// <param name="rootNodeCollection">The collection of <see cref="TreeNode"/> objects to search for the (sub-) category name.</param>
        /// <param name="value">The value, i.e. the <see cref="TreeNode"/> object which represents the (sub-) category <paramref name="subCategoryName"/> of a specific <see cref="ExcelPoolItemType"/> (output).</param>
        /// <param name="nodeIndex">If no <see cref="TreeNode"/> object found which represents the (sub-) category <paramref name="subCategoryName"/> of a specific <see cref="ExcelPoolItemType"/>,
        /// then a null-based index will be returned which indicates where to add a new <see cref="TreeNode"/> object in <paramref name="rootNodeCollection"/> which represents the (sub-) category [in alphabetic order] (output).</param>
        /// <returns></returns>
        private bool TryGetCategoryNode(IdentifierString subCategoryName, TreeNodeCollection rootNodeCollection, out TreeNode value, out int nodeIndex)
        {
            nodeIndex = 0;
            for (int j = 0; j < rootNodeCollection.Count; j++)
            {
                TreeNode treeNode = rootNodeCollection[j];

                IdentifierString treeSubCategoryTag = treeNode.Tag as IdentifierString;
                if (treeSubCategoryTag != null)
                {
                    if (treeSubCategoryTag.IDString == subCategoryName.IDString)
                    {
                        value = treeNode;
                        return true;
                    }
                    else
                    {
                        if (subCategoryName.IDString.CompareTo(treeSubCategoryTag.IDString) > 0)  // if 'subCategoryName' < 'treeSubCategoryName'
                        {
                            nodeIndex = j + 1;
                        }
                    }
                }
            }
            value = null;
            return false;
        }

        /// <summary>Handles the BeforeSelect event of the <see cref="TreeView"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/> instance containing the event data.</param>
        private void poolItemTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            mainGroupBox.Hide();
            mainGroupBox.Controls.Clear();

            if ((e == null) || (e.Node == null) || (e.Node.Tag == null))
            {
                mainGroupBox.Controls.Clear();
            }
            else
            {
                UserControl userControl = null;
                ExcelPoolItem excelPoolItem = null;

                object tag = e.Node.Tag;
                if (tag is ExcelPoolItem)
                {
                    excelPoolItem = tag as ExcelPoolItem;
                    if (excelPoolItem != null)
                    {
                        userControl = m_GeneralUserControl;
                        m_GeneralUserControl.Initialize(excelPoolItem);
                    }
                }
                else if (tag is Tuple<ExcelPoolItem, GuidedExcelDataQuery>)
                {
                    var inputTag = tag as Tuple<ExcelPoolItem, GuidedExcelDataQuery>;

                    if (inputTag != null)
                    {
                        excelPoolItem = inputTag.Item1;
                        userControl = m_InputUserControl;
                        m_InputUserControl.Initialize(inputTag.Item2);
                    }
                }
                else if (tag is Tuple<ExcelPoolItem, IdentifierString, IIdentifierStringDictionary<InfoOutputProperty>>)
                {
                    var inputTag = tag as Tuple<ExcelPoolItem, IdentifierString, IIdentifierStringDictionary<InfoOutputProperty>>;
                    if (inputTag != null)
                    {
                        excelPoolItem = inputTag.Item1;
                        userControl = m_OutputPropertyUserControl;
                        m_OutputPropertyUserControl.Initialize(inputTag.Item2.String, inputTag.Item3);
                    }
                }
                else if (tag is Tuple<ExcelPoolItem, IdentifierString, DataTable>)
                {
                    var inputTag = tag as Tuple<ExcelPoolItem, IdentifierString, DataTable>;
                    if (inputTag != null)
                    {
                        excelPoolItem = inputTag.Item1;
                        userControl = m_OutputTableUserControl;
                        m_OutputTableUserControl.Initialize(inputTag.Item2.String, inputTag.Item3);
                    }
                }
                else if (tag is Tuple<ExcelPoolItem, IdentifierString, InfoOutputParentChildDataTable>)
                {
                    var inputTag = tag as Tuple<ExcelPoolItem, IdentifierString, InfoOutputParentChildDataTable>;
                    if (inputTag != null)
                    {
                        excelPoolItem = inputTag.Item1;
                        userControl = m_OutputTableUserControl;
                        m_OutputParentChildTableUserControl.Initialize(inputTag.Item2.String, inputTag.Item3);
                    }
                }

                if ((userControl != null) && (excelPoolItem != null))
                {
                    mainGroupBox.Text = excelPoolItem.LongName.String;

                    userControl.Left = 10;
                    userControl.Top = 15;
                    userControl.Height = mainGroupBox.Height - 25;
                    userControl.Width = mainGroupBox.Width - 15;
                    userControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    mainGroupBox.Controls.Add(userControl);
                }
                mainGroupBox.Show();
            }
        }

        /// <summary>Handles the ItemAdded event of the <see cref="ExcelPool"/>.
        /// </summary>
        /// <param name="e">The <see cref="Dodoni.XLBasicComponents.ExcelPool.ItemAddedEventArgs"/> instance containing the event data.</param>
        private void ExcelPool_ItemAdded(ExcelPool.ItemAddedEventArgs e)
        {
            TreeNode excelPoolItemTypeTreeNode;
            if (m_ExcelPoolItemTypeRootNode.TryGetValue(e.Item.ObjectType.Identifier, out excelPoolItemTypeTreeNode) == true)
            {
                if (e.State == ItemAddedState.Added)
                {
                    excelPoolItemTypeTreeNode.Nodes.Add(GetExcelPoolItemNode(e.Item));  // add at the beginning, perhaps sorting later
                }
                else if (e.State == ItemAddedState.Replaced)
                {
                    /* search for the old TreeNode and replace it by a new one: */
                    for (int j = 0; j < excelPoolItemTypeTreeNode.Nodes.Count; j++)
                    {
                        ExcelPoolItem excelPoolItem = excelPoolItemTypeTreeNode.Nodes[j].Tag as ExcelPoolItem;
                        if ((excelPoolItem != null) && (excelPoolItem.ObjectType.Identifier == e.Item.ObjectType.Identifier))
                        {
                            if (excelPoolItem.ObjectName.IDString == e.Item.ObjectName.IDString)
                            {
                                TreeNode newTreeNode = GetExcelPoolItemNode(e.Item);
                                excelPoolItemTypeTreeNode.Nodes.RemoveAt(j);
                                excelPoolItemTypeTreeNode.Nodes.Insert(j, newTreeNode);

                                return; // exit the method
                            }
                        }
                    }
                    /* If the user replaces a ExcelPoolItem object with a ExcelPoolItem object of an other ExcelPoolItemType we remove all objects and add it again (fallback solution) */
                    ResetTree();
                    AddAllExcelPoolItems();
                }
            }
        }
        #endregion

        #region button event handling

        /// <summary>Handles the Click event of the cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Hide(); // the current instance will be closed only, i.e. will not be destroyed
        }

        /// <summary>Handles the Click event of the open [file] button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder filterStrBuilder = new StringBuilder();
            foreach (var objectStreamer in ObjectStreamer.Values)
            {
                if (filterStrBuilder.Length > 1)
                {
                    filterStrBuilder.Append("|");
                }
                filterStrBuilder.Append(objectStreamer.Name.String + " (*." + objectStreamer.FileExtension.String + ")|");
                filterStrBuilder.Append("*." + objectStreamer.FileExtension.String);
            }
            string filter = filterStrBuilder.ToString() + "|All files (*.*)|*.*";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Load Dodoni.net objects";
            openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IObjectStreamer objectStreamer;
                string fileExtension = ExtendedPath.GetExtension(openFileDialog.FileName);
                if (ObjectStreamer.TryGetObjectStreamerByFileExtension(fileExtension, out objectStreamer) == false)
                {
                    MessageBox.Show("Invalid file extension '" + fileExtension + "'.");
                    return; // exit function
                }
                try
                {
                    StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                    IObjectStreamReader objectStreamReader = objectStreamer.GetStreamReader(streamReader);
                    string infoMessage;
                    IEnumerable<ExcelPoolItem> excelPoolItems;
                    if (ExcelPool.TryLoadObjects(objectStreamReader, out infoMessage, out excelPoolItems) == false)
                    {
                        MessageBox.Show("Error: " + infoMessage);
                    }
                    ObjectStreamReaderInfoForm infoForm = new ObjectStreamReaderInfoForm();
                    infoForm.ShowDetails(infoMessage, excelPoolItems);

                    objectStreamReader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.Message);
                }
            }
        }

        /// <summary>Handles the Click event of the clear [pool] button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void clearPoolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to remove all items of the [Excel] pool?", "Excel pool operation [clear pool]", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                ExcelPool.Clear();
                ResetTree();
            }
        }
        #endregion

        /// <summary>Handles the Load event of the PoolInspectorForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PoolInspectorForm_Load(object sender, EventArgs e)
        {
            BuildExcelPoolItemTypeTreeOutline();
            AddAllExcelPoolItems();
        }

        /// <summary>Handles the FormClosing event of the ExcelPoolInspectorForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void ExcelPoolInspectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        #endregion
    }
}