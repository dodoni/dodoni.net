namespace Dodoni.BasicComponents.ConfigurationFiles
{
    partial class ConfigurationFileExample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.editFileName = new System.Windows.Forms.TextBox();
            this.bAddProperty = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.editPropertyName = new System.Windows.Forms.TextBox();
            this.editPropertyValue = new System.Windows.Forms.TextBox();
            this.bWriteConfigFile = new System.Windows.Forms.Button();
            this.editPropertyCollectionName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cBoxInfoOutputPropertyNames = new System.Windows.Forms.ComboBox();
            this.editInfoOutputPropertyValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cBoxInfoOutputPropertyCollectionName = new System.Windows.Forms.ComboBox();
            this.bAddExampleTable = new System.Windows.Forms.Button();
            this.bShowExampleTable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(23, 27);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(115, 29);
            this.bLoad.TabIndex = 0;
            this.bLoad.Text = "Load/Create";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File name:";
            // 
            // editFileName
            // 
            this.editFileName.Location = new System.Drawing.Point(226, 32);
            this.editFileName.Name = "editFileName";
            this.editFileName.Size = new System.Drawing.Size(202, 20);
            this.editFileName.TabIndex = 2;
            this.editFileName.Text = "ExampleConfigFile.config";
            // 
            // bAddProperty
            // 
            this.bAddProperty.Location = new System.Drawing.Point(65, 109);
            this.bAddProperty.Name = "bAddProperty";
            this.bAddProperty.Size = new System.Drawing.Size(139, 23);
            this.bAddProperty.TabIndex = 3;
            this.bAddProperty.Text = "Add/Change property";
            this.bAddProperty.UseVisualStyleBackColor = true;
            this.bAddProperty.Click += new System.EventHandler(this.bAddProperty_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(282, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Property name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(496, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Property value:";
            // 
            // editPropertyName
            // 
            this.editPropertyName.Location = new System.Drawing.Point(239, 144);
            this.editPropertyName.Name = "editPropertyName";
            this.editPropertyName.Size = new System.Drawing.Size(202, 20);
            this.editPropertyName.TabIndex = 6;
            this.editPropertyName.Text = "Color";
            // 
            // editPropertyValue
            // 
            this.editPropertyValue.Location = new System.Drawing.Point(456, 144);
            this.editPropertyValue.Name = "editPropertyValue";
            this.editPropertyValue.Size = new System.Drawing.Size(202, 20);
            this.editPropertyValue.TabIndex = 7;
            this.editPropertyValue.Text = "black";
            // 
            // bWriteConfigFile
            // 
            this.bWriteConfigFile.Location = new System.Drawing.Point(23, 290);
            this.bWriteConfigFile.Name = "bWriteConfigFile";
            this.bWriteConfigFile.Size = new System.Drawing.Size(115, 29);
            this.bWriteConfigFile.TabIndex = 8;
            this.bWriteConfigFile.Text = "Write/Store";
            this.bWriteConfigFile.UseVisualStyleBackColor = true;
            this.bWriteConfigFile.Click += new System.EventHandler(this.bWriteConfigFile_Click);
            // 
            // editPropertyCollectionName
            // 
            this.editPropertyCollectionName.Location = new System.Drawing.Point(337, 97);
            this.editPropertyCollectionName.Name = "editPropertyCollectionName";
            this.editPropertyCollectionName.Size = new System.Drawing.Size(202, 20);
            this.editPropertyCollectionName.TabIndex = 10;
            this.editPropertyCollectionName.Text = "MySettings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(380, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Property collection name:";
            // 
            // cBoxInfoOutputPropertyNames
            // 
            this.cBoxInfoOutputPropertyNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxInfoOutputPropertyNames.FormattingEnabled = true;
            this.cBoxInfoOutputPropertyNames.Location = new System.Drawing.Point(258, 396);
            this.cBoxInfoOutputPropertyNames.Name = "cBoxInfoOutputPropertyNames";
            this.cBoxInfoOutputPropertyNames.Size = new System.Drawing.Size(121, 21);
            this.cBoxInfoOutputPropertyNames.TabIndex = 11;
            this.cBoxInfoOutputPropertyNames.SelectedIndexChanged += new System.EventHandler(this.cBoxInfoOutputPropertyNames_SelectedIndexChanged);
            // 
            // editInfoOutputPropertyValue
            // 
            this.editInfoOutputPropertyValue.Location = new System.Drawing.Point(414, 397);
            this.editInfoOutputPropertyValue.Name = "editInfoOutputPropertyValue";
            this.editInfoOutputPropertyValue.ReadOnly = true;
            this.editInfoOutputPropertyValue.Size = new System.Drawing.Size(285, 20);
            this.editInfoOutputPropertyValue.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(277, 371);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Property name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(525, 371);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Property value:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Info output property output:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(78, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Property collection name:";
            // 
            // cBoxInfoOutputPropertyCollectionName
            // 
            this.cBoxInfoOutputPropertyCollectionName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxInfoOutputPropertyCollectionName.FormattingEnabled = true;
            this.cBoxInfoOutputPropertyCollectionName.Location = new System.Drawing.Point(65, 396);
            this.cBoxInfoOutputPropertyCollectionName.Name = "cBoxInfoOutputPropertyCollectionName";
            this.cBoxInfoOutputPropertyCollectionName.Size = new System.Drawing.Size(155, 21);
            this.cBoxInfoOutputPropertyCollectionName.TabIndex = 17;
            this.cBoxInfoOutputPropertyCollectionName.SelectedIndexChanged += new System.EventHandler(this.cBoxInfoOutputPropertyCollectionName_SelectedIndexChanged);
            // 
            // bAddExampleTable
            // 
            this.bAddExampleTable.Location = new System.Drawing.Point(65, 203);
            this.bAddExampleTable.Name = "bAddExampleTable";
            this.bAddExampleTable.Size = new System.Drawing.Size(139, 23);
            this.bAddExampleTable.TabIndex = 18;
            this.bAddExampleTable.Text = "Add example table";
            this.bAddExampleTable.UseVisualStyleBackColor = true;
            this.bAddExampleTable.Click += new System.EventHandler(this.bAddExampleTable_Click);
            // 
            // bShowExampleTable
            // 
            this.bShowExampleTable.Location = new System.Drawing.Point(240, 203);
            this.bShowExampleTable.Name = "bShowExampleTable";
            this.bShowExampleTable.Size = new System.Drawing.Size(139, 23);
            this.bShowExampleTable.TabIndex = 19;
            this.bShowExampleTable.Text = "Show example table...";
            this.bShowExampleTable.UseVisualStyleBackColor = true;
            this.bShowExampleTable.Click += new System.EventHandler(this.bShowExampleTable_Click);
            // 
            // ConfigurationFileExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 513);
            this.Controls.Add(this.bShowExampleTable);
            this.Controls.Add(this.bAddExampleTable);
            this.Controls.Add(this.cBoxInfoOutputPropertyCollectionName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.editInfoOutputPropertyValue);
            this.Controls.Add(this.cBoxInfoOutputPropertyNames);
            this.Controls.Add(this.editPropertyCollectionName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bWriteConfigFile);
            this.Controls.Add(this.editPropertyValue);
            this.Controls.Add(this.editPropertyName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bAddProperty);
            this.Controls.Add(this.editFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bLoad);
            this.Name = "ConfigurationFileExample";
            this.ShowIcon = false;
            this.Text = "Configuration file example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editFileName;
        private System.Windows.Forms.Button bAddProperty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editPropertyName;
        private System.Windows.Forms.TextBox editPropertyValue;
        private System.Windows.Forms.Button bWriteConfigFile;
        private System.Windows.Forms.TextBox editPropertyCollectionName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBoxInfoOutputPropertyNames;
        private System.Windows.Forms.TextBox editInfoOutputPropertyValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cBoxInfoOutputPropertyCollectionName;
        private System.Windows.Forms.Button bAddExampleTable;
        private System.Windows.Forms.Button bShowExampleTable;
    }
}