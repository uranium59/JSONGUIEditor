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

        static private TreeNode TreeNodeMake(JSONNode n)
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
    }
}
