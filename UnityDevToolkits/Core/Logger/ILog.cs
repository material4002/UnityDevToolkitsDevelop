namespace Material.UnityDevToolkits.Core.Logger
{
    /// <summary>
    /// 日志模块，用于在多个地方打印日志信息。
    /// </summary>
    public interface ILog
    {
        void Print(string message);
        
        void Warning(string message);
        
        void Error(string message);
    }
}