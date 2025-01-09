using System;
using System.Collections.Generic;
using System.Linq;
using Material.UnityDevToolkits.Core.Logger;
using System.Reflection;
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
        private Type[] _types;
        
        public SimpleContainer()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _types = _assembly.GetTypes();
            
            InitContainer();
            LoadLogger();
            LoadConfiguration();
            Config();
        }
        
        protected override void InitContainer()
        {
            //在这里对不包含特性的类型进行剔除
            _types = _types.Where(t => t.GetCustomAttributes(false).Any()).ToArray();
        }

        protected override void LoadLogger()
        {
            LoggerDic log = new LoggerDic();
            
            //在这里进行第一次遍历
            Type iLog = typeof(ILog);
            Type registerLogger = typeof(RegisterLogger);
            
            //查找合法的Logger
            List<Type> loggerTypes = _types
                .Where(t => t.GetInterfaces().Any(i => i == iLog) &&
                            t.GetCustomAttributes(registerLogger, false).Any()).ToList();
            
            //进行实例化
            if (loggerTypes.Any())
            {
                foreach (Type loggerType in loggerTypes)
                {
                    ILog l = _assembly.CreateInstance(loggerType.FullName) as ILog;
                    log.Add(l);
                }
            }
            
            //进行赋值
            Logger = log;
        }

        protected override void LoadConfiguration()
        {
            
        }

        protected override void Config()
        {
            
        }
    }
}