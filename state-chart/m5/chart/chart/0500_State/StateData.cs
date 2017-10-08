//<<<include=using_text.txt
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Detail=DrawStateBox.Detail;
using LineType=DrawUtil.LineType;
//>>>

public partial class StateData
{
    readonly string STR_STATE     = "state";
    readonly string STR_NEXTSTATE = "nextstate";
    readonly string STR_BRANCH    = "branch";

    public class Item
    {
        public int    index;
        public string key;
        public string value;
    }

    public List<Item> m_items;

    public string    State       { get { if (__state==null) __state = GetValue(STR_STATE); return __state;                     } } string __state = null;
    public string    NextState   { get { if (__nextstate==null) __nextstate = GetValue(STR_NEXTSTATE); return __nextstate;     } } string __nextstate = null;
    public int       NumBranches { get { if (__numBranches==null) __numBranches = GetBranchCount(); return (int)__numBranches; } } int? __numBranches = null;

    public Item GetItem(string key)
    {
        var v = m_items.Find(i=>i.key == key);
        return v;
    }
    public string GetContent() //state, nextstate, branchを除く命令 ブランク、およびコメント(-cmt)は除く
    {
        var list = new List<string>();
        foreach(var i in m_items)
        {
            if (i.key == STR_STATE)            continue;
            if (i.key == STR_NEXTSTATE)        continue;
            if (i.key == STR_BRANCH)           continue;

            if (i.key.EndsWith("-cmt"))        continue;
            if (string.IsNullOrEmpty(i.value)) continue;
            var v = i.value.Trim();
            if (string.IsNullOrEmpty(v))       continue;

            list.Add(v);
        }

        var o = string.Empty;
        foreach(var l in list)
        {
            if (!string.IsNullOrEmpty(o))
            {
                o += Environment.NewLine;
            }
            o+=l;
        }
        return o;
    }
    public string GetValue(string key)
    {
        var i = GetItem(key);
        return i!=null ? i.value : null;
    }
    public string GetBranchApi(int i)
    {
        var api   = "";
        var param = "";

        if (GetBranchApiParam(i,out api, out param))
        {
            return api;
        }

        return "";
    }
    public string GetBranchParam(int i)
    {
        var api   = "";
        var param = "";

        if (GetBranchApiParam(i,out api, out param))
        {
            return param;
        }

        return "";
    }
    public bool GetBranchApiParam(int i, out string api, out string param)
    {
        api   = string.Empty;
        param = string.Empty;

        var v= GetValue(STR_BRANCH);
        if (v == null) return false;
        v = v.Trim();
        if (string.IsNullOrEmpty(v)) return false;
        var lines = v.Split(';');
        for(var n = 0; n < lines.Length; n++)
        {
            if (n!=i) continue;
            var l = lines[n];
            if (!string.IsNullOrEmpty(l))
            {
                var l2 = l.Trim();
                if (!string.IsNullOrEmpty(l2))
                {
                    var words = l2.Split('(');
                    if (words!=null && words.Length>=2)
                    {
                        api = words[0].Trim();
                        param = words[1].Trim(')').Trim();
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public int GetBranchCount()
    {
        for(var i = 0; i<100; i++)
        {
            string api, param;
            if (!GetBranchApiParam(i,out api, out param))
            {
                return i;
            }
        }
        return -1;
    }
}
