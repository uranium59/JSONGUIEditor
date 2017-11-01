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
        private object[] typeList;
        private List<Control> controlList;

        private Font smallFont = new Font(FontFamily.GenericSerif, 8f);

        public BaseForm()
        {
            InitializeComponent();
            MainPanel.Click += PanelBoxClick;

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

            //AppDomain.CurrentDomain.UnhandledException += Unhandled_Exception;
        }

        private void UpdateResource(string s)
        {
            JSON.Parse(ReceiveNode, JSONExceptionCatch, s);
        }

        JSONNode RootNode = null;
        public JSONNode ReceiveNode(JSONNode n)
        {
            tview_object.Nodes.Clear();
            MainPanel.Controls.Clear();

            JSONFormUtil.MakeTreeView(n, tview_object);
            //tview_object.Nodes.Add(node1);
            RootNode = n;
            //MappingKey(tview_object.TopNode, n);
            BasePanelMake(n);
            return null;
        }
        
        private void BasePanelMake(JSONNode n)
        {
            Panel p = CreateJSONNodeGroupBox(n, null);
            p.Location = new Point(0, 35);
            p.Tag = n;
            p.Name = "root";
            Button btn_value = new Button()
            {
                Text = "+",
                Font = smallFont,
                Location = new Point(45, 10),
                Size = new Size(18, 20),
                Tag = n
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
        }

        public void JSONExceptionCatch(JSONException e)
        {
            MessageBox.Show("MyHandler caught : " + e.Message);
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

        private Panel CreateJSONNodeGroupBox(JSONNode n, Panel parent)
        {
            Panel rtn = new Panel()
            {
                Height = 10,
                AutoSize = true,
                Parent = parent,
                Tag = n
            };
            //rtn.Click += PanelBoxClick;
            rtn.BackColor = FormConstValue.baseColor;

            int nowY = 5;

            if(n is JSONObject)
            {
                JSONObject o = (JSONObject)n;
                string[] keys = o.GetAllKeys();
                foreach(string s in keys)
                {
                    nowY += CreateGroupChild(o[s], s, rtn, nowY, true);
                }
            }
            else
            {
                for(int i = 0; i < n.Count; ++i)
                {
                    nowY += CreateGroupChild(n[i], i + "", rtn, nowY, false);
                }
            }

            return rtn;
        }
        private int CreateGroupChild(JSONNode n, string index, Panel target, int nowY, bool Fixed)
        {
            Panel p = new Panel()
            {
                Height = 10,
                AutoSize = true,
                Location = new Point(0, nowY),
                Tag = n,
                Parent = target,
            };
            p.Click += PanelBoxClick;
            p.BackColor = FormConstValue.baseColor;
            target.Controls.Add(p);
            TextBox tbox_key = new TextBox()
            {
                Name = "keybox",
                Width = 80,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(0, 0),
                Text = index,
                Enabled = Fixed,
                Tag = n
            };
            p.Controls.Add(tbox_key);
            ComboBox cbox_type = new ComboBox()
            {
                Width = 70,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(FormConstValue.stepX, 0),
                Tag = n
            };
            cbox_type.Items.AddRange(typeList);
            cbox_type.SelectedText = n.type.GetTypeString();
            cbox_type.SelectedIndexChanged += ChangeType;
            p.Controls.Add(cbox_type);
            if (n.type == JSONType.Object || n.type == JSONType.Array)
            {

                Label lb_value = new Label()
                {
                    Text = "Value",
                    Height = FormConstValue.keyvalueHeight,
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 3, 2)
                };
                p.Controls.Add(lb_value);
                Button btn_value = new Button()
                {
                    Text = "M",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 - 25, 0),
                    Size = new Size(18, 20),
                    Tag = n
                };
                btn_value.Click += ShowModifyForm;
                p.Controls.Add(btn_value);
                Panel g = CreateJSONNodeGroupBox(n, p);
                btn_value = new Button()
                {
                    Text = "+",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 + 10, 0),
                    Size = new Size(18, 20),
                    Tag = n
                };
                btn_value.Click += AddNewNodeButton;
                p.Controls.Add(btn_value);
                g.BackColor = FormConstValue.baseColor;
                g.Location = new Point(FormConstValue.stepX, FormConstValue.stepY);
                p.Controls.Add(g);
            }
            else
            {
                TextBox tbox_value = new TextBox()
                {
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(FormConstValue.stepX * 2, 0),
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
                p.Controls.Add(tbox_value);
                Button delete_Button = new Button()
                {
                    Text = "-",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 3 + 20, 0),
                    Size = new Size(18, 20),
                    Tag = n
                };
                p.Controls.Add(delete_Button);
            }
            nowY += p.Height;
            return nowY;
        }
        public void AddNewNode(Panel p, JSONNode n)
        {
            JSONString s = new JSONString("type a string");
            if (n is JSONArray)
            {
                n.Add(s);
                TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, n);
                TreeNode newTreenode = new TreeNode();
                newTreenode.Tag = s;
                newTreenode.Text = s.type.GetTypeString();
                t.Nodes.Add(newTreenode);
                CreateGroupChild(s, (n.Count - 1) + "", p, p.Height - p.Margin.Bottom, false);
                return;
            }

            string newnodename = "newNode";
            int trycount = 0;

            bool success = false;
            while(!success)
            {
                if(n.IsExist(newnodename + trycount))
                {
                    trycount++;
                    continue;
                }
                n.Add(newnodename + trycount, s);
                TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, n);
                TreeNode newTreenode = new TreeNode();
                newTreenode.Tag = s;
                newTreenode.Text = s.type.GetTypeString();
                t.Nodes.Add(newTreenode);
                CreateGroupChild(s, newnodename + trycount, p, p.Height, true);
                success = true;
            }
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
            ModifyForm m = new ModifyForm(n, c);
            m.baseForm = this;
            m.Show(this);
        }
        private void AddNewNodeButton(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel p = JSONFormUtil.NextPanelFind(c, (JSONNode)c.Tag);
            AddNewNode(p, (JSONNode)c.Tag);
        }
        private void ChangeType(object sender, EventArgs e)
        {
            ComboBox c = (ComboBox)sender;
            JSONNode n = (JSONNode)c.Tag;
            TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, n);
            JSONNode newnode;
            switch(c.SelectedIndex)
            {
                case 0:
                    newnode = new JSONArray();
                    break;
                case 1:
                    newnode = new JSONBool(true);
                    break;
                case 2:
                    newnode = new JSONNull();
                    break;
                case 3:
                    newnode = new JSONNumber(0);
                    break;
                case 4:
                    newnode = new JSONObject();
                    break;
                case 5:
                    newnode = new JSONString("type a string");
                    break;
                default:
                    newnode = new JSONNull();
                    break;
            }
            t.Nodes.Clear();
            t.Text = newnode.type.GetTypeString();
            t.Tag = newnode;
            JSONNode pNode = n.parent;
            for(int i = 0; i < pNode.Count; ++i)
            {
                if(ReferenceEquals(pNode[i], n))
                {
                    pNode[i] = newnode;
                    break;
                }
            }
            Panel p = (Panel)c.Parent;

            Control keybox = FindControl(p, "keybox");
            string original_key = keybox.Text;
            bool original_fixed = keybox.Enabled;

            p.Controls.Clear();

            TextBox tbox_key = new TextBox()
            {
                Name = "keybox",
                Width = 80,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(0, 0),
                Text = original_key,
                Enabled = original_fixed,
                Tag = newnode
            };
            p.Controls.Add(tbox_key);
            ComboBox cbox_type = new ComboBox()
            {
                Width = 70,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(FormConstValue.stepX, 0),
                Tag = newnode
            };
            cbox_type.Items.AddRange(typeList);
            cbox_type.SelectedText = newnode.type.GetTypeString();
            cbox_type.SelectedIndexChanged += ChangeType;
            p.Controls.Add(cbox_type);
            if (newnode.type == JSONType.Object || newnode.type == JSONType.Array)
            {

                Label lb_value = new Label()
                {
                    Text = "Value",
                    Height = FormConstValue.keyvalueHeight,
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 3, 2)
                };
                p.Controls.Add(lb_value);
                Button btn_value = new Button()
                {
                    Text = "M",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 - 25, 0),
                    Size = new Size(18, 20),
                    Tag = newnode
                };
                btn_value.Click += ShowModifyForm;
                p.Controls.Add(btn_value);
                Panel g = CreateJSONNodeGroupBox(newnode, p);
                btn_value = new Button()
                {
                    Text = "+",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 5 / 2 + 10, 0),
                    Size = new Size(18, 20),
                    Tag = newnode
                };
                btn_value.Click += AddNewNodeButton;
                p.Controls.Add(btn_value);
                g.BackColor = FormConstValue.baseColor;
                g.Location = new Point(FormConstValue.stepX, FormConstValue.stepY);
                p.Controls.Add(g);
            }
            else
            {
                TextBox tbox_value = new TextBox()
                {
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(FormConstValue.stepX * 2, 0),
                    Tag = newnode
                };
                tbox_value.TextChanged += (txt, arg) =>
                {
                    if (txt is TextBox tbox)
                    {
                        JSONNode tagNode = tbox.Tag as JSONNode;
                        tagNode.value = tbox.Text;
                    }
                };
                tbox_value.Text = newnode.value;
                p.Controls.Add(tbox_value);
                Button delete_Button = new Button()
                {
                    Text = "-",
                    Font = smallFont,
                    Location = new Point(FormConstValue.stepX * 3 + 20, 0),
                    Size = new Size(18, 20),
                    Tag = newnode
                };
                p.Controls.Add(delete_Button);
            }
        }
        private void RemoveNode(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            JSONNode n = (JSONNode)c.Tag;
            TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, (JSONNode)c.Tag);
            TreeNode tP = t.Parent;
            tP.Nodes.Remove(t);
            Control cP = c.Parent;
            Control cPP = cP.Parent;
            cPP.Controls.Remove(cP);

            JSONNode pNode = n.parent;
            for (int i = 0; i < pNode.Count; ++i)
            {
                if (ReferenceEquals(pNode[i], n))
                {
                    //pNode[i] = newnode;
                    break;
                }
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
            if(o.FileName != "")
            {
                string s = File.ReadAllText(o.FileName);
                JSON.Parse(ReceiveNode, JSONExceptionCatch, s );
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

        private void vToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewAll v = new ViewAll(RootNode);
            v.baseForm = this;
            v.Show(this);
        }





        public static Control FindControl(Panel root, string name)
        {
            if (root == null) throw new ArgumentNullException("root");
            foreach (Control child in root.Controls)
            {
                if (child.Name == name) return child;
            }
            return null;
        }
    }
}
