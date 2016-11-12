using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class TcpServer {

    // ref http://dobon.net/vb/dotnet/internet/tcpclientserver.html

    Thread m_thread;
    log    m_log;

    public void Start(Action<string> logfunc=null)
    {
        m_thread = new Thread(startServer);
        m_thread.Start();

        m_log = new log(logfunc);
    }
    public void Update()
    {
        m_log.Update();
    }
    private void startServer()
    {
        //ListenするIPアドレス
        string ipString = "127.0.0.1";
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

        //Listenするポート番号
        int port = 2001;

        //TcpListenerオブジェクトを作成する
        System.Net.Sockets.TcpListener listener =
            new System.Net.Sockets.TcpListener(ipAdd, port);

        //Listenを開始する
        listener.Start();
        util.LogLine(string.Format("Listenを開始しました({0}:{1})。",
            ((System.Net.IPEndPoint)listener.LocalEndpoint).Address,
            ((System.Net.IPEndPoint)listener.LocalEndpoint).Port));

        //接続要求があったら受け入れる
        System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
        util.LogLine(string.Format("クライアント({0}:{1})と接続しました。",
            ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
            ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port));

        //NetworkStreamを取得
        System.Net.Sockets.NetworkStream ns = client.GetStream();

        //読み取り、書き込みのタイムアウトを10秒にする
        //デフォルトはInfiniteで、タイムアウトしない
        //(.NET Framework 2.0以上が必要)
        ns.ReadTimeout = 10000;
        ns.WriteTimeout = 10000;

        //クライアントから送られたデータを受信する
        System.Text.Encoding enc = System.Text.Encoding.UTF8;
        bool disconnected = false;
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        byte[] resBytes = new byte[256];
        int resSize = 0;
        do
        {
            //データの一部を受信する
            resSize = ns.Read(resBytes, 0, resBytes.Length);
            //Readが0を返した時はクライアントが切断したと判断
            if (resSize == 0)
            {
                disconnected = true;
                util.LogLine("クライアントが切断しました。");
                break;
            }
            //受信したデータを蓄積する
            ms.Write(resBytes, 0, resSize);
            //まだ読み取れるデータがあるか、データの最後が\nでない時は、
            // 受信を続ける
        } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
        //受信したデータを文字列に変換
        string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        ms.Close();
        //末尾の\nを削除
        resMsg = resMsg.TrimEnd('\n');
        Console.WriteLine(resMsg);

        if (!disconnected)
        {
            //クライアントにデータを送信する
            //クライアントに送信する文字列を作成
            string sendMsg = resMsg.Length.ToString();
            //文字列をByte型配列に変換
            byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
            //データを送信する
            ns.Write(sendBytes, 0, sendBytes.Length);
            Console.WriteLine(sendMsg);
        }

        //閉じる
        ns.Close();
        client.Close();
        util.LogLine("クライアントとの接続を閉じました。");

        //リスナを閉じる
        listener.Stop();
        util.LogLine("Listenerを閉じました。");
    }

    public class util
    {
        private static string buf = null;
        public static void Log(string s) { buf += s; }
        public static void LogLine(string s) { var a = buf + s; buf = null; Debug.Log(s); }
    }

    public class log
    {
        Action<string> m_logFunc;

        object m_mutex;
        string m_tmp; //途中
        string m_out; //出力用


        public log(Action<string> logfunc) { m_mutex = new object(); m_logFunc = logfunc;}
        public void Update()
        {
            lock(m_mutex)
            {
                if (m_out!=null)
                {
                    if (m_logFunc!=null) m_logFunc(m_out);
                    m_out = null;
                }
            }
        }
        public void Write(string s)
        {
            m_tmp +=s;
        }
        public void WriteLine(string s=null)
        {
            m_tmp += s + Environment.NewLine;
            lock(m_mutex)
            {
                m_out += m_tmp;
                m_tmp = null;
            }
        }
    }
}
