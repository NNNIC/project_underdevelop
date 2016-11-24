using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#if UNITY_5
using number = System.Single;
#else
using number = System.Double;
#endif

namespace slagtool
{
    public static class YSAVELOAD
    {
        static List<YVALUE> __temp;
        static List<YVALUE> m_temp
        {
            get {return __temp; }
            set
            {
                if (value==null)
                {
                    sys.logline("!!");
                   
                }
                __temp = value;
            }
        }

        /// <summary>
        /// セーブ
        /// ※pathがnull時は内部保存
        /// </summary>
        public static void Save(List<YVALUE> l, string path)
        {
            m_temp = l;

            if (path!=null)
            { 
                var data = GetBin();

                try { 
                    File.WriteAllBytes(path,data);
                } catch
                {
                    sys.error("Faild to save!");
                }
            }
        }

        /// <summary>
        /// バイナリ保存
        /// </summary>
        public static void Save(byte[] bytes, string path)
        {
            m_temp = Load(bytes);
            if (path!=null)
            {
                try { 
                    File.WriteAllBytes(path,bytes);
                } catch
                {
                    sys.error("Faild to save!");
                }
            }
        }

        /// <summary>
        /// ロード
        /// pathがnul時は内部保存を返す
        /// </summary>
        public static List<YVALUE> Load(string path)
        {
            if (path != null)
            { 
                if (!File.Exists(path))
                {
                    sys.error("File does not exist! .. " + path);
                }
                var bytes = File.ReadAllBytes(path);

                return Load(bytes);
            }
            return m_temp;
        }

        /// <summary>
        /// バイナリからロード
        /// </summary>
        public static List<YVALUE> Load(byte[] data)
        {
            var ms = new MemoryStream(data);

            List<YVALUE> l = null;
            try {
                var bf = new BinaryFormatter();
                l = (List<YVALUE>)bf.Deserialize(ms);
            }
            catch (SystemException e)
            {
                sys.error("Faild to conver to yvalue list : " + e.Message);
            }

            m_temp = l;
            return l;
        }

        /// <summary>
        /// バイナリを返す
        /// </summary>
        public static byte[] GetBin()
        {
            if (m_temp==null) return null;

            byte[] data = null;

            try { 
                using (var ms = new MemoryStream())
                { 
                    var bf = new BinaryFormatter();
                    bf.Serialize(ms, m_temp);
                    data = ms.ToArray();
                }
            }
            catch (SystemException e)
            {
                data = null;
                sys.error("Faile to convert to binary : " + e.Message);
            }

            return data;
        }
    }
}
