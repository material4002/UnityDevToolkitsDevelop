using Material.UnityDevToolkits.PlayerStateMachine;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterState(stateMachineName:"TestMachine")]
    public class TestState2:AbstractState
    {
        public override void InitAfterConstruct()
        {
            
        }

        public override void OnAwake()
        {
            Debug.Log($"Test2 OnAwake in {component.name}");
        }

        public override void OnStart()
        {
            Debug.Log($"Test2 OnStart in {component.name}");
        }

        public override void OnUpdate()
        {
            Debug.Log($"Test2 OnUpdate in {component.name}");
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