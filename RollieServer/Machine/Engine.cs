using System;
using Windows.Devices.Gpio;

namespace RollieServer.Machine
{
    internal class Engine : IDisposable
    {
        private readonly int _forwardPin;
        private readonly int _backwardPin;

        private GpioPin _forwardGpio;
        private GpioPin _backwardGpio;

        public Engine(int forwardPin, int backwardPin)
        {
            _forwardPin = forwardPin;
            _backwardPin = backwardPin;

            InitGpio();
        }

        private async void InitGpio()
        {
            var gpio = await GpioController.GetDefaultAsync();

            if (gpio == null)
            {
                _forwardGpio = null;
                _backwardGpio = null;
                return;
            }

            _forwardGpio = gpio.OpenPin(_forwardPin);
            _backwardGpio = gpio.OpenPin(_backwardPin);

            _forwardGpio.Write(GpioPinValue.Low);
            _forwardGpio.SetDriveMode(GpioPinDriveMode.Output);

            _backwardGpio.Write(GpioPinValue.Low);
            _backwardGpio.SetDriveMode(GpioPinDriveMode.Output);
        }

        // ensure to stop the servos and dispose the pins:
        public void Dispose()
        {
            Off();
            _forwardGpio?.Dispose();
            _backwardGpio?.Dispose();
        }

        // stop all servos:
        public void Off()
        {
            _forwardGpio?.Write(GpioPinValue.Low);
            _backwardGpio?.Write(GpioPinValue.Low);
        }

        // stop all servos and start moving forward
        public void Forward()
        {
            Off();
            _forwardGpio?.Write(GpioPinValue.High);
        }

        // stop all servos and start moving backward
        public void Backward()
        {
            Off();
            _backwardGpio?.Write(GpioPinValue.High);
        }
    }
}