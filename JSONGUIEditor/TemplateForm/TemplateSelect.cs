using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor.TemplateForm
{
    using JSONGUIEditor.Parser;
    //TODO : finish when main form select object completed


    public partial class TemplateSelect : Form
    {
        public const string AlertMessageNoSelect = "선택한 오브젝트가 없을 경우 루트 오브젝트를 대체합니다.\n계속하시겠습니까?";
        private JSONArray _array;

        public TemplateSelect()
        {//Dont Use This!!!
            throw new NotImplementedException();
        }
        Panel nowSelected = null;
        public TemplateSelect(Panel g)
        {//maybe move this to another position...
            if(g == null)
            {
                DialogResult ynResult = MessageBox.Show(AlertMessageNoSelect, "경고", MessageBoxButtons.OKCancel);
                if(ynResult == DialogResult.OK)
                {
                    InitializeComponent();
                    MakeList();
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            else
            {
                InitializeComponent();
                MakeList();
            }
            nowSelected = g;
        }
        
        private const string TemplateFile = "Template.json";

        public void MakeList()
        {
            string templateJSON = "";
            try
            {
                templateJSON = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + TemplateFile);
            }
            catch(FileNotFoundException)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + TemplateFile, "[]");
                templateJSON = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + TemplateFile);
            }
            JSON.Parse(new JSON.ParseCallback(AddJSONNodeToList), templateJSON);
        }

        private JSONNode AddJSONNodeToList(JSONNode n)
        {
            if(n.type != Parser.State.JSONType.Array)
            {
                MessageBox.Show("잘못된 형태의 템플릿 파일입니다\n템플릿 파일을 초기화합니다");
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + TemplateFile, "[]");
                MakeList();
            }

            _array = (JSONArray)n;
            foreach (JSONNode obj in _array)
            {
                TemplateList.Items.Add(obj["name"]);
            }
            return null;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            JSONNode n = _array[TemplateList.SelectedIndex];
            TemplateTime.Text = n["date"];
            TemplateName.Text = n["name"];
            TemplateDescription.Text = n["description"];
            TemplateObjectList.Items.Clear();
            JSON.Parse(new JSON.ParseCallback(MakeTemplateObjectList), n["template"].value.Replace("\\\"", "\""));
        }
        private JSONNode MakeTemplateObjectList(JSONNode n)
        {
            return MakeTemplateObjectList(n, 0);
        }
        private JSONNode MakeTemplateObjectList(JSONNode n, int depth)
        {
            string intend = "";
            for(int i = 0; i < depth; ++i)
            {
                intend += "  ";
            }
            if (n.type == Parser.State.JSONType.Array)
            {
                TemplateObjectList.Items.Add(intend + "배열");
                return null;
            }
            JSONObject o = (JSONObject)n;
            string[] keys = o.GetAllKeys();
            foreach(string k in keys)
            {
                TemplateObjectList.Items.Add(intend + k + " : " + Parser.State.JSONTypeFunc.GetTypeString(o[k].type));
                if(o[k].type == Parser.State.JSONType.Array || o[k].type == Parser.State.JSONType.Object)
                {
                    MakeTemplateObjectList(o[k], depth + 1);
                }
            }
            return null;
        }
    }
}
