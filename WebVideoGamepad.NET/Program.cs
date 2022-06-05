using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAppHost.NetFramework
{
    internal static class Program
    {
        private const string ConfigFilename = "config.json";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Gecko.Xpcom.Initialize("Firefox");

            if (!File.Exists(ConfigFilename))
                throw new Exception("Failed to find config");

            var configContent = File.ReadAllText(ConfigFilename);
            Config.Instance = System.Text.Json.JsonSerializer.Deserialize<Config>(configContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
