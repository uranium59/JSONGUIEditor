using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor.AdditionalForm
{
    using JSONGUIEditor.Parser;
    public partial class ViewAll : Form
    {
        public BaseForm baseForm { get; set; }

        public ViewAll()
        {
            throw new NotImplementedException();
        }

        public ViewAll(JSONNode n)
        {
            InitializeComponent();
            textBox1.Text = n.Stringify();
            textBox1.Focus();
            textBox1.SelectionLength = 0;
            KeyPreview = true;

            this.KeyPress += ViewAll_KeyPress;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool _modified = false;

        private bool CloseFormAfterModify()
        {
            if (_modified)
            {
                DialogResult d = MessageBox.Show("변경된 내용을 적용하시겠습니까?", "경고", MessageBoxButtons.OKCancel);
                if (d == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    ModifyMain();
                }
            }
            return true;
        }

        private void ModifyMain()
        {
            JSON.Parse(ModifyFinish, baseForm.JSONExceptionCatch, textBox1.Text);
            this.Close();
        }
        private JSONNode ModifyFinish(JSONNode n)
        {
            return baseForm.ReceiveNode(n);
        }

        private void ViewAll_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                if(CloseFormAfterModify())
                    this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _modified = true;
        }

        private void Modify_Click(object sender, EventArgs e)
        {
            ModifyMain();
        }
    }
}
