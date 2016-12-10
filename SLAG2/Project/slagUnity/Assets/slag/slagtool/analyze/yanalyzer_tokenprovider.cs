using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
#if NUMBERISFLOAT
using number = System.Single;
#else
using number = System.Double;
#endif

namespace slagtool
{
    public partial class yanalyze
    {
        public class TokenProvider
        {
            List<string> m_separators;
            List<YVALUE> m_target;
            List<YVALUE> m_subtarget;
            int          m_index;
            int?         m_sample_start;
            int?         m_sample_end;

            public void Init(List<YVALUE> l, int ob, int cb)
            {
                m_separators = new List<string>(lexPrimitive.operators_all);
                m_separators.Add("NEW");
                m_separators.Add(";");
                m_target = extruct_list(l,ob,cb);
                m_index = 0;
            }

            public bool Update() // return true if done
            {
                /*
                   アップデート毎に１つ要素を指定して解析へ
                   m_index       : １回のアップデートで１つ進行
                   m_subtarget   : 解析対象=m_targetの要素のm_sample_start番目からm_sample_end番目まで
                */
                m_subtarget = null;
                m_sample_start=null;
                m_sample_end  =null;

                int cnt = 0;
                for(int i = 0; i<m_target.Count; i++)        
                {
                    var v = m_target[i];                         
                    var bSep = m_separators.Contains(v.s);
                    if (cnt == m_index)                          
                    {
                        if (m_subtarget==null) m_subtarget = new List<YVALUE>();
                        if (!bSep)
                        {
                            m_subtarget.Add(v);
                            if (m_sample_start==null) m_sample_start = i;
                            m_sample_end = i;
                        }
                    }
                    if (bSep) cnt++;
                }

                m_index++;

                if (m_subtarget!=null)
                { 
                    if (m_subtarget.Count>0)
                    { 
                        _analyze(ref m_subtarget);
                        replace_list(ref m_target,(int)m_sample_start,(int)m_sample_end, m_subtarget);
                    }
                    return false; //continue;
                }
                else
                {
                    return true; // done
                }
            }

            public List<YVALUE> GetResult()
            {
                return m_target;
            }
        }

        public class TokenProvider_prefix //前置演算子
        {
            List<string> m_operators;
            List<string> m_operators_prefix;
            List<YVALUE> m_target;
            List<YVALUE> m_subtarget;
            int          m_index;
            int?         m_sample_start;
            int?         m_sample_end;

            public void Init(List<YVALUE> l, int ob, int cb)
            {
                m_operators = new List<string>(lexPrimitive.operators_binary);
                m_operators.AddRange(lexPrimitive.operators_ternay);

                m_operators_prefix = new List<string>(lexPrimitive.operators_prefix);

                m_target = extruct_list(l,ob,cb);
                m_index = 0;
            }

            public bool Update() // return true if done
            {
                m_subtarget = null;
                m_sample_start=null;
                m_sample_end  =null;

                for(int i = m_index; i<m_target.Count; i++)
                {
                    var v = m_target[i];
                    var bPreOp = m_operators_prefix.Contains(v.s); //前置演算子？
                    if (bPreOp)
                    { 
                        if (i==0) //先頭でかつexprが後続
                        {
                            if (isExpr(i+1))
                            {
                                m_sample_start= i;                                    
                                m_sample_end  = i+1;
                            }
                            else
                            {
                                throw new SystemException("This operator follows something.");
                            }
                        }
                        else //直前が他のオペレータでかつexprが後続
                        {
                            if (isOtherOp(i-1))
                            {
                                if (isExpr(i+1))
                                {
                                    m_sample_start = i;
                                    m_sample_end   = i+1;
                                }
                                else
                                {
                                    throw new SystemException("This operator follows something.");
                                }
                            }
                        }
                    }

                    m_index++;

                    if (m_sample_start!=null)
                    {
                        m_subtarget = new List<YVALUE>();
                        for(int j = (int)m_sample_start; j<= (int)m_sample_end; j++) m_subtarget.Add(m_target[j]);
                        _analyze(ref m_subtarget);
                        replace_list(ref m_target,(int)m_sample_start,(int)m_sample_end, m_subtarget);
                        return false;
                    }

                }
                return true;
            }

            public List<YVALUE> GetResult()
            {
                return m_target;
            }
            // --
            private bool isExpr(int i)
            {
                if (i<0 || i>=m_target.Count) return false;
                return m_target[i].IsType(YDEF.sx_expr);
            }
            private bool isOtherOp(int i)
            {
                if (i<0 || i>=m_target.Count) return false;
                return m_operators.Contains(m_target[i].s);
            }
        }

        public class TokenProvider_postfix //後置演算子
        {
            List<string> m_operators;
            List<string> m_operators_postfix;
            List<YVALUE> m_target;
            List<YVALUE> m_subtarget;
            int          m_index;
            int?         m_sample_start;
            int?         m_sample_end;

            public void Init(List<YVALUE> l, int ob, int cb)
            {
                m_operators = new List<string>(lexPrimitive.operators_binary);
                m_operators.AddRange(lexPrimitive.operators_ternay);

                m_operators_postfix = new List<string>(lexPrimitive.operators_postfix);

                m_target = extruct_list(l,ob,cb);
                m_index = 0;
            }

            public bool Update() // return true if done
            {
                m_subtarget = null;
                m_sample_start=null;
                m_sample_end  =null;

                for(int i = m_index; i<m_target.Count; i++)
                {
                    var v = m_target[i];
                    var bPreOp = m_operators_postfix.Contains(v.s); //後置演算子？
                    if (bPreOp)
                    { 
                        if (isExpr(i-1))
                        {
                            m_sample_start = i-1;
                            m_sample_end   = i;
                        }
                        else
                        {
                            throw new SystemException("This operator follows something.");
                        }
                    }

                    if (m_sample_start!=null)
                    {
                        m_subtarget = new List<YVALUE>();
                        for(int j = (int)m_sample_start; j<= (int)m_sample_end; j++) m_subtarget.Add(m_target[j]);
                        _analyze(ref m_subtarget);
                        replace_list(ref m_target,(int)m_sample_start,(int)m_sample_end, m_subtarget);
                        return false;
                    }

                    m_index++;
                }
                return true;
            }

            public List<YVALUE> GetResult()
            {
                return m_target;
            }
            // --
            private bool isExpr(int i)
            {
                if (i<0 || i>=m_target.Count) return false;
                return m_target[i].IsType(YDEF.sx_expr);
            }
            private bool isOtherOp(int i)
            {
                if (i<0 || i>=m_target.Count) return false;
                return m_operators.Contains(m_target[i].s);
            }
        }
    } 
}