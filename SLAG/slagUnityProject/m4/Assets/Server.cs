using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class Server
{

    #region データアクセス

    Object m_sync = new Object();

    public class Data
    {
        Object m_sync;
        string msg;

        public Data(Object o)          { m_sync = o; }

        public bool   HasMsg()         {  bool b; lock(m_sync) { b = msg!=null; } return b;        }
        public void   SetMsg(string s) {  lock(m_sync) { msg = s; }                                }
        public string GetMsg()         {  string s; lock(m_sync) { s = msg; msg = null;} return s; }        
    }

    public Data m_req;
    public Data m_res;


    #endregion

    Thread m_thread;
    public void Run()
    {
        m_req = new Data(m_sync);
        m_res = new Data(m_sync);

        m_thread = new Thread(Proc);
        m_thread.Start();
    }
    public void Stop()
    {
        m_thread.Abort();
    }
    public void Proc()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:800/");
        listener.Start();

        for (var loop = 0; loop <= 100; loop++)
        {
            var context = listener.GetContext();
            var request = context.Request;

            string request_text=null;
            foreach(var k in request.QueryString.AllKeys)
            {
                request_text += k +"=" + request.QueryString[k] +",";
            }
            if (request_text==null) request_text = "?";
            m_req.SetMsg(request_text);

            while(!m_res.HasMsg()) Thread.Sleep(5);

            var response = context.Response;

            string res_txt = m_res.GetMsg();
            var responseString ="<HTML><BODY>" + res_txt + "</BODY></HTML>"; //string.Format("<HTML><BODY> Hello world! {0} / {1}</BODY></HTML>",loop,request_text);
            var buffer = Encoding.UTF8.GetBytes(responseString); 
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
        listener.Stop();
    }
}
