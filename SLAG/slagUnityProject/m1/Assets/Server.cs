using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class Server
{

    private string __request_message;
    public string  m_reques_message {
        get {
            lock(__request_message)
            { 
                var s = __request_message;
                __request_message = null;
                return s;
            }
        }        
        set {
            lock(__request_message)
            { 
                __request_message = value;
            }
        }
    }

    private string __response_message;
    public string  m_response_message
    {
        get
        {
            lock(__response_message)
            {
                var s = __response_message;
                __response_message = null;
                return s;
            }
        }
        set
        {
            lock(__response_message)
            {
                __response_message = value;
            }
        }
    }


    Thread m_thread;
    public void Run()
    {
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
            m_reques_message = request_text;

            var response = context.Response;

            string res_txt = null;
            while(true)
            { 
                res_txt = m_response_message;
                if (res_txt!=null) break;
                Thread.Sleep(5);
            }
            var responseString = res_txt; //string.Format("<HTML><BODY> Hello world! {0} / {1}</BODY></HTML>",loop,request_text);
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
