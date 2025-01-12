namespace Material.UnityDevToolkits.Core.Config
{
    /// <summary>
    /// 不会传入程序集的初始化
    /// 可用于初始化不需要反射的模块，可进行前置的设置
    /// 会最先执行
    /// </summary>
    public interface IConfigInit
    {
        void OnConfigInit();
    }
}