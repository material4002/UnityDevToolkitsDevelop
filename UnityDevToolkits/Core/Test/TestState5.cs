using Material.UnityDevToolkits.PlayerStateMachine;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterState(stateName:"Test5",stateMachineName:"TestMachine2")]
    public class TestState5:AbstractState
    {
        public override void InitAfterConstruct()
        {
            
        }

        public override void OnAwake()
        {
            Debug.Log($"Test5 OnAwake in {component.name}");
        }

        public override void OnStart()
        {
            Debug.Log($"Test5 OnStart in {component.name}");
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnLateUpdate()
        {
            
        }

        public override void OnEnterState()
        {
            
        }

        public override void OnExitState()
        {
            
        }

        public override void OnMachineEnabled()
        {
            
        }

        public override void OnMachineDisabled()
        {
            
        }
    }
}