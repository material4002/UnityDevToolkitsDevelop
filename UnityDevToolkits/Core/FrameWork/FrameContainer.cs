using Material.UnityDevToolkits.Core.Logger;

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
        /// 日志模块，用于记录框架的运行状态
        /// </summary>
        public ILog Logger
        {
            get => _logger;
            
            protected set => _logger = value;
        }

        private ILog _logger;
        
        /// <summary>
        /// 初始化方法，可用于加载配置文件，初始化参数
        /// </summary>
        protected abstract void InitContainer();
        
        /// <summary>
        /// 日志模块加载
        /// </summary>
        protected abstract void LoadLogger();
        
        /// <summary>
        /// 配置模块的初始化，第一次遍历程序集
        /// </summary>
        protected abstract void LoadConfiguration();
        
        /// <summary>
        /// 第二次配置模块加载，会遍历程序集，加载业务
        /// </summary>
        protected abstract void Config();
    }
}