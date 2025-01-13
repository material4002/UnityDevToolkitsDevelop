using System;
using com.bbbirder;
using Material.UnityDevToolkits.SceneFilter.Structures;

namespace Material.UnityDevToolkits.SceneFilter
{
    /// <summary>
    /// 注册过滤器
    /// 路径格式 A/B/C
    /// 只会将过滤器注册到C中，如果重合不会被添加
    /// 如果想注册过滤器链请分别注册A,A/B,A/B/C
    /// </summary>
    [AttributeUsage(AttributeTargets.All,AllowMultiple = true, Inherited = false)]
    public class RegisterFilter : DirectRetrieveAttribute
    {
        private string _path = "";
        private FilterType _type = FilterType.Normal;
        
        public string Path
        {
            get { return _path; }
        }

        public FilterType Type
        {
            get { return _type; }
        }
        
        
        
        /// <summary>
        /// 注册一个过滤器
        /// </summary>
        /// <param name="path">过滤器路径类似A/B/C,C为最终注册位置</param>
        /// <param name="type">筛选方式，First会在筛选出第一个后跳出循环</param>
        public RegisterFilter( string path="",FilterType type = FilterType.Normal)
        {
            _type = type;
            _path = path;
        }
        
        public RegisterFilter(string path)
        {
            _path = path;
            _type = FilterType.Normal;
        }
    }
}