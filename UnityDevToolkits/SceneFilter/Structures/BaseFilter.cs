using System.Collections.Generic;
using UnityEngine;

namespace Material.UnityDevToolkits.SceneFilter.Structures
{
    /// <summary>
    /// 作为过滤器的基类，可用于筛选场景中的物体
    /// </summary>
    public abstract class BaseFilter:Dictionary<int,GameObject>
    {
        
        //用于初始化
        public abstract void Init();
        //用于在添加前检测是否符合添加条件
        public abstract bool Where(GameObject gameObject);
        //注册回调，在获取完成后会调用
        public abstract void Register();

    }
}