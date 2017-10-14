namespace JSONGUIEditor.TemplateForm
{
    partial class TemplateManage
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
            this.QuitButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.AddTemplateButton = new System.Windows.Forms.Button();
            this.InfoTemplateButton = new System.Windows.Forms.Button();
            this.DeleteTemplateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(494, 237);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 0;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(13, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(459, 244);
            this.listBox1.TabIndex = 1;
            // 
            // AddTemplateButton
            // 
            this.AddTemplateButton.Location = new System.Drawing.Point(494, 12);
            this.AddTemplateButton.Name = "AddTemplateButton";
            this.AddTemplateButton.Size = new System.Drawing.Size(75, 23);
            this.AddTemplateButton.TabIndex = 2;
            this.AddTemplateButton.Text = "Add";
            this.AddTemplateButton.UseVisualStyleBackColor = true;
            // 
            // InfoTemplateButton
            // 
            this.InfoTemplateButton.Location = new System.Drawing.Point(494, 41);
            this.InfoTemplateButton.Name = "InfoTemplateButton";
            this.InfoTemplateButton.Size = new System.Drawing.Size(75, 23);
            this.InfoTemplateButton.TabIndex = 3;
            this.InfoTemplateButton.Text = "Info";
            this.InfoTemplateButton.UseVisualStyleBackColor = true;
            // 
            // DeleteTemplateButton
            // 
            this.DeleteTemplateButton.Location = new System.Drawing.Point(494, 71);
            this.DeleteTemplateButton.Name = "DeleteTemplateButton";
            this.DeleteTemplateButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteTemplateButton.TabIndex = 4;
            this.DeleteTemplateButton.Text = "Delete";
            this.DeleteTemplateButton.UseVisualStyleBackColor = true;
            // 
            // TemplateManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 272);
            this.ControlBox = false;
            this.Controls.Add(this.DeleteTemplateButton);
            this.Controls.Add(this.InfoTemplateButton);
            this.Controls.Add(this.AddTemplateButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.QuitButton);
            this.Name = "TemplateManage";
            this.Text = "TemplateManage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button AddTemplateButton;
        private System.Windows.Forms.Button InfoTemplateButton;
        private System.Windows.Forms.Button DeleteTemplateButton;
    }
}