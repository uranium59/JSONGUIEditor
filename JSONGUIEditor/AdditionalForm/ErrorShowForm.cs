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
    using JSONGUIEditor.Parser.Exception;
    public partial class ErrorShowForm : Form
    {
        public ErrorShowForm()
        {
        }
        public ErrorShowForm(string s, JSONException e)
        {
            InitializeComponent();
            int startPosition = Math.Max(e.position - 100, 0);
            textbox.Text = s.Substring(startPosition, Math.Min(201, s.Length - startPosition));
            textbox.Select(100, 101);
            textbox.SelectionColor = Color.Red;
            errorLabel.Text = e.Message;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
