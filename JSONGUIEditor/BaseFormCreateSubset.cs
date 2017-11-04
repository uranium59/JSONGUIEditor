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
        public ComboBox CreateComboBox(JSONNode n)
        {
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
            return cbox_type;
        }
        public TextBox CreateKeyBox(JSONNode n, string key, bool isfixed)
        {
            TextBox tbox_key = new TextBox()
            {
                Name = "keybox",
                Width = 80,
                Height = FormConstValue.inputboxHeight,
                Location = new Point(0, 0),
                Text = key,
                Enabled = isfixed,
                Tag = n
            };
            tbox_key.TextChanged += KeyChange;
            return tbox_key;
        }
        public Button CreateDeleteButton(JSONNode n)
        {
            Button delete_Button = new Button()
            {
                Text = "-",
                Font = smallFont,
                Location = new Point(FormConstValue.stepX * 3 + 20, 0),
                Size = new Size(18, 20),
                Tag = n
            };
            delete_Button.Click += RemoveButton;
            return delete_Button;
        }
        public Label CreateValueLabel()
        {
            Label lb_value = new Label()
            {
                Text = "Value",
                Height = FormConstValue.keyvalueHeight,
                Font = smallFont,
                Location = new Point(FormConstValue.stepX * 3 - 15, 2)
            };
            return lb_value;
        }
        public Button CreateModifyButton(JSONNode n)
        {
            Button btn_value = new Button()
            {
                Text = "M",
                Font = smallFont,
                Location = new Point(FormConstValue.stepX * 5 / 2 - 30, 0),
                Size = new Size(18, 20),
                Tag = n
            };
            btn_value.Click += ShowModifyForm;
            return btn_value;
        }
        public Button CreateNewNodeButton(JSONNode n)
        {
            Button btn_value = new Button()
            {
                Text = "+",
                Font = smallFont,
                Location = new Point(FormConstValue.stepX * 5 / 2 + 5, 0),
                Size = new Size(18, 20),
                Tag = n
            };
            btn_value.Click += AddNewNodeButton;
            return btn_value;
        }
        public TextBox CreateValueTextBox(JSONNode n)
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
            return tbox_value;
        }
    }
}