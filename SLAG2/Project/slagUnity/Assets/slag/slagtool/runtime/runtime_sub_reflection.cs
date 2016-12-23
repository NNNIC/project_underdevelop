﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using ARRAY = System.Collections.Generic.List<object>;
using number = System.Double;

// constructorのタイプを調べる
//https://msdn.microsoft.com/en-us/library/h93ya84h(v=vs.110).aspx


namespace slagtool.runtime
{
    public class sub_reflection
    {
        internal static object ExecuteFunc(object o, string api, object[] parameters )
        {
            var name = api.ToUpper();

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

            var paramtypes = GetObjectsType(parameters);
            var mts = type.GetMethods();

            MethodInfo find_m = null;
            var mlist = cache_util.GetCache(name,type,paramtypes);
            mlist.AddRange(type.GetMethods());

            foreach(var m in mlist)
            {
                if (m.Name.ToUpper() != name) continue;
                var pis = m.GetParameters();
                if (_isMatchTypes(paramtypes,pis))
                {
                    find_m = m;
                    break;
                }                
            }

            if (find_m!=null)
            {
                cache_util.RecordCache(name,type,paramtypes,find_m);
                var p2 = ChangeObjs(parameters,find_m.GetParameters());
                return find_m.Invoke(obj,p2);
            }

            throw new SystemException("Cannot find method : " + type + "." + name + "(API is none or parameter typs not match.)");
        }
        private static bool _isMatchTypes(Type[] paramtypes, ParameterInfo[] pis)
        {
            var bNull_paramtypes = __isNullOrNothing(paramtypes);
            var bNull_pis        = __isNullOrNothing(pis);

            //両方nullは適合
            if (bNull_paramtypes && bNull_pis) return true;  
            //片方nullは不適合
            if ( bNull_paramtypes ^ bNull_pis) return false;
            //引数の数が異なるは不適合
            if (paramtypes.Length != pis.Length) return false;

            //全型一致検査
            for(int i = 0; i<paramtypes.Length ; i++)
            {
                var p = paramtypes[i];
                var f = pis[i].ParameterType;

                if (p==null && !f.IsValueType) continue; //Null許容はＯＫ
                if (p==f) continue;
                if (__isFloat(p) && __isFloat(f)) continue;//フロート型はdouble/single許容
                if (p.IsSubclassOf(f)) continue; //ベース一致

                return false;
            }
            return true;
        }
        private static bool __isNullOrNothing<T>(T[] x)
        {
            return (x==null || x.Length==0);
        }
        private static bool __isFloat(Type t)
        {
            return (t==typeof(Single) || t==typeof(Double));
        }

        #region タイプ収取
        private static Type[] GetObjectsType(object[] args)
        {
            if (args==null || args.Length==0) return null;
            var tlist = new List<Type>();
            foreach(var a in args)
            {
                if (a!=null)
                {
                    tlist.Add(a.GetType());
                }
                else
                {
                    tlist.Add(null);
                }
            }
            return tlist.ToArray();
        }
        #endregion

        #region オブジェクトのタイプ変換
        private static object[] ChangeObjs(object[] ol, ParameterInfo[] pis)
        {
            util.Assert(ol!=null&&pis!=null&&ol.Length==pis.Length);

            for(int i = 0;i<pis.Length; i++)
            {
                var o  = ol[i];
                var pi = pis[i];
                if (o==null) continue;
                var ot= o.GetType();
                if (ot==pi.ParameterType) continue;
                if (ot.IsEnum) continue;
                if (ot.IsSubclassOf(pi.ParameterType)) continue; 

                ol[i] = Convert.ChangeType(o,pi.ParameterType);
            }

            return ol;
        }
        #endregion

        internal static object InstantiateType(Type type, object[] parameters)
        {
            var paramtypes = GetObjectsType(parameters);
            var cts = type.GetConstructors();
            if (cts==null) return null;

            ConstructorInfo find_c = null;
            
            foreach(var c in cts)
            {
                var pis = c.GetParameters();
                if (_isMatchTypes(paramtypes,pis))
                {
                    find_c = c;
                    break;
                }
            }

            if (find_c==null) throw new SystemException("the constractor can not find " + type.ToString() );

            var p2 = ChangeObjs(parameters,find_c.GetParameters());
            return Activator.CreateInstance(type,args:p2);
        }

    }

    public class cache_util
    {
        private static Dictionary<object,ARRAY> m_hash;
        internal static ARRAY GetCache(object key)
        {
            if (m_hash!=null &&  m_hash.ContainsKey(key))
            {
                return m_hash[key];
            }
            return null;
        }
        internal static void RecordCache(object key, object val)
        {
            if (m_hash == null)
            {
                m_hash = new Dictionary<object, ARRAY>();
            }

            ARRAY vlist = m_hash.ContainsKey(key) ? m_hash[key] : new ARRAY();
            if (!vlist.Contains(val))
            {
                vlist.Add(val);
            }
            m_hash[key] = vlist;
        }

        #region Method Info用
        internal static List<MethodInfo> GetCache(string name,Type type, Type[] tlist)
        {
            var key =_makekey(name,type,tlist);
            var vlist = GetCache(key);

            var ml = new List<MethodInfo>();
            if (vlist!=null) { 
                vlist.ForEach(i=> {
                    if (i is MethodInfo)
                    {
                        ml.Add((MethodInfo)i);
                    }
                });
            }

            return ml;         
        }
        internal static void RecordCache(string name,Type type, Type[] tlist, MethodInfo m)
        {
            var key = _makekey(name,type,tlist);
            RecordCache(key,m);
        }
        private static string _makekey(string name,Type type, Type[] tlist)
        {
            string s = name + type.ToString();
            if (tlist!=null)
            {
                foreach(var t in tlist)
                {
                    s += t!=null ? t.ToString() : "null";
                }
            } 
            return s;
        }
        #endregion

        #region find_typeinfo用キャッシュ
        internal static Type GetCache_for_find_typeinfo(string searchname)
        {
            var key= "GetFindInCache_" + searchname;
            var vlist = GetCache(key);
            if (vlist!=null && vlist[0] is Type)
            {
                return (Type)vlist[0];   
            }    
            return null;
        }
        internal static void RecordCache_for_find_typeinfo(string searchname, Type val)
        {
            var key= "GetFindInCache_" + searchname;
            RecordCache(key,val);
        }
        #endregion

    }
}
