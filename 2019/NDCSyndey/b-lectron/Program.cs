using Microsoft.AspNetCore.Components.Electron;

namespace b_lectron
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ComponentsElectron.Run<Startup>("wwwroot/index.html");
        }
    }
}
