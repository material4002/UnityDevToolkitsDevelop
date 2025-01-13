using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Material.UnityDevToolkits.Manager.Injection
{
    internal class InjectionUtils
    {
        private GameObject _componentContainer;
        private readonly Type _getComponentAttributeType = typeof(GetComponent);
        
        public InjectionUtils()
        {
            _componentContainer = new GameObject("ComponentContainer");
            GameObject.DontDestroyOnLoad(_componentContainer);
        }
        
        public void GetComponent(Type managerType, object manager)
        {
            FieldInfo[] componentFields = managerType.GetFields()
                .Where(field => field.GetCustomAttributes(false).Any(a => a.GetType() == _getComponentAttributeType))
                .ToArray();

            if (componentFields.Any())
            {
                GameObject managerObj = new GameObject(managerType.Name);
                managerObj.transform.SetParent(_componentContainer.transform);
                
                foreach (FieldInfo field in componentFields)
                {
                    GameObject componentObj = new GameObject(field.FieldType.Name);
                    componentObj.transform.SetParent(managerObj.transform);
                    Component comp = componentObj.AddComponent(field.FieldType);
                    
                    field.SetValue(manager,comp);
                }
            }
        }
    }
}