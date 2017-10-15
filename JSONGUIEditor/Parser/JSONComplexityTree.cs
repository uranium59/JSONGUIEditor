using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class ComplexTree<V> : List<ComplexTree<V>>
    {
        public int Index { get; set; }
        public int EndPoint { get; set; } = 0;
        public int Complex { get; set; }
        public Task<JSONNode> task { get; set; } = null;
        public ComplexTree<V> parent { get; set; } = null;
        public int AddComplex()
        {
            Complex = Count;
            foreach (ComplexTree<V> v in this)
            {
                Complex += v.AddComplex();
            }
            return Complex;
        }
    }
}