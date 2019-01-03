namespace Dodoni.XLBasicComponents.Utilities
{
    partial class ExcelPoolInspectorOutputTableUserControl
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
            this.infoOutputGridView = new System.Windows.Forms.DataGridView();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.infoOutputGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // infoOutputGridView
            // 
            this.infoOutputGridView.AllowUserToAddRows = false;
            this.infoOutputGridView.AllowUserToDeleteRows = false;
            this.infoOutputGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoOutputGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoOutputGridView.Location = new System.Drawing.Point(6, 26);
            this.infoOutputGridView.Name = "infoOutputGridView";
            this.infoOutputGridView.ReadOnly = true;
            this.infoOutputGridView.RowHeadersVisible = false;
            this.infoOutputGridView.Size = new System.Drawing.Size(526, 375);
            this.infoOutputGridView.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(19, 7);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "label1";
            // 
            // ExcelPoolInspectorOutputTableUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.infoOutputGridView);
            this.Name = "ExcelPoolInspectorOutputTableUserControl";
            this.Size = new System.Drawing.Size(537, 408);
            ((System.ComponentModel.ISupportInitialize)(this.infoOutputGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView infoOutputGridView;
        private System.Windows.Forms.Label labelName;
    }
}
