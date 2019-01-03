namespace Dodoni.XLBasicComponents.Logging
{
    partial class GlobalLogFileForm
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
            this.bClose = new System.Windows.Forms.Button();
            this.logFileDataGridView = new System.Windows.Forms.DataGridView();
            this.bClear = new System.Windows.Forms.Button();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClassification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMessageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnObjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnObjectTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.logFileDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(509, 243);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 0;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
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
            this.ColumnObjectName,
            this.ColumnObjectTypeName,
            this.ColumnMessage});
            this.logFileDataGridView.Location = new System.Drawing.Point(8, 12);
            this.logFileDataGridView.Name = "logFileDataGridView";
            this.logFileDataGridView.ReadOnly = true;
            this.logFileDataGridView.RowHeadersVisible = false;
            this.logFileDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logFileDataGridView.ShowEditingIcon = false;
            this.logFileDataGridView.ShowRowErrors = false;
            this.logFileDataGridView.Size = new System.Drawing.Size(576, 218);
            this.logFileDataGridView.TabIndex = 1;
            // 
            // bClear
            // 
            this.bClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bClear.Location = new System.Drawing.Point(15, 243);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(75, 23);
            this.bClear.TabIndex = 3;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.Width = 75;
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
            // ColumnObjectName
            // 
            this.ColumnObjectName.HeaderText = "Object name";
            this.ColumnObjectName.Name = "ColumnObjectName";
            this.ColumnObjectName.ReadOnly = true;
            // 
            // ColumnObjectTypeName
            // 
            this.ColumnObjectTypeName.HeaderText = "Object type";
            this.ColumnObjectTypeName.Name = "ColumnObjectTypeName";
            this.ColumnObjectTypeName.ReadOnly = true;
            // 
            // ColumnMessage
            // 
            this.ColumnMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnMessage.HeaderText = "Message";
            this.ColumnMessage.Name = "ColumnMessage";
            this.ColumnMessage.ReadOnly = true;
            // 
            // GlobalLogFileForm
            // 
            this.AcceptButton = this.bClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 273);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.logFileDataGridView);
            this.Controls.Add(this.bClose);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "GlobalLogFileForm";
            this.ShowIcon = false;
            this.Text = "Dodoni.NET - Global log file";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlobalLogFileForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.logFileDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.DataGridView logFileDataGridView;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClassification;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnObjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnObjectTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMessage;
    }
}