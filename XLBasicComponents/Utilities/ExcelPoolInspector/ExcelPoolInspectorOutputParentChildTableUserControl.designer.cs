namespace Dodoni.XLBasicComponents.Utilities
{
    partial class ExcelPoolInspectorOutputParentChildTableUserControl
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
            this.parentInfoOutputGridView = new System.Windows.Forms.DataGridView();
            this.parentLabelName = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.childLabelName = new System.Windows.Forms.Label();
            this.childInfoOutputdataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.parentInfoOutputGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.childInfoOutputdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // parentInfoOutputGridView
            // 
            this.parentInfoOutputGridView.AllowUserToAddRows = false;
            this.parentInfoOutputGridView.AllowUserToDeleteRows = false;
            this.parentInfoOutputGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parentInfoOutputGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parentInfoOutputGridView.Location = new System.Drawing.Point(5, 7);
            this.parentInfoOutputGridView.Name = "parentInfoOutputGridView";
            this.parentInfoOutputGridView.ReadOnly = true;
            this.parentInfoOutputGridView.RowHeadersVisible = false;
            this.parentInfoOutputGridView.Size = new System.Drawing.Size(518, 161);
            this.parentInfoOutputGridView.TabIndex = 0;
            // 
            // parentLabelName
            // 
            this.parentLabelName.AutoSize = true;
            this.parentLabelName.Location = new System.Drawing.Point(19, 7);
            this.parentLabelName.Name = "parentLabelName";
            this.parentLabelName.Size = new System.Drawing.Size(35, 13);
            this.parentLabelName.TabIndex = 1;
            this.parentLabelName.Text = "label1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 23);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.parentInfoOutputGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.childInfoOutputdataGridView);
            this.splitContainer1.Panel2.Controls.Add(this.childLabelName);
            this.splitContainer1.Size = new System.Drawing.Size(526, 354);
            this.splitContainer1.SplitterDistance = 177;
            this.splitContainer1.TabIndex = 2;
            // 
            // childLabelName
            // 
            this.childLabelName.AutoSize = true;
            this.childLabelName.Location = new System.Drawing.Point(13, 6);
            this.childLabelName.Name = "childLabelName";
            this.childLabelName.Size = new System.Drawing.Size(35, 13);
            this.childLabelName.TabIndex = 2;
            this.childLabelName.Text = "label1";
            // 
            // childInfoOutputdataGridView
            // 
            this.childInfoOutputdataGridView.AllowUserToAddRows = false;
            this.childInfoOutputdataGridView.AllowUserToDeleteRows = false;
            this.childInfoOutputdataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.childInfoOutputdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.childInfoOutputdataGridView.Location = new System.Drawing.Point(4, 22);
            this.childInfoOutputdataGridView.Name = "childInfoOutputdataGridView";
            this.childInfoOutputdataGridView.ReadOnly = true;
            this.childInfoOutputdataGridView.RowHeadersVisible = false;
            this.childInfoOutputdataGridView.Size = new System.Drawing.Size(518, 145);
            this.childInfoOutputdataGridView.TabIndex = 3;
            // 
            // ExcelPoolInspectorOutputParentChildTableUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.parentLabelName);
            this.Name = "ExcelPoolInspectorOutputParentChildTableUserControl";
            this.Size = new System.Drawing.Size(537, 408);
            ((System.ComponentModel.ISupportInitialize)(this.parentInfoOutputGridView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.childInfoOutputdataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView parentInfoOutputGridView;
        private System.Windows.Forms.Label parentLabelName;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView childInfoOutputdataGridView;
        private System.Windows.Forms.Label childLabelName;
    }
}
