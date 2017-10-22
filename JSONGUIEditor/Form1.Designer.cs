namespace JSONGUIEditor
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.itemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.itemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.itemTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemView = new System.Windows.Forms.ToolStripMenuItem();
            this.itemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tview_object = new System.Windows.Forms.TreeView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.addTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemFile,
            this.itemEdit,
            this.itemTemplate,
            this.itemView,
            this.itemHelp});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(676, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // itemFile
            // 
            this.itemFile.Name = "itemFile";
            this.itemFile.Size = new System.Drawing.Size(37, 20);
            this.itemFile.Text = "&File";
            // 
            // itemEdit
            // 
            this.itemEdit.Name = "itemEdit";
            this.itemEdit.Size = new System.Drawing.Size(39, 20);
            this.itemEdit.Text = "&Edit";
            // 
            // itemTemplate
            // 
            this.itemTemplate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.selectTemplateToolStripMenuItem,
            this.addTemplateToolStripMenuItem});
            this.itemTemplate.Name = "itemTemplate";
            this.itemTemplate.Size = new System.Drawing.Size(68, 20);
            this.itemTemplate.Text = "&Template";
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.selectToolStripMenuItem.Text = "ManageTemplate";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // selectTemplateToolStripMenuItem
            // 
            this.selectTemplateToolStripMenuItem.Name = "selectTemplateToolStripMenuItem";
            this.selectTemplateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.selectTemplateToolStripMenuItem.Text = "SelectTemplate";
            this.selectTemplateToolStripMenuItem.Click += new System.EventHandler(this.selectTemplateToolStripMenuItem_Click);
            // 
            // itemView
            // 
            this.itemView.Name = "itemView";
            this.itemView.Size = new System.Drawing.Size(45, 20);
            this.itemView.Text = "&View";
            // 
            // itemHelp
            // 
            this.itemHelp.Name = "itemHelp";
            this.itemHelp.Size = new System.Drawing.Size(44, 20);
            this.itemHelp.Text = "&Help";
            // 
            // tview_object
            // 
            this.tview_object.Location = new System.Drawing.Point(12, 27);
            this.tview_object.Name = "tview_object";
            this.tview_object.Size = new System.Drawing.Size(161, 349);
            this.tview_object.TabIndex = 1;
            this.tview_object.DoubleClick += new System.EventHandler(this.tview_object_DoubleClick);
            // 
            // addTemplateToolStripMenuItem
            // 
            this.addTemplateToolStripMenuItem.Name = "addTemplateToolStripMenuItem";
            this.addTemplateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.addTemplateToolStripMenuItem.Text = "AddTemplate";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 388);
            this.Controls.Add(this.tview_object);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "Form1";
            this.Text = "JSON_Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem itemFile;
        private System.Windows.Forms.ToolStripMenuItem itemEdit;
        private System.Windows.Forms.ToolStripMenuItem itemTemplate;
        private System.Windows.Forms.ToolStripMenuItem itemView;
        private System.Windows.Forms.ToolStripMenuItem itemHelp;
        private System.Windows.Forms.TreeView tview_object;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTemplateToolStripMenuItem;
    }
}

