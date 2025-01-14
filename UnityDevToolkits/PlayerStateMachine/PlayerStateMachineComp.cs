using System;
using UnityEngine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    public class PlayerStateMachineComp : MonoBehaviour,IGetStateMachineBody,IChangeState,IStateLifeCircle
    {
        public string stateMachineName = "";
        public string defaultStateName = "Default";
        private StateMachineBody _body;

        private string _currentState="";
        private string _lastState="";
        private bool _isChangingState = false;

        public string CurrentStateName => _body.body.currentStateName;
        private void Awake()
        {
            //获取状态机主体
            _body = this.GetStateMachineBody(stateMachineName);
            
            //初始化状态机主体
            _body.Init(_body,this,gameObject,transform,"body",stateMachineName);
            _body.OnAwake();
            
            OnAwake();
        }

        private void Start()
        {
            _body.OnStart();
            //切换到默认状态
            ChangeState(defaultStateName);
            
            OnStart();
        }

        private void OnEnable()
        {
            _body.OnMachineEnabled();
            
            OnMachineEnabled();
        }

        private void OnDisable()
        {
            _body.OnMachineDisabled();
            
            OnMachineDisabled();
        }

        private void Update()
        {
            _body.OnUpdate();
            
            OnUpdate();;
        }

        private void LateUpdate()
        {
            _body.OnLateUpdate();
            
            OnLateUpdate();

            if (_isChangingState)
            {
                _isChangingState = false;
                _body.ChangeStateImmediately(_currentState);
                _lastState = _currentState;
            }
        }

        private void FixedUpdate()
        {
            _body.OnFixedUpdate();
            
            OnFixedUpdate();
        }

        public bool ChangeState(string stateName)
        {
            if (_body.ContainState(stateName))
            {
                _currentState = stateName;
                _isChangingState = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void OnAwake()
        {
            
        }

        public virtual void OnStart()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public virtual void OnLateUpdate()
        {
            
        }

        

        public virtual void OnMachineEnabled()
        {
            
        }

        public virtual void OnMachineDisabled()
        {
            
        }
    }
}