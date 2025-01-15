using System.Collections.Generic;

namespace Material.UnityDevToolkits.CommandPannel.Interfaces
{
    public interface ICommand
    {
        void InitCommand();
        void ExecuteCommand(IEnumerable<string> commandArgv);
        
        public string Description { get; set; }
    }
}