using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONSyntaxErrorValueNotExist :JSONException
    {
        public JSONSyntaxErrorValueNotExist():base()
        {
            _Message = string.Format("Parser cannot found Value");
        }
        public JSONSyntaxErrorValueNotExist(int index) :base()
        {
            position = index;
            _Message = string.Format("At index {0}, Parser cannot found Value", index);
        }
    }
}
