namespace Material.UnityDevToolkits.PlayerStateMachine.StateMachine
{
    public interface ICurrentState
    {
        public StateMachineBody body { get; }
        public string stateName { get; }
        public string currentStateName { get; }
    }
}