namespace Dodoni.BasicComponents.LogFileFactory
{
    partial class LoggingForm
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
            this.cbMessageType = new System.Windows.Forms.ComboBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.editMessage = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.editLogging = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbMessageType
            // 
            this.cbMessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessageType.FormattingEnabled = true;
            this.cbMessageType.Location = new System.Drawing.Point(109, 43);
            this.cbMessageType.Name = "cbMessageType";
            this.cbMessageType.Size = new System.Drawing.Size(237, 21);
            this.cbMessageType.TabIndex = 0;
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(12, 43);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 1;
            this.bAdd.Text = "Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // editMessage
            // 
            this.editMessage.Location = new System.Drawing.Point(363, 43);
            this.editMessage.Name = "editMessage";
            this.editMessage.Size = new System.Drawing.Size(267, 20);
            this.editMessage.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(648, 43);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // editLogging
            // 
            this.editLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editLogging.Location = new System.Drawing.Point(12, 134);
            this.editLogging.Multiline = true;
            this.editLogging.Name = "editLogging";
            this.editLogging.Size = new System.Drawing.Size(757, 405);
            this.editLogging.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Log file";
            // 
            // LoggingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 551);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editLogging);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.editMessage);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.cbMessageType);
            this.Name = "LoggingForm";
            this.ShowIcon = false;
            this.Text = "Dodoni.net - Logging example";
            this.Load += new System.EventHandler(this.LoggingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMessageType;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.TextBox editMessage;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox editLogging;
        private System.Windows.Forms.Label label1;
    }
}