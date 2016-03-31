using System;

namespace RollieServer.Machine
{
    internal class RollieMachine : IDisposable
    {
        public RollieMachine(Engine leftEngine, Engine rightEngine)
        {
            _leftEngine = leftEngine;
            _rightEngine = rightEngine;

            LeftState = EngineState.Off;
            RightState = EngineState.Off;
        }

        private readonly Engine _leftEngine;
        private readonly Engine _rightEngine;

        public EngineState RightState { get; set; }

        public EngineState LeftState { get; set; }

        public void Move()
        {
            MoveLeft();

            MoveRight();
        }

        private void MoveLeft()
        {
            if (LeftState == EngineState.Off)
            {
                _leftEngine.Off();
            }
            else if (LeftState == EngineState.ForwardOn)
            {
                _leftEngine.Forward();
            }
            else if (LeftState == EngineState.BackwardOn)
            {
                _leftEngine.Backward();
            }
        }

        private void MoveRight()
        {
            if (RightState == EngineState.Off)
            {
                _rightEngine.Off();
            }
            else if (RightState == EngineState.ForwardOn)
            {
                _rightEngine.Forward();
            }
            else if (RightState == EngineState.BackwardOn)
            {
                _rightEngine.Backward();
            }
        }

        public void Dispose()
        {
            _leftEngine.Dispose();
            _rightEngine.Dispose();
        }
    }
}