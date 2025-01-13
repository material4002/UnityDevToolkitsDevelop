using System;
using System.Diagnostics.CodeAnalysis;
using com.bbbirder;
using UnityEngine;

namespace Material.UnityDevToolkits.Manager.Injection
{
    public class GetComponent:DirectRetrieveAttribute
    {
        public readonly Type factoryType;
        public readonly String methodName;
        public readonly bool useFactoryMethod;
        

        public GetComponent()
        {
            factoryType = null;
            methodName = null;
            useFactoryMethod = false;
        }

        public GetComponent([NotNull]Type factoryType,[NotNull]String methodName)
        {
            this.factoryType = factoryType;
            this.methodName = methodName;
            useFactoryMethod = true;
        }
    }
}