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

    public partial class Form1 : Form
    {
        private object[] typeList;
        private List<Control> controlList;

        private Font smallFont = new Font(FontFamily.GenericSerif, 8f);

        public Form1()
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
            MainPanel.Controls.Add(CreateJSONNodeGroupBox(n, null, 0));
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
                    TextBox tbox_key = new TextBox()
                    {
                        Width = 100,
                        Height = FormConstValue.inputboxHeight,
                        Location = new Point(0, nowY),
                        Tag = o[s],
                        Text = s
                    };
                    rtn.Controls.Add(tbox_key);
                    ComboBox cbox_type = new ComboBox()
                    {
                        Width = 100,
                        Height = FormConstValue.inputboxHeight,
                        Location = new Point(FormConstValue.stepX, nowY)
                    };
                    cbox_type.Items.AddRange(typeList);
                    cbox_type.SelectedText = o[s].type.GetTypeString();
                    rtn.Controls.Add(cbox_type);
                    if (o[s].type == JSONType.Object || o[s].type == JSONType.Array)
                    {
                        Label lb_value = new Label()
                        {
                            Text = "Value",
                            Height = FormConstValue.keyvalueHeight,
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 7 / 3, nowY + 2)
                        };
                        rtn.Controls.Add(lb_value);
                        Button btn_value = new Button()
                        {
                            Text = "+",
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 5 / 2 - 38, nowY),
                            Size = new Size(18, 20)
                        };
                        rtn.Controls.Add(btn_value);
                        btn_value = new Button()
                        {
                            Text = "M",
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 5 / 2 - 59, nowY),
                            Size = new Size(18, 20)
                        };
                        rtn.Controls.Add(btn_value);
                        Panel g = CreateJSONNodeGroupBox(o[s], rtn, intend + 1);
                        nowY += FormConstValue.stepY;
                        g.Location = new Point(FormConstValue.stepX, nowY);
                        rtn.Controls.Add(g);
                    }
                    else
                    {
                        TextBox tbox_value = new TextBox()
                        {
                            Width = 100,
                            Height = FormConstValue.inputboxHeight,
                            Location = new Point(FormConstValue.stepX * 2, nowY),
                            Tag = o[s]
                        };
                        tbox_value.TextChanged += (txt, arg) =>
                        {
                            if (txt is TextBox tbox)
                            {
                                JSONNode tagNode = tbox.Tag as JSONNode;
                                tagNode.value = tbox.Text;
                            }
                        };
                        tbox_value.Text = o[s].value;
                    }
                    nowY += FormConstValue.stepY;
                }
            }
            else
            {
                for(int i = 0; i < n.Count; ++i)
                {
                    TextBox tbox_key = new TextBox()
                    {
                        Width = 100,
                        Height = FormConstValue.inputboxHeight,
                        Location = new Point(0, nowY),
                        Tag = n[i],
                        Text = i + "",
                        Enabled = false
                    };
                    rtn.Controls.Add(tbox_key);
                    ComboBox cbox_type = new ComboBox()
                    {
                        Width = 100,
                        Height = FormConstValue.inputboxHeight,
                        Location = new Point(FormConstValue.stepX, nowY)
                    };
                    cbox_type.Items.AddRange(typeList);
                    cbox_type.SelectedText = n[i].type.GetTypeString();
                    rtn.Controls.Add(cbox_type);
                    if (n[i].type == JSONType.Object || n[i].type == JSONType.Array)
                    {
                        Label lb_value = new Label()
                        {
                            Text = "Value",
                            Height = FormConstValue.keyvalueHeight,
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 7 / 3, nowY + 2)
                        };
                        rtn.Controls.Add(lb_value);
                        Button btn_value = new Button()
                        {
                            Text = "+",
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 5 / 2 - 38, nowY),
                            Size = new Size(18, 20)
                        };
                        rtn.Controls.Add(btn_value);
                        btn_value = new Button()
                        {
                            Text = "M",
                            Font = smallFont,
                            Location = new Point(FormConstValue.stepX * 5 / 2 - 59, nowY),
                            Size = new Size(18, 20)
                        };
                        rtn.Controls.Add(btn_value);

                        Panel g = CreateJSONNodeGroupBox(n[i], rtn, intend + 1);
                        nowY += FormConstValue.stepY;
                        g.Location = new Point(FormConstValue.stepX, nowY);
                        rtn.Controls.Add(g);
                    }
                    else
                    {
                        TextBox tbox_value = new TextBox()
                        {
                            Width = 100,
                            Height = FormConstValue.inputboxHeight,
                            Location = new Point(FormConstValue.stepX * 2, nowY),
                            Tag = n[i]
                        };
                        tbox_value.TextChanged += (txt, arg) =>
                        {
                            if (txt is TextBox tbox)
                            {
                                JSONNode tagNode = tbox.Tag as JSONNode;
                                tagNode.value = tbox.Text;
                            }
                        };
                        tbox_value.Text = n[i].value;
                    }
                    nowY += FormConstValue.stepY;
                }
            }

            return rtn;
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
