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
            var cfg = DnsConfig.Load("config.json");

            var logFile=cfg.LogFile;;
            var hosts = cfg.Names;
            var interval = cfg.IntervalInSeconds;
            
            RunBenchmark(hosts,logFile,interval);
            }

            static void Log(string path, string msg)
            {
                using (var sw = File.AppendText(path)){
                    sw.AutoFlush=true;
                    sw.WriteLine($"{DateTime.Now}\t{msg}");
                }
            }


            static void RunBenchmark(IEnumerable<string> hosts, string logFile, int interval){
                while (true){
                foreach(var h in hosts){
                    try{
                        var sp = System.Diagnostics.Stopwatch.StartNew();
                        var a = Dns.GetHostEntry(h);
                        sp.Stop();
                        Log(logFile,$"{h}\t[{sp.Elapsed}]");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h}[{sp.Elapsed}]");
                    }
                    catch(System.Net.Sockets.SocketException ){
                        Log(logFile,$"{h}\tNOT REACHABLE");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h} NOT REACHABLE");
                    }
                    catch(Exception exc){
                        Log(logFile,$"{h}\tERROR {exc}");
                        System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: {h} -> {exc}");
                    }
                }
                Console.Write(".");
                System.Threading.Thread.Sleep(interval*1000);
            }
        }
    }
}
