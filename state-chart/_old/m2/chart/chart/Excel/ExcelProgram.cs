using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class ExcelProgram
{
    const int NAME_COL  = 1;
    const int START_COL = 3;
    const int STATE_ROW = 1;

    ExcelLoadValues m_ld;

    public List<string> m_state_list {get;private set; }

    public void Load(string file)
    {
        m_ld = new ExcelLoadValues(file);

        //stateを収集
        var state_str = string.Empty;
        var state_list = new List<string>();
        {
            for(var c = START_COL; c<m_ld.GetMaxCol(); c++)
            {
                var s = m_ld.GetValue(STATE_ROW,c);
                if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim())) continue;
                state_list.Add(s.Trim());
            }

            //
            state_list.ForEach(s=> {
                if (!string.IsNullOrEmpty(state_str)) state_str += ",";
                state_str += s + "\n";
            });
        }
        m_state_list = state_list;
    }

    //ソース内の 存在確認後利用する部分を展開する
    //private static string convert_partsifexist(string st, string tmpstr)
    //{
    //    var output = string.Empty;
    //    var lines = tmpstr.Split('\n');
    //    for (var n = 0; n < lines.Length; n++)
    //    {
    //        var l = lines[n].TrimEnd();
    //        int index = 0;
    //        var e = EditUtil.Extract(l, out index);
    //        if (!string.IsNullOrWhiteSpace(e))
    //        {
    //            var namecmt = e + "-cmt";
    //            var indentspace = (new string(' ', index));
    //            if (!string.IsNullOrWhiteSpace(_getvalue(st,namecmt))) //コメント行あり
    //            {
    //                output += indentspace + "/*" + "\n";
    //                output += indentspace + string.Format("  [[{0}]]", namecmt) + "\n";
    //                output += indentspace + "*/" + "\n";
    //            }
    //        }
    //        output += l + "\n";
    //    }
    //    return output;
    //}
    //private static string convert_partsifexist_obs(string st,string tmpstr)
    //{
    //    var mark1 = "<<<?";
    //    var mark2 = ">>>";
    //    var output = string.Empty;
    //    var lines = tmpstr.Split('\n');
    //    for(var n = 0; n<lines.Length; n++)
    //    {
    //        var l = lines[n].TrimEnd();
    //        var oindex = l.IndexOf("<<<?");
    //        if (oindex >=0)
    //        {
    //            var name = l.Substring(oindex + mark1.Length).Trim();
    //            var bExist  =  !string.IsNullOrEmpty( _getvalue(st,name));
                
    //            for(n++; n < lines.Length; n++)
    //            {
    //                var j = lines[n].TrimEnd();
    //                if (j.Contains(mark2)) break;
    //                if (bExist) {
    //                    output += j + "\n";
    //                }
    //            }
    //        }
    //        else
    //        {
    //            output += l + "\n";
    //        }
    //    }
    //    return output;
    //}

    //ソース内のname部分を値に変換する
    //private static string convert_names(string st, string tmpstr)
    //{
    //    var output = string.Empty;
    //    var lines = tmpstr.Split('\n');
    //    for(var n = 0; n<lines.Length; n++)
    //    {
    //        var l = lines[n].TrimEnd();
    //        var oindex = l.IndexOf("[[");
    //        var cindex = l.IndexOf("]]");
    //        if (oindex >= 0 && cindex >= 0 && oindex < cindex)
    //        {
    //            var name = l.Substring(oindex+2,cindex - oindex -2);
    //            var val = _getvalue(st,name).Trim();
                
    //            var result = EditUtil.Insert(l,"[[" + name + "]]",val).TrimEnd();
    //            if (!string.IsNullOrWhiteSpace(result))
    //            {
    //                 output += result + "\n";
    //            }

    //        }
    //        else
    //        {
    //            output += l + "\n";
    //        }
    //    }
    //    return output;
    //}

    // ==== tools for this class ===
    int _getColIndexByState(string st)
    {
        var row = _getRowIndexByName("state");
        for(var c = START_COL; c<m_ld.GetMaxCol();c++)
        {
            var s = m_ld.GetValue(row,c).Trim();
            if (s==st)
            {
                return c;
            }
        }
        return -1;
    }

    int _getRowIndexByName(string nm)
    {
        for(var i = 0; i<m_ld.GetMaxRow();i++)
        {
            var s = m_ld.GetValue(i,NAME_COL).Trim();
            if (s==nm)
            {
                return i;
            }
        }
        return -1;
    }

    public string GetValue(string state, string name)
    {
        return _getvalue(state, name);
    }
    string _getvalue(string state, string name)
    {
        var row = _getRowIndexByName(name);
        var col = _getColIndexByState(state);

        return m_ld.GetValue(row,col);
    }
}
