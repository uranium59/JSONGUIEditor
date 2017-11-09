namespace JSONGUIEditor.AdditionalForm
{
    partial class ErrorShowForm
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
            this.errorLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.textbox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(14, 251);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(45, 15);
            this.errorLabel.TabIndex = 1;
            this.errorLabel.Text = "label1";
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(225, 284);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(86, 29);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // textbox
            // 
            this.textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.textbox.Location = new System.Drawing.Point(0, 0);
            this.textbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(325, 235);
            this.textbox.TabIndex = 4;
            this.textbox.Text = "";
            // 
            // ErrorShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 328);
            this.ControlBox = false;
            this.Controls.Add(this.textbox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.errorLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorShowForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.RichTextBox textbox;
    }
}