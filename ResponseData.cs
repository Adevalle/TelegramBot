using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekegrammBot
{
    internal class ResponseData
    {
            public bool valid { get; set; }
            public int updated { get; set; }
            public string @base { get; set; }
            public Dictionary<string,double> rates { get; set; }
       
    }
}
