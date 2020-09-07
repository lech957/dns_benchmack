
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace dns_check
{
    internal class DnsConfig{

        [JsonPropertyName("Names")]
        public string[] Names {get;set;}
        [JsonPropertyName("LogFile")]
        public string LogFile{get;set;} = "Log.txt";

        [JsonPropertyName("Interval")]
        public int IntervalInSeconds{get; set;} = 120;

        public static DnsConfig Load(string filePath){
            
            return JsonSerializer.Deserialize<DnsConfig>(File.ReadAllText(filePath,System.Text.Encoding.UTF8));
            
        }

    }

}