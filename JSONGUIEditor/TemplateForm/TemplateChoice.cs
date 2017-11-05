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
    public delegate void SelectTemplate(object sender, String e);

    public partial class TemplateChoice : Form
    {
        const string TEMPLETEFILE = "TempRepository.inx";
        string CURRENTDIR = String.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, "TempleteJson");
        
        public BaseForm baseForm { get; set; }

        public TemplateChoice()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            JSON.Parse(AddSelectToMain, baseForm.JSONExceptionCatch, lblJson.Text);
        }

        private JSONNode AddSelectToMain(JSONNode n)
        {
            baseForm.AddTemplateNodeToBase(n);
            this.Close();
            return n;
        }

        private void InitTempList()
        {
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
                lsbTemplist.Items.Add(obj);
        }

        private void lsbTemplist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbTemplist.SelectedIndex > -1)
            {
                BindJsonFile(lsbTemplist.SelectedItem.ToString());
                GetDesc(lsbTemplist.SelectedItem.ToString());
            }
        }

        private void BindJsonFile(String tempName)
        {
            lblJson.Text = String.Empty;

            String filePath = String.Format("{0}\\{1}.json", CURRENTDIR, tempName);
            
            if (!File.Exists(filePath))
                return;

            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                lblJson.Text = sr.ReadToEnd();
            }
        }

        private void GetDesc(string tempName)
        {
            lblDesc.Text = String.Empty;

            String filePath = String.Format("{0}\\{1}.dpt", CURRENTDIR, tempName);

            if (!File.Exists(filePath))
                return;

            using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.UTF8))
            {
                lblDesc.Text = sr.ReadToEnd();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TemplateChoice_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(CURRENTDIR))
                Directory.CreateDirectory(CURRENTDIR);

            InitTempList();
        }
    }
}
