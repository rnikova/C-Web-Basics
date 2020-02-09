using SIS.MvcFramework;

namespace Musaca.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
