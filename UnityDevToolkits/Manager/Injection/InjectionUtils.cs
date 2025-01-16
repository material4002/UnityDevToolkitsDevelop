using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Material.UnityDevToolkits.Manager.Injection
{
    internal class InjectionUtils
    {
        private GameObject _componentContainer;
        //private readonly Type _getComponentAttributeType = typeof(GetComponent);
        
        public InjectionUtils()
        {
            _componentContainer = new GameObject("ComponentContainer");
            GameObject.DontDestroyOnLoad(_componentContainer);
        }
        
        public void GetComponent(Type managerType, object manager)
        {
            FieldInfo[] componentFields = managerType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(field => field.GetCustomAttributes<GetComponent>(false).Any())
                .ToArray();

            if (componentFields.Any())
            {
                GameObject managerObj = new GameObject(managerType.Name);
                managerObj.transform.SetParent(_componentContainer.transform);
                
                foreach (FieldInfo field in componentFields)
                {
                    Injection.GetComponent attr = field.GetCustomAttribute<Injection.GetComponent>(false);
                    Component comp;
                    if (attr.useFactoryMethod)
                    {
                        MethodInfo factoryMethod = attr.factoryType.GetMethod(attr.methodName,BindingFlags.Static | BindingFlags.Public);

                        comp = factoryMethod.Invoke(null,null) as Component;
                    }
                    else
                    {
                        GameObject componentObj = new GameObject(field.FieldType.Name);
                        componentObj.transform.SetParent(managerObj.transform);
                        comp = componentObj.AddComponent(field.FieldType);
                    }
                    
                    field.SetValue(manager,comp);
                }
            }
        }
    }
}