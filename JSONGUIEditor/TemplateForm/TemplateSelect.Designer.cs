namespace JSONGUIEditor.TemplateForm
{
    partial class TemplateSelect
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.TemplateList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TemplateObjectList = new System.Windows.Forms.ListBox();
            this.TemplateName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TemplateDescription = new System.Windows.Forms.Label();
            this.TemplateTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(493, 314);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(412, 314);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 1;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // TemplateList
            // 
            this.TemplateList.FormattingEnabled = true;
            this.TemplateList.ItemHeight = 12;
            this.TemplateList.Location = new System.Drawing.Point(13, 13);
            this.TemplateList.Name = "TemplateList";
            this.TemplateList.Size = new System.Drawing.Size(278, 328);
            this.TemplateList.TabIndex = 2;
            this.TemplateList.SelectedIndexChanged += new System.EventHandler(this.TemplateList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(307, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "TemplateName";
            // 
            // TemplateObjectList
            // 
            this.TemplateObjectList.FormattingEnabled = true;
            this.TemplateObjectList.ItemHeight = 12;
            this.TemplateObjectList.Location = new System.Drawing.Point(309, 64);
            this.TemplateObjectList.Name = "TemplateObjectList";
            this.TemplateObjectList.Size = new System.Drawing.Size(259, 124);
            this.TemplateObjectList.TabIndex = 4;
            // 
            // TemplateName
            // 
            this.TemplateName.AutoSize = true;
            this.TemplateName.Location = new System.Drawing.Point(309, 29);
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.Size = new System.Drawing.Size(89, 12);
            this.TemplateName.TabIndex = 5;
            this.TemplateName.Text = "name text on it";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Description";
            // 
            // TemplateDescription
            // 
            this.TemplateDescription.AutoSize = true;
            this.TemplateDescription.Location = new System.Drawing.Point(309, 211);
            this.TemplateDescription.Name = "TemplateDescription";
            this.TemplateDescription.Size = new System.Drawing.Size(125, 12);
            this.TemplateDescription.TabIndex = 7;
            this.TemplateDescription.Text = "Description Text on it";
            // 
            // TemplateTime
            // 
            this.TemplateTime.AutoSize = true;
            this.TemplateTime.Location = new System.Drawing.Point(309, 319);
            this.TemplateTime.Name = "TemplateTime";
            this.TemplateTime.Size = new System.Drawing.Size(38, 12);
            this.TemplateTime.TabIndex = 8;
            this.TemplateTime.Text = "label5";
            // 
            // TemplateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 349);
            this.ControlBox = false;
            this.Controls.Add(this.TemplateTime);
            this.Controls.Add(this.TemplateDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TemplateName);
            this.Controls.Add(this.TemplateObjectList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TemplateList);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.CancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TemplateSelect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.ListBox TemplateList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox TemplateObjectList;
        private System.Windows.Forms.Label TemplateName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TemplateDescription;
        private System.Windows.Forms.Label TemplateTime;
    }
}