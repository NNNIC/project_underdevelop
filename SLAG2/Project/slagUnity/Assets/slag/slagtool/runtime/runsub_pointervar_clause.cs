using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using ARRAY = System.Collections.Generic.List<object>;
using number = System.Double;

namespace slagtool.runtime
{
    // Ｃ＃依存部分
    public class runsub_pointervar_clause //ピリオド区切りの文字列に対しての処理
    {
        public static StateBuffer run(YVALUE v, StateBuffer sb, PointervarMode mode = PointervarMode.GET) 
        {
            var nsb = sb;
            var item = new PointervarItem(); //先行アイテム。中身なし
            item.mode = mode;

            nsb.m_pvitem = item;

            var size = v.list_size();
            for(int i = 0 ; i<size ; i++)
            {
                var vn = v.list_at(i);
                if (vn==null) throw new SystemException("Unexpected");
                
                if (vn.IsType(YDEF.PERIOD)) continue;

                item = nsb.m_pvitem;
                item.setter = null;
                item.getter = null;

                nsb = run_script.run(vn,nsb.curnull());
                
                item = nsb.m_pvitem;
                if (item.o == null) break;                               //最近の流行りを取り入れてnullだったら後ろは処理しない

                if (i<size-1)
                {
                    if (item.getter!=null)
                    {
                        item.o = item.getter();
                        nsb.m_pvitem =item;
                    }
                }
            }

            item = nsb.m_pvitem;

            if (item.mode == PointervarMode.SET)
            {
                nsb.m_cur = item;//.setter;
            }
            else if (item.mode == PointervarMode.GET)
            { 
                if (item.getter!=null)
                {
                    nsb.m_cur = item.getter();
                }
                else
                {
                    nsb.m_cur = item.o;
                }
            }
            else if (item.mode == PointervarMode.NEW)
            {
                nsb.m_cur = item.o;  
            }
            nsb.pvitemnull();

            return nsb;
        }
        public static StateBuffer run_name(YVALUE v, StateBuffer sb)
        {
            var nsb = sb;
            var name = v.GetString();
            PointervarItem item = nsb.m_pvitem;
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
                nsb.m_pvitem = item;
                
                return nsb;                
            }
            var pretype = preobj.GetType();

            if (pretype == typeof(Literal))
            {
                var literal = (Literal)preobj;
                item = GetObj(literal.s,name, item);
                nsb.m_pvitem =item;
                return nsb;
            }

            if (pretype == typeof(Hashtable))
            {
                var ht = (Hashtable)preobj;
                var nameo = name.ToUpper();
                item.o = ht[nameo];
                item.getter = ()=>ht[nameo];
                item.setter_parametertype = null;
                item.setter = (x)=>ht[nameo]=x;
                nsb.m_pvitem = item;
                return nsb;
            }

