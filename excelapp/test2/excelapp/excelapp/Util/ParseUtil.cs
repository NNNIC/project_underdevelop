using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ParseUtil
{
    public static int? IntParse(string s)
    {
        int x;
        if (int.TryParse(s, out x))
        {
            return x;
        }
        return null;
    }
}
