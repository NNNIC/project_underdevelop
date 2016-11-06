using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace langtool.runtime.builtin
{
    public class builtin_sysfunc
    {
        static string NL = util.NL;

        #region システム
        public static object F_Sleep(bool bHelp, object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Sleep for the specified time."+NL+"Format: Sleep(sec)";
            }

            util.check_num_of_args(ol,1);

            var x = util.get_double_at(ol,0);
            if (!double.IsNaN(x))
            { 
                x = x * 1000.0f;
            }
            else
            {
                x = 1000;
            }
            System.Threading.Thread.Sleep((int)x);

            return null;
        }
        #endregion

        #region コンソール/デバッグ
        public static object F_Print(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Print a string." + NL + "ex)Print(\"hoge!\");";
            }
            util.check_num_of_args(ol,1);
            var o = util.get_ol_at(ol,0);
            Console.Write(util.convert_escape(o));
            return null;
        }
        public static object F_Println(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Print a string with line break" + NL + "ex)PrintIn(\"hoge!!\");";
            }
            util.check_num_of_args(ol,1);
            var o = util.get_ol_at(ol,0);
            Console.WriteLine(util.convert_escape(o));
            return null;
        }
        public static object F_Dump(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Dump a variable." + NL +"ex)Dump(x);";
            }

            if (ol==null) return "-null-";

            Func<object,string> tostr = null;
            Func<List<object>,string> join = (l)=> {
                string t= null;
                foreach(var e in l)
                {
                    if (t!=null) t+=",";
                    t+= tostr(e);
                }
                return t;
            };

            tostr = (a) => {
                if (a==null) return "-null-";
                if (a.GetType()==typeof(List<object>))
                {
                    var l = (List<object>)a;
                    return "(" + join(l) + ")";
                }
                return a.ToString();
            };

            string s = null;
            foreach(var o in ol)
            {
                if (s!=null) s+=",";
                s += tostr(o);
            }

            Console.WriteLine(s);

            return s;
        }
        public static object F_ReadLine(bool bHelp, object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Read a line from console." + NL + "ex)var a = ReadLine(\"Input>\")";
            }

            util.check_num_of_args(ol,1);
            var s = util.get_ol_at(ol,0);
            Console.Write(s);

            var line = Console.ReadLine();

            return line;
        }
        #endregion

        #region 変換
        public static object F_ToNumber(bool bHelp, object[] ol,StateBuffer sb=null)
        {
            if (bHelp)
            {
                return "Convert a string to a number." + NL + "ex)var a = ToNumber(\"123\");";
            }

            util.check_num_of_args(ol,1);

            var x = util.get_double_at(ol,0);
            if (!double.IsNaN(x))
            {
                return x;
            }
            return (double)0;
        }

        #endregion

        #region 数値操作
        public static object F_RandomInt(bool bHelp,object[] ol,StateBuffer sb) // 引数 0 -- 最少数  1 -- 最大数
        {
            if (bHelp)
            {
                return "Get a random integer." + NL + "format: RandomInt(min, max)";
            }

            util.check_num_of_args(ol,2);

            var min = util.get_double_at(ol,0);
            var max = util.get_double_at(ol,1);
            var diff = max - min;

            var r = new System.Random(DateTime.Now.Millisecond);
            var i = r.Next((int)diff+1);
           
            return (double)(min + i);
        }
        public static object F_ToInt(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Conver a number to an integer number.";
            }

            util.check_num_of_args(ol,1);

            var x = util.get_double_at(ol,0);
            if (!double.IsNaN(x))
            {
                var i = (int)x;
                return (double)i;
            }
            return 0;
        }
        #endregion

        #region 文字列操作
        public static object F_Substring(bool bHelp, object[] ol, StateBuffer sb)
        {
            if (bHelp)
            {
                return "Substring. see c# string.substring.";
            }

            if (ol==null) return null;
            if (ol.Length==2)
            {
                var s = ol[0].ToString();
                var n = util.get_double_at(ol,1);
                return s.Substring((int)n);
            }
            else if (ol.Length==3)
            {
                var s = ol[0].ToString();
                var n = util.get_double_at(ol,1);
                var c = util.get_double_at(ol,2);
                return s.Substring((int)n,(int)c);
            }

            util.error("Substring syntax error");

            return null;
        }

        #endregion

        #region 配列操作
        public static object F_ListSize(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Get size of the list.";
            }
            util.check_num_of_args(ol,1);

            var list = util.get_list_at(ol,0);
            if (list!=null)
            { 
                return (double)list.Count;
            }
            return (double)0;
        }
        public static object F_ListCombine(bool bHelp, object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Combine list or element." + NL +"ex) var a = ListCombine((1,2),(3,4));";
            }

            var nl = new List<object>();
            if (ol!=null) for(int i = 0; i<ol.Length; i++)
            {
                var al = util.get_list_at(ol,i);
                if (al!=null)
                {
                    nl.AddRange(al);
                }
                else
                {
                    var o = util.get_ol_at(ol,i);
                    if (o!=null)
                    {
                        nl.Add(o);
                    }
                }
            }
            return (object)nl;
        }
        public static object F_ListSelectRandom(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Select a elemenet in the list at random" + NL + "var a = ListSelectRandom(1,2,3);";
            }

            util.check_num_of_args(ol,1);

            var list = util.get_list_at(ol,0);
            if (list!=null)
            { 
                var rand = new Random(DateTime.Now.Millisecond);
                var n = rand.Next();
                var a = n % list.Count;
             
                return list[a];
            }
            return null;
        }
        public static object F_ListRemove(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Remove a element from a list." + NL + "Format: var l = ListRemove(list,element);";
            }
            util.check_num_of_args(ol,2);

            var list = util.get_list_at(ol,0);
            var s    = util.get_ol_at(ol,1);
            if (list==null || s==null) return null;
            list.Remove(s);

            return list;
        }
        public static object F_ListShuffle(bool bHelp,object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Shuffle a list.";
            }
            util.check_num_of_args(ol,1);

            var list = util.get_list_at(ol,0);
            if (list!=null)
            { 
                var nl = new List<object>();
                var rand = new Random();
                if (list!=null)
                { 
                    while(list.Count>0)
                    {
                        var n = rand.Next() % list.Count;
                        var s = list[n];
                        list.RemoveAt(n);
                        nl.Add(s);
                    }
                }
                return nl;
            }
            return null;
        }
        public static object F_ListAt(bool bHelp, object[] ol,StateBuffer sb)
        {
            if (bHelp)
            {
                return "Get a element at the number of the list.";
            }

            util.check_num_of_args(ol,2);

            var list = util.get_list_at(ol,0);
            var n    = util.get_double_at(ol,1);
            if (list!=null && !double.IsNaN(n) && n < list.Count)
            {
                return list[(int)n];
            }
            return null;
        }
        public static object F_ListSort(bool bHelp, object[] ol, StateBuffer sb)
        {
            if (bHelp)
            {
                return "Sort the list";
            }

            var src = util.get_list_at(ol,0);
            if (src==null) util.error("ListSort arg is not valid.");
            var l = new List<object>(src);
            if (l.Count>0 && l[0].GetType()==typeof(double))
            {
                l.Sort((a,b)=> (int)Math.Ceiling((double)a - (double)b));
            }
            else
            { 
                l.Sort((a,b)=>string.Compare(a.ToString(),b.ToString()));
            }
            return l;
        }
        #endregion
    }
}
