using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Manager;
using Material.UnityDevToolkits.Manager.Injection;
using Material.UnityDevToolkits.Manager.LifeCycles;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterManager]
    public class TestManager1:IManagerAfterConstruct,IManagerCoreInit,IManagerInit,IManagerLastInit,IManagerChangeScene
    {
        [GetComponent]
        public Rigidbody rb;
        
        [GetComponent]
        public BoxCollider bc;
        
        public void InitAfterConstruct()
        {
            Debug.Log("TestManager1 InitAfterConstruct");
        }

        public void ManagerInit()
        {
            Debug.Log("TestManager1 ManagerInit");
        }

        public void Awake()
        {
            Debug.Log("TestManager2 Awake");
        }

        public void Start()
        {
            Debug.Log("TestManager2 Start");
        }

        public void LastInit()
        {
            Debug.Log("TestManager1 LastInit");
        }

        public void OnEnterScene()
        {
            Debug.Log("TestManager1 OnEnterScene");
        }

        public void OnExitScene()
        {
            Debug.Log("TestManager1 OnExitScene");
        }
    }
}