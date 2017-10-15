using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONGUIEditor.Parser
{
    class JSONParserDEFINE
    {
        static public string Token_True = "true";
        static public string Token_False = "false";
        static public string Token_Null = "null";
        static public string Token_ObjectStart = "{";
        static public string Token_ObjectEnd = "}";
        static public string Token_ArrayStart = "\\[";
        static public string Token_ArrayEnd = "\\]";
        static public string Token_Colon = ":";
        static public string Token_Comma = ",";
        static public string Token_Quote = "\"";
        static public string Token_Escape = "\\";
        static public string Token_OR = "|";
        static public string Token_WhiteSpace = "\\s";
        static public string Token_MultipleWhiteSpace = Token_WhiteSpace + "*";
        static public string Token_Number = "[0-9]";
        static public string Token_Exponential = "[eE][+-]?";

        static public string Full_Exponential_number = "[-+]?"+Token_Number+ "*\\.?" + Token_Number + "+(" + Token_Exponential+ "?" + Token_Number + "+)?";
        static public string Full_String = Token_Quote + "(\\\\\\\"|[^" + Token_Quote + "])*?" + Token_Quote;

        static public string Token_Value = Full_String + "|" + Token_True + "|" + Token_False + "|" + Token_Null + "|" + Full_Exponential_number +
             "|" + Token_ObjectStart + "|" + Token_ArrayStart;

        static public string Key_ValueMatch =
            "(" +
            Full_String + ")" + Token_MultipleWhiteSpace +
            Token_Colon + Token_MultipleWhiteSpace + "(" +
            Token_Value + ")";
        //we always need to check whitespace
        static public string ValuesMatch = 
            "(" +
            Token_Value + ")";

        /* This may not be needed
    static public string Full_Object_Regex = "(?=\"([^\"]*)\"\\s*:\\s*("
        + Full_Exponential_number + "*|true|false|\"[^\"]*\")\\s*(})\\s*,?)";
        */
    }

    public class JSONParsePosition
    {
        public int line;
        public int position;

        public JSONParsePosition()
        {
            line = 0;
            position = 0;
        }
        public JSONParsePosition(int l)
        {
            line = l;
            position = 0;
        }
        public JSONParsePosition(int l, int p)
        {
            line = l;
            position = p;
        }

        public override string ToString()
        {
            return String.Format("at {0} line, {1} character", line, position);
        }
    }

    public class JSONStringifyOption
    {//아무것도 설정하지 않은 기본 옵션은 최대 최적화를 의미합니다.
        public bool indent { get; set; } = false;//들여쓰기
        public bool colonspace { get; set; } = false;// ':' 좌우에 띄어쓰기 추가
        public bool addnullobject { get; set; } = false;//null 오브젝트를 문자열화 시키는지
    }
}
