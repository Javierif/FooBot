using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FooBot.Model
{
    public class JokeResponse
    {
        public string type { get; set; }
        public ValueNode value { get; set; }
    }

    public class ValueNode
    {
        public int id { get; set; }
        public string joke { get; set; }
        public string[] categories { get; set; }
    }
}