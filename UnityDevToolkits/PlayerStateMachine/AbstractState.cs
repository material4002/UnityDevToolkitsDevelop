using UnityEngine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    /// <summary>
    /// 状态抽象类
    /// 用于定义一个状态
    /// 内部有抽象的生命周期函数
    /// </summary>
    public abstract class AbstractState : IChangeState
    {
        //状态名称
        private string _stateName;
        public string stateName => _stateName;
        
        //状态机名称
        private string _stateMachineName;
        public string stateMachineName => _stateMachineName;
        
        //状态机主体
        private StateMachineBody _body;
        public StateMachineBody body => _body;
        
        //Mono组件
        private PlayerStateMachineComp _component;
        public PlayerStateMachineComp component => _component;
        
        //快速组件gameObject
        private GameObject _gameObject;
        public GameObject gameObject => _gameObject;
        
        //快速组件transform
        private Transform _transform;
        public Transform transform => _transform;
        
        
        //    生命周期函数
        public abstract void InitAfterConstruct();
        public abstract void OnAwake();
        public abstract void OnStart();
        
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnLateUpdate();

        public abstract void OnEnterState();
        public abstract void OnExitState();
        
        public abstract void OnMachineEnabled();
        public abstract void OnMachineDisabled();

        //切换场景接口
        public virtual bool ChangeState(string stateName)
        {
            return false;
        }
        
        /// <summary>
        /// 初始化函数，不要主动调用
        /// </summary>
        public void Init(StateMachineBody body, PlayerStateMachineComp component, GameObject gameObject,
            Transform transform, string stateName,string stateMachineName)
        {
            _body = body;
            _component = component;
            _gameObject = gameObject;
            _transform = transform;
            _stateName = stateName;
            _stateMachineName = stateMachineName;
        }
    }
}