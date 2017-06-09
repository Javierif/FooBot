using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FooBot.Controllers
{
    public static class Util
    {


            public static int GetRandomNumber(int minimum, int maximum)
        {
            Random rnd = new Random();
            return rnd.Next(minimum, maximum);
        }
    
    }
}