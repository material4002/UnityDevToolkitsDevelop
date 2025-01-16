using System;
using System.Reflection;

namespace Material.UnityDevToolkits.Core.Config
{
    /// <summary>
    /// 配置接口
    /// 会传入程序集
    /// 便于使用反射进行初始化，减少遍历程序集的次数
    /// </summary>
    public interface IConfig
    {
        void BeforeConfig();
        
        /// <summary>
        /// 在遍历程序集时会将注册的类型实时传入Config函数中
        /// </summary>
        void Config(Assembly assembly,Type classType,Type attributeType,Attribute attribute);
        
        void AfterConfig();
    }
}