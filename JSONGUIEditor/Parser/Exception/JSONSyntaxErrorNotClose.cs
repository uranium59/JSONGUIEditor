using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    class JSONSyntaxErrorNotClose : JSONException
    {
        public JSONSyntaxErrorNotClose():base()
        {
            throw new NotImplementedException();
        }
        public JSONSyntaxErrorNotClose(int index):base()
        {

        }
    }
}
