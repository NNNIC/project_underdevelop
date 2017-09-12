using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExcelStateChartConverter
{
    class Program
    {
        const int NAME_COL=1;
        const int START_COL=3;

        static LoadTemplateAndValues m_ld;

        static void Main(string[] args)　// 0 --- excel file, 1 --- output dir
        {
            m_ld = new LoadTemplateAndValues(args[0]);

            //stateを収集
            var state_str = string.Empty;
            var state_list = new List<string>();
            {
                for(var c = START_COL; c<m_ld.GetMaxCol(); c++)
                {
                    var s = m_ld.GetValue(0,c);
                    if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim())) continue;
                    state_list.Add(s.Trim());
                }

                //
                state_list.ForEach(s=> {
                    if (!string.IsNullOrEmpty(state_str)) state_str += ",";
                    state_str += s + "\n";
                });
            }

            var func_str = string.Empty;
            foreach(var st in state_list)
            {
                var tmpstr = m_ld.GetInitialFuncSource();
                tmpstr = convert_partsifexist(st,tmpstr);
                tmpstr = convert_names(st,tmpstr);

                func_str +=  EditUtil.TrimLines(tmpstr);
            }

            string filename;
            var src = m_ld.GetInitalSource(out filename);
            src = EditUtil.Insert(src,"$contents1$",state_str);            
            src = EditUtil.Insert(src,"$contents2$",func_str);

            //
            src = "//This source is created by ExcelStateChartConverter.exe. Source : " + args[0] + "\n" + src;
           

            File.WriteAllText(Path.Combine(args[1],filename),src,Encoding.UTF8);

        }

        //ソース内の 存在確認後利用する部分を展開する
        private static string convert_partsifexist(string st,string tmpstr)
        {
            var mark1 = "<<<?";
            var mark2 = ">>>";
            var output = string.Empty;
            var lines = tmpstr.Split('\n');
            for(var n = 0; n<lines.Length; n++)
            {
                var l = lines[n].TrimEnd();
                var oindex = l.IndexOf("<<<?");
                if (oindex >=0)
                {
                    var name = l.Substring(oindex + mark1.Length).Trim();
                    var bExist  =  !string.IsNullOrEmpty( _getvalue(st,name));
                    
                    for(n++; n < lines.Length; n++)
                    {
                        var j = lines[n].TrimEnd();
                        if (j.Contains(mark2)) break;
                        if (bExist) {
                            output += j + "\n";
                        }
                    }
                }
                else
                {
                    output += l + "\n";
                }
            }
            return output;
        }

        //ソース内のname部分を値に変換する
        private static string convert_names(string st, string tmpstr)
        {
            var output = string.Empty;
            var lines = tmpstr.Split('\n');
            for(var n = 0; n<lines.Length; n++)
            {
                var l = lines[n].TrimEnd();
                var oindex = l.IndexOf("[[");
                var cindex = l.IndexOf("]]");
                if (oindex >= 0 && cindex >= 0 && oindex < cindex)
                {
                    var name = l.Substring(oindex+2,cindex - oindex -2);
                    var val = _getvalue(st,name);
                    
                    output += EditUtil.Insert(l,"[[" + name + "]]",val).TrimEnd() + "\n";
                }
                else
                {
                    output += l + "\n";
                }
            }
            return output;
        }

        // ==== tools for this class ===
        static int _getColIndexByState(string st)
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

        static int _getRowIndexByName(string nm)
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

        static string _getvalue(string state, string name)
        {
            var row = _getRowIndexByName(name);
            var col = _getColIndexByState(state);

            return m_ld.GetValue(row,col);
        }
 
        
    }
}
