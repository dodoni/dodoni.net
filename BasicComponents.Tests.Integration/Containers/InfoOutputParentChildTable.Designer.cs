namespace Dodoni.BasicComponents.Containers
{
    partial class InfoOutputParentChildTable
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
            this.bCreateObject = new System.Windows.Forms.Button();
            this.dataGridViewParent = new System.Windows.Forms.DataGridView();
            this.dataGridViewChild = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChild)).BeginInit();
            this.SuspendLayout();
            // 
            // bCreateObject
            // 
            this.bCreateObject.Location = new System.Drawing.Point(12, 25);
            this.bCreateObject.Name = "bCreateObject";
            this.bCreateObject.Size = new System.Drawing.Size(135, 23);
            this.bCreateObject.TabIndex = 0;
            this.bCreateObject.Text = "Create object";
            this.bCreateObject.UseVisualStyleBackColor = true;
            this.bCreateObject.Click += new System.EventHandler(this.bCreateObject_Click);
            // 
            // dataGridViewParent
            // 
            this.dataGridViewParent.AllowUserToAddRows = false;
            this.dataGridViewParent.AllowUserToDeleteRows = false;
            this.dataGridViewParent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewParent.Location = new System.Drawing.Point(12, 75);
            this.dataGridViewParent.Name = "dataGridViewParent";
            this.dataGridViewParent.ReadOnly = true;
            this.dataGridViewParent.RowHeadersVisible = false;
            this.dataGridViewParent.Size = new System.Drawing.Size(661, 168);
            this.dataGridViewParent.TabIndex = 1;
            // 
            // dataGridViewChild
            // 
            this.dataGridViewChild.AllowUserToAddRows = false;
            this.dataGridViewChild.AllowUserToDeleteRows = false;
            this.dataGridViewChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChild.Location = new System.Drawing.Point(12, 306);
            this.dataGridViewChild.Name = "dataGridViewChild";
            this.dataGridViewChild.ReadOnly = true;
            this.dataGridViewChild.RowHeadersVisible = false;
            this.dataGridViewChild.Size = new System.Drawing.Size(661, 134);
            this.dataGridViewChild.TabIndex = 2;
            // 
            // InfoOutputParentChildTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 527);
            this.Controls.Add(this.dataGridViewChild);
            this.Controls.Add(this.dataGridViewParent);
            this.Controls.Add(this.bCreateObject);
            this.Name = "InfoOutputParentChildTable";
            this.Text = "InfoOutputParentChildTable";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChild)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCreateObject;
        private System.Windows.Forms.DataGridView dataGridViewParent;
        private System.Windows.Forms.DataGridView dataGridViewChild;
    }
}