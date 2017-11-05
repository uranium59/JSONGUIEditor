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
using Newtonsoft.Json;

namespace JSONGUIEditor.TemplateForm
{
    public partial class TemplateAdd : Form
    {
        const string TEMPLETEFILE = "TempRepository.inx";
        string CURRENTDIR = String.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, "TempleteJson");

        public TemplateAdd()
        {
            InitializeComponent();
        }

        string _jsonData;

        public TemplateAdd(string jsonData)
        {
            InitializeComponent();

            _jsonData = jsonData;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(txtTempName.Text))
                MessageBox.Show("Templete 명칭을 입력 하세요.", "Validate", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                string filename = txtTempName.Text.Trim();
                TempleteAdd(filename);

                JsonFileSave(filename, _jsonData);

                if (!String.IsNullOrWhiteSpace(txtDesc.Text.Trim()))
                    DescFileSave(filename, txtDesc.Text);

                txtTempName.Text = String.Empty;
                txtDesc.Text = String.Empty;

                MessageBox.Show("추가 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void TempleteAdd(String value)
        {
            //using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE), FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}", CURRENTDIR, TEMPLETEFILE), true, System.Text.Encoding.UTF8))
            {
                stream.WriteLine(value);
            }
        }

        private void DescFileSave(String descFileName, String value)
        {
            using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}.dpt", CURRENTDIR, descFileName), false, System.Text.Encoding.UTF8))
            {
                stream.Write(value);
            }
        }

        private void JsonFileSave(String jsonFileName, String jsonStr)
        {
            using (System.IO.StreamWriter stream = new StreamWriter(String.Format("{0}\\{1}.json", CURRENTDIR, jsonFileName), false, System.Text.Encoding.UTF8))
            {
                stream.Write(jsonStr);
            }
        }
    }
}
