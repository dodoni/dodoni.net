namespace Dodoni.XLBasicComponents.Utilities
{
    partial class ExcelPoolInspectorGeneralUserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.editName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.editType = new System.Windows.Forms.TextBox();
            this.dateTimePickerCreationTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.logFileDataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClassification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMessageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.logFileDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // editName
            // 
            this.editName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editName.Enabled = false;
            this.editName.Location = new System.Drawing.Point(50, 18);
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(220, 20);
            this.editName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Creation time:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Type:";
            // 
            // editType
            // 
            this.editType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editType.Enabled = false;
            this.editType.Location = new System.Drawing.Point(327, 18);
            this.editType.Name = "editType";
            this.editType.Size = new System.Drawing.Size(245, 20);
            this.editType.TabIndex = 4;
            // 
            // dateTimePickerCreationTime
            // 
            this.dateTimePickerCreationTime.Enabled = false;
            this.dateTimePickerCreationTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerCreationTime.Location = new System.Drawing.Point(83, 60);
            this.dateTimePickerCreationTime.Name = "dateTimePickerCreationTime";
            this.dateTimePickerCreationTime.Size = new System.Drawing.Size(187, 20);
            this.dateTimePickerCreationTime.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Log file:";
            // 
            // logFileDataGridView
            // 
            this.logFileDataGridView.AllowUserToAddRows = false;
            this.logFileDataGridView.AllowUserToDeleteRows = false;
            this.logFileDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.logFileDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logFileDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnClassification,
            this.ColumnMessageType,
            this.ColumnMessage});
            this.logFileDataGridView.Location = new System.Drawing.Point(10, 135);
            this.logFileDataGridView.Name = "logFileDataGridView";
            this.logFileDataGridView.ReadOnly = true;
            this.logFileDataGridView.RowHeadersVisible = false;
            this.logFileDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logFileDataGridView.ShowEditingIcon = false;
            this.logFileDataGridView.ShowRowErrors = false;
            this.logFileDataGridView.Size = new System.Drawing.Size(560, 190);
            this.logFileDataGridView.TabIndex = 8;
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            // 
            // ColumnClassification
            // 
            this.ColumnClassification.HeaderText = "Classification";
            this.ColumnClassification.Name = "ColumnClassification";
            this.ColumnClassification.ReadOnly = true;
            // 
            // ColumnMessageType
            // 
            this.ColumnMessageType.HeaderText = "Message type";
            this.ColumnMessageType.Name = "ColumnMessageType";
            this.ColumnMessageType.ReadOnly = true;
            // 
            // ColumnMessage
            // 
            this.ColumnMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnMessage.HeaderText = "Message";
            this.ColumnMessage.Name = "ColumnMessage";
            this.ColumnMessage.ReadOnly = true;
            // 
            // ExcelPoolInspectorGeneralUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logFileDataGridView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePickerCreationTime);
            this.Controls.Add(this.editType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editName);
            this.Controls.Add(this.label1);
            this.Name = "ExcelPoolInspectorGeneralUserControl";
            this.Size = new System.Drawing.Size(579, 338);
            ((System.ComponentModel.ISupportInitialize)(this.logFileDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editType;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreationTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView logFileDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClassification;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessage;
    }
}
