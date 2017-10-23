using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor
{
    using JSONGUIEditor.TemplateForm;
    using JSONGUIEditor.Parser;
    using JSONGUIEditor.Parser.State;
    using JSONGUIEditor.AdditionalForm;

    public partial class BaseForm : Form
    {
        private object[] typeList;
        private List<Control> controlList;

        private Font smallFont = new Font(FontFamily.GenericSerif, 8f);

        public BaseForm()
        {
            InitializeComponent();

            typeList = new object[]
            {
                "Array",
                "Bool",
                "Null",
                "Number",
                "Object",
                "String"
            };
            controlList = new List<Control>();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            JSONNode n = new JSONObject();
            n["test"] = new JSONNumber(1234);
            n["asdf"] = new JSONObject();
            n["asdf"]["123"] = new JSONBool(true);
            ReceiveNode(n);
        }

        private void UpdateResource(string s)
        {
            tview_object.Nodes.Clear();
            MainPanel.Controls.Clear();

            JSON.Parse(ReceiveNode, s);
        }

        JSONNode RootNode = null;
        private JSONNode ReceiveNode(JSONNode n)
        {
            JSONFormUtil.MakeTreeView(n, tview_object);
            //tview_object.Nodes.Add(node1);
            RootNode = n;
            MappingKey(tview_object.TopNode, n);

            Panel p = CreateJSONNodeGroupBox(n, null, 0);
            p.Location = new Point(0, 35);
            p.Tag = n;
            p.Name = "root";
            Button btn_value = new Button()
            {
                Text = "+",
                Font = smallFont,
                Location = new Point(30, 10),
                Size = new Size(18, 20)
            };
            MainPanel.Controls.Add(btn_value);
            btn_value = new Button()
            {
                Text = "M",
                Font = smallFont,
                Location = new Point(10, 10),
                Size = new Size(18, 20),
                Tag = n
            };
            btn_value.Click += ShowModifyForm;
            MainPanel.Controls.Add(btn_value);
            MainPanel.Controls.Add(p);
            MainPanel.Tag = RootNode;
            return null;
        }

        Dictionary<string, JSONNode> mappingKey = new Dictionary<string, JSONNode>();

        private void MappingKey(TreeNode topNode, JSONNode node)
        {
            if (topNode == null)
            {
                return;
            }

            IEnumerator enumerator = topNode.Nodes.GetEnumerator();

            if (node is JSONObject json)
            {
                int index = 0;
                string[] keys = json.GetAllKeys();
                foreach (JSONNode n in node)
                {
                    enumerator.MoveNext();

                    mappingKey.Add(keys[index++], n);

                    if (n is JSONObject j)
                    {
                        MappingKey((TreeNode)enumerator.Current, j);
                    }
                }
            }
        }
        private void tview_object_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = tview_object.SelectedNode;
            JSONNode tempNode = node.Tag as JSONNode;
            FocusOnNode(tempNode, MainPanel);
        }

        private bool FocusOnNode(JSONNode n, Control target)
        {
            foreach(Control c in target.Controls)
            {
                if (ReferenceEquals(c.Tag, n))
                {
                    c.Focus();
                    return true;
                }
                if (FocusOnNode(n, c)) return true;
            }
            return false;
        }

        private Panel CreateJSONNodeGroupBox(JSONNode n, Panel parent, int intend = 0)
        {
            Panel rtn = new Panel()
            {
                Height = FormConstValue.inputboxHeight + FormConstValue.keyvalueHeight + FormConstValue.MarginHeight,
                AutoSize = true,
                Tag = n,
                Parent = parent,
            };
            rtn.Click += PanelBoxClick;
            rtn.BackColor = Color.Empty;

            int nowY = 5;

            if(n is JSONObject)
            {
                JSONObject o = (JSONObject)n;
                string[] keys = o.GetAllKeys();
                foreach(string s in keys)
                {
                    nowY += CreateGroupChild(o[s], s, rtn, nowY, intend, true);
                    nowY += FormConstValue.stepY;
                }
            }
            else
            {
                for(int i = 0; i < n.Count; ++i)
                {
                    nowY += CreateGroupChild(n[i], i + "", rtn, nowY, intend, false);
                    nowY += FormConstValue.stepY;
                }
            }

            return rtn;
        }
        private int CreateGroupChild(JSONNode n, string index, Panel target, int nowY, int intend, bool Fixed)
        {
            TextBox tbox_key = new TextBox()
            {
                Width = 100,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(0, nowY),
                Text = index,
                Enabled = Fixed
            };
            target.Controls.Add(tbox_key);
            ComboBox cbox_type = new ComboBox()
            {
                Width = 100,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(FormConstValue.stepX, nowY)
            };
            cbox_type.Items.AddRange(typeList);
            cbox_type.SelectedText = n.type.GetTypeString();
            target.Controls.Add(cbox_type);
            if (n.type == JSONType.Object || n.type == JSONType.Array)
            {

                Label lb_value = new Label()
                {
                    Text = "Value",
                    Height = FormConstValue.keyvalueHeight,
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 7 / 3, nowY + 2)
                };
                target.Controls.Add(lb_value);
                Button btn_value = new Button()
                {
                    Text = "+",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 - 38, nowY),
                    Size = new Size(18, 20)
                };
                target.Controls.Add(btn_value);
                btn_value = new Button()
                {
                    Text = "M",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 - 59, nowY),
                    Size = new Size(18, 20),
                    Tag = n
                };
                btn_value.Click += ShowModifyForm;
                target.Controls.Add(btn_value);
                Panel g = CreateJSONNodeGroupBox(n, target, intend + 1);
                nowY += FormConstValue.stepY;
                g.BackColor = FormConstValue.baseColor;
                g.Location = new Point(FormConstValue.stepX, nowY);
                target.Controls.Add(g);
            }
            else
            {
                TextBox tbox_value = new TextBox()
                {
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(FormConstValue.stepX * 2, nowY),
                    Tag = n
                };
                tbox_value.TextChanged += (txt, arg) =>
                {
                    if (txt is TextBox tbox)
                    {
                        JSONNode tagNode = tbox.Tag as JSONNode;
                        tagNode.value = tbox.Text;
                    }
                };
                tbox_value.Text = n.value;
                target.Controls.Add(tbox_value);
            }
            return nowY;
        }

        private void PanelBoxClick(object sender, EventArgs e)
        {
            Panel g = (Panel)sender;
            if (g.BackColor == FormConstValue.baseColor)
            {
                if(nowSelectedNode != null)
                {
                    nowSelectedNode.BackColor = FormConstValue.baseColor;
                }
                g.BackColor = Color.FromArgb(64, 0, 0, 255);
                nowSelectedNode = g;
            }
            else
            {
                g.BackColor = FormConstValue.baseColor;
                nowSelectedNode = null;
            }
        }
        private void ShowModifyForm(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            JSONNode n = (JSONNode)c.Tag;
            new ModifyForm(n, c).Show();
        }

        Panel nowSelectedNode = null;

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateManage f = new TemplateManage();
            f.Show(this);
        }

        private void selectTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateSelect f = new TemplateSelect(nowSelectedNode);
            if(!f.IsDisposed)
                f.Show(this);
        }


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
                if(s.FileName != "")
                {
                    File.WriteAllText(filePosition, RootNode.Stringify());
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
