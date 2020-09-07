using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace dns_check
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<2){

                Console.WriteLine("First argument is log file path. Use hostnames as further arguments");
                return;
            }

            var logFile=args[0];
            var hosts = new List<string>(args.Skip(1));
            
            while (true){
                foreach(var h in hosts){
                    try{
                        var sp = System.Diagnostics.Stopwatch.StartNew();
                        var a = Dns.GetHostEntry(h);
                        sp.Stop();
                        Log(logFile,$"{h}[{sp.Elapsed}]");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h}[{sp.Elapsed}]");
                    }
                    catch(System.Net.Sockets.SocketException ){
                        Log(logFile,$"{h} NOT REACHABLE");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h} NOT REACHABLE");
                    }
                    catch(Exception exc){
                        Log(logFile,$"{h} -> {exc}");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h} -> {exc}");
                    }
                }
                System.Threading.Thread.Sleep(60*1000);
            }

            static void Log(string path, string msg)
            {
                using (var sw = File.AppendText(path)){
                    sw.AutoFlush=true;
                    sw.WriteLine($"{DateTime.Now}: {msg}");
                }
            }
        }
    }
}
