using System.Collections.Generic;
using Material.UnityDevToolkits.CommandPannel.Interfaces;

namespace Material.UnityDevToolkits.CommandPanel.Presets
{
    public class ErrorCommand:ICommand
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