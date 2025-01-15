using Material.UnityDevToolkits.CommandPannel.Interfaces;
using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork;

namespace Material.UnityDevToolkits.CommandPanel.Factory
{
    public static class CommandLibFactory
    {
        private static CommandLibConfiguration _configuration;
        
        static CommandLibFactory()
        {
            _configuration = new obj().GetFrameContainer().GetConfigure<CommandLibConfiguration>();
        }

        public static IExecuteCommand GetExecuteCommand(this IGetCommandExecuter target)
        {
            return _configuration;
        }
        
        public  class obj : IGetContainer
        {
        }
    }
}