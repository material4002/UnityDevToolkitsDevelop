using System;
using System.Collections.Generic;
using System.Reflection;
using Material.UnityDevToolkits.Core.Config;

namespace Material.UnityDevToolkits.Core.FrameWork
{
    /// <summary>
    /// 框架的抽象主体。
    /// 在实现类中做各种实现方案
    ///
    /// 框架实现方案：
    /// 1. 框架初始化（加载配置文件？设置默认配置文件）
    /// 2. 加载日志模块（可否作为一个模块而非耦合在一起）
    /// 3. 遍历程序集，加载模块
    /// 4. 遍历程序集，模块初始化
    /// 5. 进入游戏生命周期
    /// 6. 场景切换时激活回调
    /// 7. 退出游戏时激活回调
    /// </summary>
    public abstract class FrameContainer
    {
        
        private static FrameContainer _instance;
        
        /// <summary>
        /// 使用单例进行储存，保证全局唯一性。
        /// 一旦被创建，不允许再次创建。
        /// </summary>
        public static FrameContainer Instance
        {
            get => _instance;
            protected set
            {
                if (_instance == null)
                {
                    _instance = value;
                }
            }
        }
        
        /// <summary>
        /// 模块容器，便于检索
        /// </summary>
        protected Dictionary<Type, object> ConfigureDic;
        /// <summary>
        /// 配置容器，在初始化时用于加载。
        /// </summary>
        protected Dictionary<Type, List<IConfig>> ConfigDic;
        
        /// <summary>
        /// 当前的程序集
        /// </summary>
        protected readonly Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
        
        
        
        /// <summary>
        /// 用于初始化框架的主体
        /// 加载程序集，构造框架
        /// </summary>
        public abstract void InitContainer();
        
        /// <summary>
        /// 进入场景前执行，用于初始化模块。
        /// 由于场景切换模块可能需要重新启动，所以需要在场景切换前重新初始化。
        /// </summary>
        protected abstract void EnterScene();
        
        /// <summary>
        /// 离开场景前执行，用于清理模块。
        /// </summary>
        protected abstract void ExitScene();


    }
}