namespace RollieServer.Machine
{
    internal enum RollieState
    {
        Off,
        ForwardOn,
        BackwardOn,
        TurnRight,
        TurnRightStrong,
        TurnLeft,
        TurnLeftStrong
    }
    internal enum EngineState
    {
        Off,
        ForwardOn,
        BackwardOn,
    }
}