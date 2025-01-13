

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
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
            //在此裁剪程序集
            types = ExecutingAssembly
                .GetTypes()
                .Where(t => !t.GetCustomAttributes(false).Any() && !t.IsAbstract && !t.IsInterface)
                .ToList();
            
            //在此初始化字典
            ConfigureDic = new Dictionary<Type, object>();
            ConfigDic = new Dictionary<Type, List<IConfig>>();
            
            //在此订阅场景切换回调
            SceneManager.sceneLoaded += (_, __) => EnterScene();
            SceneManager.sceneUnloaded += (_) => ExitScene();
        }

        public void LoadConfig()
        {
            //在此注册Config
            Type nullType = typeof(RegisterConfig.NullType);
            foreach (Type type in types)
            {
                var registerConfigs = type.GetCustomAttributes<RegisterConfig>();
                if (registerConfigs.Any())
                {
                    object instance = ExecutingAssembly.CreateInstance(type.FullName);

                    if (instance is IConfigInit)
                    {
                        IConfigInit configInit = (IConfigInit) instance;
                        configInit.OnConfigInit();
                    }

                    if (instance is ISceneChange)
                    {
                        ISceneChange sceneChange = (ISceneChange) instance;
                        onSceneEnter += sceneChange.OnSceneEnter;
                        onSceneExit += sceneChange.OnSceneExit;
                    }
                    
                    ConfigureDic.Add(type, instance);

                    if (instance is IConfig)
                    {
                        IConfig config = (IConfig) instance;
                        foreach (RegisterConfig registerConfig in registerConfigs)
                        {
                            Type registerType = registerConfig.ListenedAttributeType;
                            
                            if(registerType == nullType)continue;
                            
                            if (ConfigDic.ContainsKey(registerType))
                            {
                                ConfigDic[registerType].Add(config);
                            }
                            else
                            {
                                ConfigDic.Add(registerType, new List<IConfig> { config });
                            }
                        }
                    }

                    
                }
            }
        }

        public void InitConfig()
        {
            //在此初始化
            var keys = ConfigDic.Keys;
            foreach (Type type in types)
            {
                object[] customAttributes = type.GetCustomAttributes(false);
                Type[] attributeTypes = Array.ConvertAll<object, Type>(customAttributes, a => a.GetType());

                List<Type> intersect = new List<Type>();
                if (keys.Any())
                {
                    intersect = keys.Intersect(attributeTypes).ToList();
                }

                if (intersect.Any())
                {
                    List<IConfig> configs = new List<IConfig>();
                    foreach (Type t in intersect)
                    {
                        if (ConfigDic.TryGetValue(t, out configs))
                        {
                            foreach (IConfig config in configs)
                            {
                                config.BeforeConfig();
                                config.Config(ExecutingAssembly, type, t);
                                config.AfterConfig();
                            }
                        }
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