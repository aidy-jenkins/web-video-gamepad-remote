using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WebAppHost.NetFramework
{
    public partial class GamepadTest : Form
    {
        private GamepadReader _gamepadReader;

        public GamepadTest()
        {
            InitializeComponent();

            directionLabel.Text = string.Empty;
            buttonLabel.Text = string.Empty;
            
            var fnStateChanged = new Action<(DirectionState Direction, ButtonState Button)>(state =>
            {
                directionLabel.Text = state.Direction.ToString();
                buttonLabel.Text = state.Button.ToString();
            });

            _gamepadReader = new GamepadReader(new ControllerConfig { AxisMaxValue = 65000, DeadZoneRadius = 5000 });

            Task.Run(() => CheckState(fnStateChanged));
        }

        private (DirectionState, ButtonState) _previousState = default;

        private async Task CheckState(Action<(DirectionState Direction, ButtonState Button)> fnStateChanged)
        {
            var state = _gamepadReader.ReadState();
            if (state != _previousState)
                Invoke(new Action(() => fnStateChanged(state)));

            _previousState = state;

            await Task.Delay(50);
            _ = Task.Run(() => CheckState(fnStateChanged));
        }



    }
}
