using System;

public struct ReadResetValue<T> {

    T m_value;

    public T Read()
    {
        var v = m_value;
        m_value = default(T);
        return v;
    }
    public void Write(T v)
    {
        m_value = v;
    }

}
