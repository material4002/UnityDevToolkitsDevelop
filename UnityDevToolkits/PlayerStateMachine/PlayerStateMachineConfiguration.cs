using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.PlayerStateMachine.StateMachine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    /// <summary>
    /// 获取注册的状态
    /// 一个状态可能配置多个状态机
    /// 将状态机储存到字典中，使用工厂进行获取
    /// </summary>
    [RegisterConfig(typeof(RegisterState))]
    public class PlayerStateMachineConfiguration: IConfig, IConfigInit,IAfterConfig
    {
        private Dictionary<string, StateMachineBody> _stateMachineDic;
        private Type _abstractStateType = typeof(AbstractState);
        
        private List<(Assembly assembly, Type classType, Type attributeType,string stateName)> _delayLoadState;
       
        
        public void OnConfigInit()
        {
            _stateMachineDic = new Dictionary<string, StateMachineBody>();
            _delayLoadState = new List<(Assembly assembly, Type classType, Type attributeType,string stateName)>();
        }
        
        public void BeforeConfig()
        {
            
        }

        public void Config(Assembly assembly, Type classType, Type attributeType)
        {
            if(!classType.IsSubclassOf(_abstractStateType))return;
            RegisterState[] stateAttrs = classType.GetCustomAttributes<RegisterState>().ToArray();

            if (stateAttrs.Any())
            {
                foreach (RegisterState stateAttr in stateAttrs)
                {
                    //如果状态机名称为空，则添加到全部的状态机中
                    if (string.IsNullOrEmpty(stateAttr.stateMachineName))
                    {
                        //由于不清楚是否全部的状态机已经创建完成，所以这里不进行添加
                        //需要延迟进行加载
                        
                        //验证状态名
                        string stateName = stateAttr.stateName;
                        if(string.IsNullOrEmpty(stateAttr.stateName)) stateName = classType.Name;
                        
                        //添加到延迟加载列表中
                        _delayLoadState.Add((assembly,classType,attributeType,stateName));
                    }
                    else//如果状态机名称不为空，则添加到指定的状态机中
                    {
                        //如果不存在该状态机，则创建新的状态机
                        if (!_stateMachineDic.ContainsKey(stateAttr.stateMachineName))
                        {
                            _stateMachineDic.Add(stateAttr.stateMachineName, new StateMachineBody());
                        }
                    
                        //获取状态机
                        StateMachineBody stateMachineBody = _stateMachineDic[stateAttr.stateMachineName];
                    
                        //创建状态实例
                        AbstractState state = assembly.CreateInstance(classType.FullName) as AbstractState;
                    
                        //验证名称
                        string stateName = stateAttr.stateName;
                        if (string.IsNullOrEmpty(stateAttr.stateName)) stateName = classType.Name;
                    
                        //添加状态到状态机中
                        stateMachineBody.AddState(stateName,state);
                    }
                    
                }
            }
        }

        public void AfterConfig()
        {
            
        }


        public void AfterConfigAll()
        {
            //对不指定状态机的状态进行延迟加载
            if (_stateMachineDic.Any())
            {
                foreach (StateMachineBody body in _stateMachineDic.Values)
                {
                    if(_delayLoadState.Any())
                        foreach ((Assembly assembly, Type classType, Type attributeType,string stateName) val in _delayLoadState)
                        {
                            
                            //创建状态实例
                            AbstractState state = val.assembly.CreateInstance(val.classType.FullName) as AbstractState;
                            state.InitAfterConstruct();
                    
                            //验证名称
                            string stateName = val.stateName;
                    
                            //添加状态到状态机中
                            body.AddState(stateName,state);
                        }
                }
            }
        }

        public StateMachineBody GetStateMachineBody(string stateMachineName)
        {
            if (_stateMachineDic.ContainsKey(stateMachineName))return _stateMachineDic[stateMachineName];
            return null;
        }
    }
}