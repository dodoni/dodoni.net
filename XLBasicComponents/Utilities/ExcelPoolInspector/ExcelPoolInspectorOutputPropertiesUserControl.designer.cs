namespace Dodoni.XLBasicComponents.Utilities
{
    partial class ExcelPoolInspectorOutputPropertiesUserControl
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
            this.infoOutputPropertyDataGridView = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.infoOutputPropertyDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // infoOutputPropertyDataGridView
            // 
            this.infoOutputPropertyDataGridView.AllowUserToAddRows = false;
            this.infoOutputPropertyDataGridView.AllowUserToDeleteRows = false;
            this.infoOutputPropertyDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoOutputPropertyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoOutputPropertyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.PropertyValue});
            this.infoOutputPropertyDataGridView.Location = new System.Drawing.Point(8, 33);
            this.infoOutputPropertyDataGridView.Name = "infoOutputPropertyDataGridView";
            this.infoOutputPropertyDataGridView.ReadOnly = true;
            this.infoOutputPropertyDataGridView.RowHeadersVisible = false;
            this.infoOutputPropertyDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.infoOutputPropertyDataGridView.Size = new System.Drawing.Size(708, 422);
            this.infoOutputPropertyDataGridView.TabIndex = 0;
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Name";
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.ReadOnly = true;
            this.PropertyName.Width = 150;
            // 
            // PropertyValue
            // 
            this.PropertyValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PropertyValue.HeaderText = "Value";
            this.PropertyValue.Name = "PropertyValue";
            this.PropertyValue.ReadOnly = true;
            // 
            // propertyLabel
            // 
            this.propertyLabel.AutoSize = true;
            this.propertyLabel.Location = new System.Drawing.Point(15, 10);
            this.propertyLabel.Name = "propertyLabel";
            this.propertyLabel.Size = new System.Drawing.Size(35, 13);
            this.propertyLabel.TabIndex = 1;
            this.propertyLabel.Text = "label1";
            // 
            // ExcelPoolInspectorOutputPropertiesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyLabel);
            this.Controls.Add(this.infoOutputPropertyDataGridView);
            this.Name = "ExcelPoolInspectorOutputPropertiesUserControl";
            this.Size = new System.Drawing.Size(724, 464);
            ((System.ComponentModel.ISupportInitialize)(this.infoOutputPropertyDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView infoOutputPropertyDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyValue;
        private System.Windows.Forms.Label propertyLabel;
    }
}
