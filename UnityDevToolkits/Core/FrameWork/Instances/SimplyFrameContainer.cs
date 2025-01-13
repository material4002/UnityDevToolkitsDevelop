

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using com.bbbirder;
using Material.UnityDevToolkits.Core.Config;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

using Debug = UnityEngine.Debug;


namespace Material.UnityDevToolkits.Core.FrameWork.Instances
{
    public class SimplyFrameContainer : FrameContainer
    {
        private delegate void OnSceneEnter();
        private delegate void OnSceneExit();
        private OnSceneEnter onSceneEnter;
        private OnSceneExit onSceneExit;

        private List<Type> types;
        private static bool isInit = false;
        
        private SimplyFrameContainer()
        {
            
        }
        
        /// <summary>
        /// 只有第一次才有效
        /// </summary>
        public static void CreateFrameContainer()
        {
            if (FrameContainer.Instance == null && !isInit)
            {
                FrameContainer.Instance = new SimplyFrameContainer();
                
                //TODO: 在此处进行初始化，并进行切入
                
                isInit = true;
                FrameContainer.Instance.InitContainer();
                
            }
            
            
        }
        
        public override void InitContainer()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InitData();
            sw.Stop();
            Debug.Log($"InitData：{sw.ElapsedMilliseconds}ms");
            
            sw = new Stopwatch();
            sw.Start();
            LoadConfig();
            sw.Stop();
            Debug.Log($"LoadConfig：{sw.ElapsedMilliseconds}ms");
            
            sw = new Stopwatch();
            sw.Start();
            InitConfig();
            sw.Stop();
            Debug.Log($"InitConfig：{sw.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// 在此处先将两个初始化函数提出
        /// 用于测试性能
        /// </summary>

        public void InitData()
        {
            
            //在此初始化字典
            ConfigureDic = new Dictionary<Type, object>();
            ConfigDic = new Dictionary<Type, List<IConfig>>();
            
            //在此订阅场景切换回调
            SceneManager.sceneLoaded += (_, __) => EnterScene();
            SceneManager.sceneUnloaded += (_) => ExitScene();
        }

        public void LoadConfig()
        {
            RegisterConfig[] registerConfigAttributes = Retriever.GetAllAttributes<RegisterConfig>();
            //这里可以获取全部的Attribute,内部注册有绑定的类型
            
            //之后之间进行注册即可
            //生成实例，注册字典信息
            foreach (RegisterConfig registerConfigAttribute in registerConfigAttributes)
            {
                var type = registerConfigAttribute.targetInfo as Type;
                object instance = ExecutingAssembly.CreateInstance(type.FullName);
                ConfigureDic.Add(type,instance);

                if (instance is IConfigInit)
                {
                    ((IConfigInit)instance).OnConfigInit();
                }

                if (instance is IConfig)
                {
                    if (ConfigDic.ContainsKey(registerConfigAttribute.ListenedAttributeType))
                    {
                        ConfigDic[registerConfigAttribute.ListenedAttributeType].Add((IConfig)instance);
                    }
                    else
                    {
                        ConfigDic.Add(registerConfigAttribute.ListenedAttributeType, new List<IConfig>() { (IConfig)instance });
                    }
                }

                if (instance is ISceneChange)
                {
                    ISceneChange sceneChange = (ISceneChange) instance;
                    onSceneEnter += (sceneChange).OnSceneEnter;
                    onSceneExit += (sceneChange).OnSceneExit;
                }
            }
            
            
        }

        public void InitConfig()
        {
            //使用新方案进行初始化
            //获取全部的标签
            DirectRetrieveAttribute[] attributes = Retriever.GetAllAttributes<DirectRetrieveAttribute>();
            //遍历
            if (attributes.Any())
            {
                foreach (DirectRetrieveAttribute attribute in attributes)
                {
                    Type attrType = attribute.GetType();
                    Type classType = attribute.targetInfo as Type;
                    if(classType == null) continue;
                    
                    if (ConfigDic.ContainsKey(attrType))
                    {
                        foreach (IConfig config in ConfigDic[attrType])
                        {
                            config.BeforeConfig();
                            config.Config(ExecutingAssembly,classType,attrType);
                            config.AfterConfig();
                        }
                    }
                }
            }
            
            //执行声明周期，在全部注册完后执行
            if (ConfigureDic.Any())
            {
                foreach (object o in ConfigureDic.Values)
                {
                    if (o is IAfterConfig)
                    {
                        ((IAfterConfig)o).AfterConfigAll();
                    }
                }
            }
        }

        protected override void EnterScene()
        {
            if(onSceneEnter == null) return;
            onSceneEnter();
        }

        protected override void ExitScene()
        {
            if(onSceneExit == null) return;
            onSceneExit();
        }
    }
}