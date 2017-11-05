using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;
using System.IO;
//using Newtonsoft.Json;
using JSONGUIEditor.Parser;

namespace JSONGUIEditor.TemplateForm
{
    public partial class TemplateManage : Form
    {
        const string TEMPLETEFILE = "TempRepository.inx";
        string CURRENTDIR = String.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, "TempleteJson");

        public TemplateManage()
        {
            InitializeComponent();
		}

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		private void TemplateManage_Load(object sender, EventArgs e)
		{
			if (!Directory.Exists(CURRENTDIR))
				Directory.CreateDirectory(CURRENTDIR);

			InitTempList();
		}

		private void TempleteAdd(String value)
		{
			//using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
			using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE), true, System.Text.Encoding.UTF8))
			{
				stream.WriteLine(value);
			}
		}

		private void TempleteDelete(ArrayList arrValue)
		{
			using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE), false, System.Text.Encoding.UTF8))
			{
				foreach(String value in arrValue)
					stream.WriteLine(value);
			}
		}

		private void InitTempList()
		{
			lsbTemp.Items.Clear();

			String filePath = String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE);

			ArrayList arrList = new ArrayList();

			if (!File.Exists(filePath))
				return;

			using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
			{
				while (sr.Peek() >= 0)
				{
					arrList.Add(sr.ReadLine());
				}
			}

			foreach (String obj in arrList)
				lsbTemp.Items.Add(obj);
		}

		private void btnTempDel_Click(object sender, EventArgs e)
		{
			if (lsbTemp.Items.Count == 0)
				return;

			if (lsbTemp.SelectedItem == null)
				MessageBox.Show("삭제할 Templete 명칭을 선택 하세요.", "Validate", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
			{
				if (DialogResult.Yes == MessageBox.Show("Templete을 삭제 하시겠습니까?", "Validate", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
				{
					ArrayList arrList = new ArrayList();
                    foreach (String str in lsbTemp.Items)
                    {
                        if (!lsbTemp.SelectedItem.ToString().Equals(str))
                            arrList.Add(str);
                    }

					TempleteDelete(arrList);
                    DescFileDelete(lsbTemp.SelectedItem.ToString());
                    JsonFileDelete(lsbTemp.SelectedItem.ToString());

                    InitTempList();
				}
			}
		}

		private void lsbTemp_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (lsbTemp.SelectedIndex > -1)
            {
                BindJsonFile(lsbTemp.SelectedItem.ToString());
                GetDesc(lsbTemp.SelectedItem.ToString());
            }
		}

		private void BindJsonFile(String tempName)
		{
			String filePath = String.Format("{0}\\{1}.json", CURRENTDIR, tempName);

			List<JsonT> lstJsonT = new List<JsonT>();
			bindingSource1.DataSource = lstJsonT;

			if (!File.Exists(filePath))
            {
                MessageBox.Show("해당하는 템플릿 파일이 존재하지 않습니다");
                return;
            }

            String jsonStr=File.ReadAllText(filePath);
            JSONGUIEditor.Parser.JSON.Parse((n)=> { BindJSONCallback(n, lstJsonT, 0);
                List<JsonT> lstJsonT2 = new List<JsonT>();
                lstJsonT2.AddRange(lstJsonT.ToArray<JsonT>());
                bindingSource1.DataSource = lstJsonT2;
                return n; }, BindJSONException, jsonStr);
		}
        private void BindJSONCallback(JSONNode n, List<JsonT> list, int depth = 0)
        {
            string intend = "";
            for (int i = 0; i < depth; ++i)
            {
                intend += "  ";
            }
            string[] keys = n.GetAllKeys();
            if (keys == null) return;
            foreach(string s in keys)
            {
                JsonT node = new JsonT
                {
                    key = intend + s,
                    value = ((n[s] is JSONObject) ? "object" : (n[s] is JSONArray ? "array" : n[s].value)),
                    type = Parser.State.JSONTypeFunc.GetTypeString(n[s].type)
                };
                list.Add(node);
                BindJSONCallback(n[s], list, depth + 1);
            }
        }
        private void BindJSONException(Parser.Exception.JSONException ex)
        {
            MessageBox.Show("템플릿 파일이 손상되었습니다");
        }
        
		private void btnColSave_Click(object sender, EventArgs e)
		{
			if (lsbTemp.SelectedItem == null)
				return;

			String jsonFileName = lsbTemp.SelectedItem.ToString();
            DescFileSave(jsonFileName, txtDesc.Text);
            
			MessageBox.Show("저장 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void JsonFileDelete(String jsonFileName)
		{
			File.Delete(String.Format("{0}\\{1}.json", CURRENTDIR, jsonFileName));
		}

        private void DescFileSave(String descFileName, String value)
        {
            using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}.dpt", CURRENTDIR, descFileName), false, System.Text.Encoding.UTF8))
            {
                stream.Write(value);
            }
        }

        private void DescFileDelete(String descFileName)
        {
            File.Delete(String.Format("{0}\\{1}.dpt", CURRENTDIR, descFileName));
        }

        private void GetDesc(string tempName)
        {
            txtDesc.Text = String.Empty;

            String filePath = String.Format("{0}\\{1}.dpt", CURRENTDIR, tempName);

            if (!File.Exists(filePath))
                return;

            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                txtDesc.Text = sr.ReadToEnd();
            }
        }
    }

    public class JsonT
	{
		public string key { get; set; }
		public string value { get; set; }
        public string type { get; set; }
	}

}
