using System;
using System.Collections.Generic;
using System.Linq;
using Material.UnityDevToolkits.Core.Logger;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;
using Unity.VisualScripting;

namespace Material.UnityDevToolkits.Core.FrameWork.Instances
{
    /// <summary>
    /// FrameContainer实现类之一
    /// 通过遍历的方式实现容器功能。
    /// 效率可能略低
    /// </summary>
    public class SimpleContainer :FrameContainer
    {   
        /// <summary>
        /// 由于使用特性来标注模块
        /// 所有的框架相关的内容都有特性作为标注
        /// 所以可以剔除掉一些不含有特性的类型
        /// </summary>
        private Assembly _assembly;
        private List<Type> _types;
        
        private Dictionary<Type, IConfigInit> _configs;
        private Dictionary<Type, List<IConfig>> _configListeners;
        
        public SimpleContainer()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _types = _assembly.GetTypes().ToList();
            
            InitContainer();
            LoadConfiguration();
            Config();
        }
        
        protected override void InitContainer()
        {
            //在这里对不包含特性的类型进行剔除
            _types = _types.Where(t => t.GetCustomAttributes(false).Any()).ToList();
        }

        

        protected override void LoadConfiguration()
        {
            //进行第一次遍历，获取全部的Config
            List<Type> configTypes = _types
                .Where(t=> t.GetCustomAttribute<RegisterConfig>(false)!=null).ToList();
            
            //遍历获取的config
            if (configTypes.Any())
            {
                foreach (Type configType in configTypes)
                {
                    //创建实例，并存入字典中
                    IConfig config = _assembly.CreateInstance(configType.FullName) as IConfig;
                    _configs.Add(configType, config as IConfigInit);
                    
                    //将根据实例监听的类型进行归类，并存入字典中
                    RegisterConfig rc = configType.GetCustomAttribute<RegisterConfig>(false);
                    Type listenType = rc.ListenedAttribute.GetType();
                    if (_configListeners.ContainsKey(listenType))
                    {
                        _configListeners[listenType].Add(config);
                    }
                    else
                    {
                        _configListeners.Add(listenType, new List<IConfig>());
                        _configListeners[listenType].Add(config);
                    }
                }
            }
            
            //遍历全部的实例并初始化
            if (_configs.Any())
            {
                foreach (IConfigInit i in _configs.Values)
                {
                    i.ConfigInit();
                }
            }
        }

        protected override void Config()
        {
            //先对配置进行Before执行
            if (_configListeners.Any())
            {
                foreach (var list in _configListeners.Values)
                {
                    foreach (IConfig i in list)
                    {
                        i.BeforeConfig();
                    }
                }
            }
            
            //遍历全部实例并进行配置的加载
            
            //对程序集进行筛选
            List<Type> types = _types
                .Where(t => t.GetCustomAttributes(false)
                    .Any(a => _configListeners.ContainsKey(a.GetType())))
                .ToList();
            
            //遍历合法的类型
            if (types.Any())
            {
                foreach (Type t in types)
                {
                    //获取类型全部的特性
                    var a =  t.GetCustomAttributes(false);
                    
                    //遍历全部的特性
                    if(a.Any())continue;
                    foreach (var o in a )
                    {
                        //获取特性的类型
                        Type type = o.GetType();
                        //如果该特性的类型在字典中有匹配，则遍历字典中的配置，并向配置传入类型类型！！！非特性，而是特性所属的类型
                        if (_configListeners.ContainsKey(type))
                        {
                            foreach (IConfig ic in _configListeners[t])
                            {
                                ic.Config(_assembly,type);
                            }
                        }
                    }

                    
                }
            }
            
            //最后对配置进行After执行
            if (_configListeners.Any())
            {
                foreach (var list in _configListeners.Values)
                {
                    foreach (IConfig i in list)
                    {
                        i.AfterConfig();
                    }
                }
            }
        }
    }
}