            item = GetObj(preobj,name, item);
            nsb.m_pvitem = item;
            return nsb;
        }
        public static StateBuffer run_func(YVALUE v, StateBuffer sb, string name, List<object> ol)
        {
            var nsb = sb;
            var item = nsb.m_pvitem;
            var preobj = item.o; //先行ロケーションアイテムの値
            if (preobj == null)  //先行値がない場合は予想外
            {
                throw new SystemException("unexpected");
            }
            var pretype = preobj.GetType();
            if (pretype == typeof(Literal))
            {
                var literal = (Literal)preobj;
                item = ExecuteFunc(literal.s,name,ol,item);
                nsb.m_pvitem =item;
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
            var item = nsb.m_pvitem;
            item.o = v.GetNumber();
            nsb.m_pvitem =item;
            return nsb;
        }
        public static StateBuffer run_qstr(YVALUE v, StateBuffer sb)
        {
            var nsb = sb;
            var item = nsb.m_pvitem;
            item.o =v.GetString();
            nsb.m_pvitem = item;
            return nsb;
        }
        public static StateBuffer run_array_var(YVALUE v, StateBuffer sb, string name, object index_o)
        {
            var nsb  = sb;
            var item = nsb.m_pvitem;
            var preobj = item.o; 
            if (preobj == null)
            {
                throw new SystemException("unexpected");
            }

            //var ol = new List<Object>();
            //ol.Add(index);

            var pretype = preobj.GetType();
            if (pretype == typeof(Literal))
            {
                var literal = (Literal)preobj;
                item = ExecuteArrayVar(literal.s,name, index_o ,item);
                nsb.m_pvitem =item;
            }
            else
            {
                item = ExecuteArrayVar(preobj,name,index_o,item); // tbc
            }
            return nsb;
        }

        // -- tool for this class
#if DOTNENET20
        private static PointervarItem GetObj(string pre, string cur, PointervarItem item)
        {
            //アセンブリ調査 --- set/get不明なので直前の形で返す
            var searchname = (pre + "." + cur).ToUpper();
            var ti = find_typeinfo(searchname);
            if (ti!=null)
            {
                item.o = ti;
                return item;
            }
            //ない場合は、ピリオドで結合してリテラルとして返す
            var literal = new Literal();
            literal.s = pre + "." + cur;
            item.o = literal;
            return item;
        }
        private static PointervarItem GetObj(object o, string cur,PointervarItem item)
        {
            var name = cur.ToUpper();
            Type type = null;
            if (o is Type)
            {
                type = (Type)o;
            }
            object obj  = null;
            if (type!=null)
            {
                obj  = null;
            }
            else
            {
                type = o.GetType();
                obj  = o;
            }

            var mem1 = type.GetDefaultMembers();
            var mem2 = type.GetMembers();
            var find_mi = Array.Find(type.GetMembers(),mi=>mi.Name.ToUpper()==name);
            if (find_mi!=null)
            { 
                if (find_mi.MemberType == MemberTypes.Property)
                { 
                    var pi = type.GetProperty(find_mi.Name);
                    item.getter = ()=>pi.GetValue(obj,null);
                    item.setter_parametertype = pi.PropertyType;
                    item.setter = (x)=> pi.SetValue(obj,x,null);
                    return item;
                }
                if (find_mi.MemberType == MemberTypes.Field)
                {
                    var fi = type.GetField(find_mi.Name);
                    item.getter = ()=> fi.GetValue(obj); 
                    item.setter_parametertype = fi.FieldType;
                    item.setter = (x)=> fi.SetValue(obj,x); 
                    return item;
                }
                throw new System.Exception("unknown");
            }
            return item;
        }
#else
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
            Type type = (Type)o;
            object obj  = null;
            if (type!=null)
            {
                obj  = null;
            }
            else
            {
                type = o.GetType();
                obj  = o;
            }

            var mem1 = type.GetDefaultMembers();
            var mem2 = type.GetMembers();
            var find_mi = Array.Find(type.GetMembers(),mi=>mi.Name.ToUpper()==name);
            if (find_mi!=null)
            { 
                if (find_mi.MemberType == MemberTypes.Property)
                { 
                    var pi = type.GetProperty(find_mi.Name);
                    item.getter = ()=>  { return pi.GetValue(obj); };
                    item.setter = (x)=> { pi.SetValue(obj,x);      };
                    return item;
                }
                if (find_mi.MemberType == MemberTypes.Field)
                {
                    var fi = type.GetField(find_mi.Name);
                    item.getter = ()=>  { return fi.GetValue(obj); };
                    item.setter = (x)=> { fi.SetValue(obj,x); };
                    return item;
                }
                throw new System.Exception("unknown");
            }
            return item;
        }
#endif
        private static PointervarItem ExecuteFunc(string pre, string cur, List<object> param, PointervarItem item)
        {
            if (item!=null && item.mode == PointervarMode.NEW)
            {
                var searchname = (pre + "." + cur).ToUpper();
                var ti = find_typeinfo(searchname);
                if (ti!=null)
                {
                    item.o = runtime.reflection_util.InstantiateType(ti,param.ToArray());//Activator.CreateInstance(ti,args:param.ToArray());
                }
                return item;
            }
            throw new SystemException("unexpected");
        }
        private static PointervarItem ExecuteFunc(object o, string cur, List<object> param, PointervarItem item)
        {
            item.o = reflection_util.ExecuteFunc(o,cur,param.ToArray());
            return item;
        }
        private static PointervarItem ExecuteArrayVar(string pre, string cur, object index_o, PointervarItem item)
        {
            var index = (int)((number)index_o);
            if (item != null && item.mode == PointervarMode.NEW)
            {
                var searchname = (pre + "." + cur).ToUpper();
                var ti = find_typeinfo(searchname);
                if (ti != null)
                {
                    item.o =  Array.CreateInstance(ti, index);
                }
                return item;
            }
            throw new SystemException("unexpected");
        }
        private static PointervarItem ExecuteArrayVar(object o, string cur, object index_o, PointervarItem item)
        {
            var name = cur.ToUpper();
            int index = (index_o!=null && index_o.GetType()==typeof(number)) ? (int)((number)index_o) : -1;

            Type type = null;
            if (o is Type)
            {
                type = (Type)o;
            }
            object obj = null;
            if (type!=null)
            {
                obj = null;
            }
            else
            {
                type = o.GetType();
                obj = o;
            }

            if (type == typeof(Hashtable))
            {
                var ht = (Hashtable)obj;
                var val = ht[name];
                if (val==null)
                {
                    item.getter = null;
                    item.setter_parametertype = null;
                    item.setter = null;
                    item.o = null;
                    return item;
                }
                
                if (val.GetType()==typeof(ARRAY))
                {
                    var l= (ARRAY)val;
                    item.getter = ()=>l[index];
                    item.setter_parametertype = null;
                    item.setter = (x)=>l[index]=x;
                    item.o = l;
                    return item;
                }

                if (val.GetType()==typeof(Hashtable))
                {
                    var ht2 = (Hashtable)val;
                    item.getter = ()=>ht2[index_o];
                    item.setter_parametertype = null;
                    item.setter = (x)=>ht2[index_o] = x;
                    item.o = ht2;
                    return item;
                }

                //var l = (ARRAY)obj;
                //item.getter = () => l[index];
                //item.setter_parametertype = null;
                //item.setter = (x) => l[index] = x;
                //return item;
            }
            var mem1 = type.GetDefaultMembers();
            var mem2 = type.GetMembers();
            var find_mi = Array.Find(type.GetMembers(),mi=>mi.Name.ToUpper()==name);
            if (find_mi!=null)
            { 
                if (find_mi.MemberType == MemberTypes.Property)
                { 
                    var pi = type.GetProperty(find_mi.Name);
                    item.getter = ()=>  { return pi.GetValue(obj,new object[1] { index }); };
                    item.setter_parametertype = pi.PropertyType;
                    item.setter = (x)=> { pi.SetValue(obj,x,new object[1] {index });      };
                    return item;
                }
                if (find_mi.MemberType == MemberTypes.Field)
                {
                    var fi = type.GetField(find_mi.Name);
                    item.getter = ()=>  {
                        var z = fi.GetValue(obj);
                        if (type.IsArray)
                        {
                            var a = (Array)z;
                            return a.GetValue(index);
                        }
                        if (type.IsGenericType && true)
                        {
                            if (type is IList )
                            { 
                                var a = (IList)z;
                                return a[index];
                            }
                        }
                        return null;
                    };
                    item.setter = (x)=> {
                        var z = fi.GetValue(obj);
                        if (type.IsArray)
                        {
                            var a = (Array)z;
                            a.SetValue(x,index);
                        }
                        if (type.IsGenericType && true)
                        {
                            if (type is IList )
                            { 
                                var a = (IList)z;
                                a[index]=x;
                            }
                        }
                    };
                    return item;
                }
                throw new System.Exception("unknown");
            }
            return item;

        }

#if DOTNENET20
        public static Type find_typeinfo(string searchname)
        {
            Type find_ti = cache_util.GetCache_for_find_typeinfo(searchname);
            if (find_ti!=null) return find_ti;

            travarse_asm((ti)=>{
                if (ti.FullName.ToUpper()==searchname)
                { 
                    find_ti = ti;
                }
            });

            cache_util.RecordCache_for_find_typeinfo(searchname,find_ti);

            return find_ti;
        }
        private static void travarse_asm(Action<Type> act)
        {
            foreach(var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var types= asm.GetTypes();

                foreach(var ti in asm.GetTypes())
                {
                    act(ti);
                }
            }
        }
#else
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
#endif
    }
}
