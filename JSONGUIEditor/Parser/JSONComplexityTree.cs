using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class ComplexTree<V> : List<ComplexTree<V>>
    {
        public int StartPoint { get; set; }
        public int EndPoint { get; set; } = 0;
        public int nodeIndex { get; set; }
        public JSONNode node { get; set; }
        public Thread task { get; set; } = null;
        public Queue<int> separator = new Queue<int>();
        public ComplexTree<V> parent { get; set; } = null;
    }
}