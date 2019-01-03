namespace Dodoni.BasicComponents
{
    partial class MainForm
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
            this.bParentChildInfoOutput = new System.Windows.Forms.Button();
            this.bLogging = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bExit = new System.Windows.Forms.Button();
            this.bConfigFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bParentChildInfoOutput
            // 
            this.bParentChildInfoOutput.Location = new System.Drawing.Point(15, 64);
            this.bParentChildInfoOutput.Name = "bParentChildInfoOutput";
            this.bParentChildInfoOutput.Size = new System.Drawing.Size(161, 23);
            this.bParentChildInfoOutput.TabIndex = 2;
            this.bParentChildInfoOutput.Text = "Parent-Child InfoOutput";
            this.bParentChildInfoOutput.UseVisualStyleBackColor = true;
            this.bParentChildInfoOutput.Click += new System.EventHandler(this.bParentChildInfoOutput_Click);
            // 
            // bLogging
            // 
            this.bLogging.Location = new System.Drawing.Point(258, 64);
            this.bLogging.Name = "bLogging";
            this.bLogging.Size = new System.Drawing.Size(75, 23);
            this.bLogging.TabIndex = 3;
            this.bLogging.Text = "Logging";
            this.bLogging.UseVisualStyleBackColor = true;
            this.bLogging.Click += new System.EventHandler(this.bLogging_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Do some simple tests...";
            // 
            // bExit
            // 
            this.bExit.Location = new System.Drawing.Point(143, 199);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 5;
            this.bExit.Text = "Exit";
            this.bExit.UseVisualStyleBackColor = true;
            // 
            // bConfigFile
            // 
            this.bConfigFile.Location = new System.Drawing.Point(205, 126);
            this.bConfigFile.Name = "bConfigFile";
            this.bConfigFile.Size = new System.Drawing.Size(160, 23);
            this.bConfigFile.TabIndex = 6;
            this.bConfigFile.Text = "Config file";
            this.bConfigFile.UseVisualStyleBackColor = true;
            this.bConfigFile.Click += new System.EventHandler(this.bConfigFile_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.bExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 240);
            this.Controls.Add(this.bConfigFile);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bLogging);
            this.Controls.Add(this.bParentChildInfoOutput);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Dodoni.net - Integration test: Dodoni.BasicComponents";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bParentChildInfoOutput;
        private System.Windows.Forms.Button bLogging;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button bConfigFile;

    }
}