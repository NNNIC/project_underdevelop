using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ARRAY = System.Collections.Generic.List<object>;

namespace slagtool.runtime
{
    // Ｃ＃依存部分
    public class runsub_location_clause //ピリオド区切りの文字列に対しての処理
    {
        public static StateBuffer run(YVALUE v, StateBuffer sb) 
        {
            var nsb = sb;

            var item = new LocationItem(); //先行アイテム。中身null
            nsb.m_cur = item;
            var size = v.list_size();
            for(int i = 0 ; i<size ; i++)
            {
                var vn = v.list_at(i);
                if (vn==null) throw new SystemException("Unexpected");
                
                if (vn.IsType(YDEF.PERIOD)) continue;

                nsb = run_script.run(vn,nsb);
                if (nsb.m_cur == null) break;                               //最近の流行りを取り入れてnullだったら後ろは処理しない
            }

            var result = nsb.get_locationitem_cur();
            nsb.m_cur = result.o;

            return nsb;
        }
        public static StateBuffer run_name(YVALUE v, StateBuffer sb)
        {
            var nsb = sb;
            var name = v.GetString();
            LocationItem item = nsb.get_locationitem_cur();
            var preobj = item.o; //先行ロケーションアイテムの値
            if (preobj == null) //先行値がないのでNAMEとしてバッファを検索し、なければリテラルとして処理を以降に任せる
            {
                if (nsb.exist(name))
                { 
                    item.o = nsb.get(name);
                }
                else
                {
                    var literal = new Literal();
                    literal.s = name;
                    item.o = literal;
                }
                nsb.m_cur = item;
                
                return nsb;                
            }
            var pretype = preobj.GetType();
            if (pretype == typeof(Literal))
            {
                var literal = (Literal)preobj;
                item = GetObj(literal.s,name, item);
                nsb.m_cur = item;
            }
            else
            {
                item = GetObj(preobj,name, item);
                nsb.m_cur = item;
            }
            return nsb;
        }
        public static StateBuffer run_func(YVALUE v, StateBuffer sb, string name, List<object> ol)
        {
            var nsb = sb;
            var item = nsb.get_locationitem_cur();
            var preobj = item.o; //先行ロケーションアイテムの値
            if (preobj == null) //先行値がない場合は予想外
            {
                throw new SystemException("unexpected");
            }
            var pretype = preobj.GetType();
            if (pretype == typeof(Literal))
            {
                var literal = (Literal)preobj;
                item = ExecuteFunc(literal.s,name,ol,item);
                nsb.m_cur = item;
            }
            else
            {
                item = ExecuteFunc(preobj,name,ol,item);
            }
            return nsb;
        }
        public static StateBuffer run_num(YVALUE v, StateBuffer sb)
        {
            var nsb = sb;
            var item = nsb.get_locationitem_cur();
            item.o = v.GetNumber();
            nsb.m_cur = item;
            return nsb;
        }
        public static StateBuffer run_qstr(YVALUE v, StateBuffer sb)
        {
            var nsb = sb;
            var item = nsb.get_locationitem_cur();
            item.o =v.GetString();
            nsb.m_cur = item;
            return nsb;
        }

        // -- tool for this class
        private static LocationItem GetObj(string pre, string cur, LocationItem item)
        {
            //アセンブリ調査 --- set/get不明なので直前の形で返す
            var searchname = (pre + "." + cur).ToUpper();
            var ti = find_typeinfo(searchname);
            if (ti!=null)
            {
                item.o = ti.AsType();
                return item;
            }
            //ない場合は、ピリオドで結合してリテラルとして返す
            var literal = new Literal();
            literal.s = pre + "." + cur;
            item.o = literal;
            return item;
        }
        private static LocationItem GetObj(object o, string cur,LocationItem item)
        {
            var name = cur.ToUpper();
            Type type = null;
            object obj  = null;
            if (o.GetType()==typeof(Type))
            {
                type = (Type)o;
                obj  = null;
            }
            else
            {
                type = o.GetType();
                obj  = o;
            }
            var find_pi = Array.Find(type.GetProperties(),pi=>pi.Name.ToUpper()==name);
            if (find_pi!=null)
            {
                item.getter = ()=> { return find_pi.GetValue(obj); };
                item.setter = (x)=> {find_pi.SetValue(obj,x); };
                return item;
            }
            var find_fi = Array.Find(type.GetFields(),fi=>fi.Name.ToUpper()==name);
            if (find_fi!=null)
            {
                item.getter = ()=> { return find_fi.GetValue(obj); };
                item.setter = (x)=> { find_fi.SetValue(obj,x); };
                return item;
            }
            return null;
        }
        private static LocationItem ExecuteFunc(string pre, string cur, List<object> param, LocationItem item)
        {
            throw new SystemException("unexpected");
        }
        private static LocationItem ExecuteFunc(object o, string cur, List<object> param, LocationItem item)
        {
            var name = cur.ToUpper();
            Type type = null;
            object obj  = null;
            if (o.GetType()==typeof(Type))
            {
                type = (Type)o;
                obj  = null;
            }
            else
            {
                type = o.GetType();
                obj  = o;
            }
            var find_mi = Array.Find(type.GetMethods(),mi=>mi.Name.ToUpper()==name);
            if (find_mi!=null)
            {
                item.o = find_mi.Invoke(obj,param.ToArray());
                return item;
            }
            return null;
        }
        private static TypeInfo find_typeinfo(string searchname)
        {
            TypeInfo find_ti = null;
            travarse_asm((ti)=>{
                if (ti.FullName.ToUpper()==searchname)
                { 
                    find_ti = ti;
                }
            });
            return find_ti;
        }
        private static void travarse_asm(Action<TypeInfo> act)
        {
            foreach(var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(var ti in asm.DefinedTypes)
                {
                    act(ti);
                }
            }
        }
    }
}
