using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    /// <summary>
    /// 储存状态机的数据结构，
    /// 将状态封装在状态机中，
    /// 封装操作函数和生命周期函数。
    /// 使用工厂进行获取。
    /// 注入到Comp中
    ///
    /// 获取过程：
    /// 1.Config生成State并生成组装储存Body字典
    /// 2.工厂使用状态机名称返回body
    /// 3.comp使用注册的状态机名称从工厂获取
    /// </summary>
    public class StateMachineBody : AbstractState,ICurrentState
    {
        
        private readonly Dictionary<string, List<AbstractState>> _statesDic = null;
        
        private string _currentStateName;
        public string currentStateName => _currentStateName;
        
        private List<AbstractState> _currentStateList;
        
        
        
        public override void InitAfterConstruct()
        {
            
        }
        
        /// <summary>
        /// 由comp进行调用，调用全部的Awake
        /// </summary>
        public override void OnAwake()
        {
            if (_statesDic.Any())
            {
                foreach (List<AbstractState> list in _statesDic.Values)
                {
                    foreach (AbstractState state in list)
                    {
                        state.OnAwake();
                    }
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用全部的Start
        /// </summary>
        public override void OnStart()
        {
            if (_statesDic.Any())
            {
                foreach (List<AbstractState> list in _statesDic.Values)
                {
                    foreach (AbstractState state in list)
                    {
                        state.OnStart();
                    }
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用当前状态的的Update
        /// </summary>
        public override void OnUpdate()
        {
            if (_currentStateList.Any())
            {
                foreach (AbstractState state in _currentStateList)
                {
                    state.OnUpdate();
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用当前状态的的FixedUpdate
        /// </summary>
        public override void OnFixedUpdate()
        {
            if (_currentStateList.Any())
            {
                foreach (AbstractState state in _currentStateList)
                {
                    state.OnFixedUpdate();
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用当前状态的的LateUpdate
        /// </summary>
        public override void OnLateUpdate()
        {
            if (_currentStateList.Any())
            {
                foreach (AbstractState state in _currentStateList)
                {
                    state.OnLateUpdate();
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用当前状态的的EnterState
        /// </summary>
        public override void OnEnterState()
        {
            if (_currentStateList.Any())
            {
                foreach (AbstractState state in _currentStateList)
                {
                    state.OnEnterState();
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用当前状态的的ExitState
        /// </summary>
        public override void OnExitState()
        {
            if (_currentStateList.Any())
            {
                foreach (AbstractState state in _currentStateList)
                {
                    state.OnExitState();
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用全部的MachineEnabled
        /// </summary>
        public override void OnMachineEnabled()
        {
            if (_statesDic.Any())
            {
                foreach (List<AbstractState> list in _statesDic.Values)
                {
                    foreach (AbstractState state in list)
                    {
                        state.OnMachineEnabled();
                    }
                }
            }
        }
        
        /// <summary>
        /// 由comp进行调用，调用全部的MachineDisabled
        /// </summary>
        public override void OnMachineDisabled()
        {
            if (_statesDic.Any())
            {
                foreach (List<AbstractState> list in _statesDic.Values)
                {
                    foreach (AbstractState state in list)
                    {
                        state.OnMachineDisabled();
                    }
                }
            }
        }
        
        /// <summary>
        /// 构造器
        /// </summary>
        public StateMachineBody()
        {
            //创建字典
            _statesDic = new Dictionary<string, List<AbstractState>>();
            
            //添加默认状态
            _statesDic.Add("Default", new List<AbstractState>());
            _currentStateList = _statesDic["Default"];
        }
        
        /// <summary>
        /// 添加状态到状态机中
        /// </summary>
        /// <param name="stateName">状态的名称，不填写默认为类名</param>
        /// <param name="abstractState">状态节点</param>
        public void AddState(string stateName, AbstractState abstractState)
        {
            if(stateName == null || abstractState == null || string.IsNullOrEmpty(stateName))return;
            if (!_statesDic.ContainsKey(stateName))
            {
                _statesDic[stateName] = new List<AbstractState>();
            }
            
            _statesDic[stateName].Add(abstractState);
            
        }
        
        
        public override bool ChangeState(string stateName)
        {
            return component.ChangeState(stateName);
        }
        
        public bool ContainState(string stateName)
        {
            return _statesDic.ContainsKey(stateName);
        }

        public void ChangeStateImmediately(string stateName)
        {
            OnExitState();
            _currentStateList = _statesDic[stateName];
            OnEnterState();
        }

        public override void Init(StateMachineBody body, PlayerStateMachineComp component, GameObject gameObject, Transform transform,
            string stateName, string stateMachineName)
        {
            base.Init(body, component, gameObject, transform, stateName, stateMachineName);
            
            //为状态机添加数据
            if(_statesDic.Any())
                foreach (KeyValuePair<string,List<AbstractState>> pair in _statesDic)
                {
                    if(pair.Value.Any())
                        foreach (AbstractState state in pair.Value)    
                        {
                            state.Init(this,component,gameObject,transform,pair.Key,stateMachineName);
                        }
                }
        }
    }
}