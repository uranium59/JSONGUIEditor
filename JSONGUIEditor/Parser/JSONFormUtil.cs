using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONGUIEditor.Parser
{
    using JSONGUIEditor.Parser.State;
    public class JSONFormUtil
    {
        static public bool MakeTreeView(JSONNode n, TreeView t)
        {
            t.Nodes.Add(TreeNodeMake(n));
            return true;
        }

        static public TreeNode TreeNodeMake(JSONNode n)
        {
            TreeNode rtn = new TreeNode();
            rtn.Tag = n;
            rtn.Text = n.type.GetTypeString();
            foreach (JSONNode j in n)
            {
                TreeNode t;
                if(j.type == State.JSONType.Array || j.type == State.JSONType.Object)
                {
                    t = TreeNodeMake(j);
                }
                else
                {
                    t = new TreeNode();
                    t.Text = j.type.GetTypeString();
                    t.Tag = j;
                }
                rtn.Nodes.Add(t);
            }
            return rtn;
        }

        static public TreeNode FindTreeNode(TreeNode t, JSONNode n)
        {
            if (ReferenceEquals(t.Tag, n)) return t;

            foreach(TreeNode child in t.Nodes)
            {
                if (FindTreeNode(child, n) != null) return FindTreeNode(child, n);
            }
            return null;
        }

        static public Panel NextPanelFind(Control c)
        {
            Control next = c.Parent.GetNextControl(c, false);
            while (next != null)
            {
                if (next is Panel)
                {
                    return (Panel)next;
                }
                next = c.Parent.GetNextControl(next, false);
            }

            return null;
        }
    }
}
