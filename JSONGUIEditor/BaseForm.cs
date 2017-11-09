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
            MainPanel.BackColor = FormConstValue.baseColor;
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
            n["asd4"] = new JSONObject();
            n["asdf"]["123"] = new JSONBool(true);
            ReceiveNode(n);

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
            Panel p = CreateJSONNodeGroupBox(n, MainPanel);
            p.Location = new Point(0, 35);
            p.Tag = n;
            Button btn_value = new Button()
            {
                Text = "+",
                Font = smallFont,
                Location = new Point(45, 10),
                Size = new Size(18, 20),
                Tag = n
            };
            MainPanel.Controls.Add(btn_value);
            btn_value.Click += AddNewNodeButton;
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
            MainPanel.Tag = n;
        }

        public void JSONExceptionCatch(JSONException e)
        {
            ErrorShowForm eform = new ErrorShowForm(JSONParseThread.s, e);
            eform.Show();
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
                Height = 0,
                AutoSize = true,
                Parent = parent,
                Tag = n,
                Padding = new Padding(0, 0, 0, 0)
            };
            //rtn.Click += PanelBoxClick;
            rtn.BackColor = FormConstValue.baseColor;

            int nowY = 2;

            if(n is JSONObject)
            {
                JSONObject o = (JSONObject)n;
                string[] keys = o.GetAllKeys();
                foreach(string s in keys)
                {
                    nowY = CreateGroupChild(o[s], s, rtn, nowY, true);
                }
            }
            else
            {
                for(int i = 0; i < n.Count; ++i)
                {
                    nowY = CreateGroupChild(n[i], i + "", rtn, nowY, false);
                }
            }

            return rtn;
        }
        private int CreateGroupChild(JSONNode n, string index, Panel target, int nowY, bool Fixed)
        {
            Panel p = new Panel()
            {
                Height = 0,
                AutoSize = true,
                Location = new Point(0, nowY),
                Tag = n,
                Parent = target,
                Padding = new Padding(0, 0, 0, 0)
            };
            p.Click += PanelBoxClick;
            p.BackColor = FormConstValue.baseColor;
            target.Controls.Add(p);
            p.Controls.Add(CreateKeyBox(n, index, Fixed));
            p.Controls.Add(CreateComboBox(n));
            p.Controls.Add(CreateDeleteButton(n));
            if (n.type == JSONType.Object || n.type == JSONType.Array)
            {
                p.Controls.Add(CreateValueLabel());
                p.Controls.Add(CreateModifyButton(n));
                Panel g = CreateJSONNodeGroupBox(n, p);
                p.Controls.Add(CreateNewNodeButton(n));
                g.BackColor = FormConstValue.baseColor;
                g.Location = new Point(FormConstValue.stepX, FormConstValue.stepY);
                p.Controls.Add(g);
            }
            else
            {
                p.Controls.Add(CreateValueTextBox(n));
            }
            nowY += p.Height;
            return nowY;
        }
        public void AddNewNode(Panel p, JSONNode n)
        {
            JSONString s = new JSONString("type a string");
            string key = n.Add(s);
            TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, n);
            TreeNode newTreenode = JSONFormUtil.TreeNodeMake(s);
            t.Nodes.Add(newTreenode);
            CreateGroupChild(s, key, p, p.Height - p.Margin.Bottom, (n is JSONObject)?true:false);
            PanelReSort(p);
            return;
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
            p.Tag = newnode;

            Control keybox = FindControl(p, "keybox");
            string original_key = keybox.Text;
            bool original_fixed = keybox.Enabled;

            p.Controls.Clear();
            
            p.Controls.Add(CreateKeyBox(newnode, original_key, original_fixed));
            p.Controls.Add(CreateComboBox(newnode));
            p.Controls.Add(CreateDeleteButton(newnode));
            if (newnode.type == JSONType.Object || newnode.type == JSONType.Array)
            {
                p.Controls.Add(CreateValueLabel());
                p.Controls.Add(CreateModifyButton(newnode));
                Panel g = CreateJSONNodeGroupBox(newnode, p);
                p.Controls.Add(CreateNewNodeButton(newnode));
                g.BackColor = FormConstValue.baseColor;
                g.Location = new Point(FormConstValue.stepX, FormConstValue.stepY);
                p.Controls.Add(g);
            }
            else
            {
                p.Controls.Add(CreateValueTextBox(newnode));
            }
            PanelReSort(p);
        }

        private void KeyChange(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            JSONNode n = (JSONNode)c.Tag;
            JSONNode pn = n.parent;
            for (int i = 0; i < pn.Count; ++i)
            {
                if (ReferenceEquals(pn[i], n))
                {
                    pn.remove(i);
                    try
                    {
                        pn.Add(c.Text, n);
                    }
                    catch
                    {
                        MessageBox.Show("동일한 키는 사용할 수 없습니다");
                        pn.Add(c.Text + "duplicated", n);
                        c.Text = c.Text + "duplicated";
                        return;
                    }
                    break;
                }
            }

        }

        private void RemoveButton(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Control p = c.Parent;
            RemoveNode(p, e);
        }
        private void RemoveNode(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            JSONNode n = (JSONNode)c.Tag;
            TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, (JSONNode)c.Tag);
            TreeNode tP = t.Parent;
            if(tP == null)
            {
                MessageBox.Show("최상위 노드는 지울 수 없습니다");
                return;
            }
            tP.Nodes.Remove(t);
            Control cP = c.Parent;
            cP.Controls.Remove(c);

            JSONNode pNode = n.parent;
            for (int i = 0; i < pNode.Count; ++i)
            {
                if (ReferenceEquals(pNode[i], n))
                {
                    pNode.remove(i);
                    PanelReSort(cP);
                    break;
                }
            }
        }

        private void PanelReSort(Control p)
        {
            Control.ControlCollection cc = p.Controls;
            if (cc.Count != 0)
            {
                int i;
                for (i = 0; i < cc.Count; ++i)
                {
                    if (cc[i] is Panel) break;
                }
                int yval = i == 0 ? 2 :(i == cc.Count? cc.Count:cc[i].Location.Y);
                for (; i < cc.Count; ++i)
                {
                    cc[i].Location = new Point(cc[i].Location.X, yval);
                    yval += cc[i].Height;
                }
                p.Height = yval;
            }
            else
            {
                p.Height = 0;
            }
            if (p.Parent is Panel) PanelReSort(p.Parent);
        }

        Panel nowSelectedNode = null;

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateManage f = new TemplateManage();
            f.Show(this);
        }
        private void selectTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateChoice f = new TemplateChoice();
            if (!f.IsDisposed)
            {
                // 2017.11.02 박성규
                //Select Template로 부터 Select 버튼클릭시 Json 문자열 전달받기 위한 이벤트 핸들러
                //f.OnSelectTemplate += F_OnSelectTemplate;
                f.baseForm = this;
                f.Show(this);
            }
        }
        public void AddTemplateNodeToBase(JSONNode n)
        {
            Panel target;
            if (nowSelectedNode == null)
            {
                target = (Panel)MainPanel.Controls[2];
            }
            else
                target = (Panel)nowSelectedNode.Controls[nowSelectedNode.Controls.Count - 1];
            JSONNode node = (JSONNode)target.Tag;
            TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, node);
            string key = node.Add(n);
            TreeNode newTreenode = JSONFormUtil.TreeNodeMake(n);
            t.Nodes.Add(newTreenode);

            CreateGroupChild(n, key, target, target.Height, node.type == JSONType.Object ? true : false);

            PanelReSort(target);
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

        private void addTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(nowSelectedNode == null)return;
            JSONNode n = (JSONNode)nowSelectedNode.Tag;
            if(n.type != JSONType.Object && n.type != JSONType.Array)
            {
                MessageBox.Show("배열이나 오브젝트만 템플릿으로 추가할 수 있습니다");
                return;
            }
            TemplateAdd templateAdd = new TemplateAdd(((JSONNode)nowSelectedNode.Tag).Stringify());
            templateAdd.ShowDialog(this);
        }
    }
}
