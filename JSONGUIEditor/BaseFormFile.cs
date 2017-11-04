using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor
{
    using JSONGUIEditor.TemplateForm;
    using JSONGUIEditor.Parser;
    using JSONGUIEditor.Parser.Exception;
    using JSONGUIEditor.Parser.State;
    using JSONGUIEditor.AdditionalForm;

    public partial class BaseForm : Form
    {
        private bool isChanged { get; set; } = false;

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult d = MessageBox.Show("변경사항이 저장되지 않았습니다. 저장하시겠습니까?", "경고", MessageBoxButtons.YesNoCancel);
                if (d == DialogResult.Cancel)
                    return;
                if (d == DialogResult.No)
                {
                    Application.Exit();
                    return;
                }
                else
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            else
                Application.Exit();
        }

        private string filePosition = "";

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "JSON File|*.json";
            s.CheckFileExists = true;
            s.ShowDialog();
            if (s.FileName != "")
            {
                File.WriteAllText(filePosition, RootNode.Stringify());
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(filePosition))
            {
                File.WriteAllText(filePosition, RootNode.Stringify());
            }
            else
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "JSON File|*.json";
                s.CheckFileExists = true;
                s.ShowDialog();
                if (s.FileName != "")
                {
                    File.WriteAllText(filePosition, RootNode.Stringify());
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult d = MessageBox.Show("변경사항이 저장되지 않았습니다. 저장하시겠습니까?", "경고", MessageBoxButtons.YesNoCancel);
                if (d == DialogResult.Cancel)
                    return;
                if (d == DialogResult.No)
                {
                    Application.Exit();
                    return;
                }
                else
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }

            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "JSON File|*.json";
            o.ReadOnlyChecked = true;
            o.ShowDialog();
            if (o.FileName != "")
            {
                string s = File.ReadAllText(o.FileName);
                JSON.Parse(ReceiveNode, JSONExceptionCatch, s);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChanged)
            {
                DialogResult d = MessageBox.Show("변경사항이 저장되지 않았습니다. 저장하시겠습니까?", "경고", MessageBoxButtons.YesNoCancel);
                if (d == DialogResult.Cancel)
                    return;
                if (d == DialogResult.No)
                {
                    Application.Exit();
                    return;
                }
                else
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            else
                UpdateResource("{}");
        }
    }
}
