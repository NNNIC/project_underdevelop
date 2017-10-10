using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ReadOnceValue<T>
{
    T m_resetVal;
    T m_val;

    public ReadOnceValue(T resetVal)
    {
        m_resetVal = resetVal;
    }
    public ReadOnceValue()
    {
        m_resetVal = default(T);
    }

    public void Set(T val) { m_val = val; }
    public T    Get() {
        var x = m_val;
        m_val = m_resetVal;
        return x;
    }
    public T Peek()
    {
        return m_val;
    }
}
