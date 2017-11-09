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
        JSONNode copyTarget = null;
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyTarget == null)
                return;
            Panel target;
            
            if (nowSelectedNode == null)
            {
                target = (Panel)MainPanel.Controls[2];
            }
            else
            {
                JSONNode tmpnode = (JSONNode)nowSelectedNode.Tag;
                if (tmpnode is JSONObject || tmpnode is JSONArray)
                    target = (Panel)nowSelectedNode.Controls[nowSelectedNode.Controls.Count - 1];
                else
                {
                    MessageBox.Show("선택된 대상은 오브젝트나 배열이 아닙니다");
                    return;
                }
            }
            JSONNode node = (JSONNode)target.Tag;
            if (copyTarget.type != JSONType.Array && copyTarget.type != JSONType.Object)
            {
                JSONNode copied = copyTarget.CloneNode();
                TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, node);
                string key = node.Add(copied);
                TreeNode newTreenode = new TreeNode();
                newTreenode.Tag = copyTarget;
                newTreenode.Text = copyTarget.type.GetTypeString();
                t.Nodes.Add(newTreenode);

                CreateGroupChild(copied, key, target, target.Height, node.type == JSONType.Object ? true : false);

                PanelReSort(target);
            }
            else
            {
                if(JSONParseThread.Parsing)
                {
                    MessageBox.Show("다른 파싱이 진행중입니다");
                    return;
                }
                string parseString = copyTarget.Stringify();
                JSON.Parse((n) =>
                {
                    JSONNode copied = n;
                    TreeNode t = JSONFormUtil.FindTreeNode(tview_object.TopNode, node);
                    string key = node.Add(copied);
                    TreeNode newTreenode = JSONFormUtil.TreeNodeMake(copied);
                    t.Nodes.Add(newTreenode);

                    CreateGroupChild(copied, key, target, target.Height, node.type == JSONType.Object ? true : false);

                    PanelReSort(target);
                    return n;
                }, JSONExceptionCatch, parseString);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(nowSelectedNode == null) return;
            if (JSONParseThread.Parsing) return;
            JSONNode node = (JSONNode)nowSelectedNode.Tag;
            string parseString = node.Stringify();
            JSON.Parse((n) =>
            {
                copyTarget = n;
                return n;
            }, JSONExceptionCatch, parseString);
            RemoveNode(nowSelectedNode, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nowSelectedNode == null) return;
            if (JSONParseThread.Parsing) return;
            JSONNode node = (JSONNode)nowSelectedNode.Tag;
            string parseString = node.Stringify();
            JSON.Parse((n) =>
            {
                copyTarget = n;
                return n;
            }, JSONExceptionCatch, parseString);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(nowSelectedNode == null) return;
            RemoveNode(nowSelectedNode, e);
        }
    }
}
