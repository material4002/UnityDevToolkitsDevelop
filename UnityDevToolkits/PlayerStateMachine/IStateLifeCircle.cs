namespace Material.UnityDevToolkits.PlayerStateMachine
{
    public interface IStateLifeCircle
    {
        //    生命周期函数
        public  void OnAwake();
        public  void OnStart();
        
        public  void OnUpdate();
        public  void OnFixedUpdate();
        public  void OnLateUpdate();
        
        public  void OnMachineEnabled();
        public  void OnMachineDisabled();
    }
}