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
        static void Main(string[] args)
        {
            var parsed = ParseArgs(args);

            Gecko.Xpcom.Initialize("Firefox");

            if (!File.Exists(ConfigFilename))
                throw new Exception("Failed to find config");

            var configContent = File.ReadAllText(ConfigFilename);
            Config.Instance = JsonSerializer.Deserialize<Config>(configContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Config.Instance.App.Url = parsed.Url ?? Config.Instance.App.Url;
            Config.Instance.App.UserAgent = parsed.UserAgent ?? Config.Instance.App.UserAgent;
            Config.Instance.Frame.Fullscreen = parsed.Fullscreen ?? Config.Instance.Frame.Fullscreen;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        class CommandArgs
        {
            public string Url { get; set; }
            public string UserAgent { get; set; }
            public bool? Fullscreen { get; set; }
        }

        static CommandArgs ParseArgs(string[] args)
        {
            var parsed = new CommandArgs();

            if (!args.Any())
                return parsed;

            var indexedArgs = args.Select((arg, index) => (Name: arg, Index: index)).Where(arg => arg.Name.StartsWith("-"));

            var argDictionary = indexedArgs
                .Where(arg => !arg.Name.StartsWith("--"))
                .ToDictionary(arg => arg.Name, arg => ReadValue(args[arg.Index + 1]), StringComparer.OrdinalIgnoreCase);

            foreach (var arg in indexedArgs.Where(arg => arg.Name.StartsWith("--")))
                argDictionary.Add(arg.Name, "true");

            parsed.Url = GetValueOrDefault(argDictionary, "-url");
            parsed.UserAgent = GetValueOrDefault(argDictionary, "-useragent");
            var fullscreen = GetValueOrDefault(argDictionary, "--fullscreen");
            if (!string.IsNullOrEmpty(fullscreen))
                parsed.Fullscreen = Convert.ToBoolean(fullscreen);

            return parsed;
        }

        private static string ReadValue(string arg)
        {
            if (arg.StartsWith("\""))
                arg = arg.Substring(1);
            
            if (arg.EndsWith("\""))
                arg = arg.Substring(0, arg.Length - 2);

            return arg;
        }

        private static TValue GetValueOrDefault<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
        {
            if (!dictionary.TryGetValue(key, out var value))
                return default;
            
            return value;
        }
    }
}
