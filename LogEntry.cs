using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


namespace dns_check{

    class LogEntry{

        public DateTime Time{get; private set;}
        public string Host {get; private set;}

        public TimeSpan Duration{get; private set;}
        public string Error {get; private set;}

        public bool IsError => !string.IsNullOrEmpty(Error);

        private LogEntry(DateTime time, string host, TimeSpan duration, string error=null){
            Time=time;
            Host=host;
            Duration=duration;
            Error=error;
        }

        static LogEntry Create(string line){
            var parts = line.Split("\t",StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length ==3){

                var dt = DateTime.Parse(parts[0]);
                var host = parts[1];
                TimeSpan duration ;
                string error=null;
                if (!TimeSpan.TryParse(parts[2].Trim('[',']'), out duration)){
                    error = parts[2];
                }
                return new LogEntry(dt,host,duration,error);
            }
            return null;
        }

        public static IEnumerable<LogEntry> GetLogEntries(string logFilePath){

            foreach (var l in File.ReadAllLines(logFilePath)){
                yield return Create(l);
            }
        }

        public static IEnumerable<LogEntry> GetWorstEntries(IEnumerable<LogEntry> source, int count=10){

            return source.OrderByDescending(_ => _.Duration).Take(count);

        }

        public static IEnumerable<LogEntry> GetErrorEntries(IEnumerable<LogEntry> source, int count=10){

            return source.OrderByDescending(_ => _.Time).Where(_=>_.IsError).Take(count);

        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Error)){
                return $"{Time}\t{Host}\t{Duration}";
            }
            return $"{Time}\t{Host}\t{Error}";
        }
    }

}