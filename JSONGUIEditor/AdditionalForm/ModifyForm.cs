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
    public partial class ModifyForm : Form
    {
        public ModifyForm()
        {
            throw new NotImplementedException(); //Dont Use This!
        }
        private bool _modified = false;
        private Control _Target = null;

        public ModifyForm(JSONNode n, Control target)
        {
            InitializeComponent();
            textBox1.Text = n.Stringify();
            _Target = target;

            this.KeyPreview = true;
            this.KeyPress += ModifyForm_KeyPress;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _modified = true;
        }

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
            this.Close();
        }

        private void ModifyForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                if (CloseFormAfterModify())
                    this.Close();
            }
        }
    }
}
