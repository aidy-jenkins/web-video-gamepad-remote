using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppHost.NetFramework
{
    public class ControllerConfig
    {
        public int AxisMaxValue { get; set; }
        public int DeadZoneRadius { get; set; }
    }

    public class GamepadReader
    {
        private DirectInput _input;
        private IEnumerable<Joystick> _joysticks;
        private ControllerConfig _controllerConfig;
        private int _axisNeutralPoint;

        private const int XBOX_ONE_A_BUTTON = 0;
        private const int XBOX_ONE_B_BUTTON = 1;
        private const int XBOX_ONE_X_BUTTON = 2;
        private const int XBOX_ONE_Y_BUTTON = 3;

        //PointOfViewController[0], -1 == not pressed
        private const int XBOX_ONE_DPAD_UP = 0;
        private const int XBOX_ONE_DPAD_DOWN = 18000;
        private const int XBOX_ONE_DPAD_LEFT = 27000;
        private const int XBOX_ONE_DPAD_RIGHT = 9000;

        public GamepadReader(ControllerConfig config)
        {
            _controllerConfig = config;
            _axisNeutralPoint = config.AxisMaxValue / 2;
            _input = new DirectInput();

            _ = Task.Run(CheckForControllerChanges);
        }

        public void CheckForControllerChanges()
        {
            while (true)
            {
                var devices = _input.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly);

                _joysticks = devices.Select(device => new Joystick(_input, device.InstanceGuid)).ToArray();

                foreach (var joystick in _joysticks)
                {
                    joystick.Acquire();
                }

                Thread.Sleep(5000);
            }
        }

        public (DirectionState, ButtonState) ReadState()
        {
            DirectionState directionState = default;
            ButtonState buttonState = default;

            var joysticks = _joysticks;

            if (joysticks == null)
                return default;

            foreach (var joystick in joysticks)
            {
                try
                {

                    joystick.Poll();
                    var state = joystick.GetCurrentState();

                    directionState |= ReadControlStickState(state);
                    directionState |= ReadDPadState(state);

                    var pressedButtons = state.Buttons.Select((pressed, index) => (pressed, index)).Where(x => x.pressed);
                    foreach (var button in pressedButtons)
                        buttonState |= ReadButtonState(button);
                }
                catch { /* do nothing */ }
            }

            return (directionState, buttonState);
        }

        private static ButtonState ReadButtonState((bool pressed, int index) button)
            => button.index switch
            {
                XBOX_ONE_A_BUTTON => ButtonState.A,
                XBOX_ONE_B_BUTTON => ButtonState.B,
                XBOX_ONE_X_BUTTON => ButtonState.X,
                XBOX_ONE_Y_BUTTON => ButtonState.Y,
                _ => ButtonState.None
            };

        private static DirectionState ReadDPadState(JoystickState state)
            => state.PointOfViewControllers[0] switch
            {
                XBOX_ONE_DPAD_UP => DirectionState.Up,
                XBOX_ONE_DPAD_DOWN => DirectionState.Down,
                XBOX_ONE_DPAD_LEFT => DirectionState.Left,
                XBOX_ONE_DPAD_RIGHT => DirectionState.Right,
                _ => DirectionState.None
            };

        private DirectionState ReadControlStickState(JoystickState state)
        {
            var directionState = default(DirectionState);

            var xState = state.X - _axisNeutralPoint;
            if (Math.Abs(xState) > _controllerConfig.DeadZoneRadius)
                directionState |= xState < 0 ? DirectionState.Left : DirectionState.Right;

            var yState = state.Y - _axisNeutralPoint;
            if (Math.Abs(yState) > _controllerConfig.DeadZoneRadius)
                directionState |= yState < 0 ? DirectionState.Up : DirectionState.Down;

            return directionState;
        }
    }

    public enum DirectionState
    {
        //The last digit represents whether any direction is active
        //The middle digit indicates direction
        //The first digit indicates whether direction is horizontal

        None,       // 000
        Up = 0x1,   // 001
        Down = 0x3, // 011
        Left = 0x5, // 101
        Right = 0x7 // 111
    }

    public enum ButtonState
    {
        None = 0x0,
        A = 0x1,
        B = 0x2,
        X = 0x4,
        Y = 0x8
    }
}
