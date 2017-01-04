using System;
using System.Collections.Generic;
using slagtool;

namespace slagtool.preruntime
{ 
    public class Checktype {
        private static Type Check(string s, List<string> prefixlist)
        {
            Type find = null;
            foreach(var pre in prefixlist)
            {
                var ss = pre + "." + s;
                var ti = slagtool.runtime.sub_pointervar_clause.find_typeinfo(ss);
                if (ti!=null)
                {
                    if (find!=null)
                    {
                        sys.error("The type name is ambiguous : " + s);
                    }
                    else
                    {
                        find =ti;
                    }
                }
            }
            return find;
        }

        internal static YVALUE ChangeIfType(YVALUE v, List<string> prefixlist)
        {
            if (v.IsType(YDEF.NAME))
            {
                var vname = v.FindValueByTravarse(YDEF.NAME);
                var type = Check(vname.GetString(),prefixlist);
                if (type!=null)
                {
                    vname.type = YDEF.RUNTYPE;
                    vname.o = type;
                    vname.s = type.ToString();
                }
            }
            return v;
        }

    }
}
