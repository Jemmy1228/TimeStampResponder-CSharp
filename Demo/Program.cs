using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using LibTimeStamp;

namespace Demo
{
    
    class Program
    {
        static TSResponder tsResponder;
        static readonly string TSAPath = @"/TSA/";
        static void Main(string[] args)
        {
            PrintDesc();
            Console.ReadKey();
            Console.Clear();
            try
            {
                tsResponder = new TSResponder(File.ReadAllBytes("TSA.crt"), File.ReadAllBytes("TSA.key"), "SHA1");
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please check your cert and key!");
                Console.ReadLine();
                return;
            }
            HttpListener listener = new HttpListener();
            try
            {
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                listener.Prefixes.Add(@"http://localhost" + TSAPath);
                listener.Prefixes.Add(@"https://localhost" + TSAPath);
                listener.Start();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please run as administrator!");
                Console.ReadLine();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("HTTP Server and TimeStamp Responder started successfully!");
            Console.WriteLine("TSResponder is available at \"http://localhost/TSA/\" or \"http://localhost/TSA/yyyy-MM-ddTHH:mm:ss\"");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            while (true)
            {
                HttpListenerContext ctx = listener.GetContext();
                ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
            }
        }

        static void PrintDesc()
        {
            Console.Title = "JemmyLoveJenny Local TimeStamp Responder";
            Console.WriteLine(
                "[JemmyLoveJenny Local TimeStamp Responder]\r\n" +
                "\r\n" +
                "Please put your TSA cert chain and key in the same folder of this program and name them as \"TSA.crt\" and \"TSA.key\".\r\n" +
                "This program must run in administrator mode in order to start the local http server!\r\n" +
                "TSResponder accept UTC Time in the form of \"yyyy-MM-dd'T'HH:mm:ss\"  For example: \"2019-04-01T15:23:46\"\r\n" +
                "\r\n" +
                "Press any key to start server!"
                );
        }
        static void TaskProc(object o)
        {
            HttpListenerContext ctx = (HttpListenerContext)o;
            ctx.Response.StatusCode = 200;

            HttpListenerRequest request = ctx.Request;
            HttpListenerResponse response = ctx.Response;
            if (ctx.Request.HttpMethod != "POST")
            {
                StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.ASCII);
                writer.WriteLine("TSA Server");
                writer.Close();
                ctx.Response.Close();
            }
            else
            {
                string log = "";
                string date = request.RawUrl.Remove(0, TSAPath.Length);
                DateTime signTime;
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out signTime))
                    signTime = DateTime.UtcNow;

                BinaryReader reader = new BinaryReader(request.InputStream);
                byte[] bRequest = reader.ReadBytes((int)request.ContentLength64);

                bool RFC;
                byte[] bResponse = tsResponder.GenResponse(bRequest, signTime, out RFC);
                if (RFC)
                {
                    response.ContentType = "application/timestamp-reply";
                    log += "RFC3161     \t";
                }
                else
                {
                    response.ContentType = "application/octet-stream";
                    log += "Authenticode\t";
                }
                log += signTime;
                BinaryWriter writer = new BinaryWriter(response.OutputStream);
                writer.Write(bResponse);
                writer.Close();
                ctx.Response.Close();
                Console.WriteLine(log);
            }
        }
    }
}
