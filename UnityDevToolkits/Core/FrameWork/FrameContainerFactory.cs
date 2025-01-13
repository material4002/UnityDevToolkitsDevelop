
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork.Instances;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.FrameWork
{
    public static class FrameContainerFactory
    {
        /// <summary>
        /// 模块可通过获取此函数激活框架
        /// 可将激活链向下传递.
        /// 在此阻止反复创建
        /// </summary>
        public static FrameContainer GetFrameContainer(this IGetContainer iGetContainer)
        {
            if (FrameContainer.Instance == null)
            {
                Create();
                //TODO: 创建FrameContainer实例并返回
            }
            return FrameContainer.Instance;
        }
        
        #if UNITY_5_3_OR_NEWER
        /// <summary>
        /// 自动加载方案。
        /// 会在场景加载完成，画面刷新之前完成
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoLoad()
        {
            
            Create();
        }
        #endif
        
        /// <summary>
        /// 在此进行框架的创建
        /// </summary>
        /// <returns>初始化完成的实例</returns>
        public static FrameContainer Create()
        {
            //TODO: 在未来可通过配置文件进行创建
            SimplyFrameContainer.CreateFrameContainer();
            return FrameContainer.Instance;
        }
    }
}