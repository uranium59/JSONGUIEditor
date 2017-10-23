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

        public ModifyForm(JSONNode n, Control target)
        {
            InitializeComponent();
            textBox1.Text = n.Stringify();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
