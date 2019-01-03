namespace Dodoni.XLBasicComponents
{
    partial class UserControlConfigurationGeneral
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
            this.bReset = new System.Windows.Forms.Button();
            this.cBoxAddVBADropDownList = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxFALSE = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxTRUE = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSeparator = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxAddDropDownList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bReset
            // 
            this.bReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bReset.Location = new System.Drawing.Point(432, 308);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(75, 23);
            this.bReset.TabIndex = 2;
            this.bReset.Text = "Reset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // cBoxAddVBADropDownList
            // 
            this.cBoxAddVBADropDownList.AutoSize = true;
            this.cBoxAddVBADropDownList.Location = new System.Drawing.Point(14, 54);
            this.cBoxAddVBADropDownList.Name = "cBoxAddVBADropDownList";
            this.cBoxAddVBADropDownList.Size = new System.Drawing.Size(301, 17);
            this.cBoxAddVBADropDownList.TabIndex = 6;
            this.cBoxAddVBADropDownList.Text = "Add drop down list with available VBA functions, if desired.";
            this.cBoxAddVBADropDownList.UseVisualStyleBackColor = true;
            this.cBoxAddVBADropDownList.CheckedChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxFALSE);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxTRUE);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(10, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 70);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Excel cell representation";
            // 
            // comboBoxFALSE
            // 
            this.comboBoxFALSE.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxFALSE.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFALSE.FormattingEnabled = true;
            this.comboBoxFALSE.Items.AddRange(new object[] {
            "FALSCH",
            "FALSE"});
            this.comboBoxFALSE.Location = new System.Drawing.Point(319, 29);
            this.comboBoxFALSE.Name = "comboBoxFALSE";
            this.comboBoxFALSE.Size = new System.Drawing.Size(134, 21);
            this.comboBoxFALSE.TabIndex = 14;
            this.comboBoxFALSE.TextChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "\'FALSE\' =";
            // 
            // comboBoxTRUE
            // 
            this.comboBoxTRUE.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTRUE.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTRUE.FormattingEnabled = true;
            this.comboBoxTRUE.Items.AddRange(new object[] {
            "WAHR",
            "TRUE"});
            this.comboBoxTRUE.Location = new System.Drawing.Point(65, 29);
            this.comboBoxTRUE.Name = "comboBoxTRUE";
            this.comboBoxTRUE.Size = new System.Drawing.Size(134, 21);
            this.comboBoxTRUE.TabIndex = 12;
            this.comboBoxTRUE.TextChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "\'TRUE\' =";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Separater for Drop down list (for data advice):";
            // 
            // comboBoxSeparator
            // 
            this.comboBoxSeparator.FormattingEnabled = true;
            this.comboBoxSeparator.Items.AddRange(new object[] {
            ";",
            ","});
            this.comboBoxSeparator.Location = new System.Drawing.Point(244, 113);
            this.comboBoxSeparator.Name = "comboBoxSeparator";
            this.comboBoxSeparator.Size = new System.Drawing.Size(88, 21);
            this.comboBoxSeparator.TabIndex = 9;
            this.comboBoxSeparator.TextChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(448, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "(This feature requires \'Trust access to the VBA project object model\' in the Exce" +
                "l Trust Center)";
            // 
            // comboBoxAddDropDownList
            // 
            this.comboBoxAddDropDownList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAddDropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAddDropDownList.FormattingEnabled = true;
            this.comboBoxAddDropDownList.Location = new System.Drawing.Point(334, 18);
            this.comboBoxAddDropDownList.Name = "comboBoxAddDropDownList";
            this.comboBoxAddDropDownList.Size = new System.Drawing.Size(175, 21);
            this.comboBoxAddDropDownList.TabIndex = 11;
            this.comboBoxAddDropDownList.SelectedIndexChanged += new System.EventHandler(this.Userinput_Changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Add drop down list with user-friendly data advice (if applictable):";
            // 
            // GeneralConfigurationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxAddDropDownList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bReset);
            this.Controls.Add(this.cBoxAddVBADropDownList);
            this.Controls.Add(this.comboBoxSeparator);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "GeneralConfigurationUserControl";
            this.Size = new System.Drawing.Size(518, 342);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.CheckBox cBoxAddVBADropDownList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSeparator;
        private System.Windows.Forms.ComboBox comboBoxFALSE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxTRUE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxAddDropDownList;
        private System.Windows.Forms.Label label1;
    }
}
