using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser.Exception
{
    public class JSONException : System.Exception
    {
        protected string _Message;
        public int position { get; protected set; }

        public override string Message
        {
            get { return _Message; }
        }
    }
}
