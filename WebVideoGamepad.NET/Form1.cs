using Gecko;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebAppHost.NetFramework
{
    public partial class Form1 : Form
    {
        private GamepadReader _gamepadReader;

        public Form1()
        {
            InitializeComponent();

            _gamepadReader = new GamepadReader(Config.Instance.Controller);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var frameConfig = Config.Instance.Frame;
            if (frameConfig.Fullscreen)
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            var appConfig = Config.Instance.App;

            GeckoPreferences.User["general.useragent.override"] = appConfig.UserAgent;
            browser.Navigate(appConfig.Url);

            if (!string.IsNullOrEmpty(appConfig.InjectScript))
            {
                browser.Navigated += (o, e) =>
                {
                    using (var context = new AutoJSContext(browser.Window))
                        context.EvaluateScript(appConfig.InjectScript);
                };
            }

            _ = Task.Run(CheckStateLoop);
        }

        private void StateChanged((DirectionState Direction, ButtonState Button) state)
        {
            if (state.Direction == DirectionState.None && state.Button == ButtonState.None)
                return;

            if (state.Direction != DirectionState.None)
            {
                var key = $"Arrow{state.Direction}";
                var code = state.Direction switch
                {
                    DirectionState.Left => 37,
                    DirectionState.Up => 38,
                    DirectionState.Right => 39,
                    DirectionState.Down => 40,
                    _ => throw new Exception("Failed to map direction input")
                };
                SendKeyEvent(code, key);
            }

            if (state.Button != ButtonState.None)
            {
                var (key, code) = state.Button switch
                {
                    ButtonState.A => ("Enter", 13),
                    ButtonState.B => ("Escape", 27),
                    ButtonState.Y => ("Exit", -1),
                    _ => (string.Empty, 0)
                };

                if (code == -1)
                    this.Close();

                if (code != 0)
                    SendKeyEvent(code, key);
            }
        }

        private (DirectionState, ButtonState) _previousState = default;

        private void CheckStateLoop()
        {
            while (true)
            {
                var state = _gamepadReader.ReadState();
                if (state != _previousState)
                    Invoke(new Action(() => StateChanged(state))); //Call back to UI thread with new state


                Thread.Sleep(50);

                _previousState = state;
            }
        }

        private void SendKeyEvent(int code, string key)//, bool alt = false, bool ctrl = false, bool shift = false)
        {
            browser.Window?.WindowUtils.SendNativeKeyEvent(aNativeKeyCode: code, aCharacters: key, aNativeKeyboardLayout: 0, aModifierFlags: 0, aUnmodifiedCharacters: null);
        }
    }
}
