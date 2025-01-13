using System;
using System.Diagnostics.CodeAnalysis;
using com.bbbirder;
using UnityEditor;
using UnityEngine.UI;

namespace Material.UnityDevToolkits.Core.Config
{
    /// <summary>
    /// 用于注册配置模块
    /// 在第一次遍历的时候会扫描该特性
    /// 可以注册需要监听的注解
    /// 在第二次遍历程序集的时候会将符合条件的类型传入注册函数中
    /// </summary>
    public class RegisterConfig :DirectRetrieveAttribute
    {
        //TODO: 可能使用Type进行判断，根据情况去修改
        public readonly Type ListenedAttributeType;

        public class NullType 
        {
        }
        
        public RegisterConfig([NotNull]Type listenedAttributeType)
        {
            this.ListenedAttributeType = listenedAttributeType;
        }

        public RegisterConfig()
        {
            ListenedAttributeType = typeof(NullType);
        }
        
    }
}