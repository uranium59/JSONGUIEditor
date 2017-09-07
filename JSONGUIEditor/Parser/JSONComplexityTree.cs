using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    public class MyTree<K, V> : Dictionary<K, MyTree<K, V>>
    {
        public V Value { get; set; }
    }
}