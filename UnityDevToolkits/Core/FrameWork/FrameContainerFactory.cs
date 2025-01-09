using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork.Instances;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.FrameWork
{
    public static class FrameContainerFactory
    {
        /// <summary>
        /// 模块可通过获取此函数激活框架
        /// 可将激活链向下传递
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static FrameContainer GetFrameContainer(this IConfig config)
        {
            if (FrameContainer.Instance == null)
            {
                FrameContainer container = Create();
                //TODO: 创建FrameContainer实例并返回
            }
            return FrameContainer.Instance;
        }
        
        #if UNITY_5_3_OR_NEWER
        /// <summary>
        /// 自动加载方案。
        /// 会在场景加载完成，画面刷新之前完成
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void AutoLoad()
        {
            Create();
        }
        #endif
        
        /// <summary>
        /// 根据是否拥有配置文件变量来进行加载框架实例。
        /// </summary>
        /// <returns>初始化完成的实例</returns>
        private static FrameContainer Create()
        {
            #if STATER_PATH
            //TODO: 在检测到配置文件的时候使用配置文件进行启动
            string path = "STATER_PATH";
            return null;
            #else
            //TODO: 使用默认配置启动框架
            return new SimpleContainer();
            #endif
        }
    }
}