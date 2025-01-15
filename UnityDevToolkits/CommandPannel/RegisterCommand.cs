using System;
using System.Diagnostics.CodeAnalysis;
using com.bbbirder;

namespace Material.UnityDevToolkits.CommandPanel
{
    /// <summary>
    /// 注册一个指令
    /// 默认使用空格进行分割，例如：
    /// echo "hello world"
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true,Inherited = false)]
    public class RegisterCommand:DirectRetrieveAttribute
    {
        private readonly string _commmand;
        public string Command => _commmand;
        
        private readonly string _description;
        public string Description => _description;

        public RegisterCommand([NotNull]string command,string description = "")
        {
            _commmand = command;
            _description = description;
        }
    }
}