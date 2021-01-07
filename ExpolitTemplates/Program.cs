using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ExpolitTemplates
{
  
    class Program
    {
        public static string quanjuFile = System.AppDomain.CurrentDomain.BaseDirectory;
        public static string result = "";
        public static string url = "";
        public static string exp_url = "";  //exp url
        public static string exp_url2 = "";  //exp url
        public static string httpMethod = "POST";   //请求方法


        //主方法，就是那个FuckMain
        static void Main(string[] args)
        {
            //炫酷拉轰的启动界面
            startCNM();
            //参数处理
            var argments = CommandLineArgumentParser.Parse(args);
            instruct_getopt(args, argments);
        }


        //Exp主方法
        #region
        public static string Exp(string uri, string data)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
            byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);
            myRequest.ProtocolVersion = HttpVersion.Version11;
            myRequest.Method = httpMethod;
            myRequest.Timeout = Timeout.Infinite;
            myRequest.KeepAlive = true;
            //myRequest.ContentLength = buf.Length;
            myRequest.Referer = "";
            myRequest.ContentType = "application/octet-stream";
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36";
            myRequest.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            myRequest.Headers["Accept-Encoding"] = "gzip, deflate";
            myRequest.Headers["Accept-Language"] = "zh-CN,zh;q=0.9,en;q=0.8";
            //myRequest.Headers.Add("cookie", "");
            myRequest.MaximumAutomaticRedirections = 1;
            myRequest.AllowAutoRedirect = true;

            if (httpMethod.Equals("GET"))
            {
                try
                {
                    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string returnStr = reader.ReadToEnd();
                    reader.Close();
                    myResponse.Close();
                    return returnStr;
                }
                catch (Exception err)
                {
                    Console.WriteLine("GET请求异常！" + err.Message);
                    throw;
                }
            }
            else
            {
                //POST
                try
                {
                    HttpWebRequest myRequest2 = (HttpWebRequest)WebRequest.Create(uri);
                    byte[] buf2 = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

                    myRequest2.Method = "POST";
                    myRequest2.ContentLength = buf.Length;
                    myRequest2.ContentType = "application/json";
                    myRequest2.MaximumAutomaticRedirections = 1;
                    myRequest2.AllowAutoRedirect = true;
                    //发送请求
                    Stream stream = myRequest.GetRequestStream();
                    stream.Write(buf, 0, buf.Length);
                    stream.Close();

                    HttpWebResponse myResponse2 = (HttpWebResponse)myRequest2.GetResponse();
                    StreamReader reader = new StreamReader(myResponse2.GetResponseStream(), Encoding.UTF8);
                    string returnStr = reader.ReadToEnd();
                    reader.Close();
                    myResponse2.Close();
                    return returnStr;
                }
                catch (Exception err)
                {
                    Console.WriteLine("POST请求异常！" + err.Message);
                    throw;
                }

            }

        }
        #endregion




        //炫酷的启动界面
        #region
        public static void startCNM()
        {
            string a0 = @"+==============================================================+";
            string a1 = @"+   ______          _    _____            _                 _  +";
            string a2 = @"+  |  ____|        | |  |  __ \          | |               | | +";
            string a3 = @"+  | |__ _   _  ___| | _| |__) |_ _ _   _| | ___   __ _  __| | +";
            string a4 = @"+  |  __| | | |/ __| |/ /  ___/ _` | | | | |/ _ \ / _` |/ _` | +";
            string a5 = @"+  | |  | |_| | (__|   <| |  | (_| | |_| | | (_) | (_| | (_| | +";
            string a6 = @"+  |_|   \__,_|\___|_|\_\_|   \__,_|\__, |_|\___/ \__,_|\__,_| +";
            string a7 = @"+                                   __/ |                      +";
            string a8 = @"+                                  |___/                       +";
            string a9 = @"+==========================By&Nick=============================+";
            Console.WriteLine(a0);
            Console.WriteLine(a1);
            Console.WriteLine(a2);
            Console.WriteLine(a3);
            Console.WriteLine(a4);
            Console.WriteLine(a5);
            Console.WriteLine(a6);
            Console.WriteLine(a7);
            Console.WriteLine(a8);
            Console.WriteLine(a9);

        }
        #endregion


        //保存文件
        #region
        public static void saveFile(string fuckStr, string filename)
        {
            //false 覆盖，true追加
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(quanjuFile + "\\" + filename, false))
            {
                file.WriteLine(fuckStr);

            }
        }
        #endregion


        //帮助信息打印
        #region
        public static void helpMe()
        {
            Console.WriteLine("参数格式: \n -u 链接,格式:http://xxx.com \n -f 保存文件名,格式:-f xxx.txt \n -r 打印输出 \n -p 主机存活检测 \n -s 保存路径(默认当前目录) \n -read 读取文件");
        }
        #endregion


        //URL规范性检查
        #region
        public static bool urlCheckFuck(string url)
        {
            return Regex.IsMatch(url, "http[s]?://(?:[a-zA-Z]|[0-9]|[$-_@.&+]|[!*\\(\\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+");
        }
        #endregion


        //是否存在HTTP协议
        #region
        public static bool httpCheckFuck(string url)
        {
            return Regex.IsMatch(url, "^http[s]?://");
        }
        #endregion


        //主机存活检查
        #region
        public static bool hostCheckFuck(string url)
        {
            string host = "";
            Ping ping = new Ping();
            PingReply reply = null;
            try
            {
                reply = ping.Send(host, 1000);
                if (reply == null || (reply != null && reply.Status != IPStatus.Success))
                {
                    return false;
                }
                else if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        //结果输出
        #region
        public static void printFuck(string text)
        {
            Console.WriteLine(text);
        }
        #endregion

        //参数处理
        #region
        public static void instruct_getopt(string[] args, CommandLineArgumentParser argments)
        {
            if (args.Length == 0 || !(argments.Has("-u") || argments.Has("-f") || argments.Has("-l") || argments.Has("-r") || argments.Has("-p") || argments.Has("-s") || argments.Has("-read")))
            {
                helpMe();
                return;
            }

            try
            {
                if (argments.Has("-u"))
                {
                    if (urlCheckFuck(argments.Get("-u").Next))
                    {
                        url = argments.Get("-u").Next;
                    }
                    else
                    {
                        Console.WriteLine("url参数不合法");
                    }
                }

                if (argments.Has("-read"))
                {

                    Console.WriteLine("-read参数为:" + argments.Get("-read").Next);

                }


                if (argments.Has("-f"))
                {
                    
                    Console.WriteLine("-f参数为:" + argments.Get("-f").Next);
                }


                if (argments.Has("-l"))
                {
                    Console.WriteLine("-f参数为：" + argments.Get("-l").Next);

                }


                if (argments.Has("-r"))
                {
                    Console.WriteLine("-r参数为：" + argments.Get("-r").Next);
                }


                if (argments.Has("-p"))
                {
                    Console.WriteLine("-p参数为" + argments.Get("-p").Next);
                }


                if (argments.Has("-s"))
                {
                    Console.WriteLine("-s参数为" + argments.Get("-s").Next);
                }

            }
            catch (Exception err)
            {
                Console.WriteLine("参数有误！异常..." + err.Message);
            }

        }
        #endregion



    }
}
