using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppHost.NetFramework
{
    internal class Config
    {
        public static Config Instance { get; set; }

        public FrameConfig Frame { get; set; }
        public AppConfig App { get; set; }
        public ControllerConfig Controller { get; set; }
    }

    internal class FrameConfig
    {
        public bool Fullscreen { get; set; }
    }

    internal class AppConfig
    {
        public string Url { get; set; }
        public string UserAgent { get; set; }
        public string InjectScript { get; set; }
    }

    
}
