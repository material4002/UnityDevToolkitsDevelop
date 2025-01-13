using Material.UnityDevToolkits.Core.Config;
using Material.UnityDevToolkits.Manager;
using Material.UnityDevToolkits.Manager.LifeCycles;
using UnityEngine;

namespace Material.UnityDevToolkits.Core.Test
{
    [RegisterManager]
    public class TestManager2:IManagerAfterConstruct,IManagerCoreInit,IManagerInit,IManagerLastInit,IManagerChangeScene
    {
        public void InitAfterConstruct()
        {
            Debug.Log("TestManager2 InitAfterConstruct");
        }

        public void ManagerInit()
        {
            Debug.Log("TestManager2 ManagerInit");
        }

        

        public void LastInit()
        {
            Debug.Log("TestManager2 LastInit");
        }

        public void OnEnterScene()
        {
            Debug.Log("TestManager2 OnEnterScene");
        }

        public void OnExitScene()
        {
            Debug.Log("TestManager2 OnExitScene");
        }

        public void Awake()
        {
            Debug.Log("TestManager2 Awake");
        }

        public void Start()
        {
            Debug.Log("TestManager2 Start");
        }
    }
}