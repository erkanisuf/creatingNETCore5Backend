using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    //Class so ican get the wierd jason which was like {"key:key" : "value"}
    [Serializable]
    public class JSONTags
    {
        [JsonInclude]
        public Dictionary<string, string> tags { get; set; }

    }






}
