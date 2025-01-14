using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Core.FrameWork;
using UnityEngine;

namespace Material.UnityDevToolkits.PlayerStateMachine
{
    public static class StateMachineFactory
    {
        private static readonly PlayerStateMachineConfiguration _configuration;
        
        static StateMachineFactory()
        {
            IGetContainer obj = new Obj();
            _configuration = obj.GetFrameContainer().GetConfigure<PlayerStateMachineConfiguration>();
        }
        
        public static StateMachineBody GetStateMachineBody(this IGetStateMachineBody target,string stateMachineName)
        {
            return _configuration.GetStateMachineBody(stateMachineName);
        }
        
        private class Obj : IGetContainer{}
    }
}