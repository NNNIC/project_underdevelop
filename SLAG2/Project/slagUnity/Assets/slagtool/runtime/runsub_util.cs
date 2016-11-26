using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Text;

using ARRAY = System.Collections.Generic.List<object>;

#if UNITY_5
using number = System.Single;
#else
using number = System.Double;
#endif


namespace slagtool.runtime
{
    public class reflection_util
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

            foreach(var m in type.GetMethods())
            {
                if (m.Name.ToUpper() != name) continue;
                var pis = m.GetParameters();// .GetGenericArguments();//    GetFmtParameterType(m.ToString());
                if (_isMatchTypes(paramtypes,pis))
                {
                    return m.Invoke(obj,parameters);
                }                
            }
            return null;
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
                if (p!=f) return false;
            }
            return true;
        }
        private static bool __isNullOrNothing<T>(T[] x)
        {
            return (x==null || x.Length==0);
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
        #region 文字列よりタイプ検索
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
        #endregion

    }
}
