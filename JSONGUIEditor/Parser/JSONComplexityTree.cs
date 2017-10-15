using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class MyTree<V> : List<MyTree<V>>
    {
        public int Index { get; set; } = 0;
        public int StrCount { get; set; } = 0;
        public int Complex { get; set; } = 0;
        public MyTree<V> parent { get; set; } = null;
        public JSONNode parsedNode { get; set; } = null;
        public int AddComplex()
        {
            foreach (MyTree<V> v in this)
            {
                Complex += v.AddComplex();
            }
            return Complex;
        }
    }
}