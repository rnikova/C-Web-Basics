﻿using SIS.MvcFramework;

namespace Panda.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
