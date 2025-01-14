using Material.UnityDevToolkits.PlayerStateMachine;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterState(stateName:"Test1",stateMachineName:"TestMachine")]
    public class TestState4:AbstractState
    {
        public override void InitAfterConstruct()
        {
            
        }

        public override void OnAwake()
        {
            Debug.Log($"Test4 OnAwake in {component.name}");
        }

        public override void OnStart()
        {
            Debug.Log($"Test4 OnStart in {component.name}");
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