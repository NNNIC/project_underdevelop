using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using slagtool;

namespace slagtool.runtime
{
    public class process
    {
        List<YVALUE> m_illist;  //intermediate langege list
        StateBuffer  m_statebuf;

        public void Init(List<YVALUE> illist)
        {
            m_illist = illist;
            m_statebuf = new StateBuffer();
        }      
        public void Run()
        {
            run_script.run(m_illist[0],m_statebuf);
        }

        #region 関数関連
        public bool ExistFunc(string funcname)
        {
            return builtin.builtin_func.IsFunc(funcname);
        }
        public object CallFunc(string funcname,object[] param)
        {
            if (builtin.builtin_func.IsFunc(funcname))
            {
                return builtin.builtin_func.Run(funcname,param,m_statebuf);
            }
            throw new SystemException("CallFunc : Not Found The Function : " + funcname);
        }
        #endregion

        #region 変数関連
        public bool ExistVal(string name)
        {
            name = name.ToUpper();
            if (m_statebuf!=null&&m_statebuf.m_root_dic!=null)
            {
                return m_statebuf.m_root_dic.ContainsKey(name);
            }
            return false;
        }
        public object GetVal(string name)
        {
            var ret = _getval(name);
            if (ret==null) throw new SystemException("GetVal : Not Found Valriable : " + name);
            return ret;
        }
        private object _getval(string name)
        {
            name = name.ToUpper();
            if (m_statebuf!=null&&m_statebuf.m_root_dic!=null)
            {
                var dic = m_statebuf.m_root_dic;
                if (dic.ContainsKey(name))
                {
                    return dic[name];
                }
            }
            return null;
        }
        public double GetNumVal(string name)
        {
            var ret = _getval(name);
            if (ret==null)                       throw new SystemException("GetNumVal : Not Found Valriable : "     + name);
            if (ret.GetType() != typeof(double)) throw new SystemException("GetNumVal : Valriable is not Number : " + name);
                
            return (double)ret;
        
        }
        public string GetStrVal(string name)
        {
            var ret = _getval(name);
            if (ret==null)                       throw new SystemException("GetStrVal : Not Found Valriable : "     + name);
            if (ret.GetType() != typeof(string)) throw new SystemException("GetStrVal : Valriable is not String : " + name);
                
            return (string)ret;
        }
        public void SetVal(string name, object val,bool bCreateIfNotExist=true)
        {            
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetVal : Fail to Set ; " + name);
            }
        }
        public bool _setval(string name, object val, bool bCreateIfNotExist)
        {
            name = name.ToUpper();
            if (ExistVal(name))
            {
                m_statebuf.m_root_dic[name] = val;
                return true;
            }
            if (bCreateIfNotExist)
            {
                m_statebuf.m_root_dic[name] = val;
                return true;
            }
            return false;
        }
        public void SetNumVal(string name, double val,bool bCreateIfNotExist=true)
        {
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetNumVal : Fail to Set ; " + name);
            }
        }
        public void SetStrVal(string name, string val,bool bCreateIfNotExist=true)
        {
            if (!_setval(name,val,bCreateIfNotExist))
            {
                throw new SystemException("SetStrVal : Fail to Set ; " + name);
            }
        }
        #endregion
    }
}
