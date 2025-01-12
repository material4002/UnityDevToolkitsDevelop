

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;


namespace Material.UnityDevToolkits.Core.FrameWork.Instances
{
    public class SimplyFrameContainer : FrameContainer
    {
        
        private SimplyFrameContainer()
        {
            
        }
        
        /// <summary>
        /// 只有第一次才有效
        /// </summary>
        public static void CreateFrameContainer()
        {
            if (FrameContainer.Instance == null) FrameContainer.Instance = new SimplyFrameContainer();
        }

        protected override void InitContainer()
        {
            //在此裁剪程序集
            List<Type> types = ExecutingAssembly
                .GetTypes()
                .Where(t => !t.GetCustomAttributes(false).Any() && !t.IsAbstract && !t.IsInterface)
                .ToList();
            
            //在此初始化字典
            ConfigureDic = new Dictionary<Type, object>();
            ConfigDic = new Dictionary<Type, List<IConfig>>();
            
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
            
        }

        protected override void ExitScene()
        {
            
        }
    }
}