using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class MyTree<K, V> : Dictionary<K, MyTree<K, V>>
    {
        public int Index { get; set; } = 0;
        public int StrCount { get; set; } = 0;
        public int Complex { get; set; } = 0;
        public MyTree<K, V> parent { get; set; } = null;
        public void AddComplex()
        {
            Complex++;
            if (parent != null) parent.AddComplex();
        }
    }
}