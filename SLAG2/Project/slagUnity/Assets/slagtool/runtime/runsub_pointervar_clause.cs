﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Reflection;
using ARRAY = System.Collections.Generic.List<object>;

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
                //if (item.setter!=null)
                //{
                //    item.setter(setvalue);
                //}
                //TBI
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
            }
            else
            {
                item = GetObj(preobj,name, item);
                nsb.m_pvitem = item;
            }
            return nsb;
        }
        public static StateBuffer run_func(YVALUE v, StateBuffer sb, string name, List<object> ol)
        {
            var nsb = sb;
            var item = nsb.m_pvitem;
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

        // -- tool for this class
#if UNITY_5
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
                    item.getter = ()=>  { return pi.GetValue(obj,null); };
                    item.setter = (x)=> { pi.SetValue(obj,x,null);      };
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
                    item.o = Activator.CreateInstance(ti,args:param.ToArray());
                }
                return item;
            }
            throw new SystemException("unexpected");
        }
        private static PointervarItem ExecuteFunc(object o, string cur, List<object> param, PointervarItem item)
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

            var mts = type.GetMethods();

            var find_mi = Array.Find(type.GetMethods(),mi=>mi.Name.ToUpper()==name);
            if (find_mi!=null)
            {
                item.o = find_mi.Invoke(obj,param.ToArray());
                return item;
            }
            return null;
        }
#if UNITY_5
        private static Type find_typeinfo(string searchname)
        {
            Type find_ti = null;
            travarse_asm((ti)=>{
                if (ti.FullName.ToUpper()==searchname)
                { 
                    find_ti = ti;
                }
            });
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
