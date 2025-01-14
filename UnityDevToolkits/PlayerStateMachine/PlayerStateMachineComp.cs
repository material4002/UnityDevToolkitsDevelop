using System;
using UnityEngine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    public class PlayerStateMachineComp : MonoBehaviour,IGetStateMachineBody,IChangeState
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
            
        }

        private void Start()
        {
            _body.OnStart();
            //切换到默认状态
            ChangeState(defaultStateName);
        }

        private void OnEnable()
        {
            _body.OnMachineEnabled();
        }

        private void OnDisable()
        {
            _body.OnMachineDisabled();
        }

        private void Update()
        {
            _body.OnUpdate();
        }

        private void LateUpdate()
        {
            _body.OnLateUpdate();

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
    }
}