using JSONGUIEditor.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor
{
    using JSONGUIEditor.TemplateForm;
    public partial class Form1 : Form
    {
        private List<string> typeList;
        private List<Control> controlList;

        private Font smallFont = new Font(FontFamily.GenericSerif, 8f);

        public Form1()
        {
            InitializeComponent();

            typeList = new List<string>
            {
                "Object",
                "String",
                "number"
            };
            controlList = new List<Control>();
        }

        //[Test, Order(7)]
        //public void DepthTest()
        //{
        //    //Depth 는 최상단 루트 오브젝트에 도달하기 위해 거쳐야 하는 단계 수를 의미합니다.
        //    //Depth 는 최상단을 0으로(자기자신이므로 거리가 없으므로), 한단계씩 내려갈때 1씩 증가합니다.
        //    Assert.IsTrue(n.depth == 0);
        //    Assert.IsTrue(n["b"].depth == 1);
        //    JSONNode temp = new JSONObject();
        //    temp["t"] = 123;
        //    Assert.IsTrue(temp["t"].depth == 1);
        //    n["e"] = temp;
        //    //다른 오브젝트에 해당 오브젝트를 할당하면 자동적으로 depth가 증가합니다.
        //    Assert.IsTrue(temp["t"].depth == 2);
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            JSONNode tempNode1 = new JSONObject();
            JSONNode tempNode2 = new JSONObject();
            JSONNode tempNode3 = new JSONObject();

            tempNode1.depth = 0;
            tempNode2.depth = 1;
            tempNode3.depth = 2;

            TreeNode node1 = new TreeNode(tempNode1.ToString());
            TreeNode node2 = new TreeNode(tempNode2.ToString());
            TreeNode node3 = new TreeNode(tempNode3.ToString());

            node1.Tag = tempNode1;
            node2.Tag = tempNode2;
            node3.Tag = tempNode3;

            node1.Nodes.Add(node2);
            node2.Nodes.Add(node3);
            */
            JSONNode n = new JSONObject();
            n["test"] = new JSONNumber(1234);
            n["asdf"] = new JSONObject();
            n["asdf"]["123"] = new JSONBool(true);
            JSONFormUtil.MakeTreeView(n, tview_object);
            //tview_object.Nodes.Add(node1);
        }

        private void tview_object_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = tview_object.SelectedNode;
            JSONNode tempNode = node.Tag as JSONNode;

            if (tempNode == null)
            {
                return;
            }

            foreach (Control control in controlList)
            {
                this.Controls.Remove(control);
            }
            controlList.Clear();

            int posX = 200;
            int posY = 50;
            int stepX = 110;
            int stepY = 25;

            this.Width = 550 + tempNode.depth * stepX;

            for (int i = 0; i <= tempNode.depth; i++)
            {
                Label lb_key = new Label()
                {
                    Text = "Key",
                    Height = FormConstValue.keyvalueHeight,
                    Font = smallFont,
                    Location = new Point(posX, posY)
                };
                TextBox tbox_key = new TextBox()
                {
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(posX, posY + stepY)
                };
                ComboBox cbox_type = new ComboBox()
                {
                    DataSource = typeList,
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(posX + stepX, posY + stepY)
                };
                Label lb_value = new Label()
                {
                    Text = "Value",
                    Height = FormConstValue.keyvalueHeight,
                    Font = smallFont,
                    Location = new Point(posX + stepX * 2, posY)
                };
                TextBox tbox_value = new TextBox()
                {
                    Width = 100,
                    Height = FormConstValue.inputboxHeight,
                    Location = new Point(posX + stepX * 2, posY + stepY)
                };

                controlList.Add(lb_key);
                controlList.Add(tbox_key);
                controlList.Add(cbox_type);
                controlList.Add(lb_value);
                controlList.Add(tbox_value);

                posX += stepX;
                posY += stepY;
            }

            foreach (Control control in controlList)
            {
                this.Controls.Add(control);
            }
        }


        GroupBox nowSelectedNode = null;

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
    }
}
