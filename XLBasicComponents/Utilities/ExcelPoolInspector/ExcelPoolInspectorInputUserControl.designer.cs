namespace Dodoni.XLBasicComponents.Utilities
{
    partial class ExcelPoolInspectorInputUserControl
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
            this.inputDataGridView = new System.Windows.Forms.DataGridView();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inputDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // inputDataGridView
            // 
            this.inputDataGridView.AllowUserToAddRows = false;
            this.inputDataGridView.AllowUserToDeleteRows = false;
            this.inputDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inputDataGridView.ColumnHeadersVisible = false;
            this.inputDataGridView.Location = new System.Drawing.Point(8, 43);
            this.inputDataGridView.Name = "inputDataGridView";
            this.inputDataGridView.ReadOnly = true;
            this.inputDataGridView.RowHeadersVisible = false;
            this.inputDataGridView.Size = new System.Drawing.Size(537, 306);
            this.inputDataGridView.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(18, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "label1";
            // 
            // ExcelPoolInspectorInputUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.inputDataGridView);
            this.Name = "ExcelPoolInspectorInputUserControl";
            this.Size = new System.Drawing.Size(557, 365);
            ((System.ComponentModel.ISupportInitialize)(this.inputDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView inputDataGridView;
        private System.Windows.Forms.Label labelName;
    }
}
