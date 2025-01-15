using System.Collections.Generic;
using Material.UnityDevToolkits.CommandPannel.Interfaces;

namespace Material.UnityDevToolkits.CommandPanel.Presets
{
    /// <summary>
    /// 用于在窗口中打印信息的命令。
    /// </summary>
    public class WarningCommand: ICommand
    {
        public void InitCommand()
        {
            
        }

        public void ExecuteCommand(IEnumerable<string> commandArgv)
        {
            
        }

        public string Description { get; set; }
    }
}