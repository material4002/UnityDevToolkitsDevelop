using Material.UnityDevToolkits.PlayerStateMachine;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterState(stateName:"Test1",stateMachineName:"TestMachine")]
    public class TestState1:AbstractState
    {
        public float t = 5f;
        public bool outoftime = true;
        
        public override void InitAfterConstruct()
        {
            
        }

        public override void OnAwake()
        {
            Debug.Log($"Test1 OnAwake in {component.name}");
        }

        public override void OnStart()
        {
            Debug.Log($"Test1 OnStart in {component.name}");
        }

        public override void OnUpdate()
        {
            Debug.Log($"Test1 OnUpdate in {stateMachineName}");
            t -= Time.deltaTime;

            if (outoftime && t < 0)
            {
                ChangeState("TestState2");
                outoftime = false;
            }
        }

        public override void OnFixedUpdate()
        {
            Debug.Log($"Test1 OnFixedUpdate in {stateMachineName}");
        }

        public override void OnLateUpdate()
        {
            Debug.Log($"Test1 OnLateUpdate in {stateMachineName}");
        }

        public override void OnEnterState()
        {
            Debug.Log($"Test1 OnEnterState in {component.name}");
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