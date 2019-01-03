namespace Dodoni.XLBasicComponents.Logging
{
    partial class UserControlConfigurationLogFile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cBoxAddLogfileOnDropdownListFails = new System.Windows.Forms.CheckBox();
            this.bReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxGlobalLoggingLevel = new System.Windows.Forms.ComboBox();
            this.cBoxPopupGlobalLogfileForm = new System.Windows.Forms.CheckBox();
            this.groupBoxLocalLogging = new System.Windows.Forms.GroupBox();
            this.bLocalOutputFolderSelector = new System.Windows.Forms.Button();
            this.editLocalOutputFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxLocalOutputUsage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxLocalLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // cBoxAddLogfileOnDropdownListFails
            // 
            this.cBoxAddLogfileOnDropdownListFails.AutoSize = true;
            this.cBoxAddLogfileOnDropdownListFails.Location = new System.Drawing.Point(9, 57);
            this.cBoxAddLogfileOnDropdownListFails.Name = "cBoxAddLogfileOnDropdownListFails";
            this.cBoxAddLogfileOnDropdownListFails.Size = new System.Drawing.Size(392, 17);
            this.cBoxAddLogfileOnDropdownListFails.TabIndex = 3;
            this.cBoxAddLogfileOnDropdownListFails.Text = "Add (info) message into the global log file if data advice (Drop down list) faile" +
    "d.";
            this.cBoxAddLogfileOnDropdownListFails.UseVisualStyleBackColor = true;
            this.cBoxAddLogfileOnDropdownListFails.CheckedChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // bReset
            // 
            this.bReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bReset.Location = new System.Drawing.Point(480, 244);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(75, 23);
            this.bReset.TabIndex = 3;
            this.bReset.Text = "Reset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxGlobalLoggingLevel);
            this.groupBox1.Controls.Add(this.cBoxPopupGlobalLogfileForm);
            this.groupBox1.Controls.Add(this.cBoxAddLogfileOnDropdownListFails);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 119);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Global logging";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Logging level:";
            // 
            // comboBoxGlobalLoggingLevel
            // 
            this.comboBoxGlobalLoggingLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxGlobalLoggingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGlobalLoggingLevel.FormattingEnabled = true;
            this.comboBoxGlobalLoggingLevel.Location = new System.Drawing.Point(91, 22);
            this.comboBoxGlobalLoggingLevel.Name = "comboBoxGlobalLoggingLevel";
            this.comboBoxGlobalLoggingLevel.Size = new System.Drawing.Size(230, 21);
            this.comboBoxGlobalLoggingLevel.TabIndex = 6;
            this.comboBoxGlobalLoggingLevel.SelectedIndexChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // cBoxPopupGlobalLogfileForm
            // 
            this.cBoxPopupGlobalLogfileForm.AutoSize = true;
            this.cBoxPopupGlobalLogfileForm.Location = new System.Drawing.Point(9, 88);
            this.cBoxPopupGlobalLogfileForm.Name = "cBoxPopupGlobalLogfileForm";
            this.cBoxPopupGlobalLogfileForm.Size = new System.Drawing.Size(312, 17);
            this.cBoxPopupGlobalLogfileForm.TabIndex = 4;
            this.cBoxPopupGlobalLogfileForm.Text = "Pop-up the global log file message form if a fatal error occurs.";
            this.cBoxPopupGlobalLogfileForm.UseVisualStyleBackColor = true;
            this.cBoxPopupGlobalLogfileForm.CheckedChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // groupBoxLocalLogging
            // 
            this.groupBoxLocalLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLocalLogging.Controls.Add(this.bLocalOutputFolderSelector);
            this.groupBoxLocalLogging.Controls.Add(this.editLocalOutputFolder);
            this.groupBoxLocalLogging.Controls.Add(this.label1);
            this.groupBoxLocalLogging.Location = new System.Drawing.Point(16, 146);
            this.groupBoxLocalLogging.Name = "groupBoxLocalLogging";
            this.groupBoxLocalLogging.Size = new System.Drawing.Size(539, 91);
            this.groupBoxLocalLogging.TabIndex = 9;
            this.groupBoxLocalLogging.TabStop = false;
            this.groupBoxLocalLogging.Text = "Local logging";
            // 
            // bLocalOutputFolderSelector
            // 
            this.bLocalOutputFolderSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLocalOutputFolderSelector.Location = new System.Drawing.Point(495, 53);
            this.bLocalOutputFolderSelector.Name = "bLocalOutputFolderSelector";
            this.bLocalOutputFolderSelector.Size = new System.Drawing.Size(39, 23);
            this.bLocalOutputFolderSelector.TabIndex = 3;
            this.bLocalOutputFolderSelector.Text = "...";
            this.bLocalOutputFolderSelector.UseVisualStyleBackColor = true;
            this.bLocalOutputFolderSelector.Click += new System.EventHandler(this.bLocalOutputFolderSelector_Click);
            // 
            // editLocalOutputFolder
            // 
            this.editLocalOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editLocalOutputFolder.Location = new System.Drawing.Point(91, 55);
            this.editLocalOutputFolder.Name = "editLocalOutputFolder";
            this.editLocalOutputFolder.Size = new System.Drawing.Size(397, 20);
            this.editLocalOutputFolder.TabIndex = 2;
            this.editLocalOutputFolder.TextChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output folder:";
            // 
            // comboBoxLocalOutputUsage
            // 
            this.comboBoxLocalOutputUsage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLocalOutputUsage.FormattingEnabled = true;
            this.comboBoxLocalOutputUsage.Location = new System.Drawing.Point(107, 165);
            this.comboBoxLocalOutputUsage.Name = "comboBoxLocalOutputUsage";
            this.comboBoxLocalOutputUsage.Size = new System.Drawing.Size(252, 21);
            this.comboBoxLocalOutputUsage.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Output usage:";
            // 
            // UserControlConfigurationLogFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxLocalOutputUsage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxLocalLogging);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bReset);
            this.Name = "UserControlConfigurationLogFile";
            this.Size = new System.Drawing.Size(563, 275);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxLocalLogging.ResumeLayout(false);
            this.groupBoxLocalLogging.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cBoxAddLogfileOnDropdownListFails;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cBoxPopupGlobalLogfileForm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxGlobalLoggingLevel;
        private System.Windows.Forms.GroupBox groupBoxLocalLogging;
        private System.Windows.Forms.Button bLocalOutputFolderSelector;
        private System.Windows.Forms.TextBox editLocalOutputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxLocalOutputUsage;
        private System.Windows.Forms.Label label2;

    }
}
