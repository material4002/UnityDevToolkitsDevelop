using System;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterConfig(typeof(MonoBehaviour))]
    public class MyTestConfig: IConfigInit,IConfig,ISceneChange,IGetContainer
    {
        public void OnConfigInit()
        {
            Debug.Log("OnConfigInit");
        }

        public void BeforeConfig()
        {
            
            Debug.Log("BeforeConfig");
        }

        public void Config(Assembly assembly, Type classType, Type attributeType)
        {
            this.GetFrameContainer();
            Debug.Log($"Config:{classType.Name}, {attributeType.Name}");
        }

        public void AfterConfig()
        {
            Debug.Log("AfterConfig");
            
        }

        public void OnSceneEnter()
        {
            
            Debug.Log("OnSceneEnter");
        }

        public void OnSceneExit()
        {
            Debug.Log("OnSceneExit");
            
        }
    }
}