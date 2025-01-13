using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Manager.Injection;
using Material.UnityDevToolkits.Manager.LifeCycles;
using UnityEngine;

namespace Material.UnityDevToolkits.Manager
{
    /// <summary>
    /// 一个管理器系统，用来管理全局的管理器
    /// </summary>
    [RegisterConfig(typeof(RegisterManager))]
    public class ManagerConfiguration:IConfigInit,IConfig,ISceneChange,IGetContainer,IAfterConfig
    {
        private Dictionary<Type,object> _managerDic;

        private delegate void OnSceneEnterDelegate();
        private delegate void OnSceneExitDelegate();
        
        private OnSceneEnterDelegate _onSceneEnterDelegate;
        private OnSceneExitDelegate _onSceneExitDelegate;
        
        private InjectionUtils _injectionUtils;
        
        public void OnConfigInit()
        {
            _managerDic = new Dictionary<Type, object>();
            _injectionUtils = new InjectionUtils();
            
        }

        public void BeforeConfig()
        {
            
        }

        public void Config(Assembly assembly, Type classType, Type attributeType)
        {
            object manager = assembly.CreateInstance(classType.FullName);
            _managerDic.Add(classType, manager);
            
            _injectionUtils.GetComponent(classType,manager);

            if (manager is IManagerAfterConstruct)
            {
                IManagerAfterConstruct managerAfterConstruct = manager as IManagerAfterConstruct;
                managerAfterConstruct.InitAfterConstruct();
            }
        }

        public void AfterConfig()
        {
            
        }

        public void OnSceneEnter()
        {
            if (_onSceneEnterDelegate != null) _onSceneEnterDelegate();
        }

        public void OnSceneExit()
        {
            if (_onSceneExitDelegate != null) _onSceneExitDelegate();
        }
        
        public T GetManager<T>() where T:class
        {
            return _managerDic.TryGetValue(typeof(T),out var manager) ? (T)manager : null;
        }

        public void AfterConfigAll()
        {
            if (_managerDic.Any())
            {
                List<object> list = _managerDic.Values.ToList();
                
                list.ForEach(manager =>
                {
                    if(manager is IManagerCoreInit)((IManagerCoreInit)manager).ManagerInit();
                });
                
                list.ForEach(manager =>
                {
                    if(manager is IManagerInit)((IManagerInit)manager).Awake();
                });
                
                list.ForEach(manager =>
                {
                    if(manager is IManagerInit)((IManagerInit)manager).Start();
                });
                
                list.ForEach(manager =>
                {
                    if(manager is IManagerLastInit)((IManagerLastInit)manager).LastInit();
                });
                
                list.ForEach(manager =>
                {
                    if(manager is IManagerChangeScene)
                    {
                        var managerChangeScene = (IManagerChangeScene)manager;
                        _onSceneEnterDelegate += managerChangeScene.OnEnterScene;
                        _onSceneExitDelegate += managerChangeScene.OnExitScene;
                    }
                });
            }
        }

        
    }
}