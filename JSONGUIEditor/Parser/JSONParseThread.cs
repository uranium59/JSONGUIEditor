using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONParseThread
    {
        //if complexity is bigger than threshold, add thread into threadpool
        static public const int ComplexityThreshold = 10;
        static public const bool UsingThread = true;
        
        static public JSONNode Parse(MyTree<int, object> t, string s)
        {
            foreach(MyTree<int, object> c in t)
            {
                if(c.Complex > ComplexityThreshold)
                {
                }
            }
        }//for single thread
        
        static public JSONNode ParseThread(MyTree<int, object> t, string s)
        {
            if(c.Complex > ComplexityThreshold)
            {
            }
        }//for multithread
    }
}
