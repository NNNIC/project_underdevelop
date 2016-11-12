using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

public class test : MonoBehaviour
{

    //public class Server
    //{
    //    static Thread thread;
    //    public void Run()
    //    {
    //        thread = new Thread(Proc);
    //        thread.Start();
    //    }
    //    public void Stop()
    //    {
    //        thread.Abort();
    //    }

    //    public void Proc()
    //    {
    //        var listener = new HttpListener();
    //        listener.Prefixes.Add("http://localhost:800/");
    //        listener.Start();

    //        for (var loop = 0; loop <= 100; loop++)
    //        {
    //            var context = listener.GetContext();
    //            var request = context.Request;
    //            var response = context.Response;
    //            var responseString = string.Format("<HTML><BODY> Hello world! {0}</BODY></HTML>",loop);
    //            var buffer = Encoding.UTF8.GetBytes(responseString);
    //            response.ContentLength64 = buffer.Length;
    //            using (var output = response.OutputStream)
    //            {
    //                output.Write(buffer, 0, buffer.Length);
    //                output.Close();
    //            }
    //        }
    //        listener.Stop();
    //    }
    //}

    Server m_server;
    void Start()
    {
        m_server = new Server();
        m_server.Run();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDestroy()
    {
        m_server.Stop();
    }
}